using Harmony12;
using Newtonsoft.Json;
using Save;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UltimateAdmiral;
using UnityModManagerNet;

namespace EquipmentMod
{
	internal static class Main
	{
		[HarmonyPatch(typeof(GameSave)), HarmonyPatch("Load")]
		private class CampaignController_Patch
		{
			private static bool Initialized = true;

			private static void Postfix(GameSave __result)
			{
				Main.CampaignController_Patch.LoadLandEquipment(__result);
				Main.CampaignController_Patch.LoadShipModules(__result);
				Main.CampaignController_Patch.LoadNavalEquipment(__result);
			}

			private static void LoadShipModules(GameSave __result)
			{
				string path = "Mods/EquipmentMod/ShipModules.txt";
				bool flag = !File.Exists(path);
				if (flag)
				{
					File.CreateText(path).Close();
					Main.CampaignController_Patch.Initialized = false;
				}
				bool flag2 = !Main.CampaignController_Patch.Initialized;
				if (flag2)
				{
					List<UpgradeModule> upgrades = __result.campaign.campaignController.campSettings.shopSettings.upgradePool.upgrades;
					bool flag3 = upgrades == null;
					if (!flag3)
					{
						List<ModuleHotpatch> list = new List<ModuleHotpatch>();
						foreach (UpgradeModule current in upgrades)
						{
							list.Add(new ModuleHotpatch(current));
						}
						File.WriteAllText(path, JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings
						{
							ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
							MaxDepth = new int?(2)
						}));
					}
				}
				else
				{
					List<ModuleHotpatch> list2 = JsonConvert.DeserializeObject<List<ModuleHotpatch>>(File.ReadAllText(path));
					List<UpgradeModule> upgrades2 = __result.campaign.campaignController.campSettings.shopSettings.upgradePool.upgrades;
					using (List<ModuleHotpatch>.Enumerator enumerator2 = list2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ModuleHotpatch module = enumerator2.Current;
							try
							{
								UpgradeModule upgradeModule = upgrades2.Find((UpgradeModule x) => x.title == module.ID);
								module.Update(ref upgradeModule);
								upgradeModule.OnAfterDeserialize();
							}
							catch (Exception)
							{
							}
						}
					}
				}
			}

			private static void LoadLandEquipment(GameSave __result)
			{
				bool flag = true;
				string path = "Mods/EquipmentMod/Rifles.txt";
				bool flag2 = !File.Exists(path);
				if (flag2)
				{
					File.CreateText(path).Close();
					flag = false;
				}
				bool flag3 = !flag;
				if (flag3)
				{
					List<WeaponTemplate> rifles = __result.campaign.campaignController.campSettings.shopSettings.riflePool.rifles;
					bool flag4 = rifles == null;
					if (!flag4)
					{
						List<WeaponHotpatch> list = new List<WeaponHotpatch>();
						foreach (WeaponTemplate current in rifles)
						{
							list.Add(new WeaponHotpatch(current));
						}
						File.WriteAllText(path, JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings
						{
							ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
							MaxDepth = new int?(2)
						}));
					}
				}
				else
				{
					List<WeaponHotpatch> list2 = JsonConvert.DeserializeObject<List<WeaponHotpatch>>(File.ReadAllText(path));
					List<WeaponTemplate> rifles2 = __result.campaign.campaignController.campSettings.shopSettings.riflePool.rifles;
					using (List<WeaponHotpatch>.Enumerator enumerator2 = list2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							WeaponHotpatch updatedItem = enumerator2.Current;
							try
							{
								WeaponTemplate weaponTemplate = rifles2.Find((WeaponTemplate x) => x.textName.ToString() == updatedItem.ID);
								updatedItem.Update(ref weaponTemplate);
								weaponTemplate.OnAfterDeserialize();
							}
							catch (Exception)
							{
							}
						}
					}
				}
			}

			private static void LoadNavalEquipment(GameSave __result)
			{
				bool flag = true;
				string path = "Mods/EquipmentMod/Cannons.txt";
				bool flag2 = !File.Exists(path);
				if (flag2)
				{
					File.CreateText(path).Close();
					flag = false;
				}
				bool flag3 = !flag;
				if (flag3)
				{
					List<CannonModule> cannons = __result.campaign.campaignController.campSettings.shopSettings.cannonPool.cannons;
					bool flag4 = cannons == null;
					if (!flag4)
					{
						List<CannonHotpatch> list = new List<CannonHotpatch>();
						foreach (CannonModule current in cannons)
						{
							list.Add(new CannonHotpatch(current));
						}
						File.WriteAllText(path, JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings
						{
							ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
							MaxDepth = new int?(2)
						}));
					}
				}
				else
				{
					List<CannonHotpatch> list2 = JsonConvert.DeserializeObject<List<CannonHotpatch>>(File.ReadAllText(path));
					List<CannonModule> cannons2 = __result.campaign.campaignController.campSettings.shopSettings.cannonPool.cannons;
					using (List<CannonHotpatch>.Enumerator enumerator2 = list2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							CannonHotpatch updatedItem = enumerator2.Current;
							try
							{
								CannonModule cannonModule = cannons2.Find((CannonModule x) => x.title.ToString() == updatedItem.ID);
								updatedItem.Update(ref cannonModule);
								cannonModule.OnAfterDeserialize();
							}
							catch (Exception)
							{
							}
						}
					}
				}
			}
		}

		public static UnityModManager.ModEntry mod;

		public static Settings _settings;

		private static bool Load(UnityModManager.ModEntry modEntry)
		{
			Main.mod = modEntry;
			HarmonyInstance harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
			harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
			Main._settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
			modEntry.OnGUI = new Action<UnityModManager.ModEntry>(Main.OnGUI);
			modEntry.OnSaveGUI = new Action<UnityModManager.ModEntry>(Main.OnSaveGUI);
			return true;
		}

		private static void OnGUI(UnityModManager.ModEntry modEntry)
		{
			UnityModManagerNet.Extensions.Draw<Settings>(Main._settings, modEntry);
		}

		private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
		{
			Main._settings.Save(modEntry);
		}
	}
}
