using System;
using VitPro;
using VitPro.Engine;

namespace QGame {

	class WeaponRenderSystem : EntitySystem {

		public override bool Filter(Entity entity) {
			return entity.Get<WeaponComponent>() != null;
		}

		public override void Render(Model model) {
			base.Render(model);
			foreach (var e in Entities) {
				var weapon = e.Get<WeaponComponent>().Weapon;
				if (weapon != null)
					weapon.Render();
			}
		}

	}

}

