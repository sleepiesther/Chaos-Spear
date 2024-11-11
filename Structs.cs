using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Chaos_Spear
{
    [StructLayout(LayoutKind.Explicit)]
    public struct GOCPlayerKinematicParams
    {
        [FieldOffset(0x80)] public float xPos;
        [FieldOffset(0x84)] public float yPos;
        [FieldOffset(0x88)] public float zPos;
        [FieldOffset(0x90)] public float qRotX;
        [FieldOffset(0x94)] public float qRotY;
        [FieldOffset(0x98)] public float qRotZ;
        [FieldOffset(0x9C)] public float qRotW;
        [FieldOffset(0xD0)] public float xSpd;
        [FieldOffset(0xD4)] public float ySpd;
        [FieldOffset(0xD8)] public float zSpd;

    }
}
