namespace Chaos_Spear
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            label7 = new Label();
            button4 = new Button();
            comboBox1 = new ComboBox();
            label8 = new Label();
            comboBox2 = new ComboBox();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            button5 = new Button();
            button6 = new Button();
            comboBox3 = new ComboBox();
            label12 = new Label();
            label13 = new Label();
            button7 = new Button();
            comboBox4 = new ComboBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(86, 56);
            button1.Name = "button1";
            button1.Size = new Size(235, 115);
            button1.TabIndex = 0;
            button1.Text = "Attach";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(448, 43);
            button2.Name = "button2";
            button2.Size = new Size(303, 70);
            button2.TabIndex = 1;
            button2.Text = "Save Position";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(448, 145);
            button3.Name = "button3";
            button3.Size = new Size(303, 70);
            button3.TabIndex = 2;
            button3.Text = "Load Position";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label1.Location = new Point(5, 310);
            label1.Name = "label1";
            label1.Size = new Size(158, 32);
            label1.TabIndex = 3;
            label1.Text = "Saved X Pos:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label2.Location = new Point(5, 342);
            label2.Name = "label2";
            label2.Size = new Size(157, 32);
            label2.TabIndex = 4;
            label2.Text = "Saved Y Pos:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label3.Location = new Point(5, 373);
            label3.Name = "label3";
            label3.Size = new Size(157, 32);
            label3.TabIndex = 5;
            label3.Text = "Saved Z Pos:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label4.Location = new Point(448, 310);
            label4.Name = "label4";
            label4.Size = new Size(177, 32);
            label4.TabIndex = 8;
            label4.Text = "Current Z Pos:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label5.Location = new Point(448, 342);
            label5.Name = "label5";
            label5.Size = new Size(177, 32);
            label5.TabIndex = 7;
            label5.Text = "Current Y Pos:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label6.Location = new Point(447, 373);
            label6.Name = "label6";
            label6.Size = new Size(178, 32);
            label6.TabIndex = 6;
            label6.Text = "Current X Pos:";
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label7.Location = new Point(447, 250);
            label7.Name = "label7";
            label7.Size = new Size(97, 32);
            label7.TabIndex = 9;
            label7.Text = "Speed: ";
            // 
            // button4
            // 
            button4.Location = new Point(86, 185);
            button4.Name = "button4";
            button4.Size = new Size(235, 70);
            button4.TabIndex = 10;
            button4.Text = "Give 999 Rings";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // ComboBox1
            // 
            comboBox1.Location = new Point(700, 120);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(130, 70);
            comboBox1.TabIndex = 11;
            for (int x = 0; x < 10; x++)
            {
                comboBox1.Items.Add(x);
            }
            comboBox1.SelectedIndex = 0;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedValueChanged += comboBox1_changed;
            // 
            // Label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label8.Location = new Point(450, 117);
            label8.Name = "label8";
            label8.Size = new Size(97, 32);
            label8.TabIndex = 12;
            label8.Text = "Save position to slot: ";
            // 
            // ComboBox2
            // 
            comboBox2.Location = new Point(700, 220);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(130, 70);
            comboBox2.TabIndex = 13;
            for (int x = 0; x < 10; x++)
            {
                comboBox2.Items.Add(x);
            }
            comboBox2.SelectedIndex = 0;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedValueChanged += comboBox2_changed;
            // 
            // Label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label9.Location = new Point(450, 217);
            label9.Name = "label9";
            label9.Size = new Size(97, 32);
            label9.TabIndex = 14;
            label9.Text = "Load position from slot: ";
            // 
            // Label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label10.Location = new Point(5, 290);
            label10.Name = "label10";
            label10.Size = new Size(97, 32);
            label10.TabIndex = 15;
            label10.Text = "Showing positions stored in slots 0 : 0";
            //
            // Label11
            //
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label11.Location = new Point(448, 414);
            label11.Name = "label11";
            label11.Size = new Size(101, 32);
            label11.TabIndex = 16;
            label11.Text = "Facing: ";  
            //
            // Button5
            //            
            button5.Location = new Point(10, 415);
            button5.Name = "button5";
            button5.Size = new Size(100, 40);
            button5.TabIndex = 17;
            button5.Text = "Save to JSON";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            //
            // Button6
            //            
            button6.Location = new Point(150, 415);
            button6.Name = "button6";
            button6.Size = new Size(100, 40);
            button6.TabIndex = 18;
            button6.Text = "Load JSON";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            //
            // comboBox3
            //
            comboBox3.Location = new Point(150, 465);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(150, 70);
            comboBox3.TabIndex = 19;
            Dictionary<string, string> comboSource = new Dictionary<string, string>();
            foreach (string file in Directory.GetFiles(Path.GetDirectoryName(Application.ExecutablePath) + "\\saves", "*.json"))
            {
                comboSource.Add(file, file.Split('\\')[^1]);
            }
            comboBox3.DataSource = new BindingSource(comboSource, null);
            comboBox3.DisplayMember = "Value";
            comboBox3.ValueMember = "Key";
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            //
            // Label12
            //
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label12.Location = new Point(0, 465);
            label12.Name = "label12";
            label12.Size = new Size(101, 32);
            label12.TabIndex = 20;
            label12.Text = "Load data from: ";
            //
            // Button7
            //  
            button7.Location = new Point(86, 270);
            button7.Name = "button7";
            button7.Size = new Size(150, 20);
            button7.TabIndex = 22;
            button7.Text = "Wipe save slots";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            //
            // label13
            //
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label13.Location = new Point(448, 460);
            label13.Name = "label13";
            label13.Size = new Size(101, 32);
            label13.TabIndex = 23;
            label13.Text = "Game version: ";
            //
            // comboBox4
            //
            comboBox4.Location = new Point(600, 465);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(150, 70);
            comboBox4.TabIndex = 24;
            comboBox4.Items.Add("Current");
            comboBox4.Items.Add("Old");
            comboBox4.SelectedItem = "Current";
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.SelectedValueChanged += comboBox4_changed;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(850, 500);
            Controls.Add(comboBox4);
            Controls.Add(button7);
            Controls.Add(label13);
            Controls.Add(label12);
            Controls.Add(button6);
            Controls.Add(comboBox3);
            Controls.Add(button5);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(comboBox2);
            Controls.Add(label8);
            Controls.Add(comboBox1);
            Controls.Add(button4);
            Controls.Add(label7);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Chaos Spear";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private System.Windows.Forms.Timer timer1;
        private Label label7;
        private Button button4;
        private ComboBox comboBox1;
        private Label label8;
        private ComboBox comboBox2;
        private Label label9;
        private Label label10;
        private Label label11;
        private Button button5;
        private Button button6;
        private ComboBox comboBox3;
        private Label label12;
        private Label label13;
        private Button button7;
        private ComboBox comboBox4;
    }
}
