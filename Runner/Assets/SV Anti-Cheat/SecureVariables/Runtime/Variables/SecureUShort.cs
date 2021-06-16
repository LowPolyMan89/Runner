namespace SecureVariables
{
	using System;
	using System.Globalization;
	using UnityEngine;

	[Serializable]
	public struct SecureUShort : IComparable, IFormattable, IComparable<ushort>, IEquatable<ushort>, IComparable<SecureUShort>, IEquatable<SecureUShort>
	{
		public const ushort MaxValue = ushort.MaxValue;
		public const ushort MinValue = ushort.MinValue;

		public ushort Value => GetValue(_data, _key);

		[SerializeField] private int _data;
		[SerializeField] private int _key;

		private SecureUShort(ushort value)
		{
			GetDataAndKey(value, out _data, out _key);
		}

		public static ushort GetValue(int data, int key)
		{
			SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
			return dataUnion.UShort;
		}

		public static void GetDataAndKey(ushort value, out int data, out int key)
		{
			var dataUnion = new Bytes4Union { UShort = value };
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
		}

		public static implicit operator SecureUShort(ushort value)
		{
			return new SecureUShort(value);
		}

		public static implicit operator ushort(SecureUShort secureUShort)
		{
			return secureUShort.Value;
		}

		public static implicit operator int(SecureUShort secureUShort)
		{
			return secureUShort.Value;
		}

		public static implicit operator long(SecureUShort secureUShort)
		{
			return secureUShort.Value;
		}

		public static implicit operator float(SecureUShort secureUShort)
		{
			return secureUShort.Value;
		}

		public static implicit operator SecureFloat(SecureUShort secureUShort)
		{
			return secureUShort.Value;
		}

		public static implicit operator SecureLong(SecureUShort secureUShort)
		{
			return secureUShort.Value;
		}

		public static implicit operator SecureDouble(SecureUShort secureUShort)
		{
			return secureUShort.Value;
		}
        
		public static bool operator ==(SecureUShort a, SecureUShort b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(SecureUShort a, SecureUShort b)
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

		public int CompareTo(ushort other)
		{
			return Value.CompareTo(other);
		}

		public int CompareTo(SecureUShort other)
		{
			return Value.CompareTo(other.Value);
		}

		public override bool Equals(object other)
		{
			return Value.Equals(other);
		}

		public bool Equals(ushort other)
		{
			return Value.Equals(other);
		}

		public bool Equals(SecureUShort other)
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

		public static bool TryParse(string s, out SecureUShort result)
		{
			var isSuccess = ushort.TryParse(s, out var temp);
			result = temp;
            
			return isSuccess;
		}

		public static bool TryParse(
			string s,
			NumberStyles style,
			IFormatProvider provider,
			out ushort result)
		{
			var isSuccess = ushort.TryParse(s, style, provider, out var temp);
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
