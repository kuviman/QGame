using System;
using System.Collections.Generic;
using VitPro;

namespace QGame {

	partial class Messages {
		
		
		public class PushEntity : Message {

			[Serialize]
			public long id;

			[Serialize]
			public Vec3 v;

            PushEntity() { }
			public PushEntity(Entity entity, Vec3 dv) {
				id = entity.Id;
				v = dv;
			}
			public override IEnumerable<Message> Handle(Model model) {
				var e = model.FindEntity(id);
				if (e.Get<PushComponent>() == null)
					e.Set<PushComponent>(new PushComponent());
				e.Get<PushComponent>().Push(v);
				return null;
			}
		}

	}

}