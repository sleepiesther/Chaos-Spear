using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Chaos_Spear.Properties;
using Reloaded.Assembler;
using Reloaded.Memory;
using Reloaded.Memory.Sigscan;
using SharpHook;
using SharpHook.Native;

namespace Chaos_Spear
{
    public partial class MainWindow : Form
    {
        private bool attached;
        private Process proc;

        private ExternalMemory gameMem;
        private Scanner sigScanner;
        private Assembler assembler = new();
        private Dictionary<string, HookData> hooks;
        private Dictionary<string, InstructionData> instructions;
        private Dictionary<string, KeyCode> hotkeys;
        private string hotKeyToChange;
        private GOCPlayerKinematicParams kParams;
        private GOCPlayerKinematicParams savedParams;
        private List<SaveSlot> saveSlots = new();
        private bool boostCheat;
        private GameVersion gameVersion;

        private SimpleGlobalHook kbHook;
        private Task task;

        private JsonSerializerOptions options = new() { WriteIndented = true };
        

        public MainWindow()
        {
            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Adds 10 instances of the params struct to act as the save/load slots. 10 is a bit of an arbitrary number, but it's nice to be sure the slot index will always be a single digit number.
            for (int x = 0; x < 10; x++)
            {
                saveToSlotDropdown.Items.Add(x);
                loadFromSlotDropdown.Items.Add(x);
                renameSaveSlotDropdown.Items.Add(x);
                saveSlots.Add(new SaveSlot());
            }
            saveToSlotDropdown.SelectedIndex = 0;
            loadFromSlotDropdown.SelectedIndex = 0;
            renameSaveSlotDropdown.SelectedIndex = 0;
            
            hotkeys = new()
            {
                { "boostCheatHotkey", KeyCode.VcF6 },
                { "fillCCHotkey", KeyCode.VcF7 },
                { "maxRingsHotkey", KeyCode.VcF8 },
                { "savePosHotkey", KeyCode.VcF9 },
                { "loadPosHotkey", KeyCode.VcF10 }
            };

            boostCheatHotkeyButton.Text = hotkeys["boostCheatHotkey"].ToString().Replace("Vc", "");
            fillCCHotkeyButton.Text = hotkeys["fillCCHotkey"].ToString().Replace("Vc", "");
            maxRingsHotkeyButton.Text = hotkeys["maxRingsHotkey"].ToString().Replace("Vc", "");
            savePosHotkeyButton.Text = hotkeys["savePosHotkey"].ToString().Replace("Vc", "");
            loadPosHotkeyButton.Text = hotkeys["loadPosHotkey"].ToString().Replace("Vc", "");

            kbHook = new SimpleGlobalHook(true);
            kbHook.KeyPressed += HandleKeys;
            task = kbHook.RunAsync();
        }

