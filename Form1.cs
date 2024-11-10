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

namespace Chaos_Spear
{
    public partial class Form1 : Form
    {
        private bool attached = false;
        private Process proc;

        private int xcoordOff = 0x02993FE8;
        private int timerOff = 0x02993FE8;

        private IntPtr coordAddress, timerAddress;

        private ExternalMemory gameMem;

        nint coordAdd;
        nint camCoordAdd;
        nint timerAdd;

        private float[] savedPos = new float[3];

        float[] oldPos = {0,0,0};

        float xSpeed, zSpeed;

        private SimpleGlobalHook kbHook;
        private Task task;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void handle_keys(Object sender, KeyboardHookEventArgs e)
        {
            if (attached)
            {
                KeyCode key = e.Data.KeyCode;
                if (key == KeyCode.VcF9)
                {
                    button2_Click(sender, e);
                }
                else if (key == KeyCode.VcF10)
                {
                    button3_Click(sender, e);
                }
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
                    timerAddress = IntPtr.Add(proc.MainModule.BaseAddress, timerOff);

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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }
            gameMem.Read<nint>((nuint)coordAddress, out coordAdd);

            coordAdd += 0x88;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

            coordAdd += 0x28;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

            coordAdd += 0x0;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

            coordAdd += 0x58;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

            coordAdd += 0x1A8;
            gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

            gameMem.Read<float>((nuint)coordAdd + 0x80, out savedPos[0]);
            gameMem.Read<float>((nuint)coordAdd + 0x84, out savedPos[1]);
            gameMem.Read<float>((nuint)coordAdd + 0x88, out savedPos[2]);

            gameMem.Read<float>((nuint)coordAdd + 0xD0, out xSpeed);
            gameMem.Read<float>((nuint)coordAdd + 0xD8, out zSpeed);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!attached)
            {
                MessageBox.Show("Attach program to SXSG first");
                return;
            }

            gameMem.Write<float>((nuint)coordAdd + 0x80, savedPos[0]);
            gameMem.Write<float>((nuint)coordAdd + 0x84, savedPos[1]);
            gameMem.Write<float>((nuint)coordAdd + 0x88, savedPos[2]);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //if is in a level
                float[] curPos = new float[3];
                float speedHorizontal;

                gameMem.Read<nint>((nuint)coordAddress, out coordAdd);

                coordAdd += 0x88;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

                coordAdd += 0x28;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

                coordAdd += 0x0;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

                coordAdd += 0x58;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

                coordAdd += 0x1A8;
                gameMem.Read<nint>((nuint)coordAdd, out coordAdd);

                gameMem.Read<float>((nuint)coordAdd + 0x80, out curPos[0]);
                gameMem.Read<float>((nuint)coordAdd + 0x84, out curPos[1]);
                gameMem.Read<float>((nuint)coordAdd + 0x88, out curPos[2]);

                gameMem.Read<float>((nuint)coordAdd + 0xD0, out xSpeed);
                gameMem.Read<float>((nuint)coordAdd + 0xD8, out zSpeed);

                speedHorizontal = (float)Math.Round(Math.Sqrt(Math.Pow(xSpeed,2) + Math.Pow(zSpeed, 2)), 1);

                label1.Text = "Saved X Pos: " + savedPos[0];

                label2.Text = "Saved Y Pos: " + savedPos[1];

                label3.Text = "Saved Z Pos: " + savedPos[2];

                label4.Text = "Current X Pos: " + Math.Round(curPos[0], 1);
                label5.Text = "Current Y Pos: " + Math.Round(curPos[1], 1);
                label6.Text = "Current Z Pos: " + Math.Round(curPos[2], 1);
                label7.Text = "Speed: " + speedHorizontal;

                oldPos[0] = curPos[0];
                oldPos[1] = curPos[1];
                oldPos[2] = curPos[2];

                gameMem.Read<nint>((nuint)timerAddress, out timerAdd);

                timerAdd += 0x1B0;
                gameMem.Read<nint>((nuint)timerAdd, out timerAdd);


                timerAdd += 0x10;
                gameMem.Read<nint>((nuint)timerAdd, out timerAdd);


                timerAdd += 0x60;
                gameMem.Read<nint>((nuint)timerAdd, out timerAdd);


                timerAdd += 0xB0;
                gameMem.Read<nint>((nuint)timerAdd, out timerAdd);

                timerAdd += 0x20;
                gameMem.Read<nint>((nuint)timerAdd, out timerAdd);

                gameMem.Write<float>((nuint)timerAdd + 0x28, 99999);

            }
            catch(Exception exception) {
                /*
                timer1.Stop();
                MessageBox.Show(exception.ToString());*/
                //Nothing happens here LOOOOL
            }
            finally
            {
                
            }

        }
    }
}
