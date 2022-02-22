using Fight.Modifier;
using GOAP;
using Harmony12;
using System;
using System.Reflection;
using UltimateAdmiral;
using UltimateAdmiral.UI;
using UnityEngine;
using RewardItem;
using UnityModManagerNet;

namespace UAAS
{
	internal static class Main
	{
		[HarmonyPatch(typeof(BattleRightBottomPanel)), HarmonyPatch("OnButtonClick")]
		private class SpeedMod
		{
			private static void Postfix(BattleRightBottomPanel __instance, GameObject selection)
			{
				for (int i = __instance.speedButtons.Length - 1; i > 0; i--)
				{
					BattleRightBottomPanel.ButtonRec buttonRec = __instance.speedButtons[i];
					if (buttonRec.selection == selection)
					{
						BattleLogic.instance.pauseSound = true;
						BattleLogic.instance.GameSpeed = buttonRec.speed * 5f * Main._settings.SpeedMultiplier;
						break;
					}
				}
			}
		}

		[HarmonyPatch(typeof(PayElement)), HarmonyPatch("Pay")]
		private class Pay
		{
			private static void Prefix(PayElement __instance)
			{
				PlayerController instance = PlayerController.instance;
				if (Main._settings.MinimumReputation > 0)
				{
					instance.Reputation = Math.Max(Main._settings.MinimumReputation, instance.Reputation);
				}
				if (Main._settings.MinimumCash > 0)
				{
					instance.Gold = Math.Max(Main._settings.MinimumCash, instance.Gold);
				}
			}
		}
		
		[HarmonyPatch(typeof(CampaignController)), HarmonyPatch("CalcBattleReward")]
		private class CalcBattleReward
		{
			private static void Prefix(Campaign.Battle battle, EBattleResult result)
			{
				RewardSettings rewardSettings = null;
				switch (result)
				{
				case EBattleResult.Victory:
					rewardSettings = battle.battleSettings.victoryRewards;
					break;
				case EBattleResult.Draw:
					rewardSettings = battle.battleSettings.drawRewards;
					break;
				case EBattleResult.Defeat:
					rewardSettings = battle.battleSettings.defeatRewards;
					break;
				case EBattleResult.Passed:
					rewardSettings = battle.battleSettings.passedRewards;
					break;
				}
				if (rewardSettings == null)
				{
					return;
				}
				RewardSettings.BattleReward[] rewards = rewardSettings.rewards;
				for (int i = 0; i < rewards.Length; i++)
				{
					RewardSettings.BattleReward battleReward = rewards[i];
					if (battleReward.item.Type == ERewardType.Gold || battleReward.item.Type == ERewardType.Reputation || battleReward.item.Type == ERewardType.CareerPoints) {
						if (battleReward.item.Type == ERewardType.Gold) battleReward.count *= Main._settings.CashRewMult-1;
						if (battleReward.item.Type == ERewardType.Reputation) battleReward.count *= Main._settings.ReputationRewMult-1;
						if (battleReward.item.Type == ERewardType.Medal) battleReward.count *= Main._settings.CareerPointsRewMult-1;
						
						battleReward.Transfer(false, battle.NavalDifficulty);
					}
				}
			}
		}

