using System;
using VitPro;
using VitPro.Engine;
using System.Collections.Generic;

namespace QGame {

    partial class Messages {

        
        public class RemoveEntity : Message {

			[Serialize]
            public long entityId;

            RemoveEntity() { }

            public RemoveEntity(Entity e) {
                entityId = e.Id;
            }

            public override IEnumerable<Message> Handle(Model model) {
                model.Remove(entityId);
                return null;
            }
        }

        
        public class UpdateComponent : Message {

			[Serialize]
            public long entityId;

			[Serialize]
            public string componentName;

			[Serialize]
            public Component component;

            UpdateComponent() { }

            public UpdateComponent(string name, Component component) {
                entityId = component.Entity.Id;
                componentName = name;
                this.component = component;
            }

            public static UpdateComponent Create<T>(T component) where T : Component {
                return new UpdateComponent(typeof(T).Name, component);
            }

            public override IEnumerable<Message> Handle(Model model) {
                var e = model.FindEntity(entityId);
                if (e == null)
                    yield return new GetEntity(entityId);
                else
                    e.Set(componentName, component);
            }
        }

        
        public class UserDisconnected : Message {

			[Serialize]
            public int userId;

            UserDisconnected() { }

            public UserDisconnected(int userId) {
                this.userId = userId;
            }
            public override IEnumerable<Message> HandleClient(Client client) {
                client.Model.RemoveOwnedBy(userId);
                return null;
            }
            public override IEnumerable<Message> HandleServer(Server server) {
                server.Disconnect(userId);
                return null;
            }
        }

		
		public class CheckEntity : Message {

			[Serialize]
			public long id;

            CheckEntity() { }
			public CheckEntity(Entity e) {
				id = e.Id;
			}
			public override IEnumerable<Message> Handle(Model model) {
				if (model.FindEntity(id) == null)
					yield return new GetEntity(id);
			}
		}

        
        public class QueryEntities : Message {
			public override IEnumerable<Message> Handle(Model model) {
                List<Message> queries = new List<Message>();
				foreach (var entity in model.Entities) {
					queries.Add(new CheckEntity(entity));
                }
                yield return new MessageList(queries);
            }
        }

        
        public class GetEntity : Message {

			[Serialize]
            public long id;

            GetEntity() { }
            public GetEntity(long id) {
                this.id = id;
            }
			public override IEnumerable<Message> Handle(Model model) {
				var entity = model.FindEntity(id);
                if (entity != null) {
                    yield return new NewEntity(entity);
                } else {
                    //throw new Exception("Entity not found!");
                }   

            }
        }

        
        public class NewEntity : Message {

			[Serialize]
			public Entity entity;

            NewEntity() { }
            public NewEntity(Entity entity) {
				this.entity = entity;
			}
			public override IEnumerable<Message> Handle(Model model) {
				if (model.FindEntity(entity.Id) != null)
                    return null;
				model.Add(entity);
                return null;
            }
        }

    }

}