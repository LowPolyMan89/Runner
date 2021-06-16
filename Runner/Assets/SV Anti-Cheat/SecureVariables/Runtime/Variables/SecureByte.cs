namespace SecureVariables
{
    using System;
    using System.Globalization;
    using UnityEngine;

    [Serializable]
    public struct SecureByte : IComparable, IFormattable, IComparable<byte>, IEquatable<byte>, IComparable<SecureByte>, IEquatable<SecureByte>
    {
        public const byte MaxValue = byte.MaxValue;
        public const byte MinValue = byte.MinValue;
        
        public byte Value => GetValue(_data, _key);

        [SerializeField] private int _data;
        [SerializeField] private int _key;

        private SecureByte(byte value)
        {
            GetDataAndKey(value, out _data, out _key);
        }

        public static byte GetValue(int data, int key)
        {
            SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
            return dataUnion.Byte;
        }

        public static void GetDataAndKey(byte value, out int data, out int key)
        {
            var dataUnion = new Bytes4Union { Byte = value };
            SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
        }

        public static implicit operator SecureByte(byte value)
        {
            return new SecureByte(value);
        }

        public static implicit operator byte(SecureByte secureByte)
        {
            return secureByte.Value;
        }

        public static implicit operator long(SecureByte secureByte)
        {
            return secureByte.Value;
        }

        public static implicit operator float(SecureByte secureByte)
        {
            return secureByte.Value;
        }

        public static implicit operator SecureFloat(SecureByte secureByte)
        {
            return secureByte.Value;
        }

        public static implicit operator SecureLong(SecureByte secureByte)
        {
            return secureByte.Value;
        }

        public static implicit operator SecureDouble(SecureByte secureByte)
        {
            return secureByte.Value;
        }
        
	    public static bool operator ==(SecureByte a, SecureByte b)
	    {
		    return a.Value == b.Value;
	    }

	    public static bool operator !=(SecureByte a, SecureByte b)
	    {
		    return a.Value != b.Value;
	    }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(byte other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(SecureByte other)
        {
            return Value.CompareTo(other.Value);
        }

	    public override bool Equals(object other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(byte other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(SecureByte other)
	    {
		    return Value.Equals(other.Value);
	    }

        public override string ToString()
        {
            return Value.ToString();
        }

        public string ToString(string format)
        {
            return Value.ToString(format);
        }

        public string ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return Value.ToString(format, provider);
        }

        public static bool TryParse(string s, out SecureByte result)
        {
            var isSuccess = byte.TryParse(s, out var temp);
            result = temp;
            
            return isSuccess;
        }

        public static bool TryParse(
            string s,
            NumberStyles style,
            IFormatProvider provider,
            out byte result)
        {
            var isSuccess = byte.TryParse(s, style, provider, out var temp);
            result = temp;
            
            return isSuccess;
        }

        public static string GetInMemoryValue(int data)
        {
            return data.ToString();
        }

        public string GetInMemoryValue()
        {
            return GetInMemoryValue(_data);
        }
    }
}
