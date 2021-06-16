namespace SecureVariables.Editor
{
    using System;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SecureDouble))]
    public class SecureDoubleDrawer : BaseSecureVariableDrawer
    {
        protected override void DrawValue()
	    {
		    var value = SecureDouble.GetValue(_dataProperty.longValue, _keyProperty.longValue);
		    
            EditorGUI.BeginChangeCheck();
		    
            var newValue = EditorGUI.DoubleField(_valueRect, _label, value);
		    
            if (EditorGUI.EndChangeCheck())
            {
                SecureDouble.GetDataAndKey(newValue, out var newData, out var newKey);
                _dataProperty.longValue = newData;
                _keyProperty.longValue = newKey;
            }

            _inMemoryDataString = SecureDouble.GetInMemoryValue(_dataProperty.longValue);
        }
    }
}