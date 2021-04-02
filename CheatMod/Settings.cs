using System;
using System.IO;
using UnityEngine;
using UnityModManagerNet;

namespace UAAS
{
	public class Settings : UnityModManager.ModSettings, IDrawable
	{
		[Header("General Cheats"), Space(5f), Draw("Speed Button Multiplier (Ignores 1x)", Precision = 0, Min = 0.0)]
		public int SpeedMultiplier = 1;

		[Space(5f), Draw("Minimum Reputation (Updates when changed)", Precision = 0, Min = 0.0)]
		public int MinimumReputation = 0;

		[Space(5f), Draw("Minimum Cash (Updates when changed)", Precision = 0, Min = 0.0)]
		public int MinimumCash = 0;
		
		[Space(5f), Draw("Cash reward multiplier from mission", Precision = 1, Min = 1.0)]
		public int CashRewMult = 1;
		
		[Space(5f), Draw("Reputation reward multiplier from mission", Precision = 1, Min = 1.0)]
		public int ReputationRewMult = 1;
		
		[Space(5f), Draw("Career Points reward multiplier from mission", Precision = 1, Min = 1.0)]
		public int CareerPointsRewMult = 1;

		[Space(5f), Draw("Inf. Rifle/Cannon Stock in Shop (Buy Once to update)")]
		public bool InfRifleStock = false;

		[Space(5f), Draw("Inf. Rifle/Cannon Stock in Armory (Buy Once to update)")]
		public bool InfRifleStockArmory = false;

		[Header("Land Battle Cheats (Player Only)"), Space(5f), Draw("Take No Damage")]
		public bool NoDamage = false;
		
		[Space(5f), Draw("Fast reload")]
		public bool LandFastReload = false;

		[Space(5f), Draw("Max Morale")]
		public bool MaxMorale = false;

		[Space(5f), Draw("Max Condition")]
		public bool MaxCondition = false;

		[Space(5f), Draw("Max Supply")]
		public bool MaxSupply = false;

		[Header("Naval Battle Cheats (Player Only)"), Space(5f), Draw("Max Morale")]
		public bool MaxNavalMorale = false;
		
		[Space (5f), Draw("Fast Reload")]
		public bool NavalFastReload = false;

		[Space(5f), Draw("Max Condition")]
		public bool MaxNavalCondition = false;

		[Space(5f), Draw("Naval Sailing Speed Modifier", Precision = 1, Min = 1.0)]
		public float NavalSpeedModifier = 1f;

		[Header("Experimental"), Space(5f), Draw("Naval Officer Firepower Modifier", Precision = 1, Min = 0.0)]
		public float OfficerFirepowerModifier = 1f;

		[Space(5f), Draw("Naval Officer Efficiency Modifier", Precision = 1, Min = 0.0)]
		public float OfficerEfficiencyModifier = 1f;

		[Space(5f), Draw("Naval Officer Boarding Modifier", Precision = 1, Min = 0.0)]
		public float OfficerMeleeModifier = 1f;

		[Space(5f), Draw("Naval Officer Sailing Modifier", Precision = 1, Min = 0.0)]
		public float OfficerSailingModifier = 1f;

		[Space(5f), Draw("Land Spotting Modifier", Precision = 1, Min = 1.0)]
		public float SpottingModifier = 1f;

		[Space(5f), Draw("Land Stealth Modifier", Precision = 1, Min = 1.0)]
		public float StealthModifier = 1f;

		[Space(5f), Draw("Land Movement Speed Modifier", Precision = 1, Min = 1.0)]
		public float SpeedModifier = 1f;

		[Space(5f), Draw("Land Firepower Modifier", Precision = 1, Min = 0.0)]
		public float FirepowerModifier = 1f;

		[Space(5f), Draw("Land Efficiency Modifier", Precision = 1, Min = 0.0)]
		public float EfficiencyModifier = 1f;

		[Space(5f), Draw("Land Melee Modifier", Precision = 1, Min = 0.0)]
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
