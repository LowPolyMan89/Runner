namespace SecureVariables
{
	using System;
	using System.Globalization;
	using UnityEngine;

	[Serializable]
	public struct SecureQuaternion : IEquatable<Quaternion>, IEquatable<SecureQuaternion>
	{
		public const float kEpsilon = Quaternion.kEpsilon;
		
		public Quaternion Value => GetValue(_data, _data2, _data3, _data4, _key);

		[SerializeField] private int _data;
		[SerializeField] private int _data2;
		[SerializeField] private int _data3;
		[SerializeField] private int _data4;
		[SerializeField] private int _key;
		
		private SecureQuaternion(Quaternion value)
		{
			GetDataAndKey(value, out _data, out _data2, out _data3, out _data4, out _key);
		}

		public static Quaternion GetValue(int data, int data2, int data3, int data4, int key)
		{
			SecureVariablesManager.Cryptographer.Decrypt(data, key, out var dataUnion);
			SecureVariablesManager.Cryptographer.Decrypt(data2, key, out var dataUnion2);
			SecureVariablesManager.Cryptographer.Decrypt(data3, key, out var dataUnion3);
			SecureVariablesManager.Cryptographer.Decrypt(data4, key, out var dataUnion4);
			
			return new Quaternion(dataUnion.Float, dataUnion2.Float, dataUnion3.Float, dataUnion4.Float);
		}

		public static void GetDataAndKey(Quaternion value, out int data, out int data2, out int data3, out int data4, out int key)
		{
			GetDataAndKey(value.x, value.y, value.z, value.w, out data, out data2, out data3, out data4, out key);
		}

		public static void GetDataAndKey(float x, float y, float z, float w, out int data, out int data2, out int data3, out int data4, out int key)
		{
			var dataUnion = new Bytes4Union { Float = x };
			var dataUnion2 = new Bytes4Union { Float = y };
			var dataUnion3 = new Bytes4Union { Float = z };
			var dataUnion4 = new Bytes4Union { Float = w };
			
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion, out data, out key);
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion2, out data2, key);
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion3, out data3, key);
			SecureVariablesManager.Cryptographer.Encrypt(ref dataUnion4, out data4, key);
		}

		public static implicit operator SecureQuaternion(Quaternion value)
		{
			return new SecureQuaternion(value);
		}

		public static implicit operator Quaternion(SecureQuaternion SecureQuaternion)
		{
			return SecureQuaternion.Value;
		}
        
		public static bool operator ==(SecureQuaternion a, SecureQuaternion b)
		{
			return a.Value == b.Value;
		}

		public static bool operator !=(SecureQuaternion a, SecureQuaternion b)
		{
			return a.Value != b.Value;
		}
        
		public static bool operator ==(SecureQuaternion a, Quaternion b)
		{
			return a.Value == b;
		}

		public static bool operator !=(SecureQuaternion a, Quaternion b)
		{
			return a.Value != b;
		}

		public static Quaternion operator *(SecureQuaternion a, Quaternion b)
		{
			return a.Value * b;
		}

		public float x
		{
			get => Value.x;
			set
			{
				var currentValue = Value;
				currentValue.x = value;
				Set(currentValue);
			}
		}

		public float y
		{
			get => Value.y;
			set
			{
				var currentValue = Value;
				currentValue.y = value;
				Set(currentValue);
			}
		}

		public float z
		{
			get => Value.z;
			set
			{
				var currentValue = Value;
				currentValue.z = value;
				Set(currentValue);
			}
		}

		public float w
		{
			get => Value.w;
			set
			{
				var currentValue = Value;
				currentValue.w = value;
				Set(currentValue);
			}
		}

		public float this[int index]
		{
			get => Value[index];
			set
			{
				var currentValue = Value;
				switch (index)
				{
					case 0:
						currentValue.x = value;
						break;
					case 1:
						currentValue.y = value;
						break;
					case 2:
						currentValue.z = value;
						break;
					case 3:
						currentValue.w = value;
						break;
					default:
						throw new IndexOutOfRangeException("Invalid Quaternion index!");
				}
				
				Set(currentValue);
			}
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override  bool Equals(object other)
		{
			return Value.Equals(other);
		}

		public bool Equals(Quaternion other)
		{
			return Value.Equals(other);
		}

		public bool Equals(SecureQuaternion other)
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
		
		public void Normalize()
		{
			var newValue = Value;
			newValue.Normalize();
			
			Set(newValue);
		}
		
		public Quaternion normalized
		{
			get
			{
				return Value.normalized;
			}
		}
			
		public void SetLookRotation(Vector3 view)
		{
			Vector3 up = Vector3.up;
			SetLookRotation(view, up);
		}

		public void SetLookRotation(Vector3 view, Vector3 up)
		{
			var newValue = Value;
			newValue.SetLookRotation(view, up);
			
			Set(newValue);
		}
		
		public Vector3 eulerAngles
		{
			get { return Value.eulerAngles; }
			set 
			{
				var newValue = Value;
				newValue.eulerAngles = value;
			
				Set(newValue);
			}
		}
		
		public void ToAngleAxis(out float angle, out Vector3 axis)
		{
			Value.ToAngleAxis(out angle, out axis);
		}
		
		public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
		{
			var newValue = Value;
			newValue.SetFromToRotation(fromDirection, toDirection);
			
			Set(newValue);
		}

		public static string GetInMemoryValue(int data, int data2, int data3, int data4)
		{
			return $"{data}{data2}{data3}{data4}";
		}

		public string GetInMemoryValue()
		{
			return GetInMemoryValue(_data, _data2, _data3, _data4);
		}
		
		private void Set(Quaternion value)
		{
			GetDataAndKey(value, out _data, out _data2, out _data3, out _data4, out _key);
		}

		public void Set(float newX, float newY, float newZ, float newW)
		{
			GetDataAndKey(newX, newY, newZ, newW, out _data, out _data2, out _data3, out _data4, out _key);
		}
	}
}