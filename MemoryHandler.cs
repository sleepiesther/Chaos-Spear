using Reloaded.Assembler;
using Reloaded.Memory;
using Reloaded.Memory.Sigscan;
using System.Diagnostics;

namespace Chaos_Spear {
	public class MemoryHandler {
		public static Process proc;
		public static GameVersion gameVersion;
		public static ExternalMemory gameMem;
		public static Scanner sigScanner;
		private static Assembler assembler = new();
		public static Dictionary<string, HookData> hooks;
		public static Dictionary<string, InstructionData> instructions;
		public static GOCPlayerKinematicParams currentKParams;


		public static void AttachToGame() {
			proc = Process.GetProcessesByName("SONIC_X_SHADOW_GENERATIONS").FirstOrDefault();
			if (proc == null || proc.MainModule == null) {
				MessageBox.Show("SXSG could not be found");
				return;
			}

			gameMem = new ExternalMemory(proc);
			sigScanner = new Scanner(proc);

			// I don't feel like reinstalling 1.10.0.0 just for this so it's the default case. 1.1.0.0 and 1.1.0.1 values are sourced from the SXSG auto splitter (ty Jujstme ^v^)
			gameVersion = proc.MainModule.ModuleMemorySize switch {
				0x1CA2A000 => GameVersion.v1_1_0_0,
				0x18BEC000 => GameVersion.v1_1_0_1,
				0x17E19000 => GameVersion.v1_10_0_1,
				_ => GameVersion.v1_10_0_0
			};
			// Prevents the game from pausing when tabbed out, stol- I mean borrowed from the SXSG autosplitter (again ty Jujstme)
			var runInBackgroundSig = sigScanner.FindPattern("75 4A C0 E8 03");
			if (runInBackgroundSig.Found) {
				nint runInBackgroundAddress = proc.MainModule.BaseAddress + runInBackgroundSig.Offset;
				gameMem.Write<byte>((nuint)runInBackgroundAddress, 0xEB);
			}

			//// These hooks allow us to yoink the necessary memory addresses from the game directly, instead of relying on pointers that are very volatile between patches
			hooks = new()
			{
						{
							"boost", MemoryHandler.CreateGameHook("F3 0F 10 49 08 48 8D 44 24 10 F3 0F 5C CB C7 44 24 18", 14,
								"rcx", "rdx", 2, 0x08)
						},
						{
							"cc", MemoryHandler.CreateGameHook("8B 4E 30 4C 8D 76 60 8B 56 38 4C 8B E7 3B CA", 0,
								"rsi", "rax", 2, 0x38)
						},
						{
							"kParams", MemoryHandler.CreateGameHook("48 8D 54 24 30 0F 10 B3 80 00 00 00 41 B8 01 00 00 00 48 8B CB",
								5, "rbx", "rax", 2)
						},
						{
							"rings", MemoryHandler.CreateGameHook("48 8B 7B 48 0F B6 87 B0 05 00 00 C0 E8 03 A8 01", 0,
								"[rbx+0x30]", "rsi", 0, 0x28)
						}
					};

			//Locate the instructions that cause the boost gauge to drain so they can be NOPed later
			instructions = new()
			{
						{
							"groundBoost", gameVersion < GameVersion.v1_10_0_0
								? MemoryHandler.LocateInstruction("F3 0F 5F CA F3 0F 58 4E 68 F3 0F 11 4E 68 49 8B 47 40", 9, 5)
								: MemoryHandler.LocateInstruction("D0 F3 0F 58 56 68 49 8B CE F3 0F 11 56 68 E8 ?? ?? ?? ??", 9,
									5) //This instruction is different on later patches fsr
						},
						{
							"airBoost",
							MemoryHandler.LocateInstruction("F3 0F 5F CA F3 0F 58 4F 68 F3 0F 11 4F 68 48 8B CE", 9, 5)
						},
						{
							"gradualBoost",
							MemoryHandler.LocateInstruction("F3 0F 5F CA F3 0F 58 4F 68 F3 0F 11 4F 68 8B 40 28", 9, 5)
						}
					};
		}

