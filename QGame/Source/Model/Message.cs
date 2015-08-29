using System;
using System.Collections.Generic;

namespace QGame {

	[Serializable]
	class Message : VitPro.Net.Message {
		public virtual IEnumerable<Message> Handle(Model model) { 
			return null; 
		}
		public virtual IEnumerable<Message> HandleServer(Server server) {
			return null;
		}
		public virtual IEnumerable<Message> HandleClient(Client client) {
			return null;
		}
	}

}
