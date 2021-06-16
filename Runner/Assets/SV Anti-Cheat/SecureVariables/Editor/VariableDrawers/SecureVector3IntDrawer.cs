namespace SecureVariables.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SecureVector3Int))]
	public class SecureVector3IntDrawer : BaseSecureVariableDrawer
	{
		protected override void DrawValue()
		{
			var data2Property = _property.FindPropertyRelative("_data2");
			var data3Property = _property.FindPropertyRelative("_data3");
			
			var value = SecureVector3Int.GetValue(_dataProperty.intValue, data2Property.intValue, data3Property.intValue, _keyProperty.intValue);
		    
			EditorGUI.BeginChangeCheck();
		    
			var newValue = EditorGUI.Vector3IntField(_contentRect, _label, value);
		    
			if (EditorGUI.EndChangeCheck())
			{
				SecureVector3Int.GetDataAndKey(newValue, out var newData, out var newData2, out var newData3, out var newKey);
				
				_dataProperty.intValue = newData;
				data2Property.intValue = newData2;
				data3Property.intValue = newData3;
				_keyProperty.intValue = newKey;
			}

			//_inMemoryDataString = SecureVector3Int.GetInMemoryValue(dataProperty.intValue, data2Property.intValue, data3Property.intValue);
		}
	}
}