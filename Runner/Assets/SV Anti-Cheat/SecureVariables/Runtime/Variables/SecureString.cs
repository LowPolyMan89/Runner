namespace SecureVariables
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using UnityEngine;

    [Serializable]
    public struct SecureString : IComparable, ICloneable, IEnumerable, IEnumerable<char>, IComparable<string>, IEquatable<string>, IComparable<SecureString>, IEquatable<SecureString>
    {
        public string Value => GetValue(_data, _key);

        [SerializeField] private string _data;
        [SerializeField] private string _key;

        private SecureString(string value)
        {
            GetDataAndKey(value, out _data, out _key);
        }

        public static string GetValue(string data, string key)
        {
            return SecureVariablesManager.Cryptographer.Decrypt(data, key);
        }

        public static void GetDataAndKey(string value, out string data, out string key)
        {
            SecureVariablesManager.Cryptographer.Encrypt(value, out data, out key);
        }

        public static implicit operator SecureString(string value)
        {
            return new SecureString(value);
        }

        public static implicit operator string(SecureString secureInt)
        {
            return secureInt.Value;
        }
        
	    public static bool operator ==(SecureString a, SecureString b)
	    {
		    return a.Value == b.Value;
	    }

	    public static bool operator !=(SecureString a, SecureString b)
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

        public int CompareTo(string other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(SecureString other)
        {
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(string other)
	    {
		    return Value.Equals(other);
	    }

	    public bool Equals(SecureString other)
	    {
		    return Value.Equals(other.Value);
	    }

        public override string ToString()
        {
            return Value;
        }

        public IEnumerator GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public object Clone()
        {
            return Value.Clone();
        }

        public string ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        public static string GetInMemoryValue(string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        public string GetInMemoryValue()
        {
            return GetInMemoryValue(_data);
        }
    }
}
