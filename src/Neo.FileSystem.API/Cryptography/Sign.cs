using Google.Protobuf;
using Neo.FileSystem.API.Session;
using Neo.FileSystem.API.Refs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using System;
using System.Security.Cryptography;

namespace Neo.FileSystem.API.Cryptography
{
    public static class SignExtension
    {
        public const int RFC6979SignatureSize = 64;

        public static byte[] SignRFC6979(this ECDsa key, byte[] data)
        {
            var digest = new Sha256Digest();
            var secp256r1 = SecNamedCurves.GetByName("secp256r1");
            var ec_parameters = new ECDomainParameters(secp256r1.Curve, secp256r1.G, secp256r1.N);
            var private_key = new ECPrivateKeyParameters(new BigInteger(1, key.PrivateKey()), ec_parameters);
            var signer = new ECDsaSigner(new HMacDsaKCalculator(digest));
            var hash = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(hash, 0);
            signer.Init(true, private_key);
            var rs = signer.GenerateSignature(hash);
            var signature = new byte[RFC6979SignatureSize];
            var rbytes = rs[0].ToByteArrayUnsigned();
            var sbytes = rs[1].ToByteArrayUnsigned();
            var index = RFC6979SignatureSize / 2 - rbytes.Length;
            rbytes.CopyTo(signature, index);
            index = RFC6979SignatureSize - sbytes.Length;
            sbytes.CopyTo(signature, index);
            return signature;
        }

        public static Signature SignRFC6979(this ECDsa key, IMessage message)
        {
            return new Signature
            {
                Key = ByteString.CopyFrom(key.PublicKey()),
                Sign = ByteString.CopyFrom(key.SignRFC6979(message.ToByteArray())),
            };
        }

        public static byte[] SignData(this ECDsa key, byte[] data)
        {
            var hash = new byte[65];
            hash[0] = 0x04;
            key
                .SignHash(SHA512.Create().ComputeHash(data))
                .CopyTo(hash, 1);
            return hash;
        }

        public static Signature SignMessagePart(this ECDsa key, IMessage data)
        {
            var data2sign = data is null ? Array.Empty<byte>() : data.ToByteArray();
            var sig = new Signature
            {
                Key = ByteString.CopyFrom(key.PublicKey()),
                Sign = ByteString.CopyFrom(key.SignData(data2sign)),
            };
            return sig;
        }

        public static void SignRequest(this ECDsa key, IMessage message)
        {
            if (message is IRequest to_sign)
            {
                if (to_sign.MetaHeader is null)
                    to_sign.MetaHeader = RequestMetaHeader.Default;
                var verify_origin = to_sign.VerifyHeader;
                var meta_header = to_sign.MetaHeader;
                var verify_header = new RequestVerificationHeader();

                if (verify_origin is null)
                {
                    verify_header.BodySignature = key.SignMessagePart(to_sign.GetBody());
                }
                verify_header.MetaSignature = key.SignMessagePart(meta_header);
                if (verify_origin is null)
                    verify_header.OriginSignature = key.SignMessagePart(new RequestVerificationHeader());
                else
                    verify_header.OriginSignature = key.SignMessagePart(verify_origin);
                verify_header.Origin = verify_origin;
                to_sign.VerifyHeader = verify_header;
            }
            else
            {
                throw new InvalidOperationException("can't sign message");
            }
        }

        public static void SignResponse(this ECDsa key, IMessage message)
        {
            if (message is IResponse to_sign)
            {
                if (to_sign.MetaHeader is null)
                    to_sign.MetaHeader = new ResponseMetaHeader();
                var verify_origin = to_sign.VerifyHeader;
                var meta_header = to_sign.MetaHeader;
                var verify_header = new ResponseVerificationHeader();

                if (verify_origin is null)
                {
                    verify_header.BodySignature = key.SignMessagePart(to_sign.GetBody());
                }
                else
                {
                    verify_header.OriginSignature = key.SignMessagePart(verify_origin);
                }
                verify_header.MetaSignature = key.SignMessagePart(meta_header);
                verify_header.Origin = verify_origin;
                to_sign.VerifyHeader = verify_header;
            }
            else
            {
                throw new InvalidOperationException("can't sign message");
            }
        }

