using System;
using System.Collections.Generic;
using UltimateAdmiral;
using UnityEngine;

namespace EquipmentMod
{
	public class ModuleHotpatch
	{
		public class ModifierHotPatch
		{
			public EModifier enumID
			{
				get;
				set;
			}

			public string enumIDString
			{
				get;
				set;
			}

			public float floatValue
			{
				get;
				set;
			}

			public string stringValue
			{
				get;
				set;
			}

			public AnimationCurve curveValue
			{
				get;
				set;
			}

			public EModifierConditionDependency dependency
			{
				get;
				set;
			}

			public string dependencyString
			{
				get;
				set;
			}

			public EModifierSign sign
			{
				get;
				set;
			}

			public string signString
			{
				get;
				set;
			}

			public ModifierHotPatch()
			{
			}

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

		public string ID
		{
			get;
			set;
		}

		public int requiredCrew
		{
			get;
			set;
		}

		public int repairCrew
		{
			get;
			set;
		}

		public float repairOptimalCrew
		{
			get;
			set;
		}

		public float repairLimit
		{
			get;
			set;
		}

		public List<ModuleHotpatch.ModifierHotPatch> modifiers
		{
			get;
			set;
		}

		public string title
		{
			get;
			set;
		}

		public string description
		{
			get;
			set;
		}

		public string effect
		{
			get;
			set;
		}

		public int goldPrice
		{
			get;
			set;
		}

		public EUpgradeCategory category
		{
			get;
			set;
		}

		public string categoryString
		{
			get;
			set;
		}

		public EModuleIndex repairModule
		{
			get;
			set;
		}

		public string repairModuleString
		{
			get;
			set;
		}

		public EUpgradeSpecial speciality
		{
			get;
			set;
		}

		public string specialityString
		{
			get;
			set;
		}

		public EShipKindMask shipKinds
		{
			get;
			set;
		}

		public string shipKindsString
		{
			get;
			set;
		}

		public bool isTemporary
		{
			get;
			set;
		}

		public ModuleHotpatch()
		{
			this.modifiers = new List<ModuleHotpatch.ModifierHotPatch>();
			this.repairModule = EModuleIndex.None;
			this.shipKinds = (EShipKindMask)3;
		}

		public ModuleHotpatch(UpgradeModule template)
		{
			this.modifiers = new List<ModuleHotpatch.ModifierHotPatch>();
			this.repairModule = EModuleIndex.None;
			this.shipKinds = (EShipKindMask)3;
			this.ID = template.title.ToString();
			this.requiredCrew = template.requiredCrew;
			this.repairCrew = template.repairCrew;
			this.repairOptimalCrew = template.repairOptimalCrew;
			this.repairLimit = template.repairLimit;
			foreach (Modifier current in template.modifiers)
			{
				this.modifiers.Add(new ModuleHotpatch.ModifierHotPatch(current));
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
			template.requiredCrew = this.requiredCrew;
			template.repairCrew = this.repairCrew;
			template.repairOptimalCrew = this.repairOptimalCrew;
			template.repairLimit = this.repairLimit;
			template.modifiers.Clear();
			foreach (ModuleHotpatch.ModifierHotPatch current in this.modifiers)
			{
				template.modifiers.Add(current.Deserialize());
			}
			template.GoldPrice = this.goldPrice;
			template.category = this.category;
			template.repairModule = this.repairModule;
			template.speciality = this.speciality;
			template.shipKinds = this.shipKinds;
			template.isTemporary = this.isTemporary;
		}
	}
}
