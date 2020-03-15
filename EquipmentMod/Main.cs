using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Harmony12;
using Newtonsoft.Json;
using Save;
using UltimateAdmiral;
using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;

namespace EquipmentMod
{
    internal static class Main
    {
        public static ModEntry mod;
        public static Settings _settings;

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;
            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            _settings = Settings.Load<Settings>(modEntry);
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            _settings.Draw(modEntry);
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            _settings.Save(modEntry);
        }

        public class WeaponHotpatch
        {
            public string ID { get; set; }
            public float adaptationCoeff { get; set; }
            public float altitude { get; set; }
            public float ammoCost { get; set; }
            public int ammunition { get; set; }
            public float baseReload { get; set; }
            public int cannonRequiredStaff { get; set; }
            public float collateralRadius { get; set; }
            public float damage { get; set; }
            public float effectiveRange { get; set; }
            public float effectiveRangeHint { get; set; }
            public float meleeDamage { get; set; }
            public int price { get; set; }
            public float randHi { get; set; }
            public float randLow { get; set; }
            public string textDescription { get; set; }
            public string textName { get; set; }
            public string textShootType { get; set; }
            public AnimationCurve damageDegradation { get; set; }
            public BallisticProperties ballistics { get; set; }
            public WeaponHotpatch() { }
            public WeaponHotpatch(WeaponTemplate template)
            {
                this.ID = template.textName.ToString();
                this.adaptationCoeff = template.adaptationCoeff;
                this.altitude = template.altitude;
                this.ammoCost = template.ammoCost;
                this.ammunition = template.ammunition;
                this.baseReload = template.baseReload;
                this.cannonRequiredStaff = template.cannonRequiredStaff;
                this.collateralRadius = template.collateralRadius;
                this.damage = template.damage;
                this.effectiveRange = template.effectiveRange;
                this.effectiveRangeHint = template.effectiveRangeHint;
                this.meleeDamage = template.meleeDamage;
                this.price = template.price;
                this.randHi = template.randHi;
                this.randLow = template.randLow;
                this.textDescription = template.textDescription.ToString();
                this.textName = template.textName.ToString();
                this.textShootType = template.textShootType.ToString();
                this.damageDegradation = template.damageDegradation;
                this.ballistics = template.ballistics;
            }

            public void UpdateWeapon(ref WeaponTemplate template)
            {
                template.adaptationCoeff = this.adaptationCoeff;
                template.altitude = this.altitude;
                template.ammoCost = this.ammoCost;
                template.ammunition = this.ammunition;
                template.baseReload = this.baseReload;
                template.cannonRequiredStaff = this.cannonRequiredStaff;
                template.collateralRadius = this.collateralRadius;
                template.damage = this.damage;
                template.effectiveRange = this.effectiveRange;
                template.effectiveRangeHint = this.effectiveRangeHint;
                template.meleeDamage = this.meleeDamage;
                template.price = this.price;
                template.randHi = this.randHi;
                template.randLow = this.randLow;
                template.damageDegradation = this.damageDegradation;
                template.ballistics = this.ballistics;
            }
        }

        [HarmonyPatch(typeof(StoreRifle))]
        [HarmonyPatch("LoadCurrent")]
        private class RifleMod
        {
            private static bool Initialized = true;
            private static void Postfix(StoreRifle __instance)
            {
                string weaponConfigFile = @"Mods/EquipmentMod/Rifles.txt";
                if (!File.Exists(weaponConfigFile))
                {
                    File.CreateText(weaponConfigFile).Close();
                    Initialized = false;
                }
                if (!Initialized)
                {
                    WeaponHotpatch hotpatch = new WeaponHotpatch(__instance.weapon);
                    var currentWeaponsText = File.ReadAllText(weaponConfigFile);
                    if (String.IsNullOrEmpty(currentWeaponsText))
                        currentWeaponsText = "[]";
                    var currentWeapons = JsonConvert.DeserializeObject<List<WeaponHotpatch>>(currentWeaponsText);
                    currentWeapons.Add(hotpatch);
                    File.WriteAllText(weaponConfigFile, JsonConvert.SerializeObject(currentWeapons, Formatting.Indented));
                }
                else
                {
                    var currentWeapons = JsonConvert.DeserializeObject<List<WeaponHotpatch>>(File.ReadAllText(weaponConfigFile));
                    WeaponHotpatch currentWeapon = null;
                    foreach (var weapon in currentWeapons)
                    {
                        if (weapon.ID == __instance.weapon.textName.ToString())
                        {
                            currentWeapon = weapon;
                            break;
                        }
                    }
                    if (currentWeapon != null)
                        currentWeapon.UpdateWeapon(ref __instance.weapon);
                }

            }
        }

        public class CannonHotpatch
        {
            public string ID { get; set; }
            public float crew { get; set; }
            public float groundBatterySize { get; set; }
            public float horizontalTurnMax { get; set; }
            public float navalBatterySize { get; set; }
            public float reloadTime { get; set; }
            public float threat { get; set; }
            public string type { get; set; }
            public float verticalTurnMax { get; set; }
            public float verticalTurnMin { get; set; }
            public float weight { get; set; }

