using System;
using VitPro;

namespace QGame {

    class HealthSystem : EntitySystem {

        public override bool Filter(Entity entity) {
            return entity.Get<HealthComponent>() != null;
        }

        double nextBroadcast = 0;

        public override void Update(Model model, double dt) {
            base.Update(model, dt);
            foreach (var e in Entities) {
                if (e.Get<HealthComponent>().Health <= 0) {
                    if (model.Server != null) {
                        model.Server.RemoveEntity(e);
                    }
                }
            }
        }

        public override void UpdateOnce(Model model, double dt) {
            base.UpdateOnce(model, dt);
            if (model.Server == null)
                return;
            nextBroadcast -= dt;
            if (nextBroadcast < 0) {
                nextBroadcast = Server.LAG;
                foreach (var e in Entities) {
                    model.Server.AddToBroadcast(Messages.UpdateComponent.Create(e.Get<HealthComponent>()));
                }
            }
        }

    }

}