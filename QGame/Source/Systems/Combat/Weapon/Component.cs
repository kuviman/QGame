using System;
using System.Runtime.Serialization;
using VitPro;

namespace QGame {

	[Serializable]
	class WeaponComponent : Component {

		Weapon weapon;
		public Weapon Weapon {
			get { return weapon; }
			set {
				weapon = value;
				if (weapon != null)
					weapon.component = this;
			}
		}

        [OnDeserialized]
        void FixSerialization(StreamingContext context) {
            weapon.component = this;
        }
			
		public WeaponComponent(Weapon weapon) {
			Weapon = weapon;
		}

		public void UpdateWeapon(Weapon weapon) {
			if (weapon == null || Weapon == null || weapon.GetType() != Weapon.GetType())
				Weapon = weapon;
			else
				Weapon.Update(weapon);
		}

	}

}
