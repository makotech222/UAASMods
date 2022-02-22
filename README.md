# UAAS Mods

UnityModManager 0.21.4 supported; Add the following to the UnityModManagerConfig.xml (not required if you have the latest version of Unity Mod Manager):

```
	<GameInfo Name="Ultimate Admiral Age of Sea">
		<Folder>Ultimate Admiral Age of Sea</Folder>
		<ModsDirectory>Mods</ModsDirectory>
		<ModInfo>Info.json</ModInfo>
		<GameExe>build.exe</GameExe>
		<EntryPoint>[UnityEngine.UI.dll]UnityEngine.EventSystems.EventSystem.cctor:After</EntryPoint>
		<StartingPoint>[Assembly-CSharp.dll]UltimateAdmiral.UI.MainMenuController.Init:Before</StartingPoint>
	</GameInfo>
```

Mod installation instructions:
1. Open Unity Mod Manager and select Ultimate Admiral Age of Sea from the Install tab.
2. Then change the installation method to DoorstopProxy and click Install.
3. Download the CheatMod.zip file from git rep, then in the "Mods" tab, drag the archive into the program window.
4. Try it.

Use the Unity Mod Manager UI to change mod settings.



CheatMod:
- Speed Button Multiplier (Changes speed multiplier value by this multiplier, making game go faster) (* A bit janky with the way the UI controls works. Just visual bug though.)
- Minimum Reputation
- Minimum Gold
- Inf Rifle/Cannon shop stock (Buy once to update)
- Inf Rifle/Cannon armory stock (Buy once to update)
- Take No Damage (Land)
- Max Condition (Land and Sea)
- Max Morale (Land and Sea)
- Max Supply (Land)
- Modify Naval Unit stats (Experimental)
- Modify Land Unit stats (Experimental)
- Cash reward multiplier
- Reputation reward multiplier
- Career Points reward multiplier
- Fast reload land / sea units
- Damage multipliers land / sea units


Changelog:
v1.1:
- Add experimental cheats for editing stats.

v1.2:
Added new features:
- Cash reward multiplier
- Reputation reward multiplier
- Career Points reward multiplier
- Fast reload land / sea units

v1.3:
Added new features:
- Damage multipliers for different parts of the ship
- Ground damage multipliers
- Reload speed multipliers
- Some of the multipliers are now configurable for enemies as well

***********************************************************

Equipment Mod:
- Edit stats of all rifles
- Edit stats of all ship cannons
- Edit stats of all ship modules

Changelog:
v1.2:
- Fix crash when new guns are added to your game

v1.3:
- Expose more properties for rifles/artillery
- Add ShipModule Functionality.

v1.4:
- Improve Entry point + Refactor. Json files will now contains all elements, rather than just what is in your campaign at the time.

v1.5:
- Update to latest version of game.

v1.6:
- Update to latest version of game.


Instructions:
Install the Equipment mod using UnityModManager, start the game and load up a save. Loading a game will cause the mod to export the Rifles.txt and Cannons.txt files to the Mod's install folder inside the game directory. Exit the game, and open the two text files. You can edit the stats
of the items here. When the game is loaded again, the mod will read these files and apply them to the items.
Do not edit the 'ID' property, as it is needed to apply properly. Editing the equipment description or name is not yet supported. 

For ShipModules, you cannot edit title, effect, or description. You can add as many modifier as you want, but probably don't remove any (not tested).
