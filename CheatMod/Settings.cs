using System;
using System.IO;
using UnityEngine;
using UnityModManagerNet;

namespace UAAS
{
	public class Settings : UnityModManager.ModSettings, IDrawable
	{
		[Space(5f), Header("General Cheats")]
		[Space(5f), Draw("Speed Button Multiplier (Ignores 1x)", Precision = 0, Min = 0.0)]
		public int SpeedMultiplier = 1;

		[Space(5f), Draw("Minimum Reputation (Updates when changed)", Precision = 0, Min = 0.0)]
		public int MinimumReputation = 0;

		[Space(5f), Draw("Minimum Cash (Updates when changed)", Precision = 0, Min = 0.0)]
		public int MinimumCash = 0;
		
		[Space(5f), Draw("Cash reward multiplier from mission", Precision = 2, Min = 0.0)]
		public float CashRewMult = 1;
		
		[Space(5f), Draw("Reputation reward multiplier from mission", Precision = 2, Min = 0.0)]
		public float ReputationRewMult = 1;
		
		[Space(5f), Draw("Career Points reward multiplier from mission", Precision = 2, Min = 0.0)]
		public float CareerPointsRewMult = 1;

		[Space(5f), Draw("Inf. Rifle/Cannon Stock in Shop (Buy Once to update)")]
		public bool InfRifleStock = false;

		[Space(5f), Draw("Inf. Rifle/Cannon Stock in Armory (Buy Once to update)")]
		public bool InfRifleStockArmory = false;

		[Space(10f), Header("Land Battle Cheats (Player Only)")]
		[Space(5f), Draw("Take No Damage")]
		public bool NoDamage = false;

		[Space(5f), Draw("Damage Multiplier", Precision = 2, Min = 0.0)]
		public float AllyDamageMultiplier = 1.0f;

		[Space(5f), Draw("Fast reload")]
		public bool LandFastReload = false;

		[Space(5f), Draw("Reload Time Multiplier", Precision = 2, Min = 0.0)]
		public float AllyLandReloadTimeMult = 1.0f;

		[Space(5f), Draw("Max Morale")]
		public bool MaxMorale = false;

		[Space(5f), Draw("Max Condition")]
		public bool MaxCondition = false;

		[Space(5f), Draw("Max Supply")]
		public bool MaxSupply = false;

		[Space(10f), Header("Land Battle Cheats (Enemy Only)")]
		[Space(5f), Draw("Damage Multiplier", Precision = 2, Min = 0.0)]
		public float EnemyDamageMultiplier = 1.0f;

		[Space(5f), Draw("Reload Time Multiplier", Precision = 2, Min = 0.0)]
		public float EnemyLandReloadTimeMult = 1.0f;

		[Space(10f), Header("Naval Battle Cheats (Player Only)")]
		[Space(5f), Draw("Max Morale")]
		public bool MaxNavalMorale = false;
		
		[Space (5f), Draw("Fast Reload")]
		public bool NavalFastReload = false;

		[Space(5f), Draw("Reload Time Multiplier", Precision = 2, Min = 0.0)]
		public float AllyNavalReloadTimeMult = 1.0f;

		[Space(5f), Draw("Max Condition")]
		public bool MaxNavalCondition = false;

		[Space(5f), Draw("Sailing Speed Multiplier", Precision = 2, Min = 0.0)]
		public float AllyNavalSpeedModifier = 1f;

		[Space(5f), Draw("Crew Damage Multiplier", Precision = 2, Min = 0.0)]
		public float AllyCrewDamageModifier = 1f;

		[Space(5f), Draw("Sail Damage Multiplier", Precision = 2, Min = 0.0)]
		public float AllySailDamageModifier = 1f;

		[Space(5f), Draw("Armor Damage Multiplier", Precision = 2, Min = 0.0)]
		public float AllyArmorDamageModifier = 1f;

		[Space(5f), Draw("Structure Damage Multiplier", Precision = 2, Min = 0.0)]
		public float AllyStructureDamageModifier = 1f;

		[Space(5f), Draw("Module Damage Multiplier", Precision = 2, Min = 0.0)]
		public float AllyModuleDamageModifier = 1f;

		[Space(10f), Header("Naval Battle Cheats (Enemy Only)")]
		[Space(5f), Draw("Sailing Speed Multiplier", Precision = 2, Min = 0.0)]
		public float EnemyNavalSpeedModifier = 1f;

		[Space(5f), Draw("Crew Damage Multiplier", Precision = 2, Min = 0.0)]
		public float EnemyCrewDamageModifier = 1f;

		[Space(5f), Draw("Sail Damage Multiplier", Precision = 2, Min = 0.0)]
		public float EnemySailDamageModifier = 1f;

		[Space(5f), Draw("Armor Damage Multiplier", Precision = 2, Min = 0.0)]
		public float EnemyArmorDamageModifier = 1f;

		[Space(5f), Draw("Structure Damage Multiplier", Precision = 2, Min = 0.0)]
		public float EnemyStructureDamageModifier = 1f;

		[Space(5f), Draw("Module Damage Multiplier", Precision = 2, Min = 0.0)]
		public float EnemyModuleDamageModifier = 1f;

		[Space(5f), Draw("Reload Time Multiplier", Precision = 2, Min = 0.0)]
		public float EnemyNavalReloadTimeMult = 1.0f;

		[Space(10f), Header("Experimental")]
		[Space(5f), Draw("Naval Officer Firepower Multiplier", Precision = 2, Min = 0.0)]
		public float OfficerFirepowerModifier = 1f;

		[Space(5f), Draw("Naval Officer Efficiency Multiplier", Precision = 2, Min = 0.0)]
		public float OfficerEfficiencyModifier = 1f;

		[Space(5f), Draw("Naval Officer Boarding Multiplier", Precision = 2, Min = 0.0)]
		public float OfficerMeleeModifier = 1f;

		[Space(5f), Draw("Naval Officer Sailing Multiplier", Precision = 2, Min = 0.0)]
		public float OfficerSailingModifier = 1f;

		[Space(5f), Draw("Land Spotting Multiplier", Precision = 2, Min = 0.0)]
		public float SpottingModifier = 1f;

		[Space(5f), Draw("Land Stealth Multiplier", Precision = 2, Min = 0.0)]
		public float StealthModifier = 1f;

		[Space(5f), Draw("Land Movement Speed Multiplier", Precision = 2, Min = 0.0)]
		public float SpeedModifier = 1f;

		[Space(5f), Draw("Land Firepower Multiplier", Precision = 2, Min = 0.0)]
		public float FirepowerModifier = 1f;

		[Space(5f), Draw("Land Efficiency Multiplier", Precision = 2, Min = 0.0)]
		public float EfficiencyModifier = 1f;

		[Space(5f), Draw("Land Melee Multiplier", Precision = 2, Min = 0.0)]
		public float MeleeModifier = 1f;

		public override void Save(UnityModManager.ModEntry modEntry)
		{
			UnityModManager.ModSettings.Save<Settings>(this, modEntry);
		}

		public void OnChange()
		{
		}
		
		public override string GetPath(UnityModManager.ModEntry modEntry)
	    {
	        return Path.Combine(modEntry.Path, "Settings.xml");
	    }
	}
}
