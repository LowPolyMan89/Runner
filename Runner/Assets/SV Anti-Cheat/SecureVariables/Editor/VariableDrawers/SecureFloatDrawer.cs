namespace SecureVariables.Editor
{
    using System;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SecureFloat))]
    public class SecureFloatDrawer : BaseSecureVariableDrawer
    {
        protected override void DrawValue()
	    {
		    var value = SecureFloat.GetValue(_dataProperty.intValue, _keyProperty.intValue);
		    
            EditorGUI.BeginChangeCheck();
		    
            var newValue = EditorGUI.FloatField(_valueRect, _label, value);
		    
            if (EditorGUI.EndChangeCheck())
            {
                SecureFloat.GetDataAndKey(newValue, out var newData, out var newKey);
                _dataProperty.intValue = newData;
                _keyProperty.intValue = newKey;
            }

            _inMemoryDataString = SecureFloat.GetInMemoryValue(_dataProperty.intValue);
        }
    }
}