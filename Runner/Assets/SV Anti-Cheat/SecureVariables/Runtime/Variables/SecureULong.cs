namespace SecureVariables
{
	using System;
	using System.Globalization;
	using UnityEngine;

	[Serializable]
	public struct SecureULong : IComparable, IFormattable, IComparable<ulong>, IEquatable<ulong>, IComparable<SecureULong>, IEquatable<SecureULong>
	{
		public const ulong MaxValue = ulong.MaxValue;
		public const ulong MinValue = ulong.MinValue;

		public ulong Value => GetValue(_data, _key);

		[SerializeField] private long _data;
		[SerializeField] private long _key;


		private SecureULong(ulong value)
		{
			GetDataAndKey(value, out _data, out _key);
		}

		public static ulong GetValue(long data, long key)
		{
			SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
			return dataUnion.ULong;
		}

		public static void GetDataAndKey(ulong value, out long data, out long key)
		{
			var dataUnion = new Bytes8Union { ULong = value };
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
		}

		public static implicit operator SecureULong(ulong value)
		{
			return new SecureULong(value);
		}

		public static implicit operator ulong(SecureULong secureULong)
		{
			return secureULong.Value;
		}

		public static implicit operator SecureFloat(SecureULong secureULong)
		{
			return secureULong.Value;
		}

		public static implicit operator SecureDouble(SecureULong secureULong)
		{
			return secureULong.Value;
		}
        
		public static bool operator ==(SecureULong a, SecureULong b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(SecureULong a, SecureULong b)
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

		public int CompareTo(ulong other)
		{
			return Value.CompareTo(other);
		}

		public int CompareTo(SecureULong other)
		{
			return Value.CompareTo(other.Value);
		}

		public override bool Equals(object other)
		{
			return Value.Equals(other);
		}

		public bool Equals(ulong other)
		{
			return Value.Equals(other);
		}

		public bool Equals(SecureULong other)
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

		public static bool TryParse(string s, out SecureULong result)
		{
			var isSuccess = ulong.TryParse(s, out var temp);
			result = temp;
            
			return isSuccess;
		}

		public static bool TryParse(
			string s,
			NumberStyles style,
			IFormatProvider provider,
			out ulong result)
		{
			var isSuccess = ulong.TryParse(s, style, provider, out var temp);
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