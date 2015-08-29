using System;
using System.Collections.Generic;
using VitPro;

namespace QGame {

	class Model {

		Model() {
			Terrain = new Terrain();
		}

		public Model(Client client) : this() {
			Client = client;
		}

		public Model(Server server) : this() {
			Server = server;
		}

		public Client Client { get; private set; }
		public Server Server { get; private set; }

		IdGenerator globalIdGen = new IdGenerator();
		IdGenerator localIdGen = new IdGenerator();

		public Entity CreateEntity(bool local = true) {
			return new Entity(local ? localIdGen.Next : globalIdGen.Next);
		}

		public Terrain Terrain { get; private set; }

		Dictionary<long, Entity> entityById = new Dictionary<long,Entity>();
        List<IEntitySystem> entitySystems = new List<IEntitySystem>();

		public Entity FindEntity(long id) {
			Entity e;
			if (entityById.TryGetValue(id, out e))
				return e;
			else
				return null;
		}

		public IEnumerable<Entity> Entities {
			get {
				return entityById.Values;
			}
		}

		public IEnumerable<IEntitySystem> EntitySystems {
			get {
				return entitySystems;
			}
		}

        public void Add(Entity entity) {
            entity.Model = this;
            entityById[entity.Id] = entity;
			foreach (var system in EntitySystems)
                system.Add(entity);
        }

		public void Add(IEntitySystem system) {
			foreach (var entity in Entities)
				system.Add(entity);
			entitySystems.Add(system);
		}

        public void Render() {
			foreach (var system in EntitySystems)
                system.Render(this);
		}

		public void Update(double dt) {
			foreach (var system in EntitySystems)
				system.Update(this, dt);
		}

    }

}