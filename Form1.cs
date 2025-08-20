using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.VisualBasic.Devices;
using Reloaded.Assembler;
using Reloaded.Memory;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Definitions;
using SharpHook;
using SharpHook.Native;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Chaos_Spear
{
    public partial class Form1 : Form
    {
        private bool attached = false;
        private Process proc;

        private ExternalMemory gameMem;
        private Scanner sigScanner;

        ulong boostMemAddress;
        ulong CCMemAddress;
        ulong ringsMemAddress;
        ulong kParamsMemAddress;
        Dictionary<string, KeyCode> hotkeys;
        string hotKeyToChange;
        GOCPlayerKinematicParams kParams;
        GOCPlayerKinematicParams savedParams;
        List<saveSlot> saveSlots = new();
        bool boostCheat = false;
        string gameVersion;

        private SimpleGlobalHook kbHook;
        private Task task;

        public Form1()
        {
            this.Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
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
                saveSlots.Add(new saveSlot());
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

            boostCheatHotkeyButton.Text = hotkeys["boostCheatHotkey"].ToString();
            fillCCHotkeyButton.Text = hotkeys["fillCCHotkey"].ToString();
            maxRingsHotkeyButton.Text = hotkeys["maxRingsHotkey"].ToString();
            savePosHotkeyButton.Text = hotkeys["savePosHotkey"].ToString();
            loadPosHotkeyButton.Text = hotkeys["loadPosHotkey"].ToString();

            kbHook = new SimpleGlobalHook(true);
            kbHook.KeyPressed += handle_keys;
            task = kbHook.RunAsync();
        }

        private void attach(object sender, EventArgs e)
        {
            try
            {
                if (!attached)
                {
                    proc = Process.GetProcessesByName("SONIC_X_SHADOW_GENERATIONS").FirstOrDefault();
                    if (proc == null)
                    {
                        MessageBox.Show("SXSG could not be found");
                        return;
                    }

                    gameMem = new ExternalMemory(proc);
                    sigScanner = new Scanner(proc);

                    // I dont wanna reinstall 1.10.0.0 just for this so its the default case. 1.1.0.0 and 1.1.0.1 values are sourced from the shad gens autosplitter (ty jujstme ^v^)
                    gameVersion = proc.MainModule.ModuleMemorySize switch
                    {
                        0x1CA2A000 => "1.1.0.0",
                        0x18BEC000 => "1.1.0.1",
                        0x17E19000 => "1.10.0.1",
                        _ => "1.10.0.0"
                    };
                    gameVersionLabel.Text = "Game version: " + gameVersion;

                    var runInBackgroundSig = sigScanner.FindPattern("75 4A C0 E8 03");
                    if (runInBackgroundSig.Found)
                    {
                        nint runInBackgroundAddress = proc.MainModule.BaseAddress + runInBackgroundSig.Offset;
                        gameMem.Write<byte>((nuint)runInBackgroundAddress, 0xEB);
                    }
                    var assembler = new Assembler();
                    var boostSig = sigScanner.FindPattern("F3 0F 10 49 08 48 8D 44 24 10 F3 0F 5C CB C7 44 24 18");
                    if (boostSig.Found)
                    {
                        nuint boostAllocLoc = gameMem.Allocate(128).Address;
                        boostMemAddress = gameMem.Allocate(128).Address;

                        nint boostHookAddress = proc.MainModule.BaseAddress + boostSig.Offset + 14;
                        byte[] hook = assembler.Assemble(["use64", "push rdx", $"mov rdx, 0x{(ulong)boostAllocLoc:x}", "jmp rdx", "pop rdx", "NOP", "NOP"]);
                        byte[] stealRCX = assembler.Assemble(["use64", $"mov rdx, 0x{boostMemAddress:x}", "mov [rdx], rcx", "mov [rsp + 0x18],dword 0x3F800000", "lea rcx,[rsp + 0x08]", $"mov rdx, 0x{(ulong)boostHookAddress + 0xA:x}", "jmp rdx"]);

                        gameMem.WriteRaw(boostAllocLoc, stealRCX);
                        gameMem.WriteRaw((nuint)boostHookAddress, hook);
                    }
                    var CCSig = sigScanner.FindPattern("8B 4E 30 4C 8D 76 60 8B 56 38 4C 8B E7 3B CA");
                    if (CCSig.Found)
                    {
                        nuint CCAllocLoc = gameMem.Allocate(128).Address;
                        CCMemAddress = gameMem.Allocate(128).Address;

                        nint CCHookAddress = proc.MainModule.BaseAddress + CCSig.Offset;
                        byte[] hook = assembler.Assemble(["use64", "push rax", $"mov rax, 0x{(ulong)CCAllocLoc:x}", "jmp rax", "pop rax", "NOP", "NOP"]);
                        byte[] stealRSI = assembler.Assemble(["use64", $"mov rax, 0x{CCMemAddress:x}", "mov [rax], rsi", "mov ecx, [rsi + 0x30]", "lea r14, [rsi + 0x60]", "mov edx, [rsi + 0x38]", "mov r12, rdi", $"mov rax, 0x{(ulong)CCHookAddress + 0xA:x}", "jmp rax"]);

                        gameMem.WriteRaw(CCAllocLoc, stealRSI);
                        gameMem.WriteRaw((nuint)CCHookAddress, hook);
                    }
                    var kParamsSig = sigScanner.FindPattern("48 8D 54 24 30 0F 10 B3 80 00 00 00 41 B8 01 00 00 00 48 8B CB");
                    if (kParamsSig.Found)
                    {
                        nuint kParamsAllocLoc = gameMem.Allocate(128).Address;
                        kParamsMemAddress = gameMem.Allocate(128).Address;

                        nint kParamsHookAddress = proc.MainModule.BaseAddress + kParamsSig.Offset + 5;
                        byte[] hook = assembler.Assemble(["use64", "push rax", $"mov rax, 0x{(ulong)kParamsAllocLoc:x}", "jmp rax", "pop rax", "NOP", "NOP"]);
                        byte[] stealRBX = assembler.Assemble(["use64", $"mov rax, 0x{kParamsMemAddress:x}", "mov [rax], rbx", "movups xmm6,[rbx + 0x00000080]", "mov r8d,0x00000001", $"mov rax, 0x{(ulong)kParamsHookAddress + 0xA:x}", "jmp rax"]);

                        gameMem.WriteRaw(kParamsAllocLoc, stealRBX);
                        gameMem.WriteRaw((nuint)kParamsHookAddress, hook);
                    }
                    var ringsSig = sigScanner.FindPattern("48 8B 7B 48 0F B6 87 B0 05 00 00 C0 E8 03 A8 01");
                    if (ringsSig.Found)
                    {
                        nuint ringsAllocLoc = gameMem.Allocate(128).Address;
                        ringsMemAddress = gameMem.Allocate(128).Address;

                        nint ringsHookAddress = proc.MainModule.BaseAddress + ringsSig.Offset;
                        byte[] hook = assembler.Assemble(["use64", "push rsi", $"mov rsi, 0x{(ulong)ringsAllocLoc:x}", "jmp rsi", "pop rsi"]);
                        byte[] stealRBX = assembler.Assemble(["use64", "mov rsi, [rbx+0x30]", "add rsi, 0x28", $"mov [qword 0x{ringsMemAddress:x}], rsi", "mov rdi,[rbx + 0x48]", "movzx eax,byte [rdi + 0x000005B0]", $"mov rsi, 0x{(ulong)ringsHookAddress + 0xA:x}", "jmp rsi"]);

                        gameMem.WriteRaw(ringsAllocLoc, stealRBX);
                        gameMem.WriteRaw((nuint)ringsHookAddress, hook);
                    }
                    attached = true;
                    attachLabel.Text = "Detach";
                    attachButton.Image = Image.FromFile("images\\attachiconglowyellow.png");

                    timer1.Start();
                }
                else
                {
                    attached = false;
                    attachLabel.Text = "Attach";
                    attachButton.Image = Image.FromFile("images\\attachiconsmall.png");
                    timer1.Stop();

                    //kbHook.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void handle_keys(object sender, KeyboardHookEventArgs e)
        {
            if (attached)
            {
                KeyCode key = e.Data.KeyCode;
                if (key == hotkeys["boostCheatHotkey"])
                {
                    toggleBoostCheat(sender, e);
                }
                else if (key == hotkeys["fillCCHotkey"])
                {
                    chargeChaosControl(sender, e);
                }
                else if (key == hotkeys["maxRingsHotkey"])
                {
                    giveMaxRings(sender, e);
                }
                else if (key == hotkeys["savePosHotkey"])
                {
                    savePosition(sender, e);
                }
                else if (key == hotkeys["loadPosHotkey"])
                {
                    loadPosition(sender, e);
                }
            }
        }
        private void changeHotkey(object sender, EventArgs e)
        {
            System.Windows.Forms.Button senderButton = sender as System.Windows.Forms.Button;
            hotKeyToChange = senderButton.Name.Replace("Button", "");
            kbHook.KeyPressed -= handle_keys;
            kbHook.KeyPressed += receiveNewHotkey;
            MessageBox.Show("Please press the key you would like to assign to this button, then press ok.", "Change hotkey", MessageBoxButtons.OK, MessageBoxIcon.Information);
            kbHook.KeyPressed += handle_keys;
            kbHook.KeyPressed -= receiveNewHotkey;
        }

        private void receiveNewHotkey(object sender, KeyboardHookEventArgs e)
        {
            KeyCode key = e.Data.KeyCode;
            hotkeys[hotKeyToChange] = key;
            Control hotkeyButton = this.Controls.Find(hotKeyToChange + "Button", true).First();
            hotkeyButton.Invoke(new MethodInvoker(delegate { hotkeyButton.Text = key.ToString(); }));
        }
        private void savePosition(object sender, EventArgs e)
        {
            if (saveToSlotDropdown.InvokeRequired)
            {
                saveToSlotDropdown.Invoke(savePosition, new Object[] {saveToSlotDropdown, new EventArgs()});
                return;
            }
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }

            nint kParamsMemoryAddress;
            gameMem.Read((nuint)kParamsMemAddress, out kParamsMemoryAddress);
            gameMem.Read((nuint)kParamsMemoryAddress, out savedParams);
            savedParams.XSpd = savedParams.YSpd = savedParams.ZSpd = 0;

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

        private void loadPosition(object sender, EventArgs e)
        {
            if (loadFromSlotDropdown.InvokeRequired)
            {
                loadFromSlotDropdown.Invoke(loadPosition, new Object[] { loadFromSlotDropdown, new EventArgs() });
                return;
            }
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }

            savedParams = saveSlots[loadFromSlotDropdown.SelectedIndex].data;
            nint kParamsMemoryAddress;
            gameMem.Read((nuint)kParamsMemAddress, out kParamsMemoryAddress);
            gameMem.Write<float>((nuint)kParamsMemoryAddress + 0x80, savedParams.xPos);
            gameMem.Write<float>((nuint)kParamsMemoryAddress + 0x84, savedParams.yPos);
            gameMem.Write<float>((nuint)kParamsMemoryAddress + 0x88, savedParams.zPos);
            //why not rotate the guy
            gameMem.Write<float>((nuint)kParamsMemoryAddress + 0x90, savedParams.qRotX);
            gameMem.Write<float>((nuint)kParamsMemoryAddress + 0x94, savedParams.qRotY);
            gameMem.Write<float>((nuint)kParamsMemoryAddress + 0x98, savedParams.qRotZ);
            gameMem.Write<float>((nuint)kParamsMemoryAddress + 0x9C, savedParams.qRotW);
            gameMem.Write<float>((nuint)kParamsMemoryAddress + 0xD0, 0);
            gameMem.Write<float>((nuint)kParamsMemoryAddress + 0xD4, 0);
            gameMem.Write<float>((nuint)kParamsMemoryAddress + 0xD8, 0);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                float speedHorizontal;

                nint kParamsMemoryAddress;
                gameMem.Read((nuint)kParamsMemAddress, out kParamsMemoryAddress);
                gameMem.Read((nuint)kParamsMemoryAddress, out kParams);

                loadedSlotsLabel.Text = "Saved Positions (Slots " + saveToSlotDropdown.SelectedIndex + " : " + loadFromSlotDropdown.SelectedIndex + ")";

                savedXPosLabel.Text = "X: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].data.XPos, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].data.XPos, 3);

                savedYPosLabel.Text = "Y: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].data.YPos, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].data.YPos, 3);

                savedZPosLabel.Text = "Z: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].data.ZPos, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].data.ZPos, 3);

                currentXPosLabel.Text = "X: " + Math.Round(kParams.XPos, 1);
                currentYPosLabel.Text = "Y: " + Math.Round(kParams.YPos, 1);
                currentZPosLabel.Text = "Z: " + Math.Round(kParams.ZPos, 1);

                speedHorizontal = (float)Math.Round(Math.Sqrt(Math.Pow(kParams.XSpd, 2) + Math.Pow(kParams.ZSpd, 2)), 1);

                speedLabel.Text = "Speed: " + speedHorizontal + " (" + Math.Round(kParams.XSpd, 2) + ", " + Math.Round(kParams.YSpd, 2) + ", " + Math.Round(kParams.ZSpd, 2) + ")";

                facingAngleLabel.Text = "Facing: " + Math.Round(heading(kParams.QRotW, kParams.QRotY), 1);

                boostCheatHotkeyButton.Text = hotkeys["boostCheatHotkey"].ToString();
                fillCCHotkeyButton.Text = hotkeys["fillCCHotkey"].ToString();
                maxRingsHotkeyButton.Text = hotkeys["maxRingsHotkey"].ToString();
                savePosHotkeyButton.Text = hotkeys["savePosHotkey"].ToString();
                loadPosHotkeyButton.Text = hotkeys["loadPosHotkey"].ToString();

            }
            catch (Exception exception)
            {
                /*
                timer1.Stop();
                MessageBox.Show(exception.ToString());*/
                //Nothing happens here LOOOOL
            }
        }
        private void giveMaxRings(object sender, EventArgs e)
        {
            nint ringsMemoryAddress;
            gameMem.Read((nuint)ringsMemAddress, out ringsMemoryAddress);
            gameMem.Write<int>((nuint)ringsMemoryAddress, 999);

        }

        private void saveToJSON(object sender, EventArgs e)
        {
            try
            {
                JsonSerializerOptions options = new()
                {
                    WriteIndented = true
                };

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JSON file|*.json";
                saveFileDialog.Title = "Save a JSON file";
                saveFileDialog.ShowDialog();
                File.WriteAllText(saveFileDialog.FileName, JsonSerializer.Serialize(saveSlots, options));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void loadJSON(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Application.ExecutablePath + "\\saves";
                openFileDialog.Filter = "JSON file|*.json";
                openFileDialog.Title = "Open a JSON file";
                openFileDialog.ShowDialog();

                string jsonString = File.ReadAllText(openFileDialog.FileName);
                saveSlots = JsonSerializer.Deserialize<List<saveSlot>>(jsonString);
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
        private void wipeSaves(object sender, EventArgs e)
        {
            saveToSlotDropdown.Items.Clear();
            loadFromSlotDropdown.Items.Clear();
            renameSaveSlotDropdown.Items.Clear();
            for (int x = 0; x < 10; x++)
            {
                saveSlots[x] = new saveSlot();
                saveToSlotDropdown.Items.Add(x);
                loadFromSlotDropdown.Items.Add(x);
                renameSaveSlotDropdown.Items.Add(x);
            }
            saveToSlotDropdown.SelectedIndex = loadFromSlotDropdown.SelectedIndex = renameSaveSlotDropdown.SelectedIndex = 0;
        }
        private void renameSaveSlots(object sender, EventArgs e)
        {
            int renamedSlot = renameSaveSlotDropdown.SelectedIndex;
            string newName = renameSaveSlotInput.Text;

            saveSlots[renamedSlot].name = newName;

            if (newName != "")
            {
            saveToSlotDropdown.Items[renamedSlot] = renamedSlot + "(" + newName + ")";
            loadFromSlotDropdown.Items[renamedSlot] = renamedSlot + "(" + newName + ")";
            renameSaveSlotDropdown.Items[renamedSlot] = renamedSlot + "(" + newName + ")";
        }
            else
            {
                saveToSlotDropdown.Items[renamedSlot] = renamedSlot;
                loadFromSlotDropdown.Items[renamedSlot] = renamedSlot;
                renameSaveSlotDropdown.Items[renamedSlot] = renamedSlot;
            }
        }
        private void chargeChaosControl(object sender, EventArgs e)
        {
            nint CCMemoryAddress;
            gameMem.Read((nuint)CCMemAddress, out CCMemoryAddress);
            gameMem.Write<int>((nuint)CCMemoryAddress + 0x38, 100);
        }

        private void manualTeleport(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            try
            {
                nint kParamsMemoryAddress;
                gameMem.Read((nuint)kParamsMemAddress, out kParamsMemoryAddress);
                gameMem.Write<float>((nuint)kParamsMemoryAddress + 0x80, float.Parse(xPosInput.Text));
                gameMem.Write<float>((nuint)kParamsMemoryAddress + 0x84, float.Parse(yPosInput.Text));
                gameMem.Write<float>((nuint)kParamsMemoryAddress + 0x88, float.Parse(zPosInput.Text));
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

        private void toggleBoostCheat(object sender, EventArgs e)
        {
            if (attached)
            {
                bool currentPatch = false;
                if (boostCheat)
                {
                    // Scans for the instructions that were NOPed and restores them so the cheat is removed
                    var NOPedGroundBoostChunk = sigScanner.FindPattern("F3 0F 5F CA F3 0F 58 4E 68 ?? ?? ?? ?? ?? 49 8B 47 40");
                    if (!NOPedGroundBoostChunk.Found)
                    {
                        NOPedGroundBoostChunk = sigScanner.FindPattern("D0 F3 0F 58 56 68 49 8B CE ?? ?? ?? ?? ?? E8 ?? ?? ?? ??");
                        currentPatch = true;
                    }
                    var NOPedGroundBoostContinuous = sigScanner.FindPattern("F3 0F 5F CA F3 0F 58 4F 68 ?? ?? ?? ?? ?? 48 8B CE");
                    var NOPedAirBoostChunk = sigScanner.FindPattern("F3 0F 5F CA F3 0F 58 4F 68 ?? ?? ?? ?? ?? 8B 40 28");

                    MessageBox.Show((proc.MainModule.BaseAddress + NOPedGroundBoostChunk.Offset + 9).ToString("x") + " " + (proc.MainModule.BaseAddress + NOPedGroundBoostContinuous.Offset + 9).ToString("x") + " " + (proc.MainModule.BaseAddress + NOPedAirBoostChunk.Offset + 9).ToString("x"));

                    if ((NOPedGroundBoostChunk.Found || currentPatch) && NOPedGroundBoostContinuous.Found && NOPedAirBoostChunk.Found)
                    {
                        if (!currentPatch)
                        {
                            gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + NOPedGroundBoostChunk.Offset + 9), [0xF3, 0x0F, 0x11, 0x4E, 0x68]);
                        }
                        else
                        {
                            gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + NOPedGroundBoostChunk.Offset + 9), [0xF3, 0x0F, 0x11, 0x56, 0x68]);
                        }
                        gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + NOPedGroundBoostContinuous.Offset + 9), [0xF3, 0x0F, 0x11, 0x4F, 0x68]);
                        gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + NOPedAirBoostChunk.Offset + 9), [0xF3, 0x0F, 0x11, 0x4F, 0x68]);
                    }

                    boostCheat = false;
                    boostCheatButton.Image = Image.FromFile("images\\infboostsmall.png");
                }
                else
                {
                    // Scans for the instructions that cause the initial chunk of boost to be lost when starting a boost, and the instruction that causes boost to gradually drain
                    var groundBoostChunk = sigScanner.FindPattern("F3 0F 5F CA F3 0F 58 4E 68 F3 0F 11 4E 68 49 8B 47 40");
                    if (!groundBoostChunk.Found)
                    {
                        groundBoostChunk = sigScanner.FindPattern("D0 F3 0F 58 56 68 49 8B CE F3 0F 11 56 68 E8 ?? ?? ?? ??");
                    }
                    var groundBoostContinuous = sigScanner.FindPattern("F3 0F 5F CA F3 0F 58 4F 68 F3 0F 11 4F 68 48 8B CE");
                    var airBoostChunk = sigScanner.FindPattern("F3 0F 5F CA F3 0F 58 4F 68 F3 0F 11 4F 68 8B 40 28");

                    if (groundBoostChunk.Found && groundBoostContinuous.Found && airBoostChunk.Found)
                    {
                        Span<byte> NOP = [0x90, 0x90, 0x90, 0x90, 0x90];
                        gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + groundBoostChunk.Offset + 9), NOP);
                        gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + groundBoostContinuous.Offset + 9), NOP);
                        gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + airBoostChunk.Offset + 9), NOP);
                    }


                    // Sets boost to 100. The previous NOPs should prevent boost from ever draining, so this only needs to be done once
                    nint boostMemoryAddress;
                    gameMem.Read((nuint)boostMemAddress, out boostMemoryAddress);
                    gameMem.Write<float>((nuint)boostMemoryAddress + 0x08, 100);

                    boostCheat = true;
                    boostCheatButton.Image = Image.FromFile("images\\infboostglowyellow.png");
                }
            }
        }

        /// <summary>
        /// Follows a pointer chain to find the address in memory that it points to
        /// </summary>
        /// <param name="baseAddress">The base address of the process added to the initial offset of the pointer chain</param>
        /// <param name="offsets">List of the additional offsets in the pointer chain</param>
        /// <returns>The address that the pointer chain results in</returns>
        public nint followPointerChain(nint baseAddress, List<Int32> offsets)
        {
            nint addressWithOffset;
            gameMem.Read<nint>((nuint)baseAddress, out addressWithOffset);
            addressWithOffset += offsets[0];
            for (int x = 1; x < offsets.Count; x++)
            {
                gameMem.Read<nint>((nuint)addressWithOffset, out addressWithOffset);
                addressWithOffset += offsets[x];
            }
            return addressWithOffset;
        }

        //This is just copied from Portal Gear, the Sonic Frontiers Save Position Tool
        public float heading(float rotW, float rotY)
        {
            float angle = (float)(Math.Acos(rotW) * 2);
            bool sign = rotY > 0;
            if (sign)
            {
                if (angle < Math.PI)
                {
                    return (float)radiansToDegrees(-angle + Math.PI);
                }
                else
                {
                    return (float)radiansToDegrees(2 * Math.PI - angle + Math.PI);
                }
            }
            else
            {
                return (float)radiansToDegrees(angle + Math.PI);
            }
        }
        public double radiansToDegrees(double radians)
        {
            return radians / Math.PI * 180;
        }

        private void formClosed(object sender, FormClosedEventArgs e)
        {
            if (sigScanner != null)
            {
                var boostSig = sigScanner.FindPattern("F3 0F 10 49 08 48 8D 44 24 10 F3 0F 5C CB C7 44 24 18");
                if (boostSig.Found)
                {
                    gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + boostSig.Offset + 14), [0xC7, 0x44, 0x24, 0x18, 0x00, 0x00, 0x80, 0x3F, 0x48, 0x8D, 0x4C, 0x24, 0x08]);
                }
                var CCSig = sigScanner.FindPattern("50 48 C7 C0 ?? ?? ?? ?? FF E0 58 90 90 3B CA");
                if (CCSig.Found)
                {
                    gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + CCSig.Offset), [0x8B, 0x4E, 0x30, 0x4C, 0x8D, 0x76, 0x60, 0x8B, 0x56, 0x38, 0x4C, 0x8B, 0xE7]);
                }
                var kParamsSig = sigScanner.FindPattern("48 8D 54 24 30 50 48 C7 C0 ?? ?? ?? ?? FF E0 58 90 90 48 8B CB");
                if (kParamsSig.Found)
                {
                    gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + kParamsSig.Offset + 5), [0x0F, 0x10, 0xB3, 0x80, 0x00, 0x00, 0x00, 0x41, 0xB8, 0x01, 0x00, 0x00, 0x00]);
                }
                var ringsSig = sigScanner.FindPattern("56 48 C7 C6 ?? ?? ?? ?? FF E6 5E C0 E8 03 A8 01");
                if (ringsSig.Found)
                {
                    gameMem.WriteRaw((nuint)(proc.MainModule.BaseAddress + ringsSig.Offset), [0x48, 0x8B, 0x7B, 0x48, 0x0F, 0xB6, 0x87, 0xB0, 0x05, 0x00, 0x00, 0xC0, 0xE8, 0x03, 0xA8, 0x01]);
                }
            }
        }
    }
}
