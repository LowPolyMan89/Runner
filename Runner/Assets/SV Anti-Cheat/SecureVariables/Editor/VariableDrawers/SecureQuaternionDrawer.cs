namespace SecureVariables.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SecureQuaternion))]
	public class SecureQuaternionDrawer : BaseSecureVariableDrawer
	{
		protected override void DrawValue()
		{
			var data2Property = _property.FindPropertyRelative("_data2");
			var data3Property = _property.FindPropertyRelative("_data3");
			var data4Property = _property.FindPropertyRelative("_data4");
			
			var value = SecureQuaternion.GetValue(_dataProperty.intValue, data2Property.intValue, data3Property.intValue, data4Property.intValue, _keyProperty.intValue);
		    var euler = value.eulerAngles;
			
			EditorGUI.BeginChangeCheck();
		    
			var newEuler = EditorGUI.Vector3Field(_contentRect, _label, euler);
		    
			if (EditorGUI.EndChangeCheck())
			{
				var newQuaternion = Quaternion.Euler(newEuler);
				
				SecureQuaternion.GetDataAndKey(newQuaternion, out var newData, out var newData2, out var newData3, out var newData4, out var newKey);
				
				_dataProperty.intValue = newData;
				data2Property.intValue = newData2;
				data3Property.intValue = newData3;
				data4Property.intValue = newData4;
				_keyProperty.intValue = newKey;
			}

			//_inMemoryDataString = SecureQuaternion.GetInMemoryValue(dataProperty.intValue, data2Property.intValue, data3Property.intValue);
		}
	}
}