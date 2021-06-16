namespace SecureVariables
{
    using System;
    using System.Globalization;
    using UnityEngine;

    [Serializable]
    public struct SecureFloat : IComparable, IFormattable, IComparable<float>, IEquatable<float>, IComparable<SecureFloat>, IEquatable<SecureFloat>
    {
        public const float MaxValue = float.MaxValue;
        public const float MinValue = float.MinValue;
        
        public float Value => GetValue(_data, _key);

        [SerializeField] private int _data;
        [SerializeField] private int _key;

        private SecureFloat(float value)
        {
            GetDataAndKey(value, out _data, out _key);
        }

        public static float GetValue(int data, int key)
        {
            SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
            return dataUnion.Float;
        }

        public static void GetDataAndKey(float value, out int data, out int key)
        {
            var dataUnion = new Bytes4Union { Float = value };
            SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
        }

        public static implicit operator SecureFloat(float value)
        {
            return new SecureFloat(value);
        }

        public static implicit operator float(SecureFloat secureFloat)
        {
            return secureFloat.Value;
        }

        public static implicit operator SecureDouble(SecureFloat secureFloat)
        {
            return secureFloat.Value;
        }
        
	    public static bool operator ==(SecureFloat a, SecureFloat b)
	    {
		    return a.Value == b.Value;
	    }

	    public static bool operator !=(SecureFloat a, SecureFloat b)
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

        public int CompareTo(float other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(SecureFloat other)
        {
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object other)
	    {
		    if (other is SecureFloat)
		    {
		    	return Equals((SecureFloat)other);;
		    }
		    
		    return Value.Equals(other);
	    }

	    public bool Equals(float other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(SecureFloat other)
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

        public static bool TryParse(string s, out SecureFloat result)
        {
            var isSuccess = float.TryParse(s, out var temp);
            result = temp;
            
            return isSuccess;
        }

        public static bool TryParse(
            string s,
            NumberStyles style,
            IFormatProvider provider,
            out SecureFloat result)
        {
            var isSuccess = float.TryParse(s, style, provider, out var temp);
            result = temp;
            
            return isSuccess;
        }

        private static bool TryParse(
            string s,
            NumberStyles style,
            NumberFormatInfo info,
            out SecureFloat result)
        {
            var isSuccess = float.TryParse(s, style, info, out var temp);
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
