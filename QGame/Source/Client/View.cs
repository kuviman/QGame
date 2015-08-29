using System;
using VitPro;
using VitPro.Engine;
using UI = VitPro.Engine.UI;

namespace QGame {

	partial class ClientView : UI.State {

		Client client;
		Model model;

		public ClientView(string address, int port) {
			client = new Client(address, port);
			model = client.Model;
			Background = new SceneView(this);
		}

	}

}
