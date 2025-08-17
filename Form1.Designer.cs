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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            gameVersionLabel = new Label();
            wipeSavesButton = new Button();
            gameVersionDropdown = new ComboBox();
            chargeChaosControlButton = new Button();
            warningLabel = new Label();
            xPosInput = new TextBox();
            yPosInput = new TextBox();
            zPosInput = new TextBox();
            manualTeleportButton = new Button();
            xPosInputLabel = new Label();
            yPosInputLabel = new Label();
            zPosInputLabel = new Label();
            cheatsGroupBox = new GroupBox();
            fillCCHotkeyButton = new Button();
            maxRingsHotkeyButton = new Button();
            savePosHotkeyButton = new Button();
            loadPosHotkeyButton = new Button();
            boostCheatHotkeyButton = new Button();
            loadPosHotkeyLabel = new Label();
            maxRingsHotkeyLabel = new Label();
            savePosHotkeyLabel = new Label();
            boostCheatHotkeyLabel = new Label();
            fillCCHotkeyLabel = new Label();
            hotkeyHeaderLabel = new Label();
            boostCheatButton = new Button();
            cheatsHeadingLabel = new Label();
            currentPositionHeaderLabel = new Label();
            manualTeleportHeaderLabel = new Label();
            attachLabel = new Label();
            cheatsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // attachButton
            // 
            attachButton.Font = new Font("Segoe UI", 16F);
            attachButton.Image = (Image)resources.GetObject("attachButton.Image");
            attachButton.Location = new Point(35, 7);
            attachButton.Name = "attachButton";
            attachButton.Size = new Size(87, 77);
            attachButton.TabIndex = 0;
            attachButton.UseVisualStyleBackColor = true;
            attachButton.Click += attach;
            // 
            // savePositionButton
            // 
            savePositionButton.Font = new Font("Segoe UI", 13F);
            savePositionButton.Location = new Point(30, 149);
            savePositionButton.Name = "savePositionButton";
            savePositionButton.Size = new Size(178, 60);
            savePositionButton.TabIndex = 1;
            savePositionButton.Text = "Save Position";
            savePositionButton.UseVisualStyleBackColor = true;
            savePositionButton.Click += savePosition;
            // 
            // loadButtonButton
            // 
            loadButtonButton.Font = new Font("Segoe UI", 13F);
            loadButtonButton.Location = new Point(265, 149);
            loadButtonButton.Name = "loadButtonButton";
            loadButtonButton.Size = new Size(178, 60);
            loadButtonButton.TabIndex = 2;
            loadButtonButton.Text = "Load Position";
            loadButtonButton.UseVisualStyleBackColor = true;
            loadButtonButton.Click += loadPosition;
            // 
            // savedXPosLabel
            // 
            savedXPosLabel.AutoSize = true;
            savedXPosLabel.Font = new Font("Segoe UI", 12F);
            savedXPosLabel.Location = new Point(30, 319);
            savedXPosLabel.Name = "savedXPosLabel";
            savedXPosLabel.Size = new Size(22, 21);
            savedXPosLabel.TabIndex = 3;
            savedXPosLabel.Text = "X:";
            // 
            // savedYPosLabel
            // 
            savedYPosLabel.AutoSize = true;
            savedYPosLabel.Font = new Font("Segoe UI", 12F);
            savedYPosLabel.Location = new Point(30, 349);
            savedYPosLabel.Name = "savedYPosLabel";
            savedYPosLabel.Size = new Size(22, 21);
            savedYPosLabel.TabIndex = 4;
            savedYPosLabel.Text = "Y:";
            // 
            // savedZPosLabel
            // 
            savedZPosLabel.AutoSize = true;
            savedZPosLabel.Font = new Font("Segoe UI", 12F);
            savedZPosLabel.Location = new Point(30, 379);
            savedZPosLabel.Name = "savedZPosLabel";
            savedZPosLabel.Size = new Size(22, 21);
            savedZPosLabel.TabIndex = 5;
            savedZPosLabel.Text = "Z:";
            // 
            // currentXPosLabel
            // 
            currentXPosLabel.AutoSize = true;
            currentXPosLabel.Font = new Font("Segoe UI", 12F);
            currentXPosLabel.Location = new Point(265, 319);
            currentXPosLabel.Name = "currentXPosLabel";
            currentXPosLabel.RightToLeft = RightToLeft.No;
            currentXPosLabel.Size = new Size(22, 21);
            currentXPosLabel.TabIndex = 8;
            currentXPosLabel.Text = "X:";
            // 
            // currentYPosLabel
            // 
            currentYPosLabel.AutoSize = true;
            currentYPosLabel.Font = new Font("Segoe UI", 12F);
            currentYPosLabel.Location = new Point(265, 349);
            currentYPosLabel.Name = "currentYPosLabel";
            currentYPosLabel.Size = new Size(22, 21);
            currentYPosLabel.TabIndex = 7;
            currentYPosLabel.Text = "Y:";
            // 
            // currentZPosLabel
            // 
            currentZPosLabel.AutoSize = true;
            currentZPosLabel.Font = new Font("Segoe UI", 12F);
            currentZPosLabel.Location = new Point(265, 379);
            currentZPosLabel.Name = "currentZPosLabel";
            currentZPosLabel.Size = new Size(22, 21);
            currentZPosLabel.TabIndex = 6;
            currentZPosLabel.Text = "Z:";
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // speedLabel
            // 
            speedLabel.AutoSize = true;
            speedLabel.Font = new Font("Segoe UI", 16F);
            speedLabel.Location = new Point(13, 454);
            speedLabel.Name = "speedLabel";
            speedLabel.Size = new Size(86, 30);
            speedLabel.TabIndex = 9;
            speedLabel.Text = "Speed: ";
            // 
            // maxRingsButton
            // 
            maxRingsButton.Image = (Image)resources.GetObject("maxRingsButton.Image");
            maxRingsButton.Location = new Point(32, 85);
            maxRingsButton.Name = "maxRingsButton";
            maxRingsButton.Size = new Size(87, 76);
            maxRingsButton.TabIndex = 10;
            maxRingsButton.UseVisualStyleBackColor = true;
            maxRingsButton.Click += giveMaxRings;
            // 
            // saveToSlotDropdown
            // 
            saveToSlotDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            saveToSlotDropdown.Location = new Point(59, 236);
            saveToSlotDropdown.Name = "saveToSlotDropdown";
            saveToSlotDropdown.Size = new Size(130, 23);
            saveToSlotDropdown.TabIndex = 11;
            // 
            // saveToSlotLabel
            // 
            saveToSlotLabel.AutoSize = true;
            saveToSlotLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            saveToSlotLabel.Location = new Point(40, 212);
            saveToSlotLabel.Name = "saveToSlotLabel";
            saveToSlotLabel.Size = new Size(173, 21);
            saveToSlotLabel.TabIndex = 12;
            saveToSlotLabel.Text = "Save position to slot: ";
            // 
            // loadFromSlotDropdown
            // 
            loadFromSlotDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            loadFromSlotDropdown.Location = new Point(289, 236);
            loadFromSlotDropdown.Name = "loadFromSlotDropdown";
            loadFromSlotDropdown.Size = new Size(130, 23);
            loadFromSlotDropdown.TabIndex = 13;
            // 
            // loadFromSlotLabel
            // 
            loadFromSlotLabel.AutoSize = true;
            loadFromSlotLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            loadFromSlotLabel.Location = new Point(259, 212);
            loadFromSlotLabel.Name = "loadFromSlotLabel";
            loadFromSlotLabel.Size = new Size(195, 21);
            loadFromSlotLabel.TabIndex = 14;
            loadFromSlotLabel.Text = "Load position from slot: ";
            // 
            // loadedSlotsLabel
            // 
            loadedSlotsLabel.AutoSize = true;
            loadedSlotsLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            loadedSlotsLabel.Location = new Point(30, 292);
            loadedSlotsLabel.Name = "loadedSlotsLabel";
            loadedSlotsLabel.Size = new Size(209, 21);
            loadedSlotsLabel.TabIndex = 15;
            loadedSlotsLabel.Text = "Saved Positions (Slots 0:0)";
            // 
            // facingAngleLabel
            // 
            facingAngleLabel.AutoSize = true;
            facingAngleLabel.Font = new Font("Segoe UI", 16F);
            facingAngleLabel.Location = new Point(369, 454);
            facingAngleLabel.Name = "facingAngleLabel";
            facingAngleLabel.Size = new Size(85, 30);
            facingAngleLabel.TabIndex = 16;
            facingAngleLabel.Text = "Facing: ";
            // 
            // jsonSaveButton
            // 
            jsonSaveButton.Location = new Point(17, 536);
            jsonSaveButton.Name = "jsonSaveButton";
            jsonSaveButton.Size = new Size(100, 40);
            jsonSaveButton.TabIndex = 17;
            jsonSaveButton.Text = "Save to JSON";
            jsonSaveButton.UseVisualStyleBackColor = true;
            jsonSaveButton.Click += saveToJSON;
            // 
            // jsonLoadButton
            // 
            jsonLoadButton.Location = new Point(137, 536);
            jsonLoadButton.Name = "jsonLoadButton";
            jsonLoadButton.Size = new Size(100, 40);
            jsonLoadButton.TabIndex = 18;
            jsonLoadButton.Text = "Load JSON";
            jsonLoadButton.UseVisualStyleBackColor = true;
            jsonLoadButton.Click += loadJSON;
            // 
            // gameVersionLabel
            // 
            gameVersionLabel.AutoSize = true;
            gameVersionLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            gameVersionLabel.Location = new Point(372, 7);
            gameVersionLabel.Name = "gameVersionLabel";
            gameVersionLabel.Size = new Size(143, 25);
            gameVersionLabel.TabIndex = 23;
            gameVersionLabel.Text = "Game version: ";
            // 
            // wipeSavesButton
            // 
            wipeSavesButton.Location = new Point(255, 536);
            wipeSavesButton.Name = "wipeSavesButton";
            wipeSavesButton.Size = new Size(100, 40);
            wipeSavesButton.TabIndex = 22;
            wipeSavesButton.Text = "Wipe save slots";
            wipeSavesButton.UseVisualStyleBackColor = true;
            wipeSavesButton.Click += wipeSaves;
            // 
            // gameVersionDropdown
            // 
            gameVersionDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            gameVersionDropdown.Items.AddRange(new object[] { "Current", "Old" });
            gameVersionDropdown.Location = new Point(521, 12);
            gameVersionDropdown.Name = "gameVersionDropdown";
            gameVersionDropdown.Size = new Size(150, 23);
            gameVersionDropdown.TabIndex = 24;
            gameVersionDropdown.SelectedValueChanged += gameVersionDropdown_changed;
            // 
            // chargeChaosControlButton
            // 
            chargeChaosControlButton.Image = (Image)resources.GetObject("chargeChaosControlButton.Image");
            chargeChaosControlButton.Location = new Point(32, 179);
            chargeChaosControlButton.Margin = new Padding(2);
            chargeChaosControlButton.Name = "chargeChaosControlButton";
            chargeChaosControlButton.Size = new Size(87, 76);
            chargeChaosControlButton.TabIndex = 27;
            chargeChaosControlButton.UseVisualStyleBackColor = true;
            chargeChaosControlButton.Click += chargeChaosControl;
            // 
            // warningLabel
            // 
            warningLabel.AutoSize = true;
            warningLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            warningLabel.Location = new Point(369, 48);
            warningLabel.Name = "warningLabel";
            warningLabel.Size = new Size(38, 21);
            warningLabel.TabIndex = 31;
            warningLabel.Text = "aga";
            // 
            // xPosInput
            // 
            xPosInput.Location = new Point(518, 319);
            xPosInput.Margin = new Padding(2);
            xPosInput.Name = "xPosInput";
            xPosInput.Size = new Size(149, 23);
            xPosInput.TabIndex = 32;
            // 
            // yPosInput
            // 
            yPosInput.Location = new Point(518, 349);
            yPosInput.Margin = new Padding(2);
            yPosInput.Name = "yPosInput";
            yPosInput.Size = new Size(150, 23);
            yPosInput.TabIndex = 33;
            // 
            // zPosInput
            // 
            zPosInput.Location = new Point(518, 379);
            zPosInput.Margin = new Padding(2);
            zPosInput.Name = "zPosInput";
            zPosInput.Size = new Size(150, 23);
            zPosInput.TabIndex = 34;
            // 
            // manualTeleportButton
            // 
            manualTeleportButton.Font = new Font("Segoe UI", 13F);
            manualTeleportButton.Location = new Point(499, 149);
            manualTeleportButton.Name = "manualTeleportButton";
            manualTeleportButton.Size = new Size(178, 60);
            manualTeleportButton.TabIndex = 35;
            manualTeleportButton.Text = "Set Position";
            manualTeleportButton.UseVisualStyleBackColor = true;
            manualTeleportButton.Click += manualTeleport;
            // 
            // xPosInputLabel
            // 
            xPosInputLabel.AutoSize = true;
            xPosInputLabel.Font = new Font("Segoe UI", 12F);
            xPosInputLabel.Location = new Point(489, 319);
            xPosInputLabel.Name = "xPosInputLabel";
            xPosInputLabel.Size = new Size(22, 21);
            xPosInputLabel.TabIndex = 36;
            xPosInputLabel.Text = "X:";
            // 
            // yPosInputLabel
            // 
            yPosInputLabel.AutoSize = true;
            yPosInputLabel.Font = new Font("Segoe UI", 12F);
            yPosInputLabel.Location = new Point(489, 349);
            yPosInputLabel.Name = "yPosInputLabel";
            yPosInputLabel.Size = new Size(22, 21);
            yPosInputLabel.TabIndex = 37;
            yPosInputLabel.Text = "Y:";
            // 
            // zPosInputLabel
            // 
            zPosInputLabel.AutoSize = true;
            zPosInputLabel.Font = new Font("Segoe UI", 12F);
            zPosInputLabel.Location = new Point(489, 379);
            zPosInputLabel.Name = "zPosInputLabel";
            zPosInputLabel.Size = new Size(22, 21);
            zPosInputLabel.TabIndex = 38;
            zPosInputLabel.Text = "Z:";
            // 
            // cheatsGroupBox
            // 
            cheatsGroupBox.BackColor = SystemColors.ControlLight;
            cheatsGroupBox.Controls.Add(fillCCHotkeyButton);
            cheatsGroupBox.Controls.Add(maxRingsHotkeyButton);
            cheatsGroupBox.Controls.Add(savePosHotkeyButton);
            cheatsGroupBox.Controls.Add(loadPosHotkeyButton);
            cheatsGroupBox.Controls.Add(boostCheatHotkeyButton);
            cheatsGroupBox.Controls.Add(loadPosHotkeyLabel);
            cheatsGroupBox.Controls.Add(maxRingsHotkeyLabel);
            cheatsGroupBox.Controls.Add(savePosHotkeyLabel);
            cheatsGroupBox.Controls.Add(boostCheatHotkeyLabel);
            cheatsGroupBox.Controls.Add(fillCCHotkeyLabel);
            cheatsGroupBox.Controls.Add(hotkeyHeaderLabel);
            cheatsGroupBox.Controls.Add(boostCheatButton);
            cheatsGroupBox.Controls.Add(cheatsHeadingLabel);
            cheatsGroupBox.Controls.Add(maxRingsButton);
            cheatsGroupBox.Controls.Add(chargeChaosControlButton);
            cheatsGroupBox.Location = new Point(701, -9);
            cheatsGroupBox.Name = "cheatsGroupBox";
            cheatsGroupBox.Size = new Size(150, 626);
            cheatsGroupBox.TabIndex = 41;
            cheatsGroupBox.TabStop = false;
            // 
            // fillCCHotkeyButton
            // 
            fillCCHotkeyButton.Font = new Font("Segoe UI", 7F);
            fillCCHotkeyButton.Location = new Point(80, 450);
            fillCCHotkeyButton.Name = "fillCCHotkeyButton";
            fillCCHotkeyButton.Size = new Size(64, 27);
            fillCCHotkeyButton.TabIndex = 53;
            fillCCHotkeyButton.UseVisualStyleBackColor = true;
            fillCCHotkeyButton.Click += changeHotkey;
            // 
            // maxRingsHotkeyButton
            // 
            maxRingsHotkeyButton.Font = new Font("Segoe UI", 7F);
            maxRingsHotkeyButton.Location = new Point(80, 491);
            maxRingsHotkeyButton.Name = "maxRingsHotkeyButton";
            maxRingsHotkeyButton.Size = new Size(64, 27);
            maxRingsHotkeyButton.TabIndex = 52;
            maxRingsHotkeyButton.UseVisualStyleBackColor = true;
            maxRingsHotkeyButton.Click += changeHotkey;
            // 
            // savePosHotkeyButton
            // 
            savePosHotkeyButton.Font = new Font("Segoe UI", 7F);
            savePosHotkeyButton.Location = new Point(80, 528);
            savePosHotkeyButton.Name = "savePosHotkeyButton";
            savePosHotkeyButton.Size = new Size(64, 27);
            savePosHotkeyButton.TabIndex = 51;
            savePosHotkeyButton.UseVisualStyleBackColor = true;
            savePosHotkeyButton.Click += changeHotkey;
            // 
            // loadPosHotkeyButton
            // 
            loadPosHotkeyButton.Font = new Font("Segoe UI", 7F);
            loadPosHotkeyButton.Location = new Point(80, 565);
            loadPosHotkeyButton.Name = "loadPosHotkeyButton";
            loadPosHotkeyButton.Size = new Size(64, 27);
            loadPosHotkeyButton.TabIndex = 50;
            loadPosHotkeyButton.UseVisualStyleBackColor = true;
            loadPosHotkeyButton.Click += changeHotkey;
            // 
            // boostCheatHotkeyButton
            // 
            boostCheatHotkeyButton.Font = new Font("Segoe UI", 7F);
            boostCheatHotkeyButton.Location = new Point(80, 417);
            boostCheatHotkeyButton.Name = "boostCheatHotkeyButton";
            boostCheatHotkeyButton.Size = new Size(64, 27);
            boostCheatHotkeyButton.TabIndex = 49;
            boostCheatHotkeyButton.UseVisualStyleBackColor = true;
            boostCheatHotkeyButton.Click += changeHotkey;
            // 
            // loadPosHotkeyLabel
            // 
            loadPosHotkeyLabel.AutoSize = true;
            loadPosHotkeyLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            loadPosHotkeyLabel.Location = new Point(6, 568);
            loadPosHotkeyLabel.Name = "loadPosHotkeyLabel";
            loadPosHotkeyLabel.Size = new Size(70, 19);
            loadPosHotkeyLabel.TabIndex = 48;
            loadPosHotkeyLabel.Text = "Load pos";
            // 
            // maxRingsHotkeyLabel
            // 
            maxRingsHotkeyLabel.AutoSize = true;
            maxRingsHotkeyLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            maxRingsHotkeyLabel.Location = new Point(6, 494);
            maxRingsHotkeyLabel.Name = "maxRingsHotkeyLabel";
            maxRingsHotkeyLabel.Size = new Size(70, 19);
            maxRingsHotkeyLabel.TabIndex = 47;
            maxRingsHotkeyLabel.Text = "999 rings";
            // 
            // savePosHotkeyLabel
            // 
            savePosHotkeyLabel.AutoSize = true;
            savePosHotkeyLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            savePosHotkeyLabel.Location = new Point(6, 531);
            savePosHotkeyLabel.Name = "savePosHotkeyLabel";
            savePosHotkeyLabel.Size = new Size(69, 19);
            savePosHotkeyLabel.TabIndex = 46;
            savePosHotkeyLabel.Text = "Save pos";
            // 
            // boostCheatHotkeyLabel
            // 
            boostCheatHotkeyLabel.AutoSize = true;
            boostCheatHotkeyLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            boostCheatHotkeyLabel.Location = new Point(6, 420);
            boostCheatHotkeyLabel.Name = "boostCheatHotkeyLabel";
            boostCheatHotkeyLabel.Size = new Size(68, 19);
            boostCheatHotkeyLabel.TabIndex = 45;
            boostCheatHotkeyLabel.Text = "Inf boost";
            // 
            // fillCCHotkeyLabel
            // 
            fillCCHotkeyLabel.AutoSize = true;
            fillCCHotkeyLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            fillCCHotkeyLabel.Location = new Point(6, 454);
            fillCCHotkeyLabel.Name = "fillCCHotkeyLabel";
            fillCCHotkeyLabel.Size = new Size(50, 19);
            fillCCHotkeyLabel.TabIndex = 44;
            fillCCHotkeyLabel.Text = "Fill CC";
            // 
            // hotkeyHeaderLabel
            // 
            hotkeyHeaderLabel.AutoSize = true;
            hotkeyHeaderLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            hotkeyHeaderLabel.Location = new Point(22, 377);
            hotkeyHeaderLabel.Name = "hotkeyHeaderLabel";
            hotkeyHeaderLabel.Size = new Size(106, 32);
            hotkeyHeaderLabel.TabIndex = 33;
            hotkeyHeaderLabel.Text = "Hotkeys";
            // 
            // boostCheatButton
            // 
            boostCheatButton.Image = (Image)resources.GetObject("boostCheatButton.Image");
            boostCheatButton.Location = new Point(32, 275);
            boostCheatButton.Margin = new Padding(2);
            boostCheatButton.Name = "boostCheatButton";
            boostCheatButton.Size = new Size(87, 76);
            boostCheatButton.TabIndex = 32;
            boostCheatButton.UseVisualStyleBackColor = true;
            boostCheatButton.Click += toggleBoostCheat;
            // 
            // cheatsHeadingLabel
            // 
            cheatsHeadingLabel.AutoSize = true;
            cheatsHeadingLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cheatsHeadingLabel.Location = new Point(28, 21);
            cheatsHeadingLabel.Name = "cheatsHeadingLabel";
            cheatsHeadingLabel.Size = new Size(89, 32);
            cheatsHeadingLabel.TabIndex = 31;
            cheatsHeadingLabel.Text = "Cheats";
            // 
            // currentPositionHeaderLabel
            // 
            currentPositionHeaderLabel.AutoSize = true;
            currentPositionHeaderLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            currentPositionHeaderLabel.Location = new Point(265, 292);
            currentPositionHeaderLabel.Name = "currentPositionHeaderLabel";
            currentPositionHeaderLabel.Size = new Size(134, 21);
            currentPositionHeaderLabel.TabIndex = 42;
            currentPositionHeaderLabel.Text = "Current Position";
            // 
            // manualTeleportHeaderLabel
            // 
            manualTeleportHeaderLabel.AutoSize = true;
            manualTeleportHeaderLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            manualTeleportHeaderLabel.Location = new Point(489, 292);
            manualTeleportHeaderLabel.Name = "manualTeleportHeaderLabel";
            manualTeleportHeaderLabel.Size = new Size(135, 21);
            manualTeleportHeaderLabel.TabIndex = 43;
            manualTeleportHeaderLabel.Text = "Custom Position";
            // 
            // attachLabel
            // 
            attachLabel.AutoSize = true;
            attachLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            attachLabel.Location = new Point(137, 28);
            attachLabel.Name = "attachLabel";
            attachLabel.Size = new Size(88, 32);
            attachLabel.TabIndex = 32;
            attachLabel.Text = "Attach";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(850, 589);
            Controls.Add(attachLabel);
            Controls.Add(manualTeleportHeaderLabel);
            Controls.Add(currentPositionHeaderLabel);
            Controls.Add(cheatsGroupBox);
            Controls.Add(zPosInputLabel);
            Controls.Add(yPosInputLabel);
            Controls.Add(xPosInputLabel);
            Controls.Add(manualTeleportButton);
            Controls.Add(zPosInput);
            Controls.Add(yPosInput);
            Controls.Add(xPosInput);
            Controls.Add(warningLabel);
            Controls.Add(gameVersionDropdown);
            Controls.Add(wipeSavesButton);
            Controls.Add(gameVersionLabel);
            Controls.Add(jsonLoadButton);
            Controls.Add(jsonSaveButton);
            Controls.Add(facingAngleLabel);
            Controls.Add(loadedSlotsLabel);
            Controls.Add(loadFromSlotLabel);
            Controls.Add(loadFromSlotDropdown);
            Controls.Add(saveToSlotLabel);
            Controls.Add(saveToSlotDropdown);
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
            Name = "Form1";
            Text = "Chaos Spear";
            FormClosed += formClosed;
            Load += Form1_Load;
            cheatsGroupBox.ResumeLayout(false);
            cheatsGroupBox.PerformLayout();
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
        private Label gameVersionLabel;
        private Button wipeSavesButton;
        private ComboBox gameVersionDropdown;
        private Button chargeChaosControlButton;
        private Label warningLabel;
        private TextBox xPosInput;
        private TextBox yPosInput;
        private TextBox zPosInput;
        private Button manualTeleportButton;
        private Label xPosInputLabel;
        private Label yPosInputLabel;
        private Label zPosInputLabel;
        private GroupBox cheatsGroupBox;
        private Label cheatsHeadingLabel;
        private Label currentPositionHeaderLabel;
        private Label manualTeleportHeaderLabel;
        private Label attachLabel;
        private Button boostCheatButton;
        private Label hotkeyHeaderLabel;
        private Label loadPosHotkeyLabel;
        private Label maxRingsHotkeyLabel;
        private Label savePosHotkeyLabel;
        private Label boostCheatHotkeyLabel;
        private Label fillCCHotkeyLabel;
        private Button fillCCHotkeyButton;
        private Button maxRingsHotkeyButton;
        private Button savePosHotkeyButton;
        private Button loadPosHotkeyButton;
        private Button boostCheatHotkeyButton;
    }
}
