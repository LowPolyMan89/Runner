namespace SecureVariables.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SecureULong))]
	public class SecureULongDrawer : BaseSecureVariableDrawer
	{
		protected override void DrawValue()
		{
			var value = SecureULong.GetValue(_dataProperty.longValue, _keyProperty.longValue);
		    
			EditorGUI.BeginChangeCheck();
		    
			var newValue = (ulong)EditorGUI.LongField(_valueRect, _label, (long)Math.Max(0, value));
		    
			if (EditorGUI.EndChangeCheck())
			{
				SecureULong.GetDataAndKey(newValue, out var newData, out var newKey);
				_dataProperty.longValue = newData;
				_keyProperty.longValue = newKey;
			}

			_inMemoryDataString = SecureULong.GetInMemoryValue(_dataProperty.longValue);
		}
	}
}