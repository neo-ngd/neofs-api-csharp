using Google.Protobuf;
using NeoFS.API.v2.Refs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Math;
using System;
using System.Linq;
using System.Security.Cryptography;
using static NeoFS.API.v2.Cryptography.Helper;

namespace NeoFS.API.v2.Cryptography
{
    public static class KeyExtension
    {
        public const byte NeoAddressVersion = 0x35;
        private const int CompressedPublicKeyLength = 33;
        private const int UncompressedPublicKeyLength = 65;

        public static byte[] Compress(this byte[] public_key)
        {
            if (public_key.Length != UncompressedPublicKeyLength)
                throw new FormatException($"{nameof(Compress)} argument isn't uncompressed public key. expected length={UncompressedPublicKeyLength}, actual={public_key.Length}");
            var secp256r1 = SecNamedCurves.GetByName("secp256r1");
            var point = secp256r1.Curve.DecodePoint(public_key);
            return point.GetEncoded(true);
        }

        public static byte[] Decompress(this byte[] public_key)
        {
            if (public_key.Length != CompressedPublicKeyLength)
                throw new FormatException($"{nameof(Decompress)} argument isn't compressed public key. expected length={CompressedPublicKeyLength}, actual={public_key.Length}");
            var secp256r1 = SecNamedCurves.GetByName("secp256r1");
            var point = secp256r1.Curve.DecodePoint(public_key);
            return point.GetEncoded(false);
        }

        public static byte[] CreateSignatureRedeemScript_obsolete(this byte[] public_key)
        {
            if (public_key.Length != CompressedPublicKeyLength)
                throw new FormatException($"{nameof(CreateSignatureRedeemScript)} argument isn't compressed public key. expected length={CompressedPublicKeyLength}, actual={public_key.Length}");
            var script = new byte[] { 0x0c, (byte)CompressedPublicKeyLength }; //PUSHDATA1 33
            script = Concat(script, public_key);
            script = Concat(script, new byte[] { 0x0b }); //PUSHNULL
            script = Concat(script, new byte[] { 0x41 }); //SYSCALL
            script = Concat(script, BitConverter.GetBytes(2014135445u)); //Neo_Crypto_CheckSig
            return script;
        }

        public static byte[] CreateSignatureRedeemScript(this byte[] public_key)
        {
            if (public_key.Length != CompressedPublicKeyLength)
                throw new FormatException($"{nameof(CreateSignatureRedeemScript)} argument isn't compressed public key. expected length={CompressedPublicKeyLength}, actual={public_key.Length}");
            var script = new byte[] { 0x0c, (byte)CompressedPublicKeyLength }; //PUSHDATA1 33
            script = Concat(script, public_key);
            script = Concat(script, new byte[] { 0x41 }); //SYSCALL
            script = Concat(script, BitConverter.GetBytes(2859889780u)); //Neo_Crypto_CheckSig
            return script;
        }

        public static byte[] ToScriptHash(this byte[] script)
        {
            return script.Sha256().RIPEMD160();
        }

        public static string ToAddress(this byte[] script_hash, byte version)
        {
            Span<byte> data = stackalloc byte[21];
            data[0] = version;
            script_hash.CopyTo(data[1..]);
            return Base58.Base58CheckEncode(data);
        }

        private static byte[] GetPrivateKeyFromWIF(string wif)
        {
            if (wif == null) throw new ArgumentNullException();
            byte[] data = wif.Base58CheckDecode();
            if (data.Length != 34 || data[0] != 0x80 || data[33] != 0x01)
                throw new FormatException();
            byte[] privateKey = new byte[32];
            Buffer.BlockCopy(data, 1, privateKey, 0, privateKey.Length);
            Array.Clear(data, 0, data.Length);
            return privateKey;
        }

        public static string ToAddress(this ECDsa key)
        {
            return key.PublicKey().PublicKeyToAddress();
        }

        public static string ToAddress_obsolete(this ECDsa key)
        {
            return key.PublicKey().PublicKeyToAddress_obsolete();
        }

        public static OwnerID ToOwnerID(this ECDsa key)
        {
            return key.PublicKey().PublicKeyToOwnerID();
        }

        public static OwnerID ToOwnerID_obsolete(this ECDsa key)
        {
            return key.PublicKey().PublicKeyToOwnerID_obsolete();
        }

        public static string OwnerIDToAddress(this OwnerID owner)
        {
            return Base58.Encode(owner.Value.ToByteArray());
        }

        public static OwnerID AddressToOwnerID(this string address)
        {
            var bytes = Base58.Decode(address);
            return new OwnerID
            {
                Value = ByteString.CopyFrom(bytes),
            };
        }

        public static string PublicKeyToAddress(this byte[] public_key)
        {
            if (public_key.Length != CompressedPublicKeyLength)
                throw new FormatException(nameof(public_key) + $" isn't encoded compressed public key. expected length={CompressedPublicKeyLength}, actual={public_key.Length}");
            return public_key.CreateSignatureRedeemScript().ToScriptHash().ToAddress(NeoAddressVersion);
        }

        public static string PublicKeyToAddress_obsolete(this byte[] public_key)
        {
            if (public_key.Length != CompressedPublicKeyLength)
                throw new FormatException(nameof(public_key) + $" isn't encoded compressed public key. expected length={CompressedPublicKeyLength}, actual={public_key.Length}");
            return public_key.CreateSignatureRedeemScript_obsolete().ToScriptHash().ToAddress(NeoAddressVersion);
        }

        public static OwnerID PublicKeyToOwnerID(this byte[] public_key)
        {
            var bytes = Base58.Decode(public_key.PublicKeyToAddress());
            return new OwnerID
            {
                Value = ByteString.CopyFrom(bytes),
            };
        }

        public static OwnerID PublicKeyToOwnerID_obsolete(this byte[] public_key)
        {
            var bytes = Base58.Decode(public_key.PublicKeyToAddress_obsolete());
            return new OwnerID
            {
                Value = ByteString.CopyFrom(bytes),
            };
        }

        public static byte[] PublicKey(this ECDsa key)
        {
            var param = key.ExportParameters(false);
            var pubkey = new byte[33];
            var pos = 33 - param.Q.X.Length;

            param.Q.X.CopyTo(pubkey, pos);
            if (new System.Numerics.BigInteger(param.Q.Y.Reverse().Concat(new byte[] { 0x00 }).ToArray()).IsEven)
            {
                pubkey[0] = 0x2;
            }
            else
            {
                pubkey[0] = 0x3;
            }

            return pubkey;
        }

        public static byte[] PrivateKey(this ECDsa key)
        {
            return key.ExportParameters(true).D;
        }

        public static ECDsa LoadPrivateKey(this byte[] private_key)
        {
            var secp256r1 = SecNamedCurves.GetByName("secp256r1");
            var public_key = secp256r1.G.Multiply(new BigInteger(1, private_key)).GetEncoded(false)[1..];
            var key = ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                D = private_key,
                Q = new ECPoint
                {
                    X = public_key[..32],
                    Y = public_key[32..]
                }
            });
            return key;
        }

        public static ECDsa LoadWif(this string wif)
        {
            var private_key = GetPrivateKeyFromWIF(wif);
            return LoadPrivateKey(private_key);
        }

        public static ECDsa LoadPublicKey(this byte[] public_key)
        {
            var public_key_full = public_key.Decompress()[1..];
            var key = ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                Q = new ECPoint
                {
                    X = public_key_full[..32],
                    Y = public_key_full[32..]
                }
            });
            return key;
        }
    }
}
