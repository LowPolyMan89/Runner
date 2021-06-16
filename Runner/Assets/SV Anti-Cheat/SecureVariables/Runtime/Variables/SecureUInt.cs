namespace SecureVariables
{
	using System;
	using System.Globalization;
	using UnityEngine;

	[Serializable]
	public struct SecureUInt : IComparable, IFormattable, IComparable<uint>, IEquatable<uint>, IComparable<SecureUInt>, IEquatable<SecureUInt>
	{
		public const uint MaxValue = uint.MaxValue;
		public const uint MinValue = uint.MinValue;

		public uint Value => GetValue(_data, _key);

		[SerializeField] private int _data;
		[SerializeField] private int _key;

		private SecureUInt(uint value)
		{
			GetDataAndKey(value, out _data, out _key);
		}

		public static uint GetValue(int data, int key)
		{
			SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
			return dataUnion.UInt;
		}

		public static void GetDataAndKey(uint value, out int data, out int key)
		{
			var dataUnion = new Bytes4Union { UInt = value };
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
		}

		public static implicit operator SecureUInt(uint value)
		{
			return new SecureUInt(value);
		}

		public static implicit operator uint(SecureUInt secureUInt)
		{
			return secureUInt.Value;
		}

		public static implicit operator long(SecureUInt secureUInt)
		{
			return secureUInt.Value;
		}

		public static implicit operator float(SecureUInt secureUInt)
		{
			return secureUInt.Value;
		}

		public static implicit operator SecureFloat(SecureUInt secureUInt)
		{
			return secureUInt.Value;
		}

		public static implicit operator SecureLong(SecureUInt secureUInt)
		{
			return secureUInt.Value;
		}

		public static implicit operator SecureDouble(SecureUInt secureUInt)
		{
			return secureUInt.Value;
		}
        
		public static bool operator ==(SecureUInt a, SecureUInt b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(SecureUInt a, SecureUInt b)
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

		public int CompareTo(uint other)
		{
			return Value.CompareTo(other);
		}

		public int CompareTo(SecureUInt other)
		{
			return Value.CompareTo(other.Value);
		}

		public override bool Equals(object other)
		{
			return Value.Equals(other);
		}

		public bool Equals(uint other)
		{
			return Value.Equals(other);
		}

		public bool Equals(SecureUInt other)
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

		public static bool TryParse(string s, out SecureUInt result)
		{
			var isSuccess = uint.TryParse(s, out var temp);
			result = temp;
            
			return isSuccess;
		}

		public static bool TryParse(
			string s,
			NumberStyles style,
			IFormatProvider provider,
			out uint result)
		{
			var isSuccess = uint.TryParse(s, style, provider, out var temp);
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
