using System;
using VitPro;

namespace QGame {

    partial class Messages {

        [Serializable]
        public class ChangeHealth : Message {
            long id;
            double dhp;
            public ChangeHealth(Entity e, double dhp) {
                id = e.Id;
                this.dhp = dhp;
            }
            public override System.Collections.Generic.IEnumerable<Message> Handle(Model model) {
                var e = model.FindEntity(id);
                if (e == null)
                    yield return new GetEntity(id);
                else
                    e.Get<HealthComponent>().Health += dhp;
            }
        }

    }

}