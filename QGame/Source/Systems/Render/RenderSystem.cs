using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {

    class RenderSystem : EntitySystem {
        public override bool Filter(Entity entity) {
            return entity.Get<RenderComponent>() != null;
        }
        public override void Update(Model model, double dt) {
            base.Update(model, dt);
            foreach (var e in Entities)
                e.Get<RenderComponent>().Update(dt);
        }
        public override void Render(Model model) {
            base.Render(model);
            var entities = new List<Entity>(Entities);
            entities.Sort((Entity a, Entity b) => {
				Vec2 pa = Vec2.Rotate(a.Get<PositionComponent>().Position.XY, -model.Client.Camera.Rotation);
				Vec2 pb = Vec2.Rotate(b.Get<PositionComponent>().Position.XY, -model.Client.Camera.Rotation);
                return -pa.Y.CompareTo(pb.Y);
            });
            foreach (var e in entities) {
                RenderState.Push();
				RenderState.Translate(e.Get<PositionComponent>().Position);
                RenderState.FaceCam();
                e.Get<RenderComponent>().Render();
                RenderState.Pop();
            }
        }
    }

}