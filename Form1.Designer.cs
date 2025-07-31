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
            attachButton = new Button();
            savePositionButton = new Button();
            loadButtonButton = new Button();
            savedXPosLabel = new Label();
            savedYPosLabel = new Label();
            savedZPosLabel = new Label();
            currentXPosLabel = new Label();
            currentYPosLabel = new Label();
            currentZPosLabel = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            speedLabel = new Label();
            maxRingsButton = new Button();
            saveToSlotDropdown = new ComboBox();
            saveToSlotLabel = new Label();
            loadFromSlotDropdown = new ComboBox();
            loadFromSlotLabel = new Label();
            loadedSlotsLabel = new Label();
            facingAngleLabel = new Label();
            jsonSaveButton = new Button();
            jsonLoadButton = new Button();
            loadFromDropdown = new ComboBox();
            loadFromLabel = new Label();
            gameVersionLabel = new Label();
            wipeSavesButton = new Button();
            gameVersionDropdown = new ComboBox();
            detailedSpeedToggle = new CheckBox();
            detailedSpeedLabel = new Label();
            chargeChaosControlButton = new Button();
            boostCheatLabel = new Label();
            boostCheatToggle = new CheckBox();
            warningLabel = new Label();
            xPosInput = new TextBox();
            yPosInput = new TextBox();
            zPosInput = new TextBox();
            manualTeleportButton = new Button();
            xPosInputLabel = new Label();
            yPosInputLabel = new Label();
            zPosInputLabel = new Label();
            SuspendLayout();
            // 
            // attachButton
            // 
            attachButton.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            attachButton.Location = new Point(120, 41);
            attachButton.Margin = new Padding(4, 5, 4, 5);
            attachButton.Name = "attachButton";
            attachButton.Size = new Size(336, 192);
            attachButton.TabIndex = 0;
            attachButton.Text = "Attach";
            attachButton.UseVisualStyleBackColor = true;
            attachButton.Click += attach;
            // 
            // savePositionButton
            // 
            savePositionButton.Location = new Point(640, 72);
            savePositionButton.Margin = new Padding(4, 5, 4, 5);
            savePositionButton.Name = "savePositionButton";
            savePositionButton.Size = new Size(433, 117);
            savePositionButton.TabIndex = 1;
            savePositionButton.Text = "Save Position";
            savePositionButton.UseVisualStyleBackColor = true;
            savePositionButton.Click += savePosition;
            // 
            // loadButtonButton
            // 
            loadButtonButton.Location = new Point(640, 242);
            loadButtonButton.Margin = new Padding(4, 5, 4, 5);
            loadButtonButton.Name = "loadButtonButton";
            loadButtonButton.Size = new Size(433, 117);
            loadButtonButton.TabIndex = 2;
            loadButtonButton.Text = "Load Position";
            loadButtonButton.UseVisualStyleBackColor = true;
            loadButtonButton.Click += loadPosition;
            // 
            // savedXPosLabel
            // 
            savedXPosLabel.AutoSize = true;
            savedXPosLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            savedXPosLabel.Location = new Point(4, 681);
            savedXPosLabel.Margin = new Padding(4, 0, 4, 0);
            savedXPosLabel.Name = "savedXPosLabel";
            savedXPosLabel.Size = new Size(233, 48);
            savedXPosLabel.TabIndex = 3;
            savedXPosLabel.Text = "Saved X Pos:";
            // 
            // savedYPosLabel
            // 
            savedYPosLabel.AutoSize = true;
            savedYPosLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            savedYPosLabel.Location = new Point(4, 734);
            savedYPosLabel.Margin = new Padding(4, 0, 4, 0);
            savedYPosLabel.Name = "savedYPosLabel";
            savedYPosLabel.Size = new Size(231, 48);
            savedYPosLabel.TabIndex = 4;
            savedYPosLabel.Text = "Saved Y Pos:";
            // 
            // savedZPosLabel
            // 
            savedZPosLabel.AutoSize = true;
            savedZPosLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            savedZPosLabel.Location = new Point(4, 786);
            savedZPosLabel.Margin = new Padding(4, 0, 4, 0);
            savedZPosLabel.Name = "savedZPosLabel";
            savedZPosLabel.Size = new Size(231, 48);
            savedZPosLabel.TabIndex = 5;
            savedZPosLabel.Text = "Saved Z Pos:";
            // 
            // currentXPosLabel
            // 
            currentXPosLabel.AutoSize = true;
            currentXPosLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            currentXPosLabel.Location = new Point(638, 536);
            currentXPosLabel.Margin = new Padding(4, 0, 4, 0);
            currentXPosLabel.Name = "currentXPosLabel";
            currentXPosLabel.RightToLeft = RightToLeft.No;
            currentXPosLabel.Size = new Size(260, 48);
            currentXPosLabel.TabIndex = 8;
            currentXPosLabel.Text = "Current X Pos:";
            // 
            // currentYPosLabel
            // 
            currentYPosLabel.AutoSize = true;
            currentYPosLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            currentYPosLabel.Location = new Point(641, 593);
            currentYPosLabel.Margin = new Padding(4, 0, 4, 0);
            currentYPosLabel.Name = "currentYPosLabel";
            currentYPosLabel.Size = new Size(258, 48);
            currentYPosLabel.TabIndex = 7;
            currentYPosLabel.Text = "Current Y Pos:";
            // 
            // currentZPosLabel
            // 
            currentZPosLabel.AutoSize = true;
            currentZPosLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            currentZPosLabel.Location = new Point(640, 645);
            currentZPosLabel.Margin = new Padding(4, 0, 4, 0);
            currentZPosLabel.Name = "currentZPosLabel";
            currentZPosLabel.Size = new Size(258, 48);
            currentZPosLabel.TabIndex = 6;
            currentZPosLabel.Text = "Current Z Pos:";
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // speedLabel
            // 
            speedLabel.AutoSize = true;
            speedLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            speedLabel.Location = new Point(640, 440);
            speedLabel.Margin = new Padding(4, 0, 4, 0);
            speedLabel.Name = "speedLabel";
            speedLabel.Size = new Size(142, 48);
            speedLabel.TabIndex = 9;
            speedLabel.Text = "Speed: ";
            // 
            // maxRingsButton
            // 
            maxRingsButton.Location = new Point(120, 256);
            maxRingsButton.Margin = new Padding(4, 5, 4, 5);
            maxRingsButton.Name = "maxRingsButton";
            maxRingsButton.Size = new Size(336, 117);
            maxRingsButton.TabIndex = 10;
            maxRingsButton.Text = "Give 999 Rings";
            maxRingsButton.UseVisualStyleBackColor = true;
            maxRingsButton.Click += giveMaxRings;
            // 
            // saveToSlotDropdown
            // 
            saveToSlotDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            saveToSlotDropdown.Location = new Point(1000, 200);
            saveToSlotDropdown.Margin = new Padding(4, 5, 4, 5);
            saveToSlotDropdown.Name = "saveToSlotDropdown";
            saveToSlotDropdown.Size = new Size(184, 33);
            saveToSlotDropdown.TabIndex = 11;
            // 
            // saveToSlotLabel
            // 
            saveToSlotLabel.AutoSize = true;
            saveToSlotLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            saveToSlotLabel.Location = new Point(643, 195);
            saveToSlotLabel.Margin = new Padding(4, 0, 4, 0);
            saveToSlotLabel.Name = "saveToSlotLabel";
            saveToSlotLabel.Size = new Size(261, 32);
            saveToSlotLabel.TabIndex = 12;
            saveToSlotLabel.Text = "Save position to slot: ";
            // 
            // loadFromSlotDropdown
            // 
            loadFromSlotDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            loadFromSlotDropdown.Location = new Point(1000, 367);
            loadFromSlotDropdown.Margin = new Padding(4, 5, 4, 5);
            loadFromSlotDropdown.Name = "loadFromSlotDropdown";
            loadFromSlotDropdown.Size = new Size(184, 33);
            loadFromSlotDropdown.TabIndex = 13;
            // 
            // loadFromSlotLabel
            // 
            loadFromSlotLabel.AutoSize = true;
            loadFromSlotLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            loadFromSlotLabel.Location = new Point(643, 362);
            loadFromSlotLabel.Margin = new Padding(4, 0, 4, 0);
            loadFromSlotLabel.Name = "loadFromSlotLabel";
            loadFromSlotLabel.Size = new Size(296, 32);
            loadFromSlotLabel.TabIndex = 14;
            loadFromSlotLabel.Text = "Load position from slot: ";
            // 
            // loadedSlotsLabel
            // 
            loadedSlotsLabel.AutoSize = true;
            loadedSlotsLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            loadedSlotsLabel.Location = new Point(4, 647);
            loadedSlotsLabel.Margin = new Padding(4, 0, 4, 0);
            loadedSlotsLabel.Name = "loadedSlotsLabel";
            loadedSlotsLabel.Size = new Size(449, 32);
            loadedSlotsLabel.TabIndex = 15;
            loadedSlotsLabel.Text = "Showing positions stored in slots 0 : 0";
            // 
            // facingAngleLabel
            // 
            facingAngleLabel.AutoSize = true;
            facingAngleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            facingAngleLabel.Location = new Point(641, 713);
            facingAngleLabel.Margin = new Padding(4, 0, 4, 0);
            facingAngleLabel.Name = "facingAngleLabel";
            facingAngleLabel.Size = new Size(148, 48);
            facingAngleLabel.TabIndex = 16;
            facingAngleLabel.Text = "Facing: ";
            // 
            // jsonSaveButton
            // 
            jsonSaveButton.Location = new Point(11, 856);
            jsonSaveButton.Margin = new Padding(4, 5, 4, 5);
            jsonSaveButton.Name = "jsonSaveButton";
            jsonSaveButton.Size = new Size(143, 67);
            jsonSaveButton.TabIndex = 17;
            jsonSaveButton.Text = "Save to JSON";
            jsonSaveButton.UseVisualStyleBackColor = true;
            jsonSaveButton.Click += saveToJSON;
            // 
            // jsonLoadButton
            // 
            jsonLoadButton.Location = new Point(211, 856);
            jsonLoadButton.Margin = new Padding(4, 5, 4, 5);
            jsonLoadButton.Name = "jsonLoadButton";
            jsonLoadButton.Size = new Size(143, 67);
            jsonLoadButton.TabIndex = 18;
            jsonLoadButton.Text = "Load JSON";
            jsonLoadButton.UseVisualStyleBackColor = true;
            jsonLoadButton.Click += loadJSON;
            // 
            // loadFromDropdown
            // 
            loadFromDropdown.DisplayMember = "Value";
            loadFromDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            loadFromDropdown.Location = new Point(211, 939);
            loadFromDropdown.Margin = new Padding(4, 5, 4, 5);
            loadFromDropdown.Name = "loadFromDropdown";
            loadFromDropdown.Size = new Size(213, 33);
            loadFromDropdown.TabIndex = 19;
            loadFromDropdown.ValueMember = "Key";
            // 
            // loadFromLabel
            // 
            loadFromLabel.AutoSize = true;
            loadFromLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            loadFromLabel.Location = new Point(-3, 939);
            loadFromLabel.Margin = new Padding(4, 0, 4, 0);
            loadFromLabel.Name = "loadFromLabel";
            loadFromLabel.Size = new Size(203, 32);
            loadFromLabel.TabIndex = 20;
            loadFromLabel.Text = "Load data from: ";
            // 
            // gameVersionLabel
            // 
            gameVersionLabel.AutoSize = true;
            gameVersionLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            gameVersionLabel.Location = new Point(641, 790);
            gameVersionLabel.Margin = new Padding(4, 0, 4, 0);
            gameVersionLabel.Name = "gameVersionLabel";
            gameVersionLabel.Size = new Size(212, 38);
            gameVersionLabel.TabIndex = 23;
            gameVersionLabel.Text = "Game version: ";
            // 
            // wipeSavesButton
            // 
            wipeSavesButton.Location = new Point(120, 614);
            wipeSavesButton.Margin = new Padding(4, 5, 4, 5);
            wipeSavesButton.Name = "wipeSavesButton";
            wipeSavesButton.Size = new Size(214, 33);
            wipeSavesButton.TabIndex = 22;
            wipeSavesButton.Text = "Wipe save slots";
            wipeSavesButton.UseVisualStyleBackColor = true;
            wipeSavesButton.Click += wipeSaves;
            // 
            // gameVersionDropdown
            // 
            gameVersionDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            gameVersionDropdown.Items.AddRange(new object[] { "Current", "Old" });
            gameVersionDropdown.Location = new Point(858, 798);
            gameVersionDropdown.Margin = new Padding(4, 5, 4, 5);
            gameVersionDropdown.Name = "gameVersionDropdown";
            gameVersionDropdown.Size = new Size(213, 33);
            gameVersionDropdown.TabIndex = 24;
            gameVersionDropdown.SelectedValueChanged += gameVersionDropdown_changed;
            // 
            // detailedSpeedToggle
            // 
            detailedSpeedToggle.AutoSize = true;
            detailedSpeedToggle.Location = new Point(1044, 511);
            detailedSpeedToggle.Margin = new Padding(4, 5, 4, 5);
            detailedSpeedToggle.Name = "detailedSpeedToggle";
            detailedSpeedToggle.Size = new Size(22, 21);
            detailedSpeedToggle.TabIndex = 25;
            // 
            // detailedSpeedLabel
            // 
            detailedSpeedLabel.AutoSize = true;
            detailedSpeedLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            detailedSpeedLabel.Location = new Point(641, 498);
            detailedSpeedLabel.Margin = new Padding(4, 0, 4, 0);
            detailedSpeedLabel.Name = "detailedSpeedLabel";
            detailedSpeedLabel.Size = new Size(393, 38);
            detailedSpeedLabel.TabIndex = 26;
            detailedSpeedLabel.Text = "Show detailed speed values: ";
            // 
            // chargeChaosControlButton
            // 
            chargeChaosControlButton.Location = new Point(120, 399);
            chargeChaosControlButton.Name = "chargeChaosControlButton";
            chargeChaosControlButton.Size = new Size(336, 117);
            chargeChaosControlButton.TabIndex = 27;
            chargeChaosControlButton.Text = "Charge Chaos Control";
            chargeChaosControlButton.UseVisualStyleBackColor = true;
            chargeChaosControlButton.Click += chargeChaosControl;
            // 
            // boostCheatLabel
            // 
            boostCheatLabel.AutoSize = true;
            boostCheatLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            boostCheatLabel.Location = new Point(120, 547);
            boostCheatLabel.Margin = new Padding(4, 0, 4, 0);
            boostCheatLabel.Name = "boostCheatLabel";
            boostCheatLabel.Size = new Size(203, 38);
            boostCheatLabel.TabIndex = 29;
            boostCheatLabel.Text = "Infinite boost:";
            // 
            // boostCheatToggle
            // 
            boostCheatToggle.AutoSize = true;
            boostCheatToggle.Location = new Point(332, 561);
            boostCheatToggle.Name = "boostCheatToggle";
            boostCheatToggle.Size = new Size(22, 21);
            boostCheatToggle.TabIndex = 30;
            boostCheatToggle.UseVisualStyleBackColor = true;
            // 
            // warningLabel
            // 
            warningLabel.AutoSize = true;
            warningLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            warningLabel.Location = new Point(609, 857);
            warningLabel.Margin = new Padding(4, 0, 4, 0);
            warningLabel.Name = "warningLabel";
            warningLabel.Size = new Size(55, 32);
            warningLabel.TabIndex = 31;
            warningLabel.Text = "aga";
            // 
            // xPosInput
            // 
            xPosInput.Location = new Point(603, 973);
            xPosInput.Name = "xPosInput";
            xPosInput.Size = new Size(106, 31);
            xPosInput.TabIndex = 32;
            // 
            // yPosInput
            // 
            yPosInput.Location = new Point(747, 973);
            yPosInput.Name = "yPosInput";
            yPosInput.Size = new Size(106, 31);
            yPosInput.TabIndex = 33;
            // 
            // zPosInput
            // 
            zPosInput.Location = new Point(895, 973);
            zPosInput.Name = "zPosInput";
            zPosInput.Size = new Size(106, 31);
            zPosInput.TabIndex = 34;
            // 
            // manualTeleportButton
            // 
            manualTeleportButton.Location = new Point(1044, 973);
            manualTeleportButton.Margin = new Padding(4, 5, 4, 5);
            manualTeleportButton.Name = "manualTeleportButton";
            manualTeleportButton.Size = new Size(138, 33);
            manualTeleportButton.TabIndex = 35;
            manualTeleportButton.Text = "Teleport";
            manualTeleportButton.UseVisualStyleBackColor = true;
            manualTeleportButton.Click += manualTeleport;
            // 
            // xPosInputLabel
            // 
            xPosInputLabel.AutoSize = true;
            xPosInputLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            xPosInputLabel.Location = new Point(640, 940);
            xPosInputLabel.Margin = new Padding(4, 0, 4, 0);
            xPosInputLabel.Name = "xPosInputLabel";
            xPosInputLabel.Size = new Size(30, 32);
            xPosInputLabel.TabIndex = 36;
            xPosInputLabel.Text = "X";
            // 
            // yPosInputLabel
            // 
            yPosInputLabel.AutoSize = true;
            yPosInputLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            yPosInputLabel.Location = new Point(784, 940);
            yPosInputLabel.Margin = new Padding(4, 0, 4, 0);
            yPosInputLabel.Name = "yPosInputLabel";
            yPosInputLabel.Size = new Size(29, 32);
            yPosInputLabel.TabIndex = 37;
            yPosInputLabel.Text = "Y";
            // 
            // zPosInputLabel
            // 
            zPosInputLabel.AutoSize = true;
            zPosInputLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            zPosInputLabel.Location = new Point(936, 940);
            zPosInputLabel.Margin = new Padding(4, 0, 4, 0);
            zPosInputLabel.Name = "zPosInputLabel";
            zPosInputLabel.Size = new Size(29, 32);
            zPosInputLabel.TabIndex = 38;
            zPosInputLabel.Text = "Z";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1214, 1016);
            Controls.Add(zPosInputLabel);
            Controls.Add(yPosInputLabel);
            Controls.Add(xPosInputLabel);
            Controls.Add(manualTeleportButton);
            Controls.Add(zPosInput);
            Controls.Add(yPosInput);
            Controls.Add(xPosInput);
            Controls.Add(warningLabel);
            Controls.Add(boostCheatToggle);
            Controls.Add(boostCheatLabel);
            Controls.Add(chargeChaosControlButton);
            Controls.Add(detailedSpeedLabel);
            Controls.Add(detailedSpeedToggle);
            Controls.Add(gameVersionDropdown);
            Controls.Add(wipeSavesButton);
            Controls.Add(gameVersionLabel);
            Controls.Add(loadFromLabel);
            Controls.Add(jsonLoadButton);
            Controls.Add(loadFromDropdown);
            Controls.Add(jsonSaveButton);
            Controls.Add(facingAngleLabel);
            Controls.Add(loadedSlotsLabel);
            Controls.Add(loadFromSlotLabel);
            Controls.Add(loadFromSlotDropdown);
            Controls.Add(saveToSlotLabel);
            Controls.Add(saveToSlotDropdown);
            Controls.Add(maxRingsButton);
            Controls.Add(speedLabel);
            Controls.Add(currentXPosLabel);
            Controls.Add(currentYPosLabel);
            Controls.Add(currentZPosLabel);
            Controls.Add(savedZPosLabel);
            Controls.Add(savedYPosLabel);
            Controls.Add(savedXPosLabel);
            Controls.Add(loadButtonButton);
            Controls.Add(savePositionButton);
            Controls.Add(attachButton);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Chaos Spear";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button attachButton;
        private Button savePositionButton;
        private Button loadButtonButton;
        private Label savedXPosLabel;
        private Label savedYPosLabel;
        private Label savedZPosLabel;
        private Label currentXPosLabel;
        private Label currentYPosLabel;
        private Label currentZPosLabel;
        private System.Windows.Forms.Timer timer1;
        private Label speedLabel;
        private Button maxRingsButton;
        private ComboBox saveToSlotDropdown;
        private Label saveToSlotLabel;
        private ComboBox loadFromSlotDropdown;
        private Label loadFromSlotLabel;
        private Label loadedSlotsLabel;
        private Label facingAngleLabel;
        private Button jsonSaveButton;
        private Button jsonLoadButton;
        private ComboBox loadFromDropdown;
        private Label loadFromLabel;
        private Label gameVersionLabel;
        private Button wipeSavesButton;
        private ComboBox gameVersionDropdown;
        private CheckBox detailedSpeedToggle;
        private Label detailedSpeedLabel;
        private Button chargeChaosControlButton;
        private Label boostCheatLabel;
        private CheckBox boostCheatToggle;
        private Label warningLabel;
        private TextBox xPosInput;
        private TextBox yPosInput;
        private TextBox zPosInput;
        private Button manualTeleportButton;
        private Label xPosInputLabel;
        private Label yPosInputLabel;
        private Label zPosInputLabel;
    }
}