        private void ToggleAttach(object? sender, EventArgs e)
        {
            if (!attached)
            {
                try
                {
                    proc = Process.GetProcessesByName("SONIC_X_SHADOW_GENERATIONS").FirstOrDefault();
                    if (proc == null || proc.MainModule == null)
                    {
                        MessageBox.Show("SXSG could not be found");
                        return;
                    }

                    proc.Exited += GameClosed;

                    gameMem = new ExternalMemory(proc);
                    sigScanner = new Scanner(proc);

                    // I don't feel like reinstalling 1.10.0.0 just for this so it's the default case. 1.1.0.0 and 1.1.0.1 values are sourced from the SXSG auto splitter (ty Jujstme ^v^)
                    gameVersion = proc.MainModule.ModuleMemorySize switch
                    {
                        0x1CA2A000 => GameVersion.v1_1_0_0,
                        0x18BEC000 => GameVersion.v1_1_0_1,
                        0x17E19000 => GameVersion.v1_10_0_1,
                        _ => GameVersion.v1_10_0_0
                    };
                    
                    gameVersionLabel.Text = "Game version: " + Enum.GetName(gameVersion).Replace("_", ".");

                    // Prevents the game from pausing when tabbed out, stol- I mean borrowed from the SXSG autosplitter (again ty Jujstme)
                    var runInBackgroundSig = sigScanner.FindPattern("75 4A C0 E8 03");
                    if (runInBackgroundSig.Found)
                    {
                        nint runInBackgroundAddress = proc.MainModule.BaseAddress + runInBackgroundSig.Offset;
                        gameMem.Write<byte>((nuint)runInBackgroundAddress, 0xEB);
                    }

                    // These hooks allow us to yoink the necessary memory addresses from the game directly, instead of relying on pointers that are very volatile between patches
                    hooks = new()
                    {
                        {
                            "boost", CreateGameHook("F3 0F 10 49 08 48 8D 44 24 10 F3 0F 5C CB C7 44 24 18", 14,
                                "rcx", "rdx", 2, 0x08)
                        },
                        {
                            "cc", CreateGameHook("8B 4E 30 4C 8D 76 60 8B 56 38 4C 8B E7 3B CA", 0,
                                "rsi", "rax", 2, 0x38)
                        },
                        {
                            "kParams", CreateGameHook("48 8D 54 24 30 0F 10 B3 80 00 00 00 41 B8 01 00 00 00 48 8B CB",
                                5, "rbx", "rax", 2)
                        },
                        {
                            "rings", CreateGameHook("48 8B 7B 48 0F B6 87 B0 05 00 00 C0 E8 03 A8 01", 0,
                                "[rbx+0x30]", "rsi", 0, 0x28)
                        }
                    };

                    //Locate the instructions that cause the boost gauge to drain so they can be NOPed later
                    instructions = new()
                    {
                        {
                            "groundBoost", gameVersion < GameVersion.v1_10_0_0
                                ? LocateInstruction("F3 0F 5F CA F3 0F 58 4E 68 F3 0F 11 4E 68 49 8B 47 40", 9, 5)
                                : LocateInstruction("D0 F3 0F 58 56 68 49 8B CE F3 0F 11 56 68 E8 ?? ?? ?? ??", 9,
                                    5) //This instruction is different on later patches fsr
                        }, 
                        {
                            "airBoost", 
                            LocateInstruction("F3 0F 5F CA F3 0F 58 4F 68 F3 0F 11 4F 68 48 8B CE", 9, 5)
                        },
                        {
                            "gradualBoost",
                            LocateInstruction("F3 0F 5F CA F3 0F 58 4F 68 F3 0F 11 4F 68 8B 40 28", 9, 5)
                        }
                    };

                    attached = true;
                    attachLabel.Text = "Detach";
                    attachButton.Image = Resources.attachIconAttached;

                    timer.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured! It's possible you attached too early in the game's boot. Please try again later. If this keeps happening please message @sleepiesther on discord and show me this error message: " + ex.Message);
                    RemoveGameHooks(); // Ensure any hooks that were created before the error are removed
                }
            }
            else
            {
                try
                {
                    attached = false;
                    attachLabel.Text = "Attach";
                    gameVersionLabel.Text = "Game version: Unknown";
                    attachButton.Image = Resources.attachIconDetached;
                    
                    // Restore the game back to its vanilla state. Run in background patch isn't removed because it's leaderboard legal
                    RemoveGameHooks();
                    if (boostCheat) ToggleBoostCheat(sender, e);
                    timer.Stop();
                    kbHook.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured! This should not happen! Oopsies! Please message @sleepiesther on discord and show her this error message: " + ex.Message);
                }
            }
        }

        private void HandleKeys(object sender, KeyboardHookEventArgs e)
        {
            if (!attached) return;
            
            var key = e.Data.KeyCode;
            if (key == hotkeys["boostCheatHotkey"])
            {
                ToggleBoostCheat(sender, e);
            }
            else if (key == hotkeys["fillCCHotkey"])
            {
                ChargeChaosControl(sender, e);
            }
            else if (key == hotkeys["maxRingsHotkey"])
            {
                GiveMaxRings(sender, e);
            }
            else if (key == hotkeys["savePosHotkey"])
            {
                SavePosition(sender, e);
            }
            else if (key == hotkeys["loadPosHotkey"])
            {
                LoadPosition(sender, e);
            }
        }
        private void ChangeHotkey(object sender, EventArgs e)
        {
            // There's probably a better way to do this but this works
            Button senderButton = sender as Button;
            hotKeyToChange = senderButton.Name.Replace("Button", "");
            kbHook.KeyPressed -= HandleKeys;
            kbHook.KeyPressed += ReceiveNewHotkey;
            MessageBox.Show("Please press the key you would like to assign to this button, then press OK.", "Change hotkey", MessageBoxButtons.OK, MessageBoxIcon.Information);
            kbHook.KeyPressed += HandleKeys;
            kbHook.KeyPressed -= ReceiveNewHotkey;
        }