            public int goldPrice { get; set; }

            public BallisticsHotPatch Ballistics { get; set; } = new BallisticsHotPatch();
            

            public CannonHotpatch() { }
            public CannonHotpatch(CannonModule template)
            {
                this.ID = template.title.ToString();
                this.crew = template.crew;
                this.groundBatterySize = template.groundBatterySize;
                this.horizontalTurnMax = template.horizontalTurnMax;
                this.navalBatterySize = template.navalBatterySize;
                this.reloadTime = template.reloadTime;
                this.threat = template.threat;
                this.type = template.type.ToString();
                this.verticalTurnMax = template.verticalTurnMax;
                this.verticalTurnMin = template.verticalTurnMin;
                this.weight = template.weight;

                this.goldPrice = template.GoldPrice;

                this.Ballistics.verticalSpread = template.Ballistics.verticalSpread;
                this.Ballistics.radius = template.Ballistics.radius;
                this.Ballistics.mass = template.Ballistics.mass;
                this.Ballistics.horizontalSpread = template.Ballistics.horizontalSpread;
                this.Ballistics.gravity = template.Ballistics.gravity;
                this.Ballistics.distance = template.Ballistics.distance;
                this.Ballistics.baseY = template.Ballistics.baseY;
                this.Ballistics.armorPiercing = template.Ballistics.armorPiercing;
            }

            public void UpdateWeapon(ref CannonModule template)
            {
                template.crew = this.crew;
                template.groundBatterySize = this.groundBatterySize;
                template.horizontalTurnMax = this.horizontalTurnMax;
                template.navalBatterySize = this.navalBatterySize;
                template.reloadTime = this.reloadTime;
                template.threat = this.threat;
                template.type = this.type == "Carronade" ? ECannonType.Carronade : ECannonType.Cannon;
                template.verticalTurnMax = this.verticalTurnMax;
                template.verticalTurnMin = this.verticalTurnMin;
                template.weight = this.weight;
                template.GoldPrice = this.goldPrice;
                template.Ballistics.verticalSpread = this.Ballistics.verticalSpread;
                template.Ballistics.radius = this.Ballistics.radius;
                template.Ballistics.mass = this.Ballistics.mass;
                template.Ballistics.horizontalSpread = this.Ballistics.horizontalSpread;
                template.Ballistics.gravity = this.Ballistics.gravity;
                template.Ballistics.distance = this.Ballistics.distance;
                template.Ballistics.baseY = this.Ballistics.baseY;
                template.Ballistics.armorPiercing = this.Ballistics.armorPiercing;
            }

            public class BallisticsHotPatch
            {
                public float verticalSpread { get; set; }
                public float radius { get; set; }
                public float mass { get; set; }
                public float horizontalSpread { get; set; }
                public float gravity { get; set; }
                public float distance { get; set; }
                public float baseY { get; set; }
                public AnimationCurve armorPiercing { get; set; }
            }

        }

        [HarmonyPatch(typeof(StoreModule))]
        [HarmonyPatch("LoadCurrent")]
        private class CannonMod
        {
            private static bool Initialized = true;
            private static void Postfix(StoreModule __instance)
            {
                string weaponConfigFile = @"Mods/EquipmentMod/Cannons.txt";
                if (!File.Exists(weaponConfigFile))
                {
                    File.CreateText(weaponConfigFile).Close();
                    Initialized = false;
                }
                if (!Initialized)
                {
                    var cannonModule = __instance.module as CannonModule;
                    if (cannonModule == null)
                        return;
                    CannonHotpatch hotpatch = new CannonHotpatch(cannonModule);
                    var currentWeaponsText = File.ReadAllText(weaponConfigFile);
                    if (String.IsNullOrEmpty(currentWeaponsText))
                        currentWeaponsText = "[]";
                    var currentWeapons = JsonConvert.DeserializeObject<List<CannonHotpatch>>(currentWeaponsText);
                    currentWeapons.Add(hotpatch);
                    File.WriteAllText(weaponConfigFile, JsonConvert.SerializeObject(currentWeapons, Formatting.Indented));
                }
                else
                {
                    var cannonModule = __instance.module as CannonModule;
                    if (cannonModule == null)
                        return;
                    var currentWeapons = JsonConvert.DeserializeObject<List<CannonHotpatch>>(File.ReadAllText(weaponConfigFile));
                    CannonHotpatch currentWeapon = null;
                    foreach (var weapon in currentWeapons)
                    {
                        if (weapon.ID == __instance.module.title.ToString())
                        {
                            currentWeapon = weapon;
                            break;
                        }
                    }
                    if (currentWeapon != null)
                        currentWeapon.UpdateWeapon(ref cannonModule);
                }

            }
        }


