using System;
using VitPro;

namespace QGame {

	class WeaponSystem : EntitySystem {
		public override bool Filter(Entity entity) {
			return entity.Get<WeaponComponent>() != null;
		}

		public override void Update(Model model, double dt) {
            base.Update(model, dt);
			foreach (var e in Entities) {
				var weapon = e.Get<WeaponComponent>().Weapon;
                if (weapon != null) {
                    weapon.Update(dt);
                    if (e.Local)
                        weapon.CheckFire();
                }
			}
		}

		double nextBroadcast = 0;
        public override void UpdateOnce(Model model, double dt) {
            base.UpdateOnce(model, dt);
            nextBroadcast -= dt;
            if (nextBroadcast < 0) {
                nextBroadcast = Server.LAG;
                foreach (var e in Entities) {
					var msg = new Messages.WeaponUpdate(e);
					if (model.Server != null)
						model.Server.Broadcast(msg);
					if (model.Client != null)
						model.Client.Send(msg);
                }
            }
        }
	}

}
