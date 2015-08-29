using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {

    class CollisionSystem : IEntitySystem {
        List<Entity> staticEntitites = new List<Entity>();
        List<Entity> mobileEntities = new List<Entity>();

        public void Add(Entity entity) {
			if (entity.Get<PhysicsComponent>() == null)
				return;
			if (entity.Get<PositionComponent>() == null)
				return;
			if (entity.Get<MovementComponent>() == null)
                staticEntitites.Add(entity);
			else
                mobileEntities.Add(entity);
        }
        public void Update(Model model, double dt) {
			foreach (var entity in mobileEntities) {
				if (!entity.Local)
					continue;
                foreach (var s in staticEntitites) {
					Vec3 dr = entity.Get<PositionComponent>().Position - s.Get<PositionComponent>().Position;
					if (dr.Length < entity.Get<PhysicsComponent>().Size + s.Get<PhysicsComponent>().Size) {
						var n = dr.Unit;
						dr = n * (entity.Get<PhysicsComponent>().Size + s.Get<PhysicsComponent>().Size - dr.Length);
						entity.Get<PositionComponent>().Position += dr;
						var move = entity.Get<MovementComponent>();
						move.Velocity -= n * Vec3.Dot(n, move.Velocity);
                    }
                }
			}
        }
        public void Render(Model model) { }
    }

}