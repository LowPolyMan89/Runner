namespace SecureVariables.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SecureVector2Int))]
	public class SecureVector2IntDrawer : BaseSecureVariableDrawer
	{
		protected override void DrawValue()
		{
			var value = SecureVector2Int.GetValue(_dataProperty.longValue, _keyProperty.longValue);
		    
			EditorGUI.BeginChangeCheck();
		    
			var newValue = EditorGUI.Vector2IntField(_contentRect, _label, value);
		    
			if (EditorGUI.EndChangeCheck())
			{
				SecureVector2Int.GetDataAndKey(newValue, out var newData, out var newKey);
				_dataProperty.longValue = newData;
				_keyProperty.longValue = newKey;
			}

			_inMemoryDataString = SecureVector2Int.GetInMemoryValue(_dataProperty.longValue);
		}
	}
}