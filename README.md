# UAAS Mods

UnityModManager 0.21.4 supported; Add the following to the UnityModManagerConfig.xml:

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

Download and Install the CheatMod.zip file from the git repo. Use the Unity Mod Manager UI to change settings


CheatMod:
- Speed Button Multiplier (Changes speed multiplier value by this multiplier, making game go faster)
- Minimum Reputation
- Minimum Gold
- Inf Rifle/Cannon shop stock (Buy once to update)
- Take No Damage (Land)
- Max Condition (Land and Sea)
- Max Morale (Land and Sea)
- Max Supply (Land)