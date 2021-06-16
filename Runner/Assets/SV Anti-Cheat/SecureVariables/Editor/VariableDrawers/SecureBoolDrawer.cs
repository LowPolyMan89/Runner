namespace SecureVariables.Editor
{
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SecureBool))]
	public class SecureBoolDrawer : BaseSecureVariableDrawer
	{
	    protected override void DrawValue()
		{
			var value = SecureBool.GetValue(_dataProperty.intValue, _keyProperty.intValue);
			
	        EditorGUI.BeginChangeCheck();
			
	        var newValue = EditorGUI.Toggle(_valueRect, _label, value);
			
	        if (EditorGUI.EndChangeCheck())
	        {
	            SecureBool.GetDataAndKey(newValue, out var newData, out var newKey);
	            _dataProperty.intValue = newData;
	            _keyProperty.intValue = newKey;
	        }

	        _inMemoryDataString = SecureBool.GetInMemoryValue(_dataProperty.intValue);
	    }
	}
}