using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Reloaded.Memory.Structs;

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

    //These structs were... borrowed... from Portal Gear
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public float x  { get; set; }
        public float y  { get; set; }
        public float z  { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float w { get; set; }
    }
    public class SaveSlot
    {
        public string name { get; set; } = "";
        public bool hasSaveData { get; set; }
        public GOCPlayerKinematicParams data { get; set;}
    }

    public class InstructionData
    {
        public byte[]? originalBytes { get; set; }
        public nint originalCodeAddress { get; set; }
    }
    public class HookData : InstructionData
    {
        public MemoryAllocation redirectAlloc { get; set; }
        public MemoryAllocation storageAlloc { get; set; }
        public nuint stolenAddress  { get; set; }
    }

    public enum GameVersion
    {
        Unknown,
        v1_1_0_0,
        v1_1_0_1,
        v1_10_0_0,
        v1_10_0_1
    }
}