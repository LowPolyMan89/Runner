namespace SecureVariables
{
    public struct Bytes4Struct
    {
        public const int Length = 4;
        
        public byte Byte0;
        public byte Byte1;
        public byte Byte2;
        public byte Byte3;

        public byte GetByte(int index)
        {
            switch (index)
            {
                case 0:
                    return Byte0;
                
                case 1:
                    return Byte1;
                
                case 2:
                    return Byte2;
                
                case 3:
                    return Byte3;
            }
            
            return Byte0;
        }

        public void SetByte(int index, byte value)
        {
            switch (index)
            {
                case 0:
                    Byte0 = value;
                    break;
                
                case 1:
                    Byte1 = value;
                    break;
                
                case 2:
                    Byte2 = value;
                    break;
                
                case 3:
                    Byte3 = value;
                    break;
            }
        }
    }
}
