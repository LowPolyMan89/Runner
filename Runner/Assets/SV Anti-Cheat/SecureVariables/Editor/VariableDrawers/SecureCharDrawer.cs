namespace SecureVariables.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SecureChar))]
    public class SecureCharDrawer : BaseSecureVariableDrawer
    {
        protected override void DrawValue()
	    {
		    var value = SecureChar.GetValue(_dataProperty.intValue, _keyProperty.intValue).ToString();
		    
            EditorGUI.BeginChangeCheck();
		    
            var newValueString = EditorGUI.TextField(_valueRect, _label, value);
            var newValue = newValueString.Length > 0 ? newValueString[0] : default;
		    
            if (EditorGUI.EndChangeCheck())
            {
                SecureChar.GetDataAndKey(newValue, out var newData, out var newKey);
                _dataProperty.intValue = newData;
                _keyProperty.intValue = newKey;
            }

            _inMemoryDataString = SecureChar.GetInMemoryValue(_dataProperty.intValue);
        }
    }
}