		[HarmonyPatch(typeof(AdmiraltyManager)), HarmonyPatch("PerformPurchaseItem")]
		private class PerformPurchaseItem
		{
			private static void Prefix(AdmiraltyManager __instance)
			{
				if (Main._settings.InfRifleStock || Main._settings.InfRifleStockArmory)
				{
					EPickerContent contentType = __instance.SelectedItem.ContentType;
					if (contentType != EPickerContent.Cannon)
					{
						if (contentType == EPickerContent.Rifle)
						{
							StoreRecord storeRecord = (__instance.SelectedItem.ContentItem as RifleItem).StoreRecord;
							bool infRifleStock = Main._settings.InfRifleStock;
							if (infRifleStock)
							{
								storeRecord.Shop = 999999;
							}
							bool infRifleStockArmory = Main._settings.InfRifleStockArmory;
							if (infRifleStockArmory)
							{
								storeRecord.Armory = 999999;
							}
						}
					}
					else
					{
						CannonModule cannon = (__instance.SelectedItem.ContentItem as CannonItem).Cannon;
						StoreModule storeCannon = PlayerController.instance.GetStoreCannon(cannon, true);
						bool infRifleStock2 = Main._settings.InfRifleStock;
						if (infRifleStock2)
						{
							storeCannon.Shop = 999999;
						}
						bool infRifleStockArmory2 = Main._settings.InfRifleStockArmory;
						if (infRifleStockArmory2)
						{
							storeCannon.Armory = 999999;
						}
					}
				}
			}
		}

		[HarmonyPatch(typeof(ShipModel.Crew)), HarmonyPatch("ChangeMorale")]
		private class ChangeMorale
		{
			private static void Prefix(ShipModel.Crew __instance, ref float change)
			{
				if (Main._settings.MaxNavalMorale && __instance.ship.side == RuntimeVars.playerSide)
				{
					change = 1f;
				}
			}
		}

		[HarmonyPatch(typeof(ShipModel.Crew)), HarmonyPatch("UpdateMorale")]
		private class UpdateMorale
		{
			private static void Prefix(ShipModel.Crew __instance, ref float dt)
			{
				if (Main._settings.MaxNavalMorale && __instance.ship.side == RuntimeVars.playerSide)
				{
					dt = 1000f;
				}
			}
		}

		[HarmonyPatch(typeof(ShipModel.Crew)), HarmonyPatch("UpdateStamina")]
		private class UpdateStamina
		{
			private static void Prefix(ShipModel.Crew __instance, ref float drain)
			{
				if (Main._settings.MaxNavalCondition && __instance.ship.side == RuntimeVars.playerSide)
				{
					drain = -1000f;
				}
			}
		}

		[HarmonyPatch(typeof(ShipModel)), HarmonyPatch("RecalcAttributes")]
		private class NavalRecalcAttributes
		{
			private static void Prefix(ShipModel __instance)
			{
				if (Main._settings.MaxNavalCondition && __instance.side == RuntimeVars.playerSide)
				{
					__instance.crew.Stamina = 1f;
				}
				if (Main._settings.MaxNavalMorale && __instance.side == RuntimeVars.playerSide)
				{
					__instance.crew.Morale = 1f;
				}
			}

			private static void Postfix(ShipModel __instance)
			{
				if (Main._settings.MaxNavalCondition && __instance.side == RuntimeVars.playerSide)
				{
					__instance.actualAttributes.endurance = 100f;
					__instance.currentAttributes.endurance = 100f;
				}
				if (Main._settings.MaxNavalMorale && __instance.side == RuntimeVars.playerSide)
				{
					__instance.actualAttributes.willpower = 100f;
					__instance.currentAttributes.willpower = 100f;
				}
			}
		}

		[HarmonyPatch(typeof(NavalController)), HarmonyPatch("CreateShipModel")]
		private class CreateShipModel
		{
			private static void Postfix(NavalController __instance, ShipModel __result)
			{
				if (__result.side == RuntimeVars.playerSide)
				{
					for (int i = 0; i < 6; i++)
					{
						NavalOfficer crewOfficer = __result.crew.GetCrewOfficer((ENavalTask)i);
						if (crewOfficer == null)
						{
							break;
						}
						NavalOfficer officer = crewOfficer;
						officer.attributes.perception = officer.attributes.perception * Main._settings.OfficerFirepowerModifier;
						officer.attributes.intelligence = officer.attributes.intelligence * Main._settings.OfficerEfficiencyModifier;
						officer.attributes.strength = officer.attributes.strength * Main._settings.OfficerMeleeModifier;
						officer.attributes.dexterity = officer.attributes.dexterity * Main._settings.OfficerSailingModifier;
					}
				}
			}
		}

