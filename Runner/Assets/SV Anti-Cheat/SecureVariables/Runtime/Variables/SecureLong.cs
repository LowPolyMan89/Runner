namespace SecureVariables
{
    using System;
    using System.Globalization;
    using UnityEngine;

    [Serializable]
    public struct SecureLong : IComparable, IFormattable, IComparable<long>, IEquatable<long>, IComparable<SecureLong>, IEquatable<SecureLong>
    {
        public const long MaxValue = long.MaxValue;
        public const long MinValue = long.MinValue;

        public long Value => GetValue(_data, _key);

        [SerializeField] private long _data;
        [SerializeField] private long _key;
        
        private SecureLong(long value)
        {
            GetDataAndKey(value, out _data, out _key);
        }

        public static long GetValue(long data, long key)
        {
            SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
            return dataUnion.Long;
        }

        public static void GetDataAndKey(long value, out long data, out long key)
        {
            var dataUnion = new Bytes8Union { Long = value };
            SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
        }

        public static implicit operator SecureLong(long value)
        {
            return new SecureLong(value);
        }

        public static implicit operator long(SecureLong secureLong)
        {
            return secureLong.Value;
        }

        public static implicit operator SecureFloat(SecureLong secureLong)
        {
            return secureLong.Value;
        }

        public static implicit operator SecureDouble(SecureLong secureLong)
        {
            return secureLong.Value;
        }
        
	    public static bool operator ==(SecureLong a, SecureLong b)
	    {
		    return a.Value == b.Value;
	    }

	    public static bool operator !=(SecureLong a, SecureLong b)
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

        public int CompareTo(long other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(SecureLong other)
        {
            return Value.CompareTo(other.Value);
        }

	    public override bool Equals(object other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(long other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(SecureLong other)
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

        public static bool TryParse(string s, out SecureLong result)
        {
            var isSuccess = long.TryParse(s, out var temp);
            result = temp;
            
            return isSuccess;
        }

        public static bool TryParse(
            string s,
            NumberStyles style,
            IFormatProvider provider,
            out long result)
        {
            var isSuccess = long.TryParse(s, style, provider, out var temp);
            result = temp;
            
            return isSuccess;
        }

        public static string GetInMemoryValue(long data)
        {
            return data.ToString();
        }

        public string GetInMemoryValue()
        {
            return GetInMemoryValue(_data);
        }
    }
}