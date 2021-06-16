namespace SecureVariables.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SecureVector2))]
	public class SecureVector2Drawer : BaseSecureVariableDrawer
	{
		protected override void DrawValue()
		{
			var value = SecureVector2.GetValue(_dataProperty.longValue, _keyProperty.longValue);
		    
			EditorGUI.BeginChangeCheck();
		    
			var newValue = EditorGUI.Vector2Field(_contentRect, _label, value);
		    
			if (EditorGUI.EndChangeCheck())
			{
				SecureVector2.GetDataAndKey(newValue, out var newData, out var newKey);
				_dataProperty.longValue = newData;
				_keyProperty.longValue = newKey;
			}

			//_inMemoryDataString = SecureVector2.GetInMemoryValue(dataProperty.longValue);
		}
	}
}