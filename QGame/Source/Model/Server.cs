using System;
using System.Collections.Generic;
using System.Threading;
using VitPro;
using VitPro.Engine;
using Timer = VitPro.Engine.Timer;

namespace QGame {

	class Server : VitPro.Net.Server<Message> {

		public const double LAG = 0.05;

		public Model Model { get; private set; }

		public Server(int port) : base(port) {
			Model = new Model(this);

			Model.Add(new AISystem());
			Model.Add(new MovementSystem());
			Model.Add(new TerrainCollisionSystem());
			Model.Add(new CollisionSystem());
			Model.Add(new ServerCollisionSystem());
			Model.Add(new MovementBroadcastingSystem());

			for (int i = 0; i < 10; i++) {
				var z = Model.CreateEntity();
				z.Set<RenderComponent>(new BasicUnitRenderComponent(1.2, 1.2, new ResourcedTexture("Units/zombie.png")));
				z.Set<AIComponent>(new AIComponent());
				z.Set<PhysicsComponent>(new PhysicsComponent(0.6));
				z.Set<PositionComponent>(new PositionComponent(
					new Vec3(GRandom.NextDouble(-10, 10), GRandom.NextDouble(-10, 10), 0),
					GRandom.NextDouble(0, 2 * Math.PI)));
				z.Set<MovementComponent>(new MovementComponent(2));
				Model.Add(z);
			}

			var terrain = Model.Terrain;
			const int D = 100;

			for (int t = 0; t < 100; t++) {
				int x = GRandom.Next(-D, D);
				int y = GRandom.Next(-D, D);
				const int dist = 5;
				int sign = -1;// GRandom.Coin() ? 1 : -1;
				for (int i = x - dist; i <= x + dist; i++)
					for (int j = y - dist; j <= y + dist; j++) {
						var vertex = terrain[i, j];
						vertex.Height += sign * (1 - Math.Pow(Math.Min(1, (new Vec2(i, j) - new Vec2(x, y)).Length / dist), 2)) * 0.3;
						terrain[i, j] = vertex;
					}
			}
			for (int i = -D; i < D; i++)
				for (int j = -D; j < D; j++) {
					var vertex = terrain[i, j];
					vertex.Texture = GRandom.Probable(0.5 + terrain[i, j].Height) ? Terrain.Textures.Grass : Terrain.Textures.Dirt;
					vertex.Height += GRandom.NextDouble(0.1, 0.2);
					terrain[i, j] = vertex;
				}

			for (int i = 0; i < 10; i++) {
				var e = Model.CreateEntity(true);
				var v = new Vec3(GRandom.NextDouble(-10, 10), GRandom.NextDouble(-10, 10), 0);
				v.Z = terrain.GetHeight(v.X, v.Y);
				e.Set<PositionComponent>(new PositionComponent(v, 0));
				e.Set<PhysicsComponent>(new PhysicsComponent(0.5));
				e.Set<RenderComponent>(new StaticModelComponent(2, 4, new ResourcedTexture("Misc/tree.png")));
				Model.Add(e);
			}
		}

		public void Start() {
			App.OnUpdate += Update;
		}

		const double TICK_TIME = 0.01;
		double uncalcedTime;

		void Update(double dt) {
			Handle();
			uncalcedTime += dt;
			while (uncalcedTime >= TICK_TIME) {
				uncalcedTime -= TICK_TIME;
				Model.Update(TICK_TIME);
			}
		}

		protected override IEnumerable<Message> Handle(Message message) {
			var serverReplies = message.HandleServer(this);
			if (serverReplies != null) {
				foreach (var reply in serverReplies)
					yield return reply;
			}
			var replies = message.Handle(Model);
			if (replies != null) {
				foreach (var reply in replies)
					yield return reply;
			}
		}

		public Message Login(string handle, int sender) {
			Entity e = Model.CreateEntity();
			e.OwnerId = sender;
			e.Set<PositionComponent>(new PositionComponent(Vec3.Zero, 0));
			e.Set<MovementComponent>(new MovementComponent(3));
			e.Set<RenderComponent>(new BasicUnitRenderComponent(1, 1, new ResourcedTexture("Misc/face.png")));
			e.Set<PhysicsComponent>(new PhysicsComponent(0.5));
			return new Messages.LogicSuccess(e);
		}

	}

}
