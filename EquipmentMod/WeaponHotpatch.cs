using System;
using UltimateAdmiral;
using UnityEngine;

namespace EquipmentMod
{
	public class WeaponHotpatch
	{
		public string ID
		{
			get;
			set;
		}

		public float adaptationCoeff
		{
			get;
			set;
		}

		public float altitude
		{
			get;
			set;
		}

		public float ammoCost
		{
			get;
			set;
		}

		public int ammunition
		{
			get;
			set;
		}

		public float baseReload
		{
			get;
			set;
		}

		public int cannonRequiredStaff
		{
			get;
			set;
		}

		public float collateralRadius
		{
			get;
			set;
		}

		public float damage
		{
			get;
			set;
		}

		public float effectiveRange
		{
			get;
			set;
		}

		public float effectiveRangeHint
		{
			get;
			set;
		}

		public float meleeDamage
		{
			get;
			set;
		}

		public int price
		{
			get;
			set;
		}

		public float randHi
		{
			get;
			set;
		}

		public float randLow
		{
			get;
			set;
		}

		public string textDescription
		{
			get;
			set;
		}

		public string textName
		{
			get;
			set;
		}

		public string textShootType
		{
			get;
			set;
		}

		public AnimationCurve damageDegradation
		{
			get;
			set;
		}

		public BallisticProperties ballistics
		{
			get;
			set;
		}

		public WeaponHotpatch()
		{
		}

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

		public void Update(ref WeaponTemplate template)
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
}
