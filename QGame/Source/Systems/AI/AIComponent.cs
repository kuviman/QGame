using System;
using VitPro;

namespace QGame {

	[Serializable]
	class AIComponent : Component {

		public void Update(double dt) {
			Entity closest = null;
			double distanceToClosest = -1;
			foreach (var e in Entity.Model.Entities) {
				if (e.Local == Entity.Local)
					continue;
				var distance = (e.Get<PositionComponent>().Position - Entity.Get<PositionComponent>().Position).Length;
				if (closest == null || distance < distanceToClosest) {
					distanceToClosest = distance;
					closest = e;
				}
			}
			if (closest != null) {
				var dv = closest.Get<PositionComponent>().Position.XY -
					Entity.Get<PositionComponent>().Position.XY;
				Entity.Get<PositionComponent>().Rotation = (dv).Arg;
				dv = dv.Unit;
				Entity.Get<MovementComponent>().MoveDirection = new Vec3(dv.X, dv.Y, 0);
			}
		}

	}

}
