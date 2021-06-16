namespace SecureVariables
{
	using System;
	using System.Globalization;
	using UnityEngine;

	[Serializable]
	public struct SecureVector2 : IEquatable<Vector2>, IEquatable<SecureVector2>
	{
		public Vector2 Value => GetValue(_data, _key);

		[SerializeField] private long _data;
		[SerializeField] private long _key;

		private SecureVector2(Vector2 value)
		{
			GetDataAndKey(value, out _data, out _key);
		}
		
		public SecureVector2(float x, float y)
		{
			GetDataAndKey(x, y, out _data, out _key);
		}
		
		public float x 
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
		
		public float y 
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
		
		public float this[int index]
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
		
		public void Set(float x, float y)
		{
			GetDataAndKey(x, y, out _data, out _key);
		}

		public static Vector2 GetValue(long data, long key)
		{
			SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
			return dataUnion.Vector2;
		}

		public static void GetDataAndKey(Vector2 value, out long data, out long key)
		{
			var dataUnion = new Bytes8Union { Vector2 = value };
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
		}

		public static void GetDataAndKey(float x, float y, out long data, out long key)
		{
			var dataUnion = new Bytes8Union { Vector2 = new Vector2(x, y) };
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
		}

		public static implicit operator SecureVector2(Vector2 value)
		{
			return new SecureVector2(value);
		}

		public static implicit operator Vector2(SecureVector2 secureVector2)
		{
			return secureVector2.Value;
		}
        
		public static bool operator ==(SecureVector2 a, SecureVector2 b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(SecureVector2 a, SecureVector2 b)
		{
			return a.Value != b.Value;
		}
        
		public static bool operator ==(SecureVector2 a, Vector2 b)
		{
			return a.Value == b;
		}

		public static bool operator !=(SecureVector2 a, Vector2 b)
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

		public bool Equals(Vector2 other)
		{
			return Value.Equals(other);
		}

		public bool Equals(SecureVector2 other)
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
		
		public void Scale(Vector2 scale)
		{
			var newValue = Value;
			newValue.Scale(scale);
			
			Set(newValue);
		}
		
		public void Normalize()
		{
			var newValue = Value;
			newValue.Normalize();
			
			Set(newValue);
		}
		
		public Vector2 normalized
		{
			get
			{
				return Value.normalized;
			}
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
		
		public float SqrMagnitude()
		{
			return Value.SqrMagnitude();
		}

		public static string GetInMemoryValue(long data)
		{
			return data.ToString();
		}

		public string GetInMemoryValue()
		{
			return GetInMemoryValue(_data);
		}
		
		private void Set(Vector2 value)
		{
			GetDataAndKey(value, out _data, out _key);
		}
	}
}