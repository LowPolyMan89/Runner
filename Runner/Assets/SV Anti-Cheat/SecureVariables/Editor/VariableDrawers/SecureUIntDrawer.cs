namespace SecureVariables.Editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SecureUInt))]
	public class SecureUIntDrawer : BaseSecureVariableDrawer
	{
		protected override void DrawValue()
		{
			var value = SecureUInt.GetValue(_dataProperty.intValue, _keyProperty.intValue);
			
			EditorGUI.BeginChangeCheck();
		    
			var newValue = (uint)EditorGUI.IntField(_valueRect, _label, Mathf.Max(0, (int)value));
		    
			if (EditorGUI.EndChangeCheck())
			{
				newValue = (uint)Mathf.Max(0, newValue);

				SecureUInt.GetDataAndKey(newValue, out var newData, out var newKey);
				_dataProperty.intValue = newData;
				_keyProperty.intValue = newKey;
			}

			_inMemoryDataString = SecureUInt.GetInMemoryValue(_dataProperty.intValue);
		}
	}
}
