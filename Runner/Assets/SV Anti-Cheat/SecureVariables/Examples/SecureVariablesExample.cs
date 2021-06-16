namespace SecureVariables.Examples
{
    using System;
    using UnityEngine;
	using UnityEngine.UI;
    
    public class SecureVariablesExample : MonoBehaviour
    {
        [Header("SecureInt")]
        [SerializeField] private SecureInt _secureInt = default;
        [SerializeField] private Text _secureIntText = default;
        [SerializeField] private Button _secureIntButton = default;
    
        [Header("SecureFloat")]
        [SerializeField] private SecureFloat _secureFloat = 10.2f;
        [SerializeField] private Text _secureFloatText = default;
        [SerializeField] private Button _secureFloatButton = default;
    
        [Header("SecureDouble")]
        [SerializeField] private SecureDouble _secureDouble = 1000000000d;
        [SerializeField] private Text _secureDoubleText = default;
        [SerializeField] private Button _secureDoubleButton = default;
    
        [Header("SecureLong")]
        [SerializeField] private SecureLong _secureLong = 1;
        [SerializeField] private Text _secureLongText = default;
	    [SerializeField] private Button _secureLongButton = default;

        private void Awake()
        {
            RefreshSecureIntText();
            RefreshSecureFloatText();
            RefreshSecureDoubleText();
            RefreshSecureLongText();
        
            _secureIntButton.onClick.AddListener(ChangeSecureInt);
            _secureFloatButton.onClick.AddListener(ChangeSecureFloat);
            _secureDoubleButton.onClick.AddListener(ChangeSecureDouble);
            _secureLongButton.onClick.AddListener(ChangeSecureLong);
        }

        private void ChangeSecureInt()
        {
            _secureInt += 100;

            RefreshSecureIntText();
        }
    
        private void RefreshSecureIntText()
        {
            RefreshSecureValueText(_secureIntText, typeof(SecureInt), _secureInt.ToString(), _secureInt.GetInMemoryValue());
        }

        private void ChangeSecureFloat()
        {
            _secureFloat *= 1.5f;

            RefreshSecureFloatText();
        }
    
        private void RefreshSecureFloatText()
        {
            RefreshSecureValueText(_secureFloatText, typeof(SecureFloat), _secureFloat.ToString(), _secureFloat.GetInMemoryValue());
        }

        private void ChangeSecureDouble()
        {
            _secureDouble /= 2;

            RefreshSecureDoubleText();
        }
    
        private void RefreshSecureDoubleText()
        {
            RefreshSecureValueText(_secureDoubleText, typeof(SecureDouble), _secureDouble.ToString(), _secureDouble.GetInMemoryValue());
        }

        private void ChangeSecureLong()
        {
            _secureLong *= 2;

            RefreshSecureLongText();
        }
    
        private void RefreshSecureLongText()
        {
            RefreshSecureValueText(_secureLongText, typeof(SecureLong), _secureLong.ToString(), _secureLong.GetInMemoryValue());
        }
    
        private void RefreshSecureValueText(Text textComponent, Type valueType, string valueString, string imMemoryString)
        {
            textComponent.text = $"<b>{valueType.Name}</b>. Current value: <b>'{valueString}'</b>. In memory value: <b>'{imMemoryString}'</b>";
        }
    }
}
