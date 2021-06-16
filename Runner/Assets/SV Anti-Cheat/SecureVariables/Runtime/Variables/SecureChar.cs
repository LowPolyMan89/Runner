namespace SecureVariables
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct SecureChar : IComparable, IComparable<char>, IEquatable<char>, IComparable<SecureChar>, IEquatable<SecureChar>
    {
        public char Value => GetValue(_data, _key);

        [SerializeField] private int _data;
        [SerializeField] private int _key;

        private SecureChar(char value)
        {
            GetDataAndKey(value, out _data, out _key);
        }

        public static char GetValue(int data, int key)
        {
            SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
            return dataUnion.Char;
        }

        public static void GetDataAndKey(char value, out int data, out int key)
        {
            var dataUnion = new Bytes4Union { Char = value };
            SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
        }

        public static implicit operator SecureChar(char value)
        {
            return new SecureChar(value);
        }

	    public static implicit operator char(SecureChar secureChar)
        {
            return secureChar.Value;
        }

        public static implicit operator int(SecureChar secureChar)
        {
            return secureChar.Value;
        }

        public static implicit operator long(SecureChar secureChar)
        {
            return secureChar.Value;
        }

        public static implicit operator float(SecureChar secureChar)
        {
            return secureChar.Value;
        }

        public static implicit operator SecureFloat(SecureChar secureChar)
        {
            return secureChar.Value;
        }

        public static implicit operator SecureLong(SecureChar secureChar)
        {
            return secureChar.Value;
        }

        public static implicit operator SecureDouble(SecureChar secureChar)
        {
            return secureChar.Value;
        }
        
	    public static bool operator ==(SecureChar a, SecureChar b)
	    {
		    return a.Value == b.Value;
	    }

	    public static bool operator !=(SecureChar a, SecureChar b)
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

        public int CompareTo(char other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(SecureChar other)
        {
            return Value.CompareTo(other.Value);
        }

	    public override bool Equals(object other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(char other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(SecureChar other)
	    {
		    return Value.Equals(other.Value);
	    }

        public override string ToString()
        {
            return Value.ToString();
        }

        public string ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        public static bool TryParse(string s, out SecureChar result)
        {
            var isSuccess = char.TryParse(s, out var temp);
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
