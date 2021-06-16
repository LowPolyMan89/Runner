namespace SecureVariables
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct SecureBool : IComparable, IComparable<bool>, IEquatable<bool>, IComparable<SecureBool>, IEquatable<SecureBool>
    {
        public bool Value => GetValue(_data, _key);

        [SerializeField] private int _data;
        [SerializeField] private int _key;

        private SecureBool(bool value)
        {
            GetDataAndKey(value, out _data, out _key);
        }

        public static bool GetValue(int data, int key)
        {
            SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
            return dataUnion.Boolean;
        }

        public static void GetDataAndKey(bool value, out int data, out int key)
        {
            var dataUnion = new Bytes4Union { Boolean = value };
            SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
        }

        public static implicit operator SecureBool(bool value)
        {
            return new SecureBool(value);
        }

        public static implicit operator bool(SecureBool secureBool)
        {
            return secureBool.Value;
        }
        
	    public static bool operator ==(SecureBool a, SecureBool b)
	    {
		    return a.Value == b.Value;
	    }

	    public static bool operator !=(SecureBool a, SecureBool b)
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

        public int CompareTo(bool other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(SecureBool other)
        {
            return Value.CompareTo(other.Value);
        }

	    public override bool Equals(object other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(bool other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(SecureBool other)
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

        public static bool TryParse(string value, out SecureBool result)
        {
            var isSuccess = bool.TryParse(value, out var temp);
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
