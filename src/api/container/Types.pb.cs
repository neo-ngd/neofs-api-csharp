// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: container/types.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace NeoFS.API.Container {

  /// <summary>Holder for reflection information generated from container/types.proto</summary>
  public static partial class TypesReflection {

    #region Descriptor
    /// <summary>File descriptor for container/types.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static TypesReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChVjb250YWluZXIvdHlwZXMucHJvdG8SCWNvbnRhaW5lchoqZ2l0aHViLmNv",
            "bS9uc3BjYy1kZXYvbmV0bWFwL3NlbGVjdG9yLnByb3RvGi1naXRodWIuY29t",
            "L2dvZ28vcHJvdG9idWYvZ29nb3Byb3RvL2dvZ28ucHJvdG8imQEKCUNvbnRh",
            "aW5lchIgCgdPd25lcklEGAEgASgMQg/a3h8HT3duZXJJRMjeHwASGgoEU2Fs",
            "dBgCIAEoDEIM2t4fBFVVSUTI3h8AEhAKCENhcGFjaXR5GAMgASgEEioKBVJ1",
            "bGVzGAQgASgLMhUubmV0bWFwLlBsYWNlbWVudFJ1bGVCBMjeHwASEAoIQmFz",
            "aWNBQ0wYBSABKA1CR1orZ2l0aHViLmNvbS9uc3BjYy1kZXYvbmVvZnMtYXBp",
            "LWdvL2NvbnRhaW5lcqoCE05lb0ZTLkFQSS5Db250YWluZXLY4h4BYgZwcm90",
            "bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Netmap.SelectorReflection.Descriptor, global::Gogoproto.GogoReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::NeoFS.API.Container.Container), global::NeoFS.API.Container.Container.Parser, new[]{ "OwnerID", "Salt", "Capacity", "Rules", "BasicACL" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// The Container service definition.
  /// </summary>
  public sealed partial class Container : pb::IMessage<Container> {
    private static readonly pb::MessageParser<Container> _parser = new pb::MessageParser<Container>(() => new Container());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Container> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::NeoFS.API.Container.TypesReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Container() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Container(Container other) : this() {
      ownerID_ = other.ownerID_;
      salt_ = other.salt_;
      capacity_ = other.capacity_;
      rules_ = other.rules_ != null ? other.rules_.Clone() : null;
      basicACL_ = other.basicACL_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Container Clone() {
      return new Container(this);
    }

    /// <summary>Field number for the "OwnerID" field.</summary>
    public const int OwnerIDFieldNumber = 1;
    private pb::ByteString ownerID_ = pb::ByteString.Empty;
    /// <summary>
    /// OwnerID is a wallet address.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString OwnerID {
      get { return ownerID_; }
      set {
        ownerID_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Salt" field.</summary>
    public const int SaltFieldNumber = 2;
    private pb::ByteString salt_ = pb::ByteString.Empty;
    /// <summary>
    /// Salt is a nonce for unique container id calculation.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Salt {
      get { return salt_; }
      set {
        salt_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Capacity" field.</summary>
    public const int CapacityFieldNumber = 3;
    private ulong capacity_;
    /// <summary>
    /// Capacity defines amount of data that can be stored in the container (doesn't used for now).
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong Capacity {
      get { return capacity_; }
      set {
        capacity_ = value;
      }
    }

    /// <summary>Field number for the "Rules" field.</summary>
    public const int RulesFieldNumber = 4;
    private global::Netmap.PlacementRule rules_;
    /// <summary>
    /// Rules define storage policy for the object inside the container.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Netmap.PlacementRule Rules {
      get { return rules_; }
      set {
        rules_ = value;
      }
    }

    /// <summary>Field number for the "BasicACL" field.</summary>
    public const int BasicACLFieldNumber = 5;
    private uint basicACL_;
    /// <summary>
    /// BasicACL with access control rules for owner, system, others and
    /// permission bits for bearer token and extended ACL.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint BasicACL {
      get { return basicACL_; }
      set {
        basicACL_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Container);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Container other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (OwnerID != other.OwnerID) return false;
      if (Salt != other.Salt) return false;
      if (Capacity != other.Capacity) return false;
      if (!object.Equals(Rules, other.Rules)) return false;
      if (BasicACL != other.BasicACL) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (OwnerID.Length != 0) hash ^= OwnerID.GetHashCode();
      if (Salt.Length != 0) hash ^= Salt.GetHashCode();
      if (Capacity != 0UL) hash ^= Capacity.GetHashCode();
      if (rules_ != null) hash ^= Rules.GetHashCode();
      if (BasicACL != 0) hash ^= BasicACL.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (OwnerID.Length != 0) {
        output.WriteRawTag(10);
        output.WriteBytes(OwnerID);
      }
      if (Salt.Length != 0) {
        output.WriteRawTag(18);
        output.WriteBytes(Salt);
      }
      if (Capacity != 0UL) {
        output.WriteRawTag(24);
        output.WriteUInt64(Capacity);
      }
      if (rules_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(Rules);
      }
      if (BasicACL != 0) {
        output.WriteRawTag(40);
        output.WriteUInt32(BasicACL);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (OwnerID.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(OwnerID);
      }
      if (Salt.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Salt);
      }
      if (Capacity != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(Capacity);
      }
      if (rules_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Rules);
      }
      if (BasicACL != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(BasicACL);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Container other) {
      if (other == null) {
        return;
      }
      if (other.OwnerID.Length != 0) {
        OwnerID = other.OwnerID;
      }
      if (other.Salt.Length != 0) {
        Salt = other.Salt;
      }
      if (other.Capacity != 0UL) {
        Capacity = other.Capacity;
      }
      if (other.rules_ != null) {
        if (rules_ == null) {
          Rules = new global::Netmap.PlacementRule();
        }
        Rules.MergeFrom(other.Rules);
      }
      if (other.BasicACL != 0) {
        BasicACL = other.BasicACL;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            OwnerID = input.ReadBytes();
            break;
          }
          case 18: {
            Salt = input.ReadBytes();
            break;
          }
          case 24: {
            Capacity = input.ReadUInt64();
            break;
          }
          case 34: {
            if (rules_ == null) {
              Rules = new global::Netmap.PlacementRule();
            }
            input.ReadMessage(Rules);
            break;
          }
          case 40: {
            BasicACL = input.ReadUInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
