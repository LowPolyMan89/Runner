namespace SecureVariables
{
	using System;
	using System.Globalization;
	using UnityEngine;

	[Serializable]
	public struct SecureShort : IComparable, IFormattable, IComparable<short>, IEquatable<short>, IComparable<SecureShort>, IEquatable<SecureShort>
	{
		public const short MaxValue = short.MaxValue;
		public const short MinValue = short.MinValue;

		public short Value => GetValue(_data, _key);

		[SerializeField] private int _data;
		[SerializeField] private int _key;

		private SecureShort(short value)
		{
			GetDataAndKey(value, out _data, out _key);
		}

		public static short GetValue(int data, int key)
		{
			SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
			return dataUnion.Short;
		}

		public static void GetDataAndKey(short value, out int data, out int key)
		{
			var dataUnion = new Bytes4Union { Short = value };
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
		}

		public static implicit operator SecureShort(short value)
		{
			return new SecureShort(value);
		}

		public static implicit operator short(SecureShort secureShort)
		{
			return secureShort.Value;
		}

		public static implicit operator int(SecureShort secureShort)
		{
			return secureShort.Value;
		}

		public static implicit operator long(SecureShort secureShort)
		{
			return secureShort.Value;
		}

		public static implicit operator float(SecureShort secureShort)
		{
			return secureShort.Value;
		}

		public static implicit operator SecureFloat(SecureShort secureShort)
		{
			return secureShort.Value;
		}

		public static implicit operator SecureLong(SecureShort secureShort)
		{
			return secureShort.Value;
		}

		public static implicit operator SecureDouble(SecureShort secureShort)
		{
			return secureShort.Value;
		}

		public static implicit operator SecureInt(SecureShort secureShort)
		{
			return secureShort.Value;
		}
        
		public static bool operator ==(SecureShort a, SecureShort b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(SecureShort a, SecureShort b)
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

		public int CompareTo(short other)
		{
			return Value.CompareTo(other);
		}

		public int CompareTo(SecureShort other)
		{
			return Value.CompareTo(other.Value);
		}

		public override bool Equals(object other)
		{
			return Value.Equals(other);
		}

		public bool Equals(short other)
		{
			return Value.Equals(other);
		}

		public bool Equals(SecureShort other)
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

		public static bool TryParse(string s, out SecureShort result)
		{
			var isSuccess = short.TryParse(s, out var temp);
			result = temp;
            
			return isSuccess;
		}

		public static bool TryParse(
			string s,
			NumberStyles style,
			IFormatProvider provider,
			out short result)
		{
			var isSuccess = short.TryParse(s, style, provider, out var temp);
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
