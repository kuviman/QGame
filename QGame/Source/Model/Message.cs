using System;
using System.Collections.Generic;
using VitPro;

namespace QGame {

	
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

    
    class MessageList : Message {

		[Serialize]
        public List<Message> messages;

        MessageList() { }

        public MessageList(IEnumerable<Message> messages) {
            this.messages = new List<Message>(messages);
        }

        public override IEnumerable<Message> Handle(Model model) {
            foreach (var message in messages) {
                var replies = message.Handle(model);
                if (replies != null) {
                    foreach (var reply in replies)
                        yield return reply;
                }
            }
        }

        public override IEnumerable<Message> HandleServer(Server server) {
            foreach (var message in messages) {
                var replies = message.HandleServer(server);
                if (replies != null) {
                    foreach (var reply in replies)
                        yield return reply;
                }
            }
        }

        public override IEnumerable<Message> HandleClient(Client client) {
            foreach (var message in messages) {
                var replies = message.HandleClient(client);
                if (replies != null) {
                    foreach (var reply in replies)
                        yield return reply;
                }
            }
        }
    }

}
