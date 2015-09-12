using System;
using VitPro;

namespace QGame {

	partial class Messages {

		[Serializable]
		public class WeaponUpdate : Message {

			long entityId;
			Weapon weapon;

			public WeaponUpdate(Entity entity) {
				entityId = entity.Id;
				this.weapon = entity.Get<WeaponComponent>().Weapon;
			}

			public override System.Collections.Generic.IEnumerable<Message> Handle(Model model) {
				var e = model.FindEntity(entityId);
				if (e == null)
					yield return new GetEntity(entityId);
				else {
					e.Get<WeaponComponent>().UpdateWeapon(weapon);
				}
			}

		}

	}

}
