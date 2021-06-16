namespace SecureVariables.Editor
{
	using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SecureInt))]
    public class SecureIntDrawer : BaseSecureVariableDrawer
    {
	    protected override void DrawValue()
	    {
		    EditorGUI.BeginChangeCheck();
		    
		    var value = SecureInt.GetValue(_dataProperty.intValue, _keyProperty.intValue);
		    var newValue = EditorGUI.IntField(_valueRect, _label, value);
		    
		    if (EditorGUI.EndChangeCheck())
		    {
			    SecureInt.GetDataAndKey(newValue, out var newData, out var newKey);
			    _dataProperty.intValue = newData;
			    _keyProperty.intValue = newKey;
		    }

		    _inMemoryDataString = SecureInt.GetInMemoryValue(_dataProperty.intValue);
	    }
    }
}
