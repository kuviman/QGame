using System;
using NDesk.Options;
using log4net;
using VitPro;
using VitPro.Engine;
using UI = VitPro.Engine.UI;

namespace QGame {

	class SplitScreen : UI.State {
		UI.StateFrame s1, s2;
		public SplitScreen(string ip, int port) {
            var c1 = new ClientView(ip, port);
            OnClose += c1.Close;
			s1 = new UI.StateFrame(c1);
            var c2 = new ClientView(ip, port);
            OnClose += c2.Close;
			s2 = new UI.StateFrame(c2);
			s1.Origin = s2.Origin = Vec2.Zero;
			Frame.Add(s1);
			Frame.Add(s2);
		}
		public override void Update(double dt) {
			base.Update(dt);
			s1.Size = s2.Size = new Vec2(Frame.Size.X / 2, Frame.Size.Y);
			s2.Position = new Vec2(Frame.Size.X / 2, 0);
		}
		public override void Render() {
			base.Render();
			RenderState.Push();
			RenderState.Color = Color.Black;
			RenderState.View2d(0, 0, RenderState.Width, RenderState.Height);
			Draw.Line(RenderState.Width / 2, 0, RenderState.Width / 2, RenderState.Height, 3);
			RenderState.Pop();
		}
        public override void KeyDown(Key key) {
            base.KeyDown(key);
            if (key == Key.Escape)
                Close();
        }
	}

    class Menu : UI.State {
        public Menu() {
            Zoom = 1.5;
            BackgroundColor = Color.Sky;
            var list = new UI.ElementList();
            int port = 7777;
            list.Add(new UI.Button("Start", () => {
                var server = new Server(port);
                var view = new ClientView("127.0.0.1", port);
                server.Start();
                view.OnClose += server.Stop;
                PushState(view);
            }));
            list.Add(new UI.Button("Start splitscreen", () => {
                var server = new Server(port);
                var view = new SplitScreen("127.0.0.1", port);
                server.Start();
                view.OnClose += server.Stop;
                PushState(view);
            }));
            var l2 = new UI.ElementList();
            l2.Horizontal = true;
            var ipInput = new UI.TextInput(200);
            l2.Add(ipInput);
            l2.Add(new UI.Button("Connect", () => {
                PushState(new ClientView(ipInput.Value, port));
            }));
            list.Add(l2);
            list.Anchor = list.Origin = new Vec2(0.5, 0.5);
            Frame.Add(list);
        }
    }

	class QGame {
        static ILog log = LogManager.GetLogger(typeof(QGame));

        static void Main(string[] args) {
            App.Init();
            log.Info("Parsing command line arguments");
            bool startServer = false;
            string ip = null;
            new OptionSet()
                .Add("server", (string val) => { startServer = val != null; })
                .Add("connect=", (string val) => { ip = val; })
                .Parse(args);
            if (startServer) {
                log.Info("Starting server");
                new Server(7777).Run();
            } else {
                log.Info("Starting the game");
                State state;
                if (ip == null)
                    state = new Menu();
                else
                    state = new ClientView(ip, 7777);
                App.Run(state);
            }
        }

	}

}
