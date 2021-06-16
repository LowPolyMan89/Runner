namespace SecureVariables
{
    using System;
    using System.Globalization;
    using UnityEngine;

    [Serializable]
    public struct SecureInt : IComparable, IFormattable, IComparable<int>, IEquatable<int>, IComparable<SecureInt>, IEquatable<SecureInt>
    {
        public const int MaxValue = int.MaxValue;
        public const int MinValue = int.MinValue;
        
        public int Value => GetValue(_data, _key);

        [SerializeField] private int _data;
        [SerializeField] private int _key;

        private SecureInt(int value)
        {
            GetDataAndKey(value, out _data, out _key);
        }

        public static int GetValue(int data, int key)
        {
            SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
            return dataUnion.Int;
        }

        public static void GetDataAndKey(int value, out int data, out int key)
        {
            var dataUnion = new Bytes4Union { Int = value };
            SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
        }

        public static implicit operator SecureInt(int value)
        {
            return new SecureInt(value);
        }

        public static implicit operator int(SecureInt secureInt)
        {
            return secureInt.Value;
        }

        public static implicit operator long(SecureInt secureInt)
        {
            return secureInt.Value;
        }

        public static implicit operator float(SecureInt secureInt)
        {
            return secureInt.Value;
        }

        public static implicit operator SecureFloat(SecureInt secureInt)
        {
            return secureInt.Value;
        }

        public static implicit operator SecureLong(SecureInt secureInt)
        {
            return secureInt.Value;
        }

        public static implicit operator SecureDouble(SecureInt secureInt)
        {
            return secureInt.Value;
        }
        
	    public static bool operator ==(SecureInt a, SecureInt b)
	    {
		    return a.Value == b.Value;
	    }

	    public static bool operator !=(SecureInt a, SecureInt b)
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

        public int CompareTo(int other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(SecureInt other)
        {
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(int other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(SecureInt other)
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

        public static bool TryParse(string s, out SecureInt result)
        {
            var isSuccess = int.TryParse(s, out var temp);
            result = temp;
            
            return isSuccess;
        }

        public static bool TryParse(
            string s,
            NumberStyles style,
            IFormatProvider provider,
            out int result)
        {
            var isSuccess = int.TryParse(s, style, provider, out var temp);
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
