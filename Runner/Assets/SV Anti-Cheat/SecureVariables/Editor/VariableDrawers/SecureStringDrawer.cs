namespace SecureVariables.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SecureString))]
    public class SecureStringDrawer : BaseSecureVariableDrawer
    {
        protected override void DrawValue()
	    {
		    var value = SecureString.GetValue(_dataProperty.stringValue, _keyProperty.stringValue);
		    
            EditorGUI.BeginChangeCheck();
		
            var newValue = EditorGUI.TextField(_valueRect, _label, value);
		    
            if (EditorGUI.EndChangeCheck())
            {
                SecureString.GetDataAndKey(newValue, out var newData, out var newKey);
                _dataProperty.stringValue = newData;
                _keyProperty.stringValue = newKey;
            }

            _inMemoryDataString = SecureString.GetInMemoryValue(_dataProperty.stringValue);
        }
    }
}