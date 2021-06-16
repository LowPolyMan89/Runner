namespace SecureVariables.Editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SecureShort))]
	public class SecureShortDrawer : BaseSecureVariableDrawer
	{
		protected override void DrawValue()
		{
			var value = SecureShort.GetValue(_dataProperty.intValue, _keyProperty.intValue);
			
			EditorGUI.BeginChangeCheck();
		    
			var newValue = (short)EditorGUI.IntField(_valueRect, _label, value);
		    
			if (EditorGUI.EndChangeCheck())
			{
				SecureShort.GetDataAndKey(newValue, out var newData, out var newKey);
				_dataProperty.intValue = newData;
				_keyProperty.intValue = newKey;
			}

			_inMemoryDataString = SecureShort.GetInMemoryValue(_dataProperty.intValue);
		}
	}
}
