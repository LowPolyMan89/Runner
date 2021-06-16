namespace SecureVariables
{
    public struct Bytes8Struct
    {
        public const int Length = 8;
        
        public byte Byte0;
        public byte Byte1;
        public byte Byte2;
        public byte Byte3;
        public byte Byte4;
        public byte Byte5;
        public byte Byte6;
        public byte Byte7;

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
                
                case 4:
                    return Byte4;
                
                case 5:
                    return Byte5;
                
                case 6:
                    return Byte6;
                
                case 7:
                    return Byte7;
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
                
                case 4:
                    Byte4 = value;
                    break;
                
                case 5:
                    Byte5 = value;
                    break;
                
                case 6:
                    Byte6 = value;
                    break;
                
                case 7:
                    Byte7 = value;
                    break;
            }
        }
    }
}