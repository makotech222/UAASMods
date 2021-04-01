using System;
using UltimateAdmiral;
using UnityEngine;

namespace EquipmentMod
{
	public class CannonHotpatch
	{
		public class BallisticsHotPatch
		{
			public float verticalSpread
			{
				get;
				set;
			}

			public float radius
			{
				get;
				set;
			}

			public float mass
			{
				get;
				set;
			}

			public float horizontalSpread
			{
				get;
				set;
			}

			public float gravity
			{
				get;
				set;
			}

			public float distance
			{
				get;
				set;
			}

			public float baseY
			{
				get;
				set;
			}

			public AnimationCurve armorPiercing
			{
				get;
				set;
			}
		}

		public string ID
		{
			get;
			set;
		}

		public float crew
		{
			get;
			set;
		}

		public float groundBatterySize
		{
			get;
			set;
		}

		public float horizontalTurnMax
		{
			get;
			set;
		}

		public float navalBatterySize
		{
			get;
			set;
		}

		public float reloadTime
		{
			get;
			set;
		}

		public float threat
		{
			get;
			set;
		}

		public string type
		{
			get;
			set;
		}

		public float verticalTurnMax
		{
			get;
			set;
		}

		public float verticalTurnMin
		{
			get;
			set;
		}

		public float weight
		{
			get;
			set;
		}

		public int goldPrice
		{
			get;
			set;
		}

		public CannonHotpatch.BallisticsHotPatch Ballistics
		{
			get;
			set;
		}

		public CannonHotpatch()
		{
			this.Ballistics = new CannonHotpatch.BallisticsHotPatch();
		}

		public CannonHotpatch(CannonModule template)
		{
			this.Ballistics = new CannonHotpatch.BallisticsHotPatch();
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

		public void Update(ref CannonModule template)
		{
			template.crew = this.crew;
			template.groundBatterySize = this.groundBatterySize;
			template.horizontalTurnMax = this.horizontalTurnMax;
			template.navalBatterySize = this.navalBatterySize;
			template.reloadTime = this.reloadTime;
			template.threat = this.threat;
			template.type = ((this.type == "Carronade") ? ECannonType.Carronade : ECannonType.Cannon);
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
	}
}
