using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {

    [Serializable]
    class Entity {

		[NonSerialized]
		bool _local;
		public bool Local {
			get { return _local; }
			internal set { _local = value; }
		}

		public int OwnerId { get; internal set; }

        [NonSerialized]
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

		private void Set(string name, Component component) {
            component.Entity = this;
            components[name] = component;
        }

		private T Get<T>(string name) where T : Component {
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

    }

    [Serializable]
    class Component {

        public Entity Entity { get; set; }

    }

}