		public static HookData CreateGameHook(string signature, int offset, string registerToSteal, string registerToUse, int NOPCount, int addressAdd = 0) {
			var sig = sigScanner.FindPattern(signature);

			if (!sig.Found) {
				throw new SigNotFoundException("Could not find signature: " + signature);
			}

			HookData hookData = new() {
				redirectAlloc = gameMem.Allocate(128),
				storageAlloc = gameMem.Allocate(128),
				originalCodeAddress = proc.MainModule.BaseAddress + sig.Offset + offset,
			};
			hookData.originalBytes = gameMem.ReadRaw((nuint)hookData.originalCodeAddress, 11 + NOPCount);

			//Hooks into the game and redirects it to custom instructions at a known address
			List<byte> hook = assembler.Assemble(["use64", $"push {registerToUse}", $"mov {registerToUse}, 0x{(ulong)hookData.redirectAlloc.Address:x}", $"jmp {registerToUse}", $"pop {registerToUse}"]).ToList();

			//If the hook is longer than expected it'll cause a crash. This typically happens when the hook is created too early and the redirect address is made super long for some reason
			if (hook.Count != 11) {
				throw new HookSizeMismatchException("Hook size mismatch: " + Convert.ToHexString(hook.ToArray()));
			}

			// NOPs added to make sure that the hook doesn't leave a partial instruction, as that would cause a crash
			for (int x = 0; x < NOPCount; x++) {
				hook.Add(0x90);
			}

			//Steal the value from the target register by inserting it into a place in memory at a known address
			List<byte> stealRegister = assembler.Assemble([
				"use64", $"mov {registerToUse}, {registerToSteal}", $"add {registerToUse}, {addressAdd}", $"mov [qword 0x{hookData.storageAlloc.Address:x}], {registerToUse}"
			]).ToList();


			//Add the original bytes that got overwritten by the hook so the game doesn't crash
			stealRegister.AddRange(hookData.originalBytes);

			//Return to the pop instruction in the hook
			stealRegister.AddRange(assembler.Assemble([
				"use64", $"mov {registerToUse}, 0x{(ulong)hookData.originalCodeAddress + 0xA:x}", $"jmp {registerToUse}"
			]));

			// Write the register steal before the hook so that the hook doesn't jmp to nothing while waiting for the register steal to be written
			gameMem.WriteRaw(hookData.redirectAlloc.Address, stealRegister.ToArray());
			gameMem.WriteRaw((nuint)hookData.originalCodeAddress, hook.ToArray());

			return hookData;
		}

		public static void RemoveGameHooks() {
			if (hooks is not Dictionary<string, HookData>) return;
			foreach (var hook in hooks) {
				gameMem.WriteRaw((nuint)hook.Value.originalCodeAddress, hook.Value.originalBytes);
				gameMem.Free(hook.Value.redirectAlloc);
				gameMem.Free(hook.Value.storageAlloc);
			}
		}

		public static InstructionData LocateInstruction(string signature, int offset, int instructionLength) {
			// Scans for the instructions that cause the initial chunk of boost to be lost when starting a boost, and the instruction that causes boost to gradually drain
			var sig = sigScanner.FindPattern(signature);
			if (!sig.Found) throw new SigNotFoundException("Could not find signature: " + signature);

			return new InstructionData() {
				originalBytes = gameMem.ReadRaw((nuint)(proc.MainModule.BaseAddress + sig.Offset + offset),
					instructionLength),
				originalCodeAddress = proc.MainModule.BaseAddress + sig.Offset + offset
			};
		}

		public static void NOPInstructions() {
			foreach (InstructionData instruction in instructions.Values) {
				byte[] NOPs = new byte[5];

				for (int i = 0; i < instruction.originalBytes.Length; i++) {
					NOPs[i] = 0x90;
				}

				gameMem.WriteRaw((nuint)instruction.originalCodeAddress, NOPs);
			}
		}

		public static void RestoreInstructions() {
			if (instructions is not Dictionary<string, InstructionData>) return;
			foreach (var instruction in instructions) {
				gameMem.WriteRaw((nuint)instruction.Value.originalCodeAddress, instruction.Value.originalBytes);
			}
		}

		public static void Update() {
			if (proc.HasExited) {
				throw new GameExitedException();
			}

			//Continually grab mem addresses from our allocated memory so the rest of the program can use it
			foreach (var hook in hooks) {
				gameMem.Read(hook.Value.storageAlloc.Address, out nuint stolenAddress); // Properties cannot be outs in this case, so we use a temp var instead
				hook.Value.stolenAddress = stolenAddress;
			}

			ReadKParams();
		}

		public static void ReadKParams() {
			if (hooks["kParams"].stolenAddress == 0) {
				MessageBox.Show("You need to unpause the game for at least a frame after attaching/entering a level for Chaos Spear to be able to properly work. Please unpause.");
				return;
			}
			gameMem.Read(hooks["kParams"].stolenAddress, out currentKParams);
		}

		public static void WritePosition(Vector3 pos, Quaternion rot, bool resetSpeed) {
			if (hooks["kParams"].stolenAddress == 0) {
				MessageBox.Show("You need to unpause the game for at least a frame after attaching/entering a level for Chaos Spear to be able to properly work. Please unpause.");
				return;
			}

			gameMem.Write(hooks["kParams"].stolenAddress + 0x80, pos);
			//why not rotate the guy
			gameMem.Write(hooks["kParams"].stolenAddress + 0x90, rot);
			if (!resetSpeed) return;
			gameMem.Write(hooks["kParams"].stolenAddress + 0xD0, new Vector3());
		}

		public static void WriteToStolenAddress(string hookName, dynamic valueToWrite) {
			if (hooks[hookName].stolenAddress == 0) {
				MessageBox.Show("You need to unpause the game for at least a frame after attaching/entering a level for Chaos Spear to be able to properly work. Please unpause.");
				return;
			}
			gameMem.Write(hooks[hookName].stolenAddress, valueToWrite);
		}
	}
}