		[HarmonyPatch(typeof(ShipViewController)), HarmonyPatch("UpdateKinematicForces")]
		private class UpdateKinematicForces
		{
			private static void Prefix(ShipViewController __instance, ref ShipModel ___model, ref ShipViewConfig ___viewConfig)
			{
				if (___model.side == RuntimeVars.playerSide)
				{
					___viewConfig.forwardForceModifier = Main._settings.AllyNavalSpeedModifier;
					___viewConfig.backwardForceModifier = Main._settings.AllyNavalSpeedModifier;
				} else {
					___viewConfig.forwardForceModifier = Main._settings.EnemyNavalSpeedModifier;
					___viewConfig.backwardForceModifier = Main._settings.EnemyNavalSpeedModifier;
				}
			}
		}
		
		[HarmonyPatch(typeof(ShipViewController.Deck)), HarmonyPatch("EndVolley")]
		private class EndVolley
		{
			private static void Postfix(ShipViewController.Deck __instance, ref ShipModel ___model, ref float __state)
			{
				if (__instance.model.board.Ship.side == RuntimeVars.playerSide && Main._settings.NavalFastReload)
				{
					__instance.Timer = 0f;
				}
				if (!Main._settings.NavalFastReload) {
					if (__instance.model.board.Ship.side == RuntimeVars.playerSide) {
						__instance.Timer *= Main._settings.AllyNavalReloadTimeMult;
					} else {
						__instance.Timer *= Main._settings.EnemyNavalReloadTimeMult;
					}
				}
			}
		}

		[HarmonyPatch (typeof (ShipViewController.ModuleSystem)), HarmonyPatch ("DamageCrew")]
		private class DamageCrew {
			private static void Prefix (ref float damage, ref ShipViewController sender, ref ShipModel ___model) {
				if (sender != null) {
					if (___model.side != RuntimeVars.playerSide) {
						damage *= Main._settings.AllyCrewDamageModifier;
					} else {
						damage *= Main._settings.EnemyCrewDamageModifier;
					}
				}
			}
		}

		[HarmonyPatch (typeof (ShipViewController.ModuleSystem)), HarmonyPatch ("DamageSail")]
		private class DamageSail {
			private static void Prefix (ref float damage, ref ShipViewController sender, ref ShipModel ___model) {
				if (sender != null) {
					if (___model.side != RuntimeVars.playerSide) {
						damage *= Main._settings.AllySailDamageModifier;
					} else {
						damage *= Main._settings.EnemySailDamageModifier;
					}
				}
			}
		}

		[HarmonyPatch (typeof (ShipViewController.ModuleSystem)), HarmonyPatch ("DamageArmor")]
		private class DamageArmor {
			private static void Prefix (ref float damage, ref ShipViewController sender, ref ShipModel ___model) {
				if (sender != null) {
					if (___model.side != RuntimeVars.playerSide) {
						damage *= Main._settings.AllyArmorDamageModifier;
					} else {
						damage *= Main._settings.EnemyArmorDamageModifier;
					}
				}
			}
		}

		[HarmonyPatch (typeof (ShipViewController.ModuleSystem)), HarmonyPatch ("DamageStructure")]
		private class DamageStructure {
			private static void Prefix (ref float damage, ref ShipViewController sender, ref ShipModel ___model) {
				if (sender != null) {
					if (___model.side != RuntimeVars.playerSide) {
						damage *= Main._settings.AllyStructureDamageModifier;
					} else {
						damage *= Main._settings.EnemyStructureDamageModifier;
					}
				}
			}
		}

		[HarmonyPatch (typeof (ShipViewController.ModuleSystem)), HarmonyPatch ("DamageModule")]
		private class DamageModule {
			private static void Prefix (ref EModuleIndex moduleIndex, ref float damage, ref ShipModel ___model) {
				if (damage > 0) {
					if (___model.side != RuntimeVars.playerSide) {
						damage *= Main._settings.AllyModuleDamageModifier;
					} else {
						damage *= Main._settings.EnemyModuleDamageModifier;
					}
				}
			}
		}

