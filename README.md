# Chaos Spear
A practice tool for Shadow Generations. 

**This program currently doesn't work on patch 1.10.0.1, sorry!**
**THIS IS NOT A MOD, THIS IS A SEPARATE PROGRAM**

**PLEASE REFRAIN FROM USING THIS TOOL IN ONLINE MODE**

## Features:
- Save and load position and rotation, with 10 slots for storing data.
- Save and load positions to and from JSON files.
- Give yourself 999 rings at any time.

## How to download:
- Download the .zip folder from the most recent release, and unzip it.

  **OR**

- Clone this repository, and build the program yourself using the .NET framework.

## How to run:
- Load Chaos Spear by running Chaos_Spear.exe, and load Shadow Generations at the same time.
- Choose your game version in the dropdown. Choose "Current" if you are playing on a version that is compatible with the Tokyo DLC, choose "Old" if you are playing on a version from before the Tokyo DLC release
- When both are open and running, click the "Attach" button on Chaos Spear. It should change to say "Detach". If successful, Chaos Spear is now linked to the game.
- Once in a playable level (e.g. White Space, a boss, a main stage etc), use the "Save Position" button to save Shadow's current position. Choose a save slot to save to using the dropdown labelled "Save position to slot:". Saving to a slot with data already in it will override that data, so be careful!
- Use the "Load Position" button to load the position data saved in the current load slot. Choose a slot to load to using the dropdown labelled "Load positon from slot:". All empty slots default to 0, 0, 0. There's no need to worry about loading from an empty slot.
- Use the "Give 999 Rings" button to immediately grant yourself 999 rings.
- Use the "Save to JSON" button to save all of your current slots to a JSON file (name is hardcoded to saves/save.json).
- Use the "Load from JSON" button to load the data from the file selected in the dropdown.

## Notes:
- You can see the data stored in both the slot you have set to save to, and the slot you have set to load from in the bottom left corner. It is formatted like: "Saved [AXIS] Pos: [data for the slot selected to save to] : [data for the slot selected to load from]" 
- Messing around with positions may cause levels to break, particularly in more scripted areas like the RH sections of SCA1, RC1, and SH1; and bosses. If anything breaks, quitting/retrying the level, dying, or reloading your save should fix everything.
- Teleporting across large distances of a level may cause you to land in a random kill plane even when your destination is safe in normal gameplay, especially if you are teleporting to a position behind your current one. To fix this when teleporting forward, use the save slots to save positions at the end of segments of the level, and teleport segment by segment until you reach your destination. This should trick the game into thinking you've progressed through the level properly, and therefore remove any kill planes. The best way to fix this when teleporting backwards is to just retry.
- Attempting to save, load, or give yourself rings while not in a level (e.g. while in the main menu) will cause Chaos Spear to break (this won't cause any damage to the game). So don't do that :D.
- As stated before, please do not use this tool in online mode. SEGA has expressed interest in finding and removing hackers/hacked times from the Shadow Generations in-game leaderboards. While it is unlikely that they will ban anyone from the leaderboards for using tools like Chaos Spear in online mode, it is still a possibility. It is safer to use this tool only in single player.
- Obviously do not use this tool to actually set and submit erroneous times to the in-game leaderboards. I should not have to explain why this isn't ok. This also puts your account at greater risk of being affected by any ban waves.
- The way Chaos Spear saves position is by grabbing Shadow's coordinates from the game's memory and storing them in the program. It loads positions by injecting those coordinates into the games memory. What this means is you can save coordinates in one stage (e.g RH1) and load them in another stage (e.g SCA2). This isn't very useful, but its cool.
- This also means you can transfer coordinates from one save to another.
- Because all slots default to 0, 0, 0, you can use Chaos Spear to find what the origin of every level is. For example, the origin of White Space is just above the collection room.
- The JSON format used has an array of strings before any of the position data, this is the name of every save slot. You'll have to manually edit this if you want to change any names. (Don't make the names too long, you will run out of space in the window :P)
- There are saves included in the download for every level, including position data for most big skips/sections
