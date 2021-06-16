namespace SecureVariables.Editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SecureUShort))]
	public class SecureUShortDrawer : BaseSecureVariableDrawer
	{
		protected override void DrawValue()
		{
			EditorGUI.BeginChangeCheck();
		    
			var value = SecureUShort.GetValue(_dataProperty.intValue, _keyProperty.intValue);
			var newValue = (ushort)EditorGUI.IntField(_valueRect, _label, value);
		    
			if (EditorGUI.EndChangeCheck())
			{
				SecureUShort.GetDataAndKey(newValue, out var newData, out var newKey);
				_dataProperty.intValue = newData;
				_keyProperty.intValue = newKey;
			}

			_inMemoryDataString = SecureUShort.GetInMemoryValue(_dataProperty.intValue);
		}
	}
}
