namespace SecureVariables
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    public struct Bytes4Union
    {
        [FieldOffset(0)]
        public Bytes4Struct Bytes;
        
        [FieldOffset(0)]
        public int Int;
        
        [FieldOffset(0)]
        public float Float;
        
        [FieldOffset(0)]
        public bool Boolean;
        
        [FieldOffset(0)]
        public byte Byte;
        
        [FieldOffset(0)]
        public char Char;
        
        [FieldOffset(0)]
	    public short Short;
        
        [FieldOffset(0)]
	    public uint UInt;
        
        [FieldOffset(0)]
	    public sbyte SByte;
        
        [FieldOffset(0)]
	    public ushort UShort;
    }
}
