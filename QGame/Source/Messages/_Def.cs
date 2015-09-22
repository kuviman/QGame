using System;
using System.Collections.Generic;
using VitPro.Net;
using VitPro;

namespace QGame {

    static partial class Messages {

		
		public class Login : Message {

			[Serialize]
			public string handle;

            Login() { }
			public Login(string handle) {
				this.handle = handle;
			}
			public override IEnumerable<Message> HandleServer(Server server) {
				yield return server.Login(handle, Sender);
			}
		}

		
		public class LogicSuccess : Message {

			[Serialize]
            public Entity entity;

            LogicSuccess() { }
			public LogicSuccess(Entity entity) {
				this.entity = entity;
			}
			public override IEnumerable<Message> HandleClient(Client client) {
				entity.Local = true;
				client.Model.Add(entity);
				client.Player = entity;
				return null;
			}
		}

    }

}