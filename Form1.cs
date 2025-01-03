using System;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;
using Reloaded.Memory;
using Reloaded.Memory.Sigscan;
using SharpHook;
using SharpHook.Native;
using Reloaded.Memory.Sigscan.Definitions;

namespace Chaos_Spear
{
    public partial class Form1 : Form
    {
        private bool attached = false;
        private Process proc;

        private int xcoordOff = 0x029D2C28;
        private int ringOff = 0x029D2C28;


        private IntPtr coordAddress, ringsAddress;

        private ExternalMemory gameMem;

        nint coordAdd;
        nint camCoordAdd;
        nint ringAdd;

        GOCPlayerKinematicParams kParams;
        GOCPlayerKinematicParams savedParams;
        List<GOCPlayerKinematicParams> saveSlots = new List<GOCPlayerKinematicParams>();
        int slotToSave = 0;
        int slotToLoad = 0;
        private float[] savedPos = new float[3];

        float[] oldPos = { 0, 0, 0 };


        private SimpleGlobalHook kbHook;
        private Task task;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Adds 10 instances of the params struct to act as the save/load slots. 10 is a bit of an arbitrary number, but it's nice to be sure the slot index will always be a single digit number. If you want to change the amount of slots, keep in mind that it is also hardcoded into the comboBox initialisations so you'll have to change that too.
            for (int x = 0; x < 10; x++)
            {
                saveSlots.Add(new GOCPlayerKinematicParams());
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
                    coordAddress = IntPtr.Add(proc.MainModule.BaseAddress, xcoordOff);
                    ringsAddress = IntPtr.Add(proc.MainModule.BaseAddress, ringOff);

                    gameMem = new ExternalMemory(proc);

                    attached = true;
                    button1.Text = "Detach";

                    timer1.Start();
                    
                    kbHook = new SimpleGlobalHook(true);
                    kbHook.KeyPressed += handle_keys;
                    task = kbHook.RunAsync();
                }
                else
                {
                    attached = false;
                    button1.Text = "Attach";
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
                if (key == KeyCode.VcF8){
                    button4_Click(sender, e);
                }
                else if (key == KeyCode.VcF9)
                {
                    button2_Click(sender, e);
                }
                else if (key == KeyCode.VcF10)
                {
                    button3_Click(sender, e);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            gameMem.Read<nint>((nuint)coordAddress, out coordAdd);

            coordAdd += 0x1B0;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

            coordAdd += 0x20;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

            coordAdd += 0x168;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

            coordAdd += 0x0;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

            coordAdd += 0x20;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

            coordAdd += 0x48;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);
            gameMem.Read((nuint)coordAdd, out savedParams);
            
            saveSlots[slotToSave] = savedParams;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }

            savedParams = saveSlots[slotToLoad];
            gameMem.Write<float>((nuint)coordAdd + 0x80, savedParams.xPos);
            gameMem.Write<float>((nuint)coordAdd + 0x84, savedParams.yPos);
            gameMem.Write<float>((nuint)coordAdd + 0x88, savedParams.zPos);
            //why not rotate the guy
            gameMem.Write<float>((nuint)coordAdd + 0xC0, savedParams.qRotX);
            gameMem.Write<float>((nuint)coordAdd + 0xC4, savedParams.qRotY);
            gameMem.Write<float>((nuint)coordAdd + 0xC8, savedParams.qRotZ);
            gameMem.Write<float>((nuint)coordAdd + 0xCC, savedParams.qRotW);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //if is in a level
                float[] curPos = new float[3];
                float speedHorizontal;

                gameMem.Read<nint>((nuint)coordAddress, out coordAdd);

                coordAdd += 0x1B0;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

                coordAdd += 0x20;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

                coordAdd += 0x168;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

                coordAdd += 0x0;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

                coordAdd += 0x20;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

                coordAdd += 0x48;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);
                gameMem.Read((nuint)coordAdd, out kParams);

                

                speedHorizontal = (float)Math.Round(Math.Sqrt(Math.Pow(kParams.xSpd, 2) + Math.Pow(kParams.zSpd, 2)), 1);

                label1.Text = "Saved X Pos: " + Math.Round(saveSlots[slotToSave].xPos, 3) + " : " + Math.Round(saveSlots[slotToLoad].xPos, 3);

                label2.Text = "Saved Y Pos: " + Math.Round(saveSlots[slotToSave].yPos, 3) + " : " + Math.Round(saveSlots[slotToLoad].yPos, 3);

                label3.Text = "Saved Z Pos: " + Math.Round(saveSlots[slotToSave].zPos, 3) + " : " + Math.Round(saveSlots[slotToLoad].zPos, 3);

                label4.Text = "Current X Pos: " + Math.Round(kParams.xPos, 1);
                label5.Text = "Current Y Pos: " + Math.Round(kParams.yPos, 1);
                label6.Text = "Current Z Pos: " + Math.Round(kParams.zPos, 1);
                label7.Text = "Speed: " + speedHorizontal;

                label11.Text = "Facing: " + Math.Round(heading(kParams.qRotW, kParams.qRotY), 1);

            }
            catch (Exception exception)
            {
                /*
                timer1.Stop();
                MessageBox.Show(exception.ToString());*/
                //Nothing happens here LOOOOL
            }
            finally
            {

            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            ringsAddress = IntPtr.Add(proc.MainModule.BaseAddress, ringOff);
            gameMem.Read<nint>((nuint)ringsAddress, out ringAdd);
            gameMem.Read<nint>((nuint)ringAdd + 0x1B0, out ringAdd);
            gameMem.Read<nint>((nuint)ringAdd + 0x20, out ringAdd);
            gameMem.Read<nint>((nuint)ringAdd + 0x168, out ringAdd);
            gameMem.Read<nint>((nuint)ringAdd + 0x0, out ringAdd);
            gameMem.Read<nint>((nuint)ringAdd + 0x20, out ringAdd);
            gameMem.Read<nint>((nuint)ringAdd + 0x30, out ringAdd);
            gameMem.Write<int>((nuint)ringAdd + 0x28, 999);

        }
        private void comboBox1_changed(object sender, EventArgs e)
        {
            slotToSave = comboBox1.SelectedIndex;
            label10.Text = "Showing positions stored in slots " + slotToSave + " : " + slotToLoad;
        }
        private void comboBox2_changed(object sender, EventArgs e)
        {
            slotToLoad = comboBox2.SelectedIndex;
            label10.Text = "Showing positions stored in slots " + slotToSave + " : " + slotToLoad;
        }

        //This is just copied from Portal Gear, the Sonic Frontiers Save Position Tool
        public float heading(float rotW, float rotY)
        {
            float angle = (float)(Math.Acos(rotW) * 2);
            bool sign = rotY > 0;
            if (sign)
            {
                if(angle < Math.PI)
                {
                    return (float)radiansToDegrees(-angle+Math.PI);
                }
                else
                {
                    return (float)radiansToDegrees(2* Math.PI - angle + Math.PI);
                }
            }
            else
            {
                return (float)radiansToDegrees(angle+Math.PI);
            }
        }
        public double radiansToDegrees(double radians)
        {
            return radians / Math.PI * 180;
        }
    }
}
