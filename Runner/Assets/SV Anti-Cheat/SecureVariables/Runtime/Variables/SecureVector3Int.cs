namespace SecureVariables
{
	using System;
	using System.Globalization;
	using UnityEngine;

	[Serializable]
	public struct SecureVector3Int : IEquatable<Vector3Int>, IEquatable<SecureVector3Int>
	{
		public Vector3Int Value => GetValue(_data, _data2, _data3, _key);

		[SerializeField] private int _data;
		[SerializeField] private int _data2;
		[SerializeField] private int _data3;
		[SerializeField] private int _key;

		private SecureVector3Int(Vector3Int value)
		{
			GetDataAndKey(value, out _data, out _data2, out _data3, out _key);
		}
		
		public SecureVector3Int(int x, int y, int z)
		{
			GetDataAndKey(x, y, z, out _data, out _data2, out _data3, out _key);
		}
		
		public SecureVector3Int(int x, int y)
		{
			GetDataAndKey(x, y, 0, out _data, out _data2, out _data3, out _key);
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
				Set(value, currentValue.y, currentValue.z);
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
				Set(currentValue.x, value, currentValue.z);
			}
		}
		
		public int z 
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
		
		public int this[int index]
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
		
		public void Set(int x, int y, int z)
		{
			GetDataAndKey(x, y, z, out _data, out _data2, out _data3, out _key);
		}

		public static Vector3Int GetValue(int data, int data2, int data3, int key)
		{
			SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
			SecureVariablesManager.Cryptographer.Decrypt(data2, key, out var dataUnion2);
			SecureVariablesManager.Cryptographer.Decrypt(data3, key, out var dataUnion3);
			
			return new Vector3Int(dataUnion.Int, dataUnion2.Int, dataUnion3.Int);
		}

		public static void GetDataAndKey(Vector3Int value, out int data, out int data2, out int data3, out int key)
		{
			GetDataAndKey(value.x, value.y, value.z, out data, out data2, out data3, out key);
		}

		public static void GetDataAndKey(int x, int y, int z, out int data, out int data2, out int data3, out int key)
		{
			var dataUnion = new Bytes4Union { Int = x };
			var dataUnion2 = new Bytes4Union { Int = y };
			var dataUnion3 = new Bytes4Union { Int = z };
			
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion2, out data2, key);
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion3, out data3, key);
		}

		public static implicit operator SecureVector3Int(Vector3Int value)
		{
			return new SecureVector3Int(value);
		}

		public static implicit operator Vector3Int(SecureVector3Int secureVector3Int)
		{
			return secureVector3Int.Value;
		}
        
		public static bool operator ==(SecureVector3Int a, SecureVector3Int b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(SecureVector3Int a, SecureVector3Int b)
		{
			return a.Value != b.Value;
		}
        
		public static bool operator ==(SecureVector3Int a, Vector3Int b)
		{
			return a.Value == b;
		}

		public static bool operator !=(SecureVector3Int a, Vector3Int b)
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

		public bool Equals(Vector3Int other)
		{
			return Value.Equals(other);
		}

		public bool Equals(SecureVector3Int other)
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
		
		public void Scale(Vector3Int scale)
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
		
		public void Clamp(Vector3Int min, Vector3Int max)
		{
			var value = Value;
			
			var x = Math.Max(min.x, value.x);
			x = Math.Min(max.x, x);
			var y = Math.Max(min.y, value.y);
			y = Math.Min(max.y, value.y);
			var z = Math.Max(min.z, value.z);
			z = Math.Min(max.z, value.z);
			
			Set(x, y, z);
		}

		public static string GetInMemoryValue(int data, int data2, int data3)
		{
			return $"{data}{data2}{data3}";
		}

		public string GetInMemoryValue()
		{
			return GetInMemoryValue(_data, _data2, _data3);
		}
		
		private void Set(Vector3Int value)
		{
			GetDataAndKey(value, out _data, out _data2, out _data3, out _key);
		}
	}
}