        private static BigInteger[] DecodeSignature(byte[] sig)
        {
            if (sig.Length != RFC6979SignatureSize) throw new FormatException($"Wrong signature size, expect={RFC6979SignatureSize}, actual={sig.Length}");
            var rs = new BigInteger[2];
            rs[0] = new BigInteger(1, sig[..32]);
            rs[1] = new BigInteger(1, sig[32..]);
            return rs;
        }

        public static bool VerifyRFC6979(this byte[] public_key, byte[] data, byte[] sig)
        {
            var rs = DecodeSignature(sig);
            var digest = new Sha256Digest();
            var signer = new ECDsaSigner(new HMacDsaKCalculator(digest));
            var secp256r1 = SecNamedCurves.GetByName("secp256r1");
            var ec_parameters = new ECDomainParameters(secp256r1.Curve, secp256r1.G, secp256r1.N);
            var bc_public_key = new ECPublicKeyParameters(secp256r1.Curve.DecodePoint(public_key), ec_parameters);
            var hash = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(hash, 0);
            signer.Init(false, bc_public_key);
            return signer.VerifySignature(hash, rs[0], rs[1]);
        }

        public static bool VerifyRFC6979(this Signature signature, IMessage message)
        {
            return signature.Key.ToByteArray().VerifyRFC6979(message.ToByteArray(), signature.Sign.ToByteArray());
        }

        public static bool VerifyData(this ECDsa key, byte[] data, byte[] sig)
        {
            return key.VerifyHash(SHA512.Create().ComputeHash(data), sig[1..]);
        }

        public static bool VerifyMessagePart(this Signature sig, IMessage data)
        {
            if (sig is null) return false;
            using var key = sig.Key.ToByteArray().LoadPublicKey();
            var data2verify = data is null ? Array.Empty<byte>() : data.ToByteArray();
            return key.VerifyData(data2verify, sig.Sign.ToByteArray());
        }

        private static bool VerifyMatryoshkaLevel1(IMessage body, ResponseMetaHeader meta_header, ResponseVerificationHeader verify_header)
        {
            if (verify_header is null) return false;
            if (!VerifyMessagePart(verify_header.MetaSignature, meta_header))
                return false;
            var origin = verify_header.Origin;
            if (origin is null)
            {
                if (!VerifyMessagePart(verify_header.BodySignature, body))
                    return false;
                return true;
            }
            else
            {
                if (!VerifyMessagePart(verify_header.OriginSignature, verify_header.Origin))
                    return false;
            }
            if (verify_header.BodySignature is null) return false;
            return VerifyMatryoshkaLevel1(body, meta_header.Origin, origin);
        }

        public static bool VerifyResponse(this IMessage message)
        {
            if (message is IResponse to_verify)
            {
                return VerifyMatryoshkaLevel1(to_verify.GetBody(), to_verify.MetaHeader, to_verify.VerifyHeader);
            }
            throw new InvalidOperationException("can't verify message");
        }

        private static bool VerifyMatryoshkaLevel2(IMessage body, RequestMetaHeader meta_header, RequestVerificationHeader verify_header)
        {
            if (!VerifyMessagePart(verify_header.MetaSignature, meta_header))
                return false;
            if (!VerifyMessagePart(verify_header.OriginSignature, verify_header.Origin))
                return false;
            var origin = verify_header.Origin;
            if (origin is null)
            {
                if (!VerifyMessagePart(verify_header.BodySignature, body))
                    return false;
                return true;
            }
            if (verify_header.BodySignature is null) return false;
            return VerifyMatryoshkaLevel2(body, meta_header.Origin, origin);
        }

        public static bool VerifyRequest(this IMessage message)
        {
            if (message is IRequest to_sign)
            {
                return VerifyMatryoshkaLevel2(to_sign.GetBody(), to_sign.MetaHeader, to_sign.VerifyHeader);
            }
            throw new InvalidOperationException("can't verify message");
        }
    }
}
