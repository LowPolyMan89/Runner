namespace SecureVariables.Editor
{
	using UnityEditor;
    using UnityEngine;

	public abstract class BaseSecureVariableDrawer : PropertyDrawer
	{
		protected SerializedProperty _property;
		protected SerializedProperty _dataProperty;
		protected SerializedProperty _keyProperty;
		protected GUIContent _label;
		protected Rect _contentRect;
		protected Rect _valueRect;
		protected Rect _dataRect;
		protected string _inMemoryDataString;
	    
		protected abstract void DrawValue();
	    
	    public override void OnGUI(Rect contentRect, SerializedProperty property, GUIContent label)
		{
			_property = property;
			_dataProperty = property.FindPropertyRelative("_data");
			_keyProperty = property.FindPropertyRelative("_key");
			_contentRect = contentRect;
			
			_label = EditorGUI.BeginProperty(contentRect, label, property);

			CalculateValueRectAndDataRect();
			DrawValue();
			DrawData();

			EditorGUI.EndProperty();
		}

	    private void CalculateValueRectAndDataRect()
		{
			_valueRect = new Rect(_contentRect.x, _contentRect.y, _contentRect.width + EditorGUIUtility.labelWidth , _contentRect.height);
			var half = _valueRect.width / 2f;
			_valueRect.width -= half;
			
			_dataRect = _valueRect;
			_dataRect.x += half;
			_dataRect.width -= EditorGUIUtility.labelWidth;
		}

	    private void DrawData()
		{
			if (string.IsNullOrEmpty(_inMemoryDataString))
			{
				return;
			}
			
		    GUI.enabled = false;
		    
		    EditorGUI.TextField(_dataRect, GUIContent.none, _inMemoryDataString);
		    
		    GUI.enabled = true;
	    }
    }
}
