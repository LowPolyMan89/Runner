namespace SecureVariables
{
    using System;
    using System.Globalization;
    using UnityEngine;

    [Serializable]
    public struct SecureDouble : IComparable, IFormattable, IComparable<double>, IEquatable<double>, IComparable<SecureDouble>, IEquatable<SecureDouble>
    {
        public const double MaxValue = double.MaxValue;
        public const double MinValue = double.MinValue;
        
        public double Value => GetValue(_data, _key);

        [SerializeField] private long _data;
        [SerializeField] private long _key;

        private SecureDouble(double value)
        {
            GetDataAndKey(value, out _data, out _key);
        }

        public static double GetValue(long data, long key)
        {
            SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
            return dataUnion.Double;
        }

        public static void GetDataAndKey(double value, out long data, out long key)
        {
            var dataUnion = new Bytes8Union { Double = value };
            SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
        }

        public static implicit operator SecureDouble(double value)
        {
            return new SecureDouble(value);
        }

        public static implicit operator double(SecureDouble secureDouble)
        {
            return secureDouble.Value;
        }
        
	    public static bool operator ==(SecureDouble a, SecureDouble b)
	    {
		    return a.Value == b.Value;
	    }

	    public static bool operator !=(SecureDouble a, SecureDouble b)
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

        public int CompareTo(double other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(SecureDouble other)
        {
            return Value.CompareTo(other.Value);
        }

	    public override bool Equals(object other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(double other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(SecureDouble other)
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

        public static bool TryParse(string s, out SecureDouble result)
        {
            var isSuccess = double.TryParse(s, out var temp);
            result = temp;
            
            return isSuccess;
        }

        public static bool TryParse(
            string s,
            NumberStyles style,
            IFormatProvider provider,
            out SecureDouble result)
        {
            var isSuccess = double.TryParse(s, style, provider, out var temp);
            result = temp;
            
            return isSuccess;
        }

        private static bool TryParse(
            string s,
            NumberStyles style,
            NumberFormatInfo info,
            out SecureDouble result)
        {
            var isSuccess = double.TryParse(s, style, info, out var temp);
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