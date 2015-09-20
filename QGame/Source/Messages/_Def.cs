using System;
using System.Collections.Generic;
using VitPro.Net;
using VitPro;

namespace QGame {

    static partial class Messages {

		[Serializable]
		public class Login : Message {
			string handle;
			public Login(string handle) {
				this.handle = handle;
			}
			public override IEnumerable<Message> HandleServer(Server server) {
				yield return server.Login(handle, Sender);
			}
		}

		[Serializable]
		public class LogicSuccess : Message {
			Entity.Proto entity;
			public LogicSuccess(Entity entity) {
				this.entity = new Entity.Proto(entity);
			}
			public override IEnumerable<Message> HandleClient(Client client) {
				var e = entity.Reconstruct();
				e.Local = true;
				client.Model.Add(e);
				client.Player = e;
				return null;
			}
		}

    }

}