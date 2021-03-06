﻿using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {

	
    class Entity {
		
		bool _local = false;
		public bool Local {
			get { return _local; }
			internal set { _local = value; }
		}

		[Serialize]
		public int OwnerId { get; internal set; }

		Model _model;
		public Model Model {
			get { return _model; }
			internal set { _model = value; }
		}

		[Serialize]
        public long Id { get; private set; }

		public Entity() { }

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

		[Serialize]
		public IEnumerable<KeyValuePair<string, Component>> Components {
			get {
				return components;
			}
			set {
				components = new Dictionary<string, Component>();
				foreach (var entry in value) {
					Set(entry.Key, entry.Value);
				}
			}
		}

    }

    
    class Component {
		
        Entity _entity;
        public Entity Entity {
            get { return _entity; }
            internal set { _entity = value; }
        }

    }

}