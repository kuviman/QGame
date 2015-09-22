using System;
using VitPro;

namespace QGame {
	
	class Weapon {
		
		internal WeaponComponent component;

		public Entity Entity { get { return component.Entity; } }

		[Serialize]
		public bool Firing { get; set; }

		public virtual void Update(double dt) {}
		public virtual void Render() {}

		public virtual void Update(Weapon upd) {
			Firing = upd.Firing;
		}

        public virtual void CheckFire() { }

	}

}
