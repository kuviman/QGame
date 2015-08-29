using System;
using VitPro;

namespace QGame {

	[Serializable]
	class MovementSystem : EntitySystem {
		public override bool Filter(Entity entity) {
			return entity.Get<MovementComponent>() != null && entity.Get<MovementPredictionComponent>() == null;
		}

		public override void Update(Model model, double dt) {
			base.Update(model, dt);
			foreach (var e in Entities) {
				e.Get<MovementComponent>().Update(dt);
				var p = e.Get<PushComponent>();
				if (p != null) {
					p.Update(dt);
				}
			}

		}
	}

}
