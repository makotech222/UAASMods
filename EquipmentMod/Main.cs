using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Fight.Modifier;
using GOAP;
using Harmony12;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UltimateAdmiral;
using UltimateAdmiral.UI;
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
                    currentWeapon.UpdateWeapon(ref cannonModule);
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
