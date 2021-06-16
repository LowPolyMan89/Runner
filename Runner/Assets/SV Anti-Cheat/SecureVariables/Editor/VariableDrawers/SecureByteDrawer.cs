namespace SecureVariables.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SecureByte))]
    public class SecureByteDrawer : BaseSecureVariableDrawer
    {
        protected override void DrawValue()
	    {
		    var value = SecureByte.GetValue(_dataProperty.intValue, _keyProperty.intValue);
		    
            EditorGUI.BeginChangeCheck();
		    
            var newValue = (byte)EditorGUI.IntField(_valueRect, _label, value);
		    
            if (EditorGUI.EndChangeCheck())
            {
                SecureByte.GetDataAndKey(newValue, out var newData, out var newKey);
                _dataProperty.intValue = newData;
                _keyProperty.intValue = newKey;
            }

            _inMemoryDataString = SecureByte.GetInMemoryValue(_dataProperty.intValue);
        }
    }
}