using System;
using UnityModManagerNet;

namespace EquipmentMod
{
	public class Settings : UnityModManager.ModSettings, IDrawable
	{
		public override void Save(UnityModManager.ModEntry modEntry)
		{
			UnityModManager.ModSettings.Save<Settings>(this, modEntry);
		}

		public void OnChange()
		{
		}
	}
}
