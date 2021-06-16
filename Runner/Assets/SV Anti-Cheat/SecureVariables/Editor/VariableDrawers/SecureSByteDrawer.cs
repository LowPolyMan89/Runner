﻿namespace SecureVariables.Editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SecureSByte))]
	public class SecureSByteDrawer : BaseSecureVariableDrawer
	{
		protected override void DrawValue()
		{
			EditorGUI.BeginChangeCheck();
		    
			var value = SecureSByte.GetValue(_dataProperty.intValue, _keyProperty.intValue);
			var newValue = (sbyte)EditorGUI.IntField(_valueRect, _label, value);
		    
			if (EditorGUI.EndChangeCheck())
			{
				SecureSByte.GetDataAndKey(newValue, out var newData, out var newKey);
				_dataProperty.intValue = newData;
				_keyProperty.intValue = newKey;
			}

			_inMemoryDataString = SecureSByte.GetInMemoryValue(_dataProperty.intValue);
		}
	}
}
