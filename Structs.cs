using System.Runtime.InteropServices;
using Reloaded.Memory.Structs;

namespace Chaos_Spear {
	[StructLayout(LayoutKind.Explicit)]
	public struct GOCPlayerKinematicParams {
		[FieldOffset(0x80)] private Vector3 pos;
		[FieldOffset(0x90)] private Quaternion rot;
		[FieldOffset(0xD0)] private Vector3 spd;

		public Vector3 Pos { get => pos; set => pos = value; }
		public Quaternion Rot { get => rot; set => rot = value; }
		public Vector3 Spd { get => spd; set => spd = value; }

		public GOCPlayerKinematicParams(Vector3 _pos, Quaternion _rot, Vector3 _spd) {
			pos = _pos;
			rot = _rot;
			spd = _spd;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3 {
		public float x { get; set; }
		public float y { get; set; }
		public float z { get; set; }

		public Vector3() {
			x = y = z = 0;
		}
		public Vector3(float _x, float _y, float _z) {
			x = _x;
			y = _y;
			z = _z;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Quaternion {
		public float x { get; set; }
		public float y { get; set; }
		public float z { get; set; }
		public float w { get; set; }

		public Quaternion() {
			x = y = z = w = 0;
		}
		public Quaternion(float _x, float _y, float _z, float _w) {
			x = _x;
			y = _y;
			z = _z;
			w = _w;
		}
	}
	public class SaveSlot {
		public string name { get; set; }
		public bool hasSaveData { get; set; }
		public GOCPlayerKinematicParams data { get; set; }

		public SaveSlot() {
			name = "";
			hasSaveData = false;
			data = new GOCPlayerKinematicParams();
		}

		public SaveSlot(string _name, bool _hasSaveData, GOCPlayerKinematicParams _data) {
			name = _name;
			hasSaveData = _hasSaveData;
			data = _data;
		}
	}

	public class InstructionData {
		public byte[]? originalBytes { get; set; }
		public nint originalCodeAddress { get; set; }
	}
	public class HookData : InstructionData {
		public MemoryAllocation redirectAlloc { get; set; }
		public MemoryAllocation storageAlloc { get; set; }
		public nuint stolenAddress { get; set; }
	}

	public enum GameVersion {
		Unknown,
		v1_1_0_0,
		v1_1_0_1,
		v1_10_0_0,
		v1_10_0_1
	}
}