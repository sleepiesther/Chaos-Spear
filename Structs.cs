using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Chaos_Spear
{
    [StructLayout(LayoutKind.Explicit)]
    public struct GOCPlayerKinematicParams
    {
        [FieldOffset(0x80)] private float xPos;
        [FieldOffset(0x84)] private float yPos;
        [FieldOffset(0x88)] private float zPos;
        [FieldOffset(0x90)] private float qRotX;
        [FieldOffset(0x94)] private float qRotY;
        [FieldOffset(0x98)] private float qRotZ;
        [FieldOffset(0x9C)] private float qRotW;
        [FieldOffset(0xD0)] private float xSpd;
        [FieldOffset(0xD4)] private float ySpd;
        [FieldOffset(0xD8)] private float zSpd;

        public float XPos {get => xPos; set => xPos = value;}
        public float YPos {get => yPos; set => yPos = value;}
        public float ZPos {get => zPos; set => zPos = value;}
        public float QRotX {get => qRotX; set => qRotX = value;}
        public float QRotY {get => qRotY; set => qRotY = value;}
        public float QRotZ {get => qRotZ; set => qRotZ = value;}
        public float QRotW {get => qRotW; set => qRotW = value;}
        public float XSpd {get => xSpd; set => xSpd = value;}
        public float YSpd {get => ySpd; set => ySpd = value;}
        public float ZSpd {get => zSpd; set => zSpd = value;}
    }
    public class saveSlot
    {
        public string name { get; set; } = "";
        public bool hasSaveData { get; set; } = false;
        public GOCPlayerKinematicParams data { get; set;} = new();
    } 
}