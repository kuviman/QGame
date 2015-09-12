using System;
using VitPro;

namespace QGame {

	class MovementBroadcastingSystem : EntitySystem {
		public override bool Filter(Entity entity) {
			return entity.Get<MovementComponent>() != null;
		}
		double nextBroadcast = 0;
        public override void UpdateOnce(Model model, double dt) {
            base.UpdateOnce(model, dt);
            nextBroadcast -= dt;
            if (nextBroadcast < 0) {
                nextBroadcast = Server.LAG;
                foreach (var e in Entities) {
                    model.Server.Broadcast(new Messages.MovementUpdate(e));
                }
            }
        }
	}

}
