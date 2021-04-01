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
					bool flag = buttonRec.selection == selection;
					if (flag)
					{
						BattleLogic.instance.pauseSound = true;
						BattleLogic.instance.GameSpeed = buttonRec.speed * 5f * (float)Main._settings.SpeedMultiplier;
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
				bool flag = Main._settings.MinimumReputation > 0;
				if (flag)
				{
					instance.Reputation = Math.Max(Main._settings.MinimumReputation, instance.Reputation);
				}
				bool flag2 = Main._settings.MinimumCash > 0;
				if (flag2)
				{
					instance.Gold = Math.Max(Main._settings.MinimumCash, instance.Gold);
				}
			}
		}

		[HarmonyPatch(typeof(PaySection)), HarmonyPatch("Pay")]
		private class PaySectionPay
		{
			private static void Prefix(PaySection __instance)
			{
				PlayerController instance = PlayerController.instance;
				bool flag = Main._settings.MinimumReputation > 0;
				if (flag)
				{
					instance.Reputation = Math.Max(Main._settings.MinimumReputation, instance.Reputation);
				}
				bool flag2 = Main._settings.MinimumCash > 0;
				if (flag2)
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
						if (battleReward.item.Type == ERewardType.Gold) battleReward.count *= (Main._settings.CashRevMult-1);
						if (battleReward.item.Type == ERewardType.Reputation) battleReward.count *= (Main._settings.ReputationRevMult-1);
						if (battleReward.item.Type == ERewardType.Medal) battleReward.count *= (Main._settings.CareerPointsRevMult-1);
						
						battleReward.Transfer(false, battle.NavalDifficulty);
					}
				}
				if (battle.HasBonusReward)
				{
					rewardSettings = battle.battleSettings.bonusRewards;
					if (rewardSettings != null)
					{
						rewards = rewardSettings.rewards;
						for (int j = 0; j < rewards.Length; j++)
						{
							RewardSettings.BattleReward battleReward2 = rewards[j];
							if (battleReward2.item.Type == ERewardType.Gold || battleReward2.item.Type == ERewardType.Reputation || battleReward2.item.Type == ERewardType.Medal) {
								if (battleReward2.item.Type == ERewardType.Gold) battleReward2.count *= (Main._settings.CashRevMult-1);
								if (battleReward2.item.Type == ERewardType.Reputation) battleReward2.count *= (Main._settings.ReputationRevMult-1);
								if (battleReward2.item.Type == ERewardType.Medal) battleReward2.count *= (Main._settings.CareerPointsRevMult-1);
								
								battleReward2.Transfer(false, EDifficulty.None);
							}
						}
					}
				}
			}
		}

		[HarmonyPatch(typeof(AdmiraltyManager)), HarmonyPatch("PerformPurchaseItem")]
		private class PerformPurchaseItem
		{
			private static void Prefix(AdmiraltyManager __instance)
			{
				bool flag = Main._settings.InfRifleStock || Main._settings.InfRifleStockArmory;
				if (flag)
				{
					EPickerContent contentType = __instance.SelectedItem.ContentType;
					bool flag2 = contentType != EPickerContent.Cannon;
					if (flag2)
					{
						bool flag3 = contentType == EPickerContent.Rifle;
						if (flag3)
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
				bool flag = Main._settings.MaxNavalMorale && __instance.ship.side == RuntimeVars.playerSide;
				if (flag)
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
				bool flag = Main._settings.MaxNavalMorale && __instance.ship.side == RuntimeVars.playerSide;
				if (flag)
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
				bool flag = Main._settings.MaxNavalCondition && __instance.ship.side == RuntimeVars.playerSide;
				if (flag)
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
				bool flag = Main._settings.MaxNavalCondition && __instance.side == RuntimeVars.playerSide;
				if (flag)
				{
					__instance.crew.Stamina = 1f;
				}
				bool flag2 = Main._settings.MaxNavalMorale && __instance.side == RuntimeVars.playerSide;
				if (flag2)
				{
					__instance.crew.Morale = 1f;
				}
			}

			private static void Postfix(ShipModel __instance)
			{
				bool flag = Main._settings.MaxNavalCondition && __instance.side == RuntimeVars.playerSide;
				if (flag)
				{
					__instance.actualAttributes.endurance = 100f;
					__instance.currentAttributes.endurance = 100f;
				}
				bool flag2 = Main._settings.MaxNavalMorale && __instance.side == RuntimeVars.playerSide;
				if (flag2)
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
				bool flag = __result.side == RuntimeVars.playerSide;
				if (flag)
				{
					for (int i = 0; i < 6; i++)
					{
						NavalOfficer crewOfficer = __result.crew.GetCrewOfficer((ENavalTask)i);
						bool flag2 = crewOfficer == null;
						if (flag2)
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
				bool flag = ___model.side == RuntimeVars.playerSide;
				if (flag)
				{
					___viewConfig.forwardForceModifier = Main._settings.NavalSpeedModifier;
					___viewConfig.backwardForceModifier = Main._settings.NavalSpeedModifier;
				}
			}
		}
		
		[HarmonyPatch(typeof(ShipViewController.Deck)), HarmonyPatch("EndVolley")]
		private class EndVolley
		{
			private static void Postfix(ShipViewController.Deck __instance, ref ShipModel ___model)
			{
				bool flag = __instance.model.board.Ship.side == RuntimeVars.playerSide;
				if (flag && Main._settings.NavalFastReload)
				{
					__instance.Timer = 0f;
				}
			}
		}

		[HarmonyPatch(typeof(UnitModel)), HarmonyPatch("ApplyDamage")]
		private class ApplyDamage
		{
			private static void Prefix(UnitModel __instance)
			{
				bool flag = Main._settings.NoDamage && __instance.side == RuntimeVars.playerSide;
				if (flag)
				{
					__instance.currentFrameData.damage = 0f;
				}
			}
		}

		[HarmonyPatch(typeof(UnitModel)), HarmonyPatch("ShootParamsUpdate")]
		private class ShootParamsUpdate
		{
			private static void Postfix(UnitModel __instance)
			{
				bool flag = Main._settings.MaxSupply && __instance.side == RuntimeVars.playerSide;
				if (flag)
				{
					__instance.ammunitionCurrentCost = __instance.ammunitionTotalCost;
				}
				bool flag2 = Main._settings.LandFastReload && __instance.side == RuntimeVars.playerSide;
				if (flag2)
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
				bool flag = Main._settings.MaxMorale && ___data.side == RuntimeVars.playerSide;
				if (flag)
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
				bool flag = Main._settings.MaxCondition && ___data.side == RuntimeVars.playerSide;
				if (flag)
				{
					dt = 10000f;
				}
			}

			private static void Postfix(Fatigue __instance, ref UnitModel ___data)
			{
				bool flag = Main._settings.MaxCondition && ___data.side == RuntimeVars.playerSide;
				if (flag)
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
				bool flag = __result.side == RuntimeVars.playerSide;
				if (flag)
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
					bool isValid = __result.isValid;
					if (isValid)
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
