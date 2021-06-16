namespace SecureVariables
{
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Explicit)]
    public struct Bytes8Union
    {
        [FieldOffset(0)]
        public Bytes8Struct Bytes;
        
        [FieldOffset(0)]
        public long Long;
        
        [FieldOffset(0)]
        public double Double;
        
        [FieldOffset(0)]
        public ulong ULong;
        
        [FieldOffset(0)]
        public Vector2 Vector2;
        
        [FieldOffset(0)]
        public Vector2Int Vector2Int;
    }
}