        public class ModuleHotpatch
        {
            public string ID { get; set; }
            public float moduleWeight { get; set; }
            public int requiredCrew { get; set; }
            public int repairCrew { get; set; }
            public float repairOptimalCrew { get; set; }
            public float repairLimit { get; set; }
            public List<ModifierHotPatch> modifiers { get; set; } = new List<ModifierHotPatch>();
            public string title { get; set; }
            public string description { get; set; }
            public string effect { get; set; }
            public int goldPrice { get; set; }
            public EUpgradeCategory category { get; set; }
            public string categoryString { get; set; }
            public EModuleIndex repairModule { get; set; } = EModuleIndex.None;
            public string repairModuleString { get; set; }
            public EUpgradeSpecial speciality { get; set; }
            public string specialityString { get; set; }
            public EShipKindMask shipKinds { get; set; } = (EShipKindMask)3;
            public string shipKindsString { get; set; }
            public bool isTemporary { get; set; }

            public ModuleHotpatch() { }
            public ModuleHotpatch(UpgradeModule template)
            {
                this.ID = template.title.ToString();
                this.moduleWeight = template.moduleWeight;
                this.requiredCrew = template.requiredCrew;
                this.repairCrew = template.repairCrew;
                this.repairOptimalCrew = template.repairOptimalCrew;
                this.repairLimit = template.repairLimit;
                foreach (var mod in template.modifiers)
                {
                    this.modifiers.Add(new ModifierHotPatch(mod));
                }
                this.title = template.title;
                this.description = template.description;
                this.effect = template.effect;

                this.goldPrice = template.GoldPrice;
                this.category = template.category;
                this.categoryString = template.category.ToString();
                this.repairModule = template.repairModule;
                this.repairModuleString = template.repairModule.ToString();
                this.speciality = template.speciality;
                this.specialityString = template.speciality.ToString();
                this.shipKinds = template.shipKinds;
                this.shipKindsString = template.shipKinds.ToString();
                this.isTemporary = template.isTemporary;
            }

            public void Update(ref UpgradeModule template)
            {
                template.moduleWeight = this.moduleWeight;
                template.requiredCrew = this.requiredCrew;
                template.repairCrew = this.repairCrew;
                template.repairOptimalCrew = this.repairOptimalCrew;
                template.repairLimit = this.repairLimit;
                template.modifiers.Clear();
                foreach (var modifier in this.modifiers)
                {
                    template.modifiers.Add(modifier.Deserialize());
                }

                template.GoldPrice = this.goldPrice;
                template.category = this.category;
                template.repairModule = this.repairModule;
                template.speciality = this.speciality;
                template.shipKinds = this.shipKinds;
                template.isTemporary = this.isTemporary;
            }

            public class ModifierHotPatch
            {
                public EModifier enumID { get; set; }
                public string enumIDString { get; set; }
                public float floatValue { get; set; }
                public string stringValue { get; set; }
                public AnimationCurve curveValue { get; set; }
                public EModifierConditionDependency dependency { get; set; }
                public string dependencyString { get; set; }
                public EModifierSign sign { get; set; }
                public string signString { get; set; }

                public ModifierHotPatch() { }
                public ModifierHotPatch(Modifier m)
                {
                    this.enumID = m.enumID;
                    this.floatValue = m.floatValue;
                    this.stringValue = m.stringValue;
                    this.curveValue = m.curveValue;
                    this.dependency = m.dependency;
                    this.sign = m.sign;
                    this.enumIDString = m.enumID.ToString();
                    this.dependencyString = m.dependency.ToString();
                    this.signString = m.sign.ToString();
                }

                public Modifier Deserialize()
                {
                    return new Modifier(this.enumID)
                    {
                        curveValue = this.curveValue,
                        dependency = this.dependency,
                        floatValue = this.floatValue,
                        sign = this.sign,
                        stringValue = this.stringValue
                    };
                }
            }

        }

        [HarmonyPatch(typeof(Game))]
        [HarmonyPatch("Load")]
        private class CampaignController_Patch
        {
            private static bool Initialized = true;
            private static void Postfix(Game __result)
            {
                string ConfigFile = @"Mods/EquipmentMod/ShipModules.txt";
                if (!File.Exists(ConfigFile))
                {
                    File.CreateText(ConfigFile).Close();
                    Initialized = false;
                }
                if (!Initialized)
                {
                    var upgradeModules = __result.campaign.campaignControllerData.campSettings.shopSettings.upgradePool.upgrades;
                    if (upgradeModules == null)
                        return;
                    var slimModules = new List<ModuleHotpatch>();
                    foreach (var mod in upgradeModules)
                    {
                        slimModules.Add(new ModuleHotpatch(mod));
                    }
                    File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(slimModules, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, MaxDepth = 2 }));
                }
                else
                {
                    var currentModules = JsonConvert.DeserializeObject<List<ModuleHotpatch>>(File.ReadAllText(ConfigFile));
                    var upgradeModules = __result.campaign.campaignControllerData.campSettings.shopSettings.upgradePool.upgrades;
                    foreach (var module in currentModules)
                    {
                        try
                        {
                            var matchingModule = upgradeModules.Find(x => x.title == module.ID);
                            module.Update(ref matchingModule);
                            matchingModule.OnAfterDeserialize();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

            }
        }
    }

    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        {
        }
    }
}
