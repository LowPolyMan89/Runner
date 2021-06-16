namespace SecureVariables.Editor
{
    using System;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SecureLong))]
    public class SecureLongDrawer : BaseSecureVariableDrawer
    {
        protected override void DrawValue()
	    {
		    var value = SecureLong.GetValue(_dataProperty.longValue, _keyProperty.longValue);
		    
            EditorGUI.BeginChangeCheck();
		    
            var newValue = EditorGUI.LongField(_valueRect, _label, value);
		    
            if (EditorGUI.EndChangeCheck())
            {
                SecureLong.GetDataAndKey(newValue, out var newData, out var newKey);
                _dataProperty.longValue = newData;
                _keyProperty.longValue = newKey;
            }

            _inMemoryDataString = SecureLong.GetInMemoryValue(_dataProperty.longValue);
        }
    }
}