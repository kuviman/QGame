using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {

    class ServerCollisionSystem : EntitySystem {
		const double MAX_PUSH_PER_SEC = 1;

		public override bool Filter(Entity entity) {
			return entity.Get<PhysicsComponent>() != null && entity.Get<MovementComponent>() != null;
		}

        public override void Update(Model model, double dt) {
			base.Update(model, dt);
			foreach (var e in Entities) {
				if (e.Get<PushComponent>() == null) {
					e.Set<PushComponent>(new PushComponent());
				}
				e.Get<PushComponent>().Update(dt);
			}
			foreach (var m1 in Entities) {
				foreach (var m2 in Entities) {
                    if (m1 == m2)
						continue;
					Vec3 dr = m2.Get<PositionComponent>().Position - m1.Get<PositionComponent>().Position;
					if (dr.Length < m1.Get<PhysicsComponent>().Size + m2.Get<PhysicsComponent>().Size) {
						var n = dr.Unit;
						dr = n * (m1.Get<PhysicsComponent>().Size + m2.Get<PhysicsComponent>().Size - dr.Length) / 2;
						if (!m1.Local || !m2.Local)
							dr = Vec3.Clamp(dr, dt * MAX_PUSH_PER_SEC);
						m1.Get<PushComponent>().Push(-dr);
						m2.Get<PushComponent>().Push(+dr);

						var dv = Vec3.Zero;
						if (m2.Local)
							dv += m2.Get<MovementComponent>().Velocity;
						if (m1.Local)
							dv -= m1.Get<MovementComponent>().Velocity;
						var dvn = Vec3.Dot(dv, n);
						if (dvn < 0) {
							if (m1.Local)
								m1.Get<MovementComponent>().Velocity += n * dvn;
							if (m2.Local)
								m2.Get<MovementComponent>().Velocity -= n * dvn;
						}
                    }
                }
            }
        }
    }

}