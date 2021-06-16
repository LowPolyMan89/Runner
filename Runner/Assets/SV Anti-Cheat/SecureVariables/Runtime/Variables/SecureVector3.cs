namespace SecureVariables
{
	using System;
	using System.Globalization;
	using UnityEngine;

	[Serializable]
	public struct SecureVector3 : IEquatable<Vector3>, IEquatable<SecureVector3>
	{
		public const float kEpsilon = Vector3.kEpsilon;
		public const float kEpsilonNormalSqrt = Vector3.kEpsilonNormalSqrt;
		
		public Vector3 Value => GetValue(_data, _data2, _data3, _key);

		[SerializeField] private int _data;
		[SerializeField] private int _data2;
		[SerializeField] private int _data3;
		[SerializeField] private int _key;

		private SecureVector3(Vector3 value)
		{
			GetDataAndKey(value, out _data, out _data2, out _data3, out _key);
		}
		
		public SecureVector3(float x, float y, float z)
		{
			GetDataAndKey(x, y, z, out _data, out _data2, out _data3, out _key);
		}
		
		public SecureVector3(float x, float y)
		{
			GetDataAndKey(x, y, 0, out _data, out _data2, out _data3, out _key);
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
				Set(value, currentValue.y, currentValue.z);
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
				Set(currentValue.x, value, currentValue.z);
			}
		}
		
		public float z 
		{ 
			get 
			{
				return Value.z;
			} 
			set
			{
				var currentValue = Value;
				Set(currentValue.x, currentValue.y, value);
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
					case 2: return Value.z;
					default:
						throw new IndexOutOfRangeException(string.Format("Invalid Vector3Int index addressed: {0}!", index));
				}
			}
		
			set
			{
				var currentValue = Value;
				
				switch (index)
				{
					case 0: Set(value, currentValue.y, currentValue.z); break;
					case 1: Set(currentValue.x, value, currentValue.z); break;
					case 2: Set(currentValue.x, currentValue.y, value); break;
					default:
						throw new IndexOutOfRangeException(string.Format("Invalid Vector3Int index addressed: {0}!", index));
				}
			}
		}
		
		public void Set(float x, float y, float z)
		{
			GetDataAndKey(x, y, z, out _data, out _data2, out _data3, out _key);
		}

		public static Vector3 GetValue(int data, int data2, int data3, int key)
		{
			SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
			SecureVariablesManager.Cryptographer.Decrypt(data2, key, out var dataUnion2);
			SecureVariablesManager.Cryptographer.Decrypt(data3, key, out var dataUnion3);
			
			return new Vector3(dataUnion.Float, dataUnion2.Float, dataUnion3.Float);
		}

		public static void GetDataAndKey(Vector3 value, out int data, out int data2, out int data3, out int key)
		{
			GetDataAndKey(value.x, value.y, value.z, out data, out data2, out data3, out key);
		}

		public static void GetDataAndKey(float x, float y, float z, out int data, out int data2, out int data3, out int key)
		{
			var dataUnion = new Bytes4Union { Float = x };
			var dataUnion2 = new Bytes4Union { Float = y };
			var dataUnion3 = new Bytes4Union { Float = z };
			
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion2, out data2, key);
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion3, out data3, key);
		}

		public static implicit operator SecureVector3(Vector3 value)
		{
			return new SecureVector3(value);
		}

		public static implicit operator Vector3(SecureVector3 secureVector3)
		{
			return secureVector3.Value;
		}
        
		public static bool operator ==(SecureVector3 a, SecureVector3 b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(SecureVector3 a, SecureVector3 b)
		{
			return a.Value != b.Value;
		}
        
		public static bool operator ==(SecureVector3 a, Vector3 b)
		{
			return a.Value == b;
		}

		public static bool operator !=(SecureVector3 a, Vector3 b)
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

		public bool Equals(Vector3 other)
		{
			return Value.Equals(other);
		}

		public bool Equals(SecureVector3 other)
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
		
		public void Scale(Vector3 scale)
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
		
		public Vector3 normalized
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

		public static string GetInMemoryValue(int data, int data2, int data3)
		{
			return $"{data}{data2}{data3}";
		}

		public string GetInMemoryValue()
		{
			return GetInMemoryValue(_data, _data2, _data3);
		}
		
		private void Set(Vector3 value)
		{
			GetDataAndKey(value, out _data, out _data2, out _data3, out _key);
		}
	}
}