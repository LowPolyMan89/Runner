namespace SecureVariables
{
	using System;
	using System.Globalization;
	using UnityEngine;

	[Serializable]
	public struct SecureVector2Int : IEquatable<Vector2Int>, IEquatable<SecureVector2Int>
	{
		public Vector2Int Value => GetValue(_data, _key);

		[SerializeField] private long _data;
		[SerializeField] private long _key;

		private SecureVector2Int(Vector2Int value)
		{
			GetDataAndKey(value, out _data, out _key);
		}
		
		public SecureVector2Int(int x, int y)
		{
			GetDataAndKey(x, y, out _data, out _key);
		}
		
		public int x 
		{ 
			get 
			{
				return Value.x;
			} 
			set
			{
				var currentValue = Value;
				Set(value, currentValue.y);
			}
		}
		
		public int y 
		{ 
			get 
			{
				return Value.y;
			} 
			set
			{
				var currentValue = Value;
				Set(currentValue.x, value);
			}
		}
		
		public int this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return Value.x;
					case 1: return Value.y;
					default:
						throw new IndexOutOfRangeException(string.Format("Invalid Vector3Int index addressed: {0}!", index));
				}
			}
			
			set
			{
				switch (index)
				{
					case 0: Set(value, Value.y); break;
					case 1: Set(Value.x, value); break;
					default:
						throw new IndexOutOfRangeException(string.Format("Invalid Vector3Int index addressed: {0}!", index));
				}
			}
		}
		
		public void Set(int x, int y)
		{
			GetDataAndKey(x, y, out _data, out _key);
		}

		public static Vector2Int GetValue(long data, long key)
		{
			SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
			return dataUnion.Vector2Int;
		}

		public static void GetDataAndKey(Vector2Int value, out long data, out long key)
		{
			var dataUnion = new Bytes8Union { Vector2Int = value };
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
		}

		public static void GetDataAndKey(int x, int y, out long data, out long key)
		{
			var dataUnion = new Bytes8Union { Vector2Int = new Vector2Int(x, y) };
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
		}

		public static implicit operator SecureVector2Int(Vector2Int value)
		{
			return new SecureVector2Int(value);
		}

		public static implicit operator Vector2Int(SecureVector2Int secureVector2Int)
		{
			return secureVector2Int.Value;
		}
        
		public static bool operator ==(SecureVector2Int a, SecureVector2Int b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(SecureVector2Int a, SecureVector2Int b)
		{
			return a.Value != b.Value;
		}
        
		public static bool operator ==(SecureVector2Int a, Vector2Int b)
		{
			return a.Value == b;
		}

		public static bool operator !=(SecureVector2Int a, Vector2Int b)
		{
			return a.Value != b;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override bool Equals(object other)
		{
			return Value.Equals(other);
		}

		public bool Equals(Vector2Int other)
		{
			return Value.Equals(other);
		}

		public bool Equals(SecureVector2Int other)
		{
			return Value.Equals(other.Value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}
		
		public void Scale(Vector2Int scale)
		{
			var newValue = Value;
			newValue.Scale(scale);
			
			Set(newValue);
		}
		
		public float magnitude
		{
			get
			{ 
				return Value.magnitude;
			}
		}
		
		public float sqrMagnitude
		{
			get
			{ 
				return Value.sqrMagnitude;
			}
		}

		public static string GetInMemoryValue(long data)
		{
			return data.ToString();
		}

		public string GetInMemoryValue()
		{
			return GetInMemoryValue(_data);
		}
		
		private void Set(Vector2Int value)
		{
			GetDataAndKey(value, out _data, out _key);
		}
	}
}