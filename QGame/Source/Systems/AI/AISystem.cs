using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {

	class AISystem : EntitySystem {
		public override bool Filter(Entity entity) {
			return entity.Get<AIComponent>() != null;
		}

		public override void Update(Model model, double dt) {
			base.Update(model, dt);
			foreach (var e in Entities)
				e.Get<AIComponent>().Update(dt);
		}
    }

}