		[HarmonyPatch(typeof(UnitModel)), HarmonyPatch("ApplyDamage")]
		private class ApplyDamage
		{
			private static void Prefix(UnitModel __instance)
			{
				if (__instance.side == RuntimeVars.playerSide)
				{
					if (Main._settings.NoDamage)
						__instance.currentFrameData.damage = 0f;
					else
						__instance.currentFrameData.damage *= Main._settings.EnemyDamageMultiplier;
				} else {
					__instance.currentFrameData.damage *= Main._settings.AllyDamageMultiplier;
				}
			}
		}

		[HarmonyPatch(typeof(UnitModel)), HarmonyPatch("ShootParamsUpdate")]
		private class ShootParamsUpdate
		{
			private static void Prefix (UnitModel __instance, ref float dReloadProgress) {
				if (!Main._settings.LandFastReload) {
					if (__instance.side == RuntimeVars.playerSide) {
						dReloadProgress *= Main._settings.AllyLandReloadTimeMult;
					} else {
						dReloadProgress *= Main._settings.EnemyLandReloadTimeMult;
					}
				}
			}

			private static void Postfix(UnitModel __instance)
			{
				if (Main._settings.MaxSupply && __instance.side == RuntimeVars.playerSide)
				{
					__instance.ammunitionCurrentCost = __instance.ammunitionTotalCost;
				}
				if (Main._settings.LandFastReload && __instance.side == RuntimeVars.playerSide)
				{
					__instance.reloadProgress = 1f;
				}
			}
		}

		[HarmonyPatch(typeof(Morale)), HarmonyPatch("Update")]
		private class MoraleUpdate
		{
			private static void Postfix(Morale __instance, ref UnitModel ___data)
			{
				if (Main._settings.MaxMorale && ___data.side == RuntimeVars.playerSide)
				{
					__instance.morale = 1f;
					___data.morale = 1f;
				}
			}
		}

		[HarmonyPatch(typeof(Fatigue)), HarmonyPatch("Update")]
		private class FatigueUpdate
		{
			private static void Prefix(Fatigue __instance, ref UnitModel ___data, ref float dt)
			{
				if (Main._settings.MaxCondition && ___data.side == RuntimeVars.playerSide)
				{
					dt = 10000f;
				}
			}

			private static void Postfix(Fatigue __instance, ref UnitModel ___data)
			{
				if (Main._settings.MaxCondition && ___data.side == RuntimeVars.playerSide)
				{
					___data.condition = 1f;
				}
			}
		}

		[HarmonyPatch(typeof(UnitManager)), HarmonyPatch("CreateUnit")]
		private class UnitManager_StatModifier
		{
			private static void Postfix(UnitManager __instance, ref UnitModel __result)
			{
				if (__result.side == RuntimeVars.playerSide)
				{
					__result.maxSpeed *= Main._settings.SpeedModifier;
					__result.entity.spottingRange *= Main._settings.SpottingModifier;
					__result.entity.stealthRange *= Main._settings.StealthModifier;
					Attributes[] array = new Attributes[]
					{
						__result.startAttributes,
						__result.actualAttributes,
						__result.currentAttributes
					};
					for (int i = 0; i < array.Length; i++)
					{
						Attributes attributes = array[i];
						attributes.perception *= Main._settings.FirepowerModifier;
						attributes.intelligence *= Main._settings.EfficiencyModifier;
						attributes.strength *= Main._settings.MeleeModifier;
					}
					__result.startAttributes = array[0];
					__result.actualAttributes = array[1];
					__result.currentAttributes = array[2];
					if (__result.isValid)
					{
						BattlePanel.Instance.UpdateContent(true);
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
