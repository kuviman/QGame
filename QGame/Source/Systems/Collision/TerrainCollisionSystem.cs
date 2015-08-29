using System;
using VitPro;

namespace QGame {

	class TerrainCollisionSystem : EntitySystem {
		
		public override bool Filter(Entity entity) {
			return entity.Local && entity.Get<PositionComponent>() != null;
		}

		public override void Update(Model model, double dt) {
			base.Update(model, dt);

			foreach (var e in Entities) {
				Vec2 xy = e.Get<PositionComponent>().Position.XY;
				e.Get<PositionComponent>().Position = new Vec3(xy.X, xy.Y, model.Terrain.GetHeight(xy.X, xy.Y));
			}
		}

	}

}
