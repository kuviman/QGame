using System;
using VitPro;

namespace QGame {

	class MovementPredictionSystem : EntitySystem {

		public override bool Filter(Entity entity) {
			return !entity.Local && entity.Get<MovementComponent>() != null;
		}

		public override void Update(Model model, double dt) {
			base.Update(model, dt);
			foreach (var e in Entities) {
				if (e.Get<MovementPredictionComponent>() == null)
					e.Set<MovementPredictionComponent>(new MovementPredictionComponent());
				e.Get<MovementPredictionComponent>().Update(dt);
			}
		}

	}

}
