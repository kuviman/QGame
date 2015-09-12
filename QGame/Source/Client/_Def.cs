using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {

	class Client : VitPro.Net.Client<Message> {

		public Model Model { get; private set; }

		protected override IEnumerable<Message> Handle(Message message) {
			var clientReplies = message.HandleClient(this);
			if (clientReplies != null) {
				foreach (var reply in clientReplies)
					yield return reply;
			}
			var replies = message.Handle(Model);
			if (replies != null) {
				foreach (var reply in replies)
					yield return reply;
			}
		}

		public Client(string address, int port) : base(address, port) {
			Camera = new Camera(Math.PI / 2);
			Camera.UpAngle = -Math.PI / 2.2;
			Camera.Distance = 6;

			Model = new Model(this);
			Model.Add(new MovementSystem());
			Model.Add(new MovementPredictionSystem());
			Model.Add(new CollisionSystem());
			Model.Add(new TerrainCollisionSystem());
            Model.Add(new WeaponSystem());
            Model.Add(new RenderSystem());
            Model.Add(new WeaponRenderSystem());
			Model.Add(new HealthRenderSystem());
            Model.Add(new HealthSystem());

			string handle = "kuviman";
			Send(new Messages.Login(handle));

			App.OnUpdate += Update;
		}

        public override void Stop() {
            base.Stop();
            App.OnUpdate -= Update;
        }

		public Camera Camera { get; internal set; }

		public Entity Player { get; internal set; }

        internal Exception disconnectedReason;

		double nextUpd = 0;
		public void Update(double dt) {
            try {
                Handle();
            } catch (VitPro.Net.NetException e) {
                disconnectedReason = e;
            }
			if (Player == null)
				return;
			nextUpd -= dt;
			if (nextUpd < 0) {
				nextUpd = Server.LAG;
				Send(new Messages.QueryEntities());
				Send(new Messages.MovementUpdate(Player));
				var pos = Player.Get<PositionComponent>().Position;
				Vec2i? unloaded = Model.Terrain.ClosestUnloaded(
					(int)pos.X, (int)pos.Y, 20);
				if (unloaded.HasValue) {
					int x = unloaded.Value.X;
					int y = unloaded.Value.Y;
					const int d = 5;
					Send(new Messages.QueryTerrain(x - d, y - d, x + d, y + d));
				}
			}
			Model.Update(dt);
            Model.UpdateOnce(dt);
		}

	}

}
