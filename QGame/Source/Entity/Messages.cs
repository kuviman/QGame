using System;
using VitPro;
using VitPro.Engine;
using System.Collections.Generic;

namespace QGame {

    partial class Messages {

		[Serializable]
		public class CheckEntity : Message {
			long id;
			public CheckEntity(Entity e) {
				id = e.Id;
			}
			public override IEnumerable<Message> Handle(Model model) {
				if (model.FindEntity(id) == null)
					yield return new GetEntity(id);
			}
		}

        [Serializable]
        public class QueryEntities : Message {
			public override IEnumerable<Message> Handle(Model model) {
				foreach (var entity in model.Entities) {
					yield return new CheckEntity(entity);
                }
            }
        }

        [Serializable]
        public class GetEntity : Message {
            long id;
            public GetEntity(long id) {
                this.id = id;
            }
			public override IEnumerable<Message> Handle(Model model) {
				var entity = model.FindEntity(id);
				if (entity == null) {
					throw new Exception("Entity not found!");
				}
				yield return new NewEntity(entity);
            }
        }

        [Serializable]
        public class NewEntity : Message {
            Entity entity;
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