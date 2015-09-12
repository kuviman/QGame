using System;
using VitPro;
using VitPro.Engine;
using UI = VitPro.Engine.UI;
using log4net;

namespace QGame {

	partial class ClientView : UI.State {
        static ILog log = LogManager.GetLogger(typeof(ClientView));

		Client client;
		Model model;

		public ClientView(string address, int port) {
			client = new Client(address, port);
			model = client.Model;
			Background = new SceneView(this);
		}

        public override void Close() {
            base.Close();
            client.Stop();
        }

        public override void KeyDown(Key key) {
            base.KeyDown(key);
            if (key == Key.Escape)
                Close();
        }

        public override void Update(double dt) {
            base.Update(dt);
            if (client.disconnectedReason != null) {
                log.Warn("Disconnected from the server", client.disconnectedReason);
                Close();
            }
        }

	}

}
