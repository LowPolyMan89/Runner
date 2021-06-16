namespace SecureVariables
{
    public interface ICryptographer
    {
	    void Encrypt(ref Bytes4Union dataUnion, out int data, out int key);
        
        void Encrypt(ref Bytes4Union dataUnion, out int data, int key);
        
        void Encrypt(ref Bytes8Union dataUnion, out long data, out long key);
        
        void Encrypt(string value, out string data, out string key);

        void Decrypt(int data, int key, out Bytes4Union result);

        void Decrypt(long data, long key, out Bytes8Union result);

        string Decrypt(string data, string key);
    }
}
