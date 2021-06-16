namespace SecureVariables
{
    using System;
    using System.Text;
    using UnityEngine;
    using Random = System.Random;

    public class KeyedCryptographer : ICryptographer
    {
        public const int MaxStringKeyLength = 6;
        
        private string _keyStringPart1 = "©Ђ,эЖЈhfЧ";
        private string _keyStringPart2 = "m‰ЬфTDzЈhfCЈm‰Ж";
        private string _keyStringPart3 = "Cm‰ЬфTDzz}Ы";

        private bool _isKeyGenerated;
        private byte[] _key;
        private char[] _keyChars;
        
        Random _random = new Random();
        
        public void Encrypt(ref Bytes4Union dataUnion, out int data, out int key)
        {
            var keyUnion = new Bytes4Union();
            
            Encode(ref dataUnion.Bytes, ref keyUnion.Bytes);

            data = dataUnion.Int;
            key = keyUnion.Int;
        }
        
        public void Encrypt(ref Bytes4Union dataUnion, out int data, int key)
        {
	        var keyUnion = new Bytes4Union();
	        keyUnion.Int = key;
            
            Encode(ref dataUnion.Bytes, keyUnion.Bytes);

            data = dataUnion.Int;
        }

        public void Encrypt(ref Bytes8Union dataUnion, out long data, out long key)
        {
            var keyUnion = new Bytes8Union();
            
            Encode(ref dataUnion.Bytes, ref keyUnion.Bytes);

            data = dataUnion.Long;
            key = keyUnion.Long;
        }

        public void Encrypt(string value, out string data, out string key)
        {
            Encode(value, out data, out key);
        }

        public void Decrypt(int data, int key, out Bytes4Union result)
        {
            result = new Bytes4Union { Int = data };
            var keyUnion = new Bytes4Union { Int = key };
            
            Decode(ref result.Bytes, ref keyUnion.Bytes);
        }

        public void Decrypt(long data, long key, out Bytes8Union result)
        {
            result = new Bytes8Union { Long = data };
            var keyUnion = new Bytes8Union { Long = key };
            
            Decode(ref result.Bytes, ref keyUnion.Bytes);
        }

        public string Decrypt(string data, string key)
        {
            return Decode(data, key);
        }

        private void Encode(ref Bytes4Struct dataBytes, ref Bytes4Struct keyBytes)
        {
            var internalKey = GetKey();
            var internalKeyLength = internalKey.Length;

            for (var i = 0; i < Bytes4Struct.Length; i++)
            {
                var temp = dataBytes.GetByte(i);
                var key = internalKey[_random.Next(0, internalKeyLength)];
                temp ^= key;
                
                dataBytes.SetByte(i, temp);
                keyBytes.SetByte(i, key);
            }
        }

        private void Encode(ref Bytes4Struct dataBytes, Bytes4Struct keyBytes)
        {
            for (var i = 0; i < Bytes4Struct.Length; i++)
            {
                var temp = dataBytes.GetByte(i);
	            var key = keyBytes.GetByte(i);
                temp ^= key;
                
                dataBytes.SetByte(i, temp);
            }
        }

        private void Encode(ref Bytes8Struct dataBytes, ref Bytes8Struct keyBytes)
        {
            var internalKey = GetKey();
            var internalKeyLength = internalKey.Length;

            for (var i = 0; i < Bytes8Struct.Length; i++)
            {
                var temp = dataBytes.GetByte(i);
                var key = internalKey[_random.Next(0, internalKeyLength)];
                temp ^= key;
                
                dataBytes.SetByte(i, temp);
                keyBytes.SetByte(i, key);
            }
        }

        private void Encode(string value, out string data, out string key)
        {
            if (string.IsNullOrEmpty(value))
            {
                data = "";
                key = "";
                return;
            }
            
            var internalKeyChars = GetKeyChars();
            var valueChars = value.ToCharArray();
            var valueLength = value.Length;
            var keyCharsLength = Mathf.Min(value.Length, MaxStringKeyLength);

            var result = new char[valueLength];
            var keyChars = new Char[keyCharsLength];
            var j = 0;

            for (var i = 0; i < valueLength; i++)
            {
                var keyValue = internalKeyChars[j];

                if (i < keyCharsLength)
                {
                    keyChars[i] = keyValue;
                }
                
                result[i] = (char)(valueChars[i] ^ keyValue);

                if (++j == keyCharsLength)
                {
                    j = 0;
                }
            }

            data = new string(result);
            key = new string(keyChars);
        }

        private void Decode(ref Bytes4Struct dataBytes, ref Bytes4Struct keyBytes)
        {
            for (var i = 0; i < Bytes4Struct.Length; i++)
            {
                var temp = dataBytes.GetByte(i);
                temp ^= keyBytes.GetByte(i);
                
                dataBytes.SetByte(i, temp);
            }
        }

        private void Decode(ref Bytes8Struct dataBytes, ref Bytes8Struct keyBytes)
        {
            for (var i = 0; i < Bytes8Struct.Length; i++)
            {
                var temp = dataBytes.GetByte(i);
                temp ^= keyBytes.GetByte(i);
                
                dataBytes.SetByte(i, temp);
            }
        }

        private string Decode(string data, string key)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(key))
            {
                return "";
            }
            
            var dataChars = data.ToCharArray();
            var dataLength = dataChars.Length;
            var keyChars = key.ToCharArray();
            var keyCharsLength = keyChars.Length;

            var result = new char[dataLength];
            var j = 0;

            for (var i = 0; i < dataLength; i++)
            {
                result[i] = (char)(dataChars[i] ^ keyChars[j]);

                if (++j == keyCharsLength)
                {
                    j = 0;
                }
            }

            return new string(result);
        }

        private byte[] GetKey()
        {
            if (!_isKeyGenerated)
            {
                GenerateKey();
            }
            
            return _key;
        }

        private char[] GetKeyChars()
        {
            if (!_isKeyGenerated)
            {
                GenerateKey();
            }
            
            return _keyChars;
        }
        
        private void GenerateKey()
        {
            var keyString = _keyStringPart1 + _keyStringPart2 + _keyStringPart3;
            _key = Encoding.UTF8.GetBytes(keyString);
            _keyChars = keyString.ToCharArray();
            _isKeyGenerated = true;
        }
    }
}
