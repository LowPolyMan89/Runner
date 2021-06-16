namespace SecureVariables
{
	using System;
	using System.Globalization;
	using UnityEngine;

	[Serializable]
	public struct SecureSByte : IComparable, IFormattable, IComparable<sbyte>, IEquatable<sbyte>, IComparable<SecureSByte>, IEquatable<SecureSByte>
	{
		public const sbyte MaxValue = sbyte.MaxValue;
		public const sbyte MinValue = sbyte.MinValue;

		public sbyte Value => GetValue(_data, _key);

		[SerializeField] private int _data;
		[SerializeField] private int _key;

		private SecureSByte(sbyte value)
		{
			GetDataAndKey(value, out _data, out _key);
		}

		public static sbyte GetValue(int data, int key)
		{
			SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
			return dataUnion.SByte;
		}

		public static void GetDataAndKey(sbyte value, out int data, out int key)
		{
			var dataUnion = new Bytes4Union { SByte = value };
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
		}

		public static implicit operator SecureSByte(sbyte value)
		{
			return new SecureSByte(value);
		}

		public static implicit operator sbyte(SecureSByte secureSByte)
		{
			return secureSByte.Value;
		}

		public static implicit operator int(SecureSByte secureSByte)
		{
			return secureSByte.Value;
		}

		public static implicit operator long(SecureSByte secureSByte)
		{
			return secureSByte.Value;
		}

		public static implicit operator float(SecureSByte secureSByte)
		{
			return secureSByte.Value;
		}

		public static implicit operator SecureFloat(SecureSByte secureSByte)
		{
			return secureSByte.Value;
		}

		public static implicit operator SecureLong(SecureSByte secureSByte)
		{
			return secureSByte.Value;
		}

		public static implicit operator SecureDouble(SecureSByte secureSByte)
		{
			return secureSByte.Value;
		}

		public static implicit operator SecureInt(SecureSByte secureSByte)
		{
			return secureSByte.Value;
		}
        
		public static bool operator ==(SecureSByte a, SecureSByte b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(SecureSByte a, SecureSByte b)
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

		public int CompareTo(sbyte other)
		{
			return Value.CompareTo(other);
		}

		public int CompareTo(SecureSByte other)
		{
			return Value.CompareTo(other.Value);
		}

		public override bool Equals(object other)
		{
			return Value.Equals(other);
		}

		public bool Equals(sbyte other)
		{
			return Value.Equals(other);
		}

		public bool Equals(SecureSByte other)
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

		public static bool TryParse(string s, out SecureSByte result)
		{
			var isSuccess = sbyte.TryParse(s, out var temp);
			result = temp;
            
			return isSuccess;
		}

		public static bool TryParse(
			string s,
			NumberStyles style,
			IFormatProvider provider,
			out sbyte result)
		{
			var isSuccess = sbyte.TryParse(s, style, provider, out var temp);
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
