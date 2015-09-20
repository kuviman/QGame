using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {
	
    class Entity {
		
		bool _local;
		public bool Local {
			get { return _local; }
			internal set { _local = value; }
		}

		public int OwnerId { get; internal set; }

		Model _model;
		public Model Model {
			get { return _model; }
			internal set { _model = value; }
		}

        public long Id { get; private set; }

        public Entity(long id) {
			Local = true;
            Id = id;
			OwnerId = -1;
        }

		public void Set(string name, Component component) {
            component.Entity = this;
            components[name] = component;
        }

		public T Get<T>(string name) where T : Component {
            if (components.ContainsKey(name))
                return components[name] as T;
            return null;
        }

		public void Set<T>(Component component) where T : Component {
			Set(typeof(T).Name, component);
		}

		public T Get<T>() where T : Component {
			return Get<T>(typeof(T).Name);
		}

        Dictionary<string, Component> components = new Dictionary<string, Component>();

		[Serializable]
		public class Proto {
			int ownerId;
			long id;
			Dictionary<string,Component> components = new Dictionary<string, Component>();
			public Proto(Entity e) {
				foreach (var c in e.components) {
					components[c.Key] = c.Value;
				}
				ownerId = e.OwnerId;
				id = e.Id;
			}
			public Entity Reconstruct() {
				Entity e = new Entity(id);
				e.Local = false;
				e.OwnerId = ownerId;
				foreach (var c in components)
					e.Set(c.Key, c.Value);
				return e;
			}
		}

    }

    [Serializable]
    class Component {

		[NonSerialized]
        Entity _entity;
        public Entity Entity {
            get { return _entity; }
            internal set { _entity = value; }
        }

    }

}