        private void ReceiveNewHotkey(object sender, KeyboardHookEventArgs e)
        {
            var key = e.Data.KeyCode;
            hotkeys[hotKeyToChange] = key;
            var hotkeyButton = Controls.Find(hotKeyToChange + "Button", true).First();
            hotkeyButton.Invoke(new MethodInvoker(delegate { hotkeyButton.Text = key.ToString().Replace("Vc", ""); }));
        }
        private void SavePosition(object sender, EventArgs e)
        {
            if (saveToSlotDropdown.InvokeRequired)
            {
                saveToSlotDropdown.Invoke(SavePosition, new object[] { saveToSlotDropdown, EventArgs.Empty });
                return;
            }
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            if (hooks["kParams"].stolenAddress == 0)
            {
                MessageBox.Show("You need to unpause the game for at least a frame after attaching/entering a level for Chaos Spear to be able to properly work. Please unpause.");
                return;
            }
            try
            {
                gameMem.Read(hooks["kParams"].stolenAddress, out savedParams);

                if (emptyAutoSave.Checked)
                {
                    for (int x = 0; x < saveSlots.Count; x++)
                    {
                        if (!saveSlots[x].hasSaveData)
                        {
                            saveToSlotDropdown.SelectedIndex = x;
                            break;
                        }
                    }
                }
                saveSlots[saveToSlotDropdown.SelectedIndex].data = savedParams;
                saveSlots[saveToSlotDropdown.SelectedIndex].hasSaveData = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadPosition(object sender, EventArgs e)
        {
            if (loadFromSlotDropdown.InvokeRequired)
            {
                loadFromSlotDropdown.Invoke(LoadPosition, new object[] { loadFromSlotDropdown, EventArgs.Empty });
                return;
            }
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            if (hooks["kParams"].stolenAddress == 0)
            {
                MessageBox.Show("You need to unpause the game for at least a frame after attaching/entering a level for Chaos Spear to be able to properly work. Please unpause.");
                return;
            }

            savedParams = saveSlots[loadFromSlotDropdown.SelectedIndex].data;
            gameMem.Write(hooks["kParams"].stolenAddress + 0x80, new Vector3() {x = savedParams.xPos, y = savedParams.yPos, z = savedParams.zPos});
            //why not rotate the guy
            gameMem.Write(hooks["kParams"].stolenAddress + 0x90, new Quaternion() {x = savedParams.QRotX, y = savedParams.QRotY, z = savedParams.QRotZ, w = savedParams.QRotW});
            if (!resetSpeedCheckbox.Checked) return;
            gameMem.Write(hooks["kParams"].stolenAddress + 0xD0, new Vector3() {x = 0, y = 0, z = 0});

        }
        
        private void ManualTeleport(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            if (hooks["kParams"].stolenAddress == 0)
            {
                MessageBox.Show("You need to unpause the game for at least a frame after attaching/entering a level for Chaos Spear to be able to properly work. Please unpause.");
                return;
            }
            try
            {
                gameMem.Write(hooks["kParams"].stolenAddress + 0x80, new Vector3()
                {
                    x = float.Parse(xPosInput.Text, CultureInfo.CurrentCulture), 
                    y = float.Parse(yPosInput.Text, CultureInfo.CurrentCulture), 
                    z = float.Parse(zPosInput.Text, CultureInfo.CurrentCulture)
                });
                if (!resetSpeedCheckbox.Checked) return;
                gameMem.Write(hooks["kParams"].stolenAddress + 0xD0, new Vector3() {x = 0, y = 0, z = 0});
            }
            catch (FormatException)
            {
                MessageBox.Show("Please input a number into every box :3");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void GiveMaxRings(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            if (hooks["rings"].stolenAddress == 0)
            {
                MessageBox.Show("You need to unpause the game for at least a frame after attaching/entering a level for Chaos Spear to be able to properly work. Please unpause.");
                return;
            }
            try
            {
                gameMem.Write(hooks["rings"].stolenAddress, 999);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
        } 
        private void ChargeChaosControl(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            if (hooks["cc"].stolenAddress == 0)
            {
                MessageBox.Show("You need to unpause the game for at least a frame after attaching/entering a level for Chaos Spear to be able to properly work. Please unpause.");
                return;
            }
            try
            {
                gameMem.Write(hooks["cc"].stolenAddress, 100);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ToggleBoostCheat(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            if (hooks["boost"].stolenAddress == 0)
            {
                MessageBox.Show("You need to unpause the game for at least a frame after attaching/entering a level for Chaos Spear to be able to properly work. Please unpause.");
                return;
            }
            try
            {
                if (boostCheat)
                {
                    RestoreInstructions();
                    boostCheat = false;
                    boostCheatButton.Image = Resources.infiniteBoostDeactivated;
                }
                else
                {
                    foreach (var instruction in instructions)
                    {
                        gameMem.WriteRaw((nuint)instruction.Value.originalCodeAddress, [0x90, 0x90, 0x90, 0x90, 0x90]);
                    }

                    // Sets boost to 100. The previous NOPs should prevent boost from ever draining, so this only needs to be done once
                    gameMem.Write<float>(hooks["boost"].stolenAddress, 100);

                    boostCheat = true;
                    boostCheatButton.Image = Resources.infiniteBoostActivated;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Continually grab mem addresses from our allocated memory so the rest of the program can use it
                foreach (var hook in hooks)
                {
                    gameMem.Read(hook.Value.storageAlloc.Address, out nuint stolenAddress); // Properties cannot be outs in this case, so we use a temp var instead
                    hook.Value.stolenAddress = stolenAddress;
                }
                
                gameMem.Read(hooks["kParams"].stolenAddress, out kParams);

                loadedSlotsLabel.Text = "Saved Positions (Slots " + saveToSlotDropdown.SelectedIndex + " : " + loadFromSlotDropdown.SelectedIndex + ")";

                savedXPosLabel.Text = "X: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].data.XPos, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].data.XPos, 3);

                savedYPosLabel.Text = "Y: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].data.YPos, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].data.YPos, 3);

                savedZPosLabel.Text = "Z: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].data.ZPos, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].data.ZPos, 3);

                currentXPosLabel.Text = "X: " + Math.Round(kParams.XPos, 1);
                currentYPosLabel.Text = "Y: " + Math.Round(kParams.YPos, 1);
                currentZPosLabel.Text = "Z: " + Math.Round(kParams.ZPos, 1);

                var speedHorizontal = (float)Math.Round(Math.Sqrt(Math.Pow(kParams.XSpd, 2) + Math.Pow(kParams.ZSpd, 2)), 1);

                speedLabel.Text = "Speed: " + speedHorizontal + " (" + Math.Round(kParams.XSpd, 2) + ", " + Math.Round(kParams.YSpd, 2) + ", " + Math.Round(kParams.ZSpd, 2) + ")";

                facingAngleLabel.Text = "Facing: " + Math.Round(Heading(kParams.QRotW, kParams.QRotY), 1);

                boostCheatHotkeyButton.Text = hotkeys["boostCheatHotkey"].ToString().Replace("Vc", "");
                fillCCHotkeyButton.Text = hotkeys["fillCCHotkey"].ToString().Replace("Vc", "");
                maxRingsHotkeyButton.Text = hotkeys["maxRingsHotkey"].ToString().Replace("Vc", "");
                savePosHotkeyButton.Text = hotkeys["savePosHotkey"].ToString().Replace("Vc", "");
                loadPosHotkeyButton.Text = hotkeys["loadPosHotkey"].ToString().Replace("Vc", "");

            }
            catch
            {
                // Empty catch because reading memory on a timer can naturally produce errors, and we don't want to spam the user with useless errors.
            }
        }

        private void SaveToJson(object sender, EventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JSON file|*.json";
                saveFileDialog.Title = "Save a JSON file";
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName == "")
                {
                    return;
                }
                File.WriteAllText(saveFileDialog.FileName, JsonSerializer.Serialize(saveSlots, options));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadJson(object sender, EventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Application.ExecutablePath + "\\saves";
                openFileDialog.Filter = "JSON file|*.json";
                openFileDialog.Title = "Open a JSON file";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName == "")
                {
                    return;
                }

                var jsonString = File.ReadAllText(openFileDialog.FileName);
                saveSlots = JsonSerializer.Deserialize<List<SaveSlot>>(jsonString);
                saveToSlotDropdown.Items.Clear();
                loadFromSlotDropdown.Items.Clear();
                renameSaveSlotDropdown.Items.Clear();

                for (int x = 0; x < saveSlots.Count - 1; x++)
                {
                    if (saveSlots[x].name != "")
                    {
                        saveToSlotDropdown.Items.Add(x + " (" + saveSlots[x].name + ")");
                        loadFromSlotDropdown.Items.Add(x + " (" + saveSlots[x].name + ")");
                        renameSaveSlotDropdown.Items.Add(x + " (" + saveSlots[x].name + ")");
                    }
                    else
                    {
                        saveToSlotDropdown.Items.Add(x);
                        loadFromSlotDropdown.Items.Add(x);
                        renameSaveSlotDropdown.Items.Add(x);
                    }
                }
                saveToSlotDropdown.SelectedIndex = loadFromSlotDropdown.SelectedIndex = renameSaveSlotDropdown.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void WipeSaves(object sender, EventArgs e)
        {
            saveToSlotDropdown.Items.Clear();
            loadFromSlotDropdown.Items.Clear();
            renameSaveSlotDropdown.Items.Clear();
            for (var x = 0; x < 10; x++)
            {
                saveSlots[x] = new SaveSlot();
                saveToSlotDropdown.Items.Add(x);
                loadFromSlotDropdown.Items.Add(x);
                renameSaveSlotDropdown.Items.Add(x);
            }
            saveToSlotDropdown.SelectedIndex = loadFromSlotDropdown.SelectedIndex = renameSaveSlotDropdown.SelectedIndex = 0;
        }
        private void RenameSaveSlots(object sender, EventArgs e)
        {
            var renamedSlot = renameSaveSlotDropdown.SelectedIndex;
            var newName = renameSaveSlotInput.Text;

            saveSlots[renamedSlot].name = newName;

            if (newName != "")
            {
                saveToSlotDropdown.Items[renamedSlot] = renamedSlot + " (" + newName + ")";
                loadFromSlotDropdown.Items[renamedSlot] = renamedSlot + " (" + newName + ")";
                renameSaveSlotDropdown.Items[renamedSlot] = renamedSlot + " (" + newName + ")";
            }
            else
            {
                saveToSlotDropdown.Items[renamedSlot] = renamedSlot;
                loadFromSlotDropdown.Items[renamedSlot] = renamedSlot;
                renameSaveSlotDropdown.Items[renamedSlot] = renamedSlot;
            }
        }
        
        private HookData CreateGameHook(string signature, int offset, string registerToSteal, string registerToUse, int NOPCount, int addressAdd = 0)
        {
            var sig = sigScanner.FindPattern(signature);
            
            if (!sig.Found)
            {
                throw new Exception("Could not find signature: " + signature);
            }
       
            HookData hookData = new()
            {
                redirectAlloc = gameMem.Allocate(128),
                storageAlloc = gameMem.Allocate(128),
                originalCodeAddress = proc.MainModule.BaseAddress + sig.Offset + offset,
            };
            hookData.originalBytes = gameMem.ReadRaw((nuint)hookData.originalCodeAddress, 11 + NOPCount);

            //Hooks into the game and redirects it to custom instructions at a known address
            List<byte> hook = assembler.Assemble(["use64", $"push {registerToUse}", $"mov {registerToUse}, 0x{(ulong)hookData.redirectAlloc.Address:x}", $"jmp {registerToUse}", $"pop {registerToUse}"]).ToList();
            
            //If the hook is longer than expected it'll cause a crash. This typically happens when the hook is created too early and the redirect address is made super long for some reason
            if (hook.Count != 11)
            {
                throw new Exception("Hook size mismatch: " + Convert.ToHexString(hook.ToArray()));
            }
            
            // NOPs added to make sure that the hook doesn't leave half a partial instruction, as that would cause a crash
            for (int x = 0; x < NOPCount; x++)
            {
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

        private void RemoveGameHooks()
        {
            if (hooks is not Dictionary<string, HookData>) return;
            foreach (var hook in hooks)
            {
                gameMem.WriteRaw((nuint)hook.Value.originalCodeAddress, hook.Value.originalBytes);
                gameMem.Free(hook.Value.redirectAlloc);
                gameMem.Free(hook.Value.storageAlloc);
            }
        }

        private InstructionData LocateInstruction(string signature, int offset, int instructionLength)
        {
            // Scans for the instructions that cause the initial chunk of boost to be lost when starting a boost, and the instruction that causes boost to gradually drain
            var sig = sigScanner.FindPattern(signature); 
            if (!sig.Found) throw new Exception("Could not find signature: " + signature);
            
            return new InstructionData()
            {
                originalBytes = gameMem.ReadRaw((nuint)(proc.MainModule.BaseAddress + sig.Offset + offset),
                    instructionLength),
                originalCodeAddress = proc.MainModule.BaseAddress + sig.Offset + offset
            };
        }

        private void RestoreInstructions()
        {
            if (instructions is not Dictionary<string, InstructionData>) return;
            foreach (var instruction in instructions)
            {
                gameMem.WriteRaw((nuint)instruction.Value.originalCodeAddress, instruction.Value.originalBytes);
            }
        }

        //This is just copied from Portal Gear, the Sonic Frontiers Save Position Tool
        private float Heading(float rotW, float rotY)
        {
            float angle = (float)(Math.Acos(rotW) * 2);
            bool sign = rotY > 0;
            if (sign)
            {
                if (angle < Math.PI)
                {
                    return (float)RadiansToDegrees(-angle + Math.PI);
                }
                else
                {
                    return (float)RadiansToDegrees(2 * Math.PI - angle + Math.PI);
                }
            }
            else
            {
                return (float)RadiansToDegrees(angle + Math.PI);
            }
        }
        private double RadiansToDegrees(double radians)
        {
            return radians / Math.PI * 180;
        }

        private void ShowCredits(object sender, EventArgs e)
        {
            MessageBox.Show("Onaku - Creating the original Chaos Spear, help with using Cheat Engine\n\nLabrys (github.com/Labreezy) - Work on the original Chaos Spear, facing angle code, help with creating game hooks, and other general programming support \n\nJujstme - Version detection code I... borrowed... from the auto splitter \n\nMoha - Entirely redesigning the UI, testing \n\nLayla (www.twitch.tv/yukilayla) - Design feedback, giving me the idea to add custom hotkeys \n\nLillie (@lillypad6199.bsky.social) - Testing \n\nArcanox (@arcanox.me on bsky) - General programming help \n\nMay (@mayberryzoom.bsky.social) - General programming help", "Credits");
        }
        
        // Make sure a detach is complete if the game or Chaos Spear is closing.
        // Not detaching on game close both looks weird and can cause an error if the user later attempts to detach from a now non-existant process, and not detaching on form close means that later instances of Chaos Spear will be unable to attach to the same process.
        private new void Closing(object sender, FormClosingEventArgs e)
        {
            if (attached) ToggleAttach(sender, e);
        }

        private void GameClosed(object? sender, EventArgs e)
        {
            if (attached) ToggleAttach(sender, e);
        }
    }
}