using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using Reloaded.Memory;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Definitions;
using SharpHook;
using SharpHook.Native;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Chaos_Spear
{
    public partial class Form1 : Form
    {
        private bool attached = false;
        private Process proc;
        private int xcoordOff;
        private int ringOff;
        private int ccOff;
        private int boostOff;

        private IntPtr coordAddress, ringsAddress, ccAddress, boostAddress;

        private ExternalMemory gameMem;

        nint coordAdd;
        nint camCoordAdd;
        nint ringAdd;
        nint ccAdd;
        nint boostAdd;

        GOCPlayerKinematicParams kParams;
        GOCPlayerKinematicParams savedParams;
        List<GOCPlayerKinematicParams> saveSlots = new List<GOCPlayerKinematicParams>();
        string currentVersion = "Old";
        private float[] savedPos = new float[3];

        float[] oldPos = { 0, 0, 0 };
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
                Dictionary<string, string> jsonFiles = new();
                foreach (string file in Directory.GetFiles(Path.GetDirectoryName(Application.ExecutablePath) + "\\saves", "*.json"))
                {
                    jsonFiles.Add(file, file.Split('\\')[^1]);
                }
                loadFromDropdown.DataSource = new BindingSource(jsonFiles, null);
                saveSlots.Add(new GOCPlayerKinematicParams());
            }
            saveToSlotDropdown.SelectedIndex = 0;
            loadFromSlotDropdown.SelectedIndex = 0;
            gameVersionDropdown.SelectedIndex = 1;
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
                    if (gameVersionDropdown.SelectedItem.ToString() == "Current")
                    {
                        xcoordOff = 0x029D2C28;
                        ringOff = 0x029D2C28;
                    }
                    else if (gameVersionDropdown.SelectedItem.ToString() == "Old")
                    {
                        xcoordOff = 0x02993FE8;
                        ringOff = 0x02993FE8;
                        ccOff = 0x02999C60;
                        boostOff = 0x02993FE8;
                    }
                    coordAddress = IntPtr.Add(proc.MainModule.BaseAddress, xcoordOff);
                    ringsAddress = IntPtr.Add(proc.MainModule.BaseAddress, ringOff);

                    gameMem = new ExternalMemory(proc);

                    attached = true;
                    attachButton.Text = "Detach";

                    timer1.Start();

                    kbHook = new SimpleGlobalHook(true);
                    kbHook.KeyPressed += handle_keys;
                    task = kbHook.RunAsync();
                }
                else
                {
                    attached = false;
                    attachButton.Text = "Attach";
                    timer1.Stop();

                    //kbHook.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void handle_keys(Object sender, KeyboardHookEventArgs e)
        {
            if (attached)
            {
                KeyCode key = e.Data.KeyCode;
                if (key == KeyCode.VcF7)
                {
                    chargeChaosControl(sender, e);
                }
                else if (key == KeyCode.VcF8)
                {
                    giveMaxRings(sender, e);
                }
                else if (key == KeyCode.VcF9)
                {
                    savePosition(sender, e);
                }
                else if (key == KeyCode.VcF10)
                {
                    loadPosition(sender, e);
                }
            }
        }

        private void savePosition(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            List<Int32> kParamOffsets = new();
            if (gameVersionDropdown.InvokeRequired)
            {
                gameVersionDropdown.Invoke(new MethodInvoker(delegate { currentVersion = gameVersionDropdown.SelectedItem.ToString(); }));
            }
            if (currentVersion == "Current")
            {
                kParamOffsets = [0x1B0, 0x20, 0x168, 0x0, 0x20, 0x48, 0x0];
            }
            else if (currentVersion == "Old")
            {
                kParamOffsets = [0x88, 0x28, 0x0, 0x58, 0x1A8, 0x0];
            }
            gameMem.Read<nint>((nuint)coordAddress, out coordAdd);
            coordAdd = followPointerChain(coordAddress, kParamOffsets);
            gameMem.Read((nuint)coordAdd, out savedParams);

            if (saveToSlotDropdown.InvokeRequired)
            {
                saveToSlotDropdown.Invoke(new MethodInvoker(delegate { saveSlots[saveToSlotDropdown.SelectedIndex] = savedParams; }));
            }
            
        }

        private void loadPosition(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }

            if (loadFromSlotDropdown.InvokeRequired)
            {
                saveToSlotDropdown.Invoke(new MethodInvoker(delegate { savedParams = saveSlots[loadFromSlotDropdown.SelectedIndex]; }));
            }
            gameMem.Write<float>((nuint)coordAdd + 0x80, savedParams.XPos);
            gameMem.Write<float>((nuint)coordAdd + 0x84, savedParams.YPos);
            gameMem.Write<float>((nuint)coordAdd + 0x88, savedParams.ZPos);
            //why not rotate the guy
            gameMem.Write<float>((nuint)coordAdd + 0x90, savedParams.QRotX);
            gameMem.Write<float>((nuint)coordAdd + 0x94, savedParams.QRotY);
            gameMem.Write<float>((nuint)coordAdd + 0x98, savedParams.QRotZ);
            gameMem.Write<float>((nuint)coordAdd + 0x9C, savedParams.QRotW);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                float speedHorizontal;
                List<Int32> kParamOffsets = new();

                if (gameVersionDropdown.SelectedItem.ToString() == "Current")
                {
                    kParamOffsets = [0x1B0, 0x20, 0x168, 0x0, 0x20, 0x48, 0x0];
                }
                else
                {
                    kParamOffsets = [0x88, 0x28, 0x0, 0x58, 0x1A8, 0x0];
                }
                gameMem.Read<nint>((nuint)coordAddress, out coordAdd);
                coordAdd = followPointerChain(coordAddress, kParamOffsets);
                gameMem.Read((nuint)coordAdd, out kParams);

                speedHorizontal = (float)Math.Round(Math.Sqrt(Math.Pow(kParams.XSpd, 2) + Math.Pow(kParams.ZSpd, 2)), 1);

                loadedSlotsLabel.Text = "Showing positions stored in slots " + saveToSlotDropdown.SelectedIndex + " : " + loadFromSlotDropdown.SelectedIndex;

                savedXPosLabel.Text = "Saved X Pos: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].XPos, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].XPos, 3);

                savedYPosLabel.Text = "Saved Y Pos: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].YPos, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].YPos, 3);

                savedZPosLabel.Text = "Saved Z Pos: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].ZPos, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].ZPos, 3);

                currentXPosLabel.Text = "Current X Pos: " + Math.Round(kParams.XPos, 1);
                currentYPosLabel.Text = "Current Y Pos: " + Math.Round(kParams.YPos, 1);
                currentZPosLabel.Text = "Current Z Pos: " + Math.Round(kParams.ZPos, 1);
                string speedText;
                if (detailedSpeedToggle.Checked)
                {
                    speedText = "Speed: " + speedHorizontal + " (" + Math.Round(kParams.XSpd, 2) + ", " + Math.Round(kParams.YSpd, 2) + ", " + Math.Round(kParams.ZSpd, 2) + ")";
                }
                else
                {
                    speedText = "Speed: " + speedHorizontal;
                }

                speedLabel.Text = speedText;

                facingAngleLabel.Text = "Facing: " + Math.Round(heading(kParams.QRotW, kParams.QRotY), 1);

                if (boostCheatToggle.Checked && gameVersionDropdown.SelectedItem.ToString() == "Old")
                {
                    boostAddress = IntPtr.Add(proc.MainModule.BaseAddress, boostOff);
                    boostAdd = followPointerChain(boostAddress, [0x88, 0x28, 0x0, 0x130, 0x18, 0xA8, 0x40, 0x18, 0x18, 0x60]);
                    gameMem.Write<float>((nuint)boostAdd, 100);
                }

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
            List<Int32> ringOffsets = new();
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            if (gameVersionDropdown.InvokeRequired)
            {
                gameVersionDropdown.Invoke(new MethodInvoker(delegate { currentVersion = gameVersionDropdown.SelectedItem.ToString(); }));
            }
            if (currentVersion == "Old")
            {
                ringOffsets = [0x150, 0x460, 0x48, 0x88, 0x28, 0x0, 0x2D0, 0x30, 0x28];
            }
            else
            {
                ringOffsets = [0x10, 0x20, 0x168, 0x0, 0x20, 0x30, 0x28];
            }
            ringsAddress = IntPtr.Add(proc.MainModule.BaseAddress, ringOff);
            ringAdd = followPointerChain(ringsAddress, ringOffsets);
            gameMem.Write<int>((nuint)ringAdd, 999);

        }

        private void saveToJSON(object sender, EventArgs e)
        {
            try
            {
                List<dynamic> saveSlotsWithNames = new();
                List<string> names = new();

                for (int x = 0; x < saveSlots.Count; x++)
                {
                    names.Add("");
                }
                saveSlotsWithNames.Add(names);
                foreach (GOCPlayerKinematicParams slot in saveSlots)
                {
                    saveSlotsWithNames.Add(slot);
                }

                File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "\\saves\\save.json", JsonSerializer.Serialize(saveSlotsWithNames));
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
                string jsonString = File.ReadAllText(loadFromDropdown.SelectedValue.ToString());
                List<dynamic> jsonData = JsonSerializer.Deserialize<List<dynamic>>(jsonString);
                saveToSlotDropdown.Items.Clear();
                loadFromSlotDropdown.Items.Clear();
                for (int x = 0; x < jsonData.Count - 1; x++)
                {
                    if (jsonData[0][x].ToString() != "")
                    {
                        saveToSlotDropdown.Items.Add(x + " (" + jsonData[0][x] + ")");
                        loadFromSlotDropdown.Items.Add(x + " (" + jsonData[0][x] + ")");
                    }
                    else
                    {
                        saveToSlotDropdown.Items.Add(x);
                        loadFromSlotDropdown.Items.Add(x);
                    }
                }
                saveToSlotDropdown.SelectedIndex = 0;
                loadFromSlotDropdown.SelectedIndex = 0;
                for (int x = 1; x < jsonData.Count; x++)
                {
                    saveSlots[x - 1] = JsonSerializer.Deserialize<GOCPlayerKinematicParams>(jsonData[x]);
                }
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
            for (int x = 0; x < 10; x++)
            {
                saveSlots[x] = new GOCPlayerKinematicParams();
                saveToSlotDropdown.Items.Add(x);
                loadFromSlotDropdown.Items.Add(x);
            }
            saveToSlotDropdown.SelectedIndex = 0;
            loadFromSlotDropdown.SelectedIndex = 0;
        }
        private void gameVersionDropdown_changed(object sender, EventArgs e)
        {
            if (gameVersionDropdown.SelectedItem.ToString() == "Current")
            {
                warningLabel.Text = "Boost and Chaos Control don't work on this version. \nIf you find the pointers make a pull request :)";
            }
            else
            {
                warningLabel.Text = "";
            }
        }
        private void chargeChaosControl(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            if (gameVersionDropdown.InvokeRequired)
            {
                gameVersionDropdown.Invoke(new MethodInvoker(delegate { currentVersion = gameVersionDropdown.SelectedItem.ToString(); }));
            }
            if (currentVersion == "Old")
            {
                ccAddress = IntPtr.Add(proc.MainModule.BaseAddress, ccOff);
                ccAdd = followPointerChain(ccAddress, [0x30, 0x88, 0x28, 0x0, 0x2D0, 0x38, 0x108, 0x18, 0x80, 0x38]);
                gameMem.Write<int>((nuint)ccAdd, 100);
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


    }
}
