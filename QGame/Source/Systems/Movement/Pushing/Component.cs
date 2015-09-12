using System;
using VitPro;

namespace QGame {

	[Serializable]
	class PushComponent : Component {

		const double PUSH_SPEED = 5;
		
		public void Push(Vec3 v) {
			if (!Entity.Local) {
				pushV += v;
			} else if (Entity.Model.Server != null) {
				Entity.Get<PositionComponent>().Position += v;
			} else {
				pushV += v;
			}
		}

		Vec3 pushV = Vec3.Zero;

		double nextSend = 0;

		public void Update(double dt) {
			nextSend -= dt;
			if (Entity.Local) {
				var dv = pushV * GMath.Clamp(dt * PUSH_SPEED, 1);
				Entity.Get<PositionComponent>().Position += dv;
				pushV -= dv;
			}
		}

        public void UpdateOnce(double dt) {
            if (!Entity.Local && nextSend < 0) {
                nextSend = Server.LAG;
                if (Entity.Model.Server != null)
                    Entity.Model.Server.SendTo(new Messages.PushEntity(Entity, pushV), Entity.OwnerId);
                else
                    Entity.Model.Client.Send(new Messages.PushEntity(Entity, pushV));
                pushV = Vec3.Zero;
            }
        }

	}

}
