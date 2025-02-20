﻿using Google.Protobuf;
using System;

namespace Neo.FileStorage.API.Cryptography
{
    public static class UUIDExtension
    {

        public static Guid ToUUID(this ByteString id)
        {
            return Guid.Parse(id.ToByteArray().ToHexString());
        }

        public static ByteString ToByteString(this Guid id)
        {
            return ByteString.CopyFrom(id.Bytes());
        }

        public static byte[] Bytes(this Guid id)
        {
            return id.ToString().Replace("-", String.Empty).HexToBytes();
        }
    }
}
