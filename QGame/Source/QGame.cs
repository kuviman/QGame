using System;
using NDesk.Options;
using log4net;
using VitPro;
using VitPro.Engine;
using UI = VitPro.Engine.UI;

namespace QGame {

	class SplitScreen : UI.State {
		UI.Element s1, s2;
		public SplitScreen(string ip, int port) {
			s1 = new UI.StateFrame(new ClientView(ip, port));
			s2 = new UI.StateFrame(new ClientView(ip, port));
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
	}

	class QGame {
        static ILog logger = LogManager.GetLogger(typeof(QGame));

        static void Main(string[] args) {
            logger.Info("Parsing command line arguments");
            string ip = "127.0.0.1";
            int port = 7777;
            bool startServer = true;
            bool fullscreen = false;
			bool splitscreen = false;
            new OptionSet()
				.Add("splitscreen", (string val) => { splitscreen = val != null; })
                .Add("fullscreen", (string val) => { fullscreen = val != null; })
                .Add("connect=", (string val) => {
                    if (val.Contains(":")) {
                        var ipAndPort = val.Split(':');
                        ip = ipAndPort[0];
                        port = int.Parse(ipAndPort[1]);
                    } else
                        ip = val;
                    startServer = false;
                }).Parse(args);
            if (startServer) {
                logger.Info("Starting server");
				new Server(port).Start();
            }
            logger.Info("Starting the game");
            App.Fullscreen = fullscreen;
			if (splitscreen)
            	App.Run(new SplitScreen(ip, port));
			else
				App.Run(new ClientView(ip, port));
        }

	}

}
