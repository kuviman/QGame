using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {

    interface IEntitySystem {

        void Add(Entity entity);
        void Update(Model model, double dt);
        void Render(Model model);

    }

    abstract class EntitySystem : IEntitySystem {

        Group<Entity> entities = new Group<Entity>();
        public IEnumerable<Entity> Entities {
            get {
                foreach (var entity in entities)
                    yield return entity;
            }
        }

        public event Action<Entity> OnAdd;

        public void Add(Entity entity) {
            if (Filter(entity)) {
                entities.Add(entity);
                OnAdd.Apply(entity);
            }
        }

        public abstract bool Filter(Entity entity);

        public virtual void Update(Model model, double dt) {
            entities.Refresh();
        }

        public virtual void Render(Model model) { }

    }

}