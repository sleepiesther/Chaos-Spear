using Chaos_Spear.Properties;
using SharpHook;
using SharpHook.Native;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Chaos_Spear {
	public partial class MainWindow : Form {
		private bool attached;

		private Dictionary<string, KeyCode> hotkeys;
		private string hotKeyToChange;
		private List<SaveSlot> saveSlots = new();
		private bool boostCheat;

		private SimpleGlobalHook kbHook;
		private Task task;

		private JsonSerializerOptions options = new() { WriteIndented = true };


		public MainWindow() {
			Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			// Adds 10 instances of the saveslot struct to act as the save/load slots. 10 is a bit of an arbitrary number, but it's nice to be sure the slot index will always be a single digit number.
			for (int x = 0; x < 10; x++) {
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

		private void ToggleAttach(object? sender, EventArgs e) {
			if (!attached) {
				try {
					MemoryHandler.AttachToGame();
					gameVersionLabel.Text = "Game version: " + Enum.GetName(MemoryHandler.gameVersion).Replace("_", ".");

					attached = true;
					attachLabel.Text = "Detach";
					attachButton.Image = Resources.attachIconAttached;

					timer.Start();
				}
				catch (Exception ex) {
					MessageBox.Show("An error occured! It's possible you attached too early in the game's boot. Please try again later. If this keeps happening please message @sleepiesther on discord and show me this error message: " + ex.Message);
					MemoryHandler.RemoveGameHooks(); // Ensure any hooks that were created before the error are removed
				}
			}
			else {
				try {
					attachLabel.Text = "Attach";
					gameVersionLabel.Text = "Game version: Unknown";
					attachButton.Image = Resources.attachIconDetached;

					// Restore the game back to its vanilla state. Run in background patch isn't removed because it's leaderboard legal
					MemoryHandler.RemoveGameHooks();
					if (boostCheat) ToggleBoostCheat(sender, e);
					timer.Stop();
					kbHook.Dispose();
					attached = false;
				}
				catch (Exception ex) {
					MessageBox.Show("An error occured! This should not happen! Oopsies! Please message @sleepiesther on discord and show her this error message: " + ex.Message);
				}
			}
		}

		private void HandleKeys(object sender, KeyboardHookEventArgs e) {
			if (!attached) return;

			var key = e.Data.KeyCode;
			if (key == hotkeys["boostCheatHotkey"]) {
				ToggleBoostCheat(sender, e);
			}
			else if (key == hotkeys["fillCCHotkey"]) {
				ChargeChaosControl(sender, e);
			}
			else if (key == hotkeys["maxRingsHotkey"]) {
				GiveMaxRings(sender, e);
			}
			else if (key == hotkeys["savePosHotkey"]) {
				SavePosition(sender, e);
			}
			else if (key == hotkeys["loadPosHotkey"]) {
				LoadPosition(sender, e);
			}
		}
		private void ChangeHotkey(object sender, EventArgs e) {
			// There's probably a better way to do this but this works
			Button senderButton = sender as Button;
			hotKeyToChange = senderButton.Name.Replace("Button", "");
			kbHook.KeyPressed -= HandleKeys;
			kbHook.KeyPressed += ReceiveNewHotkey;
			MessageBox.Show("Please press the key you would like to assign to this button, then press OK.", "Change hotkey", MessageBoxButtons.OK, MessageBoxIcon.Information);
			kbHook.KeyPressed += HandleKeys;
			kbHook.KeyPressed -= ReceiveNewHotkey;
		}

		private void ReceiveNewHotkey(object sender, KeyboardHookEventArgs e) {
			var key = e.Data.KeyCode;
			hotkeys[hotKeyToChange] = key;
			var hotkeyButton = Controls.Find(hotKeyToChange + "Button", true).First();
			hotkeyButton.Invoke(new MethodInvoker(delegate { hotkeyButton.Text = key.ToString().Replace("Vc", ""); }));
		}
		private void SavePosition(object sender, EventArgs e) {
			if (saveToSlotDropdown.InvokeRequired) {
				saveToSlotDropdown.Invoke(SavePosition, new object[] { saveToSlotDropdown, EventArgs.Empty });
				return;
			}
			if (!attached) {
				MessageBox.Show("Attach program to SXSG first");
				return;
			}

			if (emptyAutoSave.Checked) {
				for (int x = 0; x < saveSlots.Count; x++) {
					if (!saveSlots[x].hasSaveData) {
						saveToSlotDropdown.SelectedIndex = x;
						break;
					}
				}
			}
			saveSlots[saveToSlotDropdown.SelectedIndex].data = MemoryHandler.currentKParams;
			saveSlots[saveToSlotDropdown.SelectedIndex].hasSaveData = true;
		}

		private void LoadPosition(object sender, EventArgs e) {
			if (loadFromSlotDropdown.InvokeRequired) {
				loadFromSlotDropdown.Invoke(LoadPosition, new object[] { loadFromSlotDropdown, EventArgs.Empty });
				return;
			}
			if (!attached) {
				MessageBox.Show("Attach program to SXSG first");
				return;
			}

			GOCPlayerKinematicParams savedParams = saveSlots[loadFromSlotDropdown.SelectedIndex].data;

			MemoryHandler.WritePosition(savedParams.Pos, savedParams.Rot, resetSpeedCheckbox.Checked);
		}

		private void ManualTeleport(object sender, EventArgs e) {
			if (!attached) {
				MessageBox.Show("Attach program to SXSG first");
				return;
			}
			try {
				MemoryHandler.WritePosition(new Vector3() {
					x = float.Parse(xPosInput.Text, CultureInfo.CurrentCulture),
					y = float.Parse(yPosInput.Text, CultureInfo.CurrentCulture),
					z = float.Parse(zPosInput.Text, CultureInfo.CurrentCulture)
				}, new Quaternion(), resetSpeedCheckbox.Checked);
			}
			catch (FormatException) {
				MessageBox.Show("Please input a number into every box :3");
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void GiveMaxRings(object sender, EventArgs e) {
			if (!attached) {
				MessageBox.Show("Attach program to SXSG first");
				return;
			}
			try {
				MemoryHandler.WriteToStolenAddress("rings", 999);
			}
			catch (Exception exception) {
				MessageBox.Show(exception.Message);
			}

		}
		private void ChargeChaosControl(object sender, EventArgs e) {
			if (!attached) {
				MessageBox.Show("Attach program to SXSG first");
				return;
			}
			try {
				MemoryHandler.WriteToStolenAddress("cc", 100);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ToggleBoostCheat(object sender, EventArgs e) {
			if (!attached) {
				MessageBox.Show("Attach program to SXSG first");
				return;
			}
			try {
				if (boostCheat) {
					MemoryHandler.RestoreInstructions();
					boostCheat = false;
					boostCheatButton.Image = Resources.infiniteBoostDeactivated;
				}
				else {

					MemoryHandler.NOPInstructions();
					// Sets boost to 100. The previous NOPs should prevent boost from ever draining, so this only needs to be done once
					MemoryHandler.WriteToStolenAddress("boost", 100);

					boostCheat = true;
					boostCheatButton.Image = Resources.infiniteBoostActivated;

				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

		}

		private void timer_Tick(object sender, EventArgs e) {
			try {
				MemoryHandler.Update();

				loadedSlotsLabel.Text = "Saved Positions (Slots " + saveToSlotDropdown.SelectedIndex + " : " + loadFromSlotDropdown.SelectedIndex + ")";

				savedXPosLabel.Text = "X: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].data.Pos.x, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].data.Pos.x, 3);

				savedYPosLabel.Text = "Y: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].data.Pos.y, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].data.Pos.y, 3);

				savedZPosLabel.Text = "Z: " + Math.Round(saveSlots[saveToSlotDropdown.SelectedIndex].data.Pos.z, 3) + " : " + Math.Round(saveSlots[loadFromSlotDropdown.SelectedIndex].data.Pos.z, 3);

				currentXPosLabel.Text = "X: " + Math.Round(MemoryHandler.currentKParams.Pos.x, 1);
				currentYPosLabel.Text = "Y: " + Math.Round(MemoryHandler.currentKParams.Pos.y, 1);
				currentZPosLabel.Text = "Z: " + Math.Round(MemoryHandler.currentKParams.Pos.z, 1);

				var speedHorizontal = (float)Math.Round(Math.Sqrt(Math.Pow(MemoryHandler.currentKParams.Spd.x, 2) + Math.Pow(MemoryHandler.currentKParams.Spd.z, 2)), 1);

				speedLabel.Text = "Speed: " + speedHorizontal + " (" + Math.Round(MemoryHandler.currentKParams.Spd.x, 2) + ", " + Math.Round(MemoryHandler.currentKParams.Spd.y, 2) + ", " + Math.Round(MemoryHandler.currentKParams.Spd.z, 2) + ")";

				facingAngleLabel.Text = "Facing: " + Math.Round(Heading(MemoryHandler.currentKParams.Rot.w, MemoryHandler.currentKParams.Rot.y), 1);

				boostCheatHotkeyButton.Text = hotkeys["boostCheatHotkey"].ToString().Replace("Vc", "");
				fillCCHotkeyButton.Text = hotkeys["fillCCHotkey"].ToString().Replace("Vc", "");
				maxRingsHotkeyButton.Text = hotkeys["maxRingsHotkey"].ToString().Replace("Vc", "");
				savePosHotkeyButton.Text = hotkeys["savePosHotkey"].ToString().Replace("Vc", "");
				loadPosHotkeyButton.Text = hotkeys["loadPosHotkey"].ToString().Replace("Vc", "");

			}
			catch (GameExitedException) {
				GameClosed();
			}
			catch {
				// Empty catch because reading memory on a timer can naturally produce errors, and we don't want to spam the user with useless errors.
			}
		}

		private void SaveToJson(object sender, EventArgs e) {
			try {
				var saveFileDialog = new SaveFileDialog();
				saveFileDialog.Filter = "JSON file|*.json";
				saveFileDialog.Title = "Save a JSON file";
				saveFileDialog.ShowDialog();
				if (saveFileDialog.FileName == "") {
					return;
				}
				File.WriteAllText(saveFileDialog.FileName, JsonSerializer.Serialize(saveSlots, options));
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		private void LoadJson(object sender, EventArgs e) {
			try {
				var openFileDialog = new OpenFileDialog();
				openFileDialog.InitialDirectory = Application.ExecutablePath + "\\saves";
				openFileDialog.Filter = "JSON file|*.json";
				openFileDialog.Title = "Open a JSON file";
				openFileDialog.ShowDialog();
				if (openFileDialog.FileName == "") {
					return;
				}

				var jsonString = File.ReadAllText(openFileDialog.FileName);

				JsonNode saveDataNode = JsonNode.Parse(jsonString)!;

				saveSlots.Clear();

				//Handle old and new KParams formatting
				for (int i = 0; i < 10; i++) {
					if (saveDataNode[i]["data"]["XPos"] != null) {
						string name = saveDataNode[i]["name"].GetValue<string>();
						bool hasSaveData = saveDataNode[i]["hasSaveData"].GetValue<bool>();
						Vector3 pos = new Vector3(saveDataNode[i]["data"]["XPos"].GetValue<float>(), saveDataNode[i]["data"]["YPos"].GetValue<float>(), saveDataNode[i]["data"]["ZPos"].GetValue<float>());
						Quaternion rot = new Quaternion(saveDataNode[i]["data"]["QRotX"].GetValue<float>(), saveDataNode[i]["data"]["QRotY"].GetValue<float>(), saveDataNode[i]["data"]["QRotZ"].GetValue<float>(), saveDataNode[i]["data"]["QRotW"].GetValue<float>());
						Vector3 spd = new Vector3();
						saveSlots.Add(new SaveSlot(name, hasSaveData, new GOCPlayerKinematicParams(pos, rot, spd)));
					}
					else {
						saveSlots.Add(saveDataNode[i].Deserialize<SaveSlot>());
					}
				}

				saveToSlotDropdown.Items.Clear();
				loadFromSlotDropdown.Items.Clear();
				renameSaveSlotDropdown.Items.Clear();

				for (int x = 0; x < saveSlots.Count - 1; x++) {
					if (saveSlots[x].name != "") {
						saveToSlotDropdown.Items.Add(x + " (" + saveSlots[x].name + ")");
						loadFromSlotDropdown.Items.Add(x + " (" + saveSlots[x].name + ")");
						renameSaveSlotDropdown.Items.Add(x + " (" + saveSlots[x].name + ")");
					}
					else {
						saveToSlotDropdown.Items.Add(x);
						loadFromSlotDropdown.Items.Add(x);
						renameSaveSlotDropdown.Items.Add(x);
					}
				}
				saveToSlotDropdown.SelectedIndex = loadFromSlotDropdown.SelectedIndex = renameSaveSlotDropdown.SelectedIndex = 0;
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		private void WipeSaves(object sender, EventArgs e) {
			saveToSlotDropdown.Items.Clear();
			loadFromSlotDropdown.Items.Clear();
			renameSaveSlotDropdown.Items.Clear();
			for (var x = 0; x < 10; x++) {
				saveSlots[x] = new SaveSlot();
				saveToSlotDropdown.Items.Add(x);
				loadFromSlotDropdown.Items.Add(x);
				renameSaveSlotDropdown.Items.Add(x);
			}
			saveToSlotDropdown.SelectedIndex = loadFromSlotDropdown.SelectedIndex = renameSaveSlotDropdown.SelectedIndex = 0;
		}
		private void RenameSaveSlots(object sender, EventArgs e) {
			var renamedSlot = renameSaveSlotDropdown.SelectedIndex;
			var newName = renameSaveSlotInput.Text;

			saveSlots[renamedSlot].name = newName;

			if (newName != "") {
				saveToSlotDropdown.Items[renamedSlot] = renamedSlot + " (" + newName + ")";
				loadFromSlotDropdown.Items[renamedSlot] = renamedSlot + " (" + newName + ")";
				renameSaveSlotDropdown.Items[renamedSlot] = renamedSlot + " (" + newName + ")";
			}
			else {
				saveToSlotDropdown.Items[renamedSlot] = renamedSlot;
				loadFromSlotDropdown.Items[renamedSlot] = renamedSlot;
				renameSaveSlotDropdown.Items[renamedSlot] = renamedSlot;
			}
		}

		//This is just copied from Portal Gear, the Sonic Frontiers Save Position Tool
		private float Heading(float rotW, float rotY) {
			float angle = (float)(Math.Acos(rotW) * 2);
			bool sign = rotY > 0;
			if (sign) {
				if (angle < Math.PI) {
					return (float)RadiansToDegrees(-angle + Math.PI);
				}
				else {
					return (float)RadiansToDegrees(2 * Math.PI - angle + Math.PI);
				}
			}
			else {
				return (float)RadiansToDegrees(angle + Math.PI);
			}
		}
		private double RadiansToDegrees(double radians) {
			return radians / Math.PI * 180;
		}

		private void ShowCredits(object sender, EventArgs e) {
			MessageBox.Show("Onaku - Creating the original Chaos Spear, help with using Cheat Engine\n\nLabrys (github.com/Labreezy) - Work on the original Chaos Spear, facing angle code, help with creating game hooks, and other general programming support \n\nJujstme - Version detection code I... borrowed... from the auto splitter \n\nMoha - Entirely redesigning the UI, testing \n\nLayla (www.twitch.tv/yukilayla) - Design feedback, giving me the idea to add custom hotkeys \n\nLillie (@lillypad6199.bsky.social) - Testing \n\nArcanox (@arcanox.me on bsky) - General programming help \n\nMay (@mayberryzoom.bsky.social) - General programming help", "Credits");
		}

		// Make sure a detach is complete if the game or Chaos Spear is closing.
		// Not detaching on game close both looks weird and can cause an error if the user later attempts to detach from a now non-existant process, and not detaching on form close means that later instances of Chaos Spear will be unable to attach to the same process.
		private new void Closing(object sender, FormClosingEventArgs e) {
			if (attached) ToggleAttach(sender, e);
		}

		private void GameClosed() {
			if (attached) ToggleAttach(new object(), new EventArgs());
		}
	}
}