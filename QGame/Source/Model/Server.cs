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
			Model.Add(new CollisionSystem());
			Model.Add(new ServerCollisionSystem());
			Model.Add(new TerrainCollisionSystem());
			Model.Add(new MovementBroadcastingSystem());
			Model.Add(new WeaponSystem());
            Model.Add(new HealthSystem());

			for (int i = 0; i < 10; i++) {
                AddZombie();
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

        public void Run() {
            var timer = new Timer();
            while (true) {
                Update(timer.Tick());
            }
        }

        public override void Stop() {
            base.Stop();
            App.OnUpdate -= Update;
        }

		const double TICK_TIME = 0.01;
		double uncalcedTime;

        const int MAX_TICKS_PER_UPDATE = 50;

        void AddZombie() {
            var z = Model.CreateEntity();
            z.Set<RenderComponent>(new BasicUnitRenderComponent(1.2, 1.2, new ResourcedTexture("Units/zombie.png")));
            z.Set<AIComponent>(new AIComponent());
            z.Set<PhysicsComponent>(new PhysicsComponent(0.6));
            z.Set<PositionComponent>(new PositionComponent(
                new Vec3(GRandom.NextDouble(-10, 10), GRandom.NextDouble(-10, 10), 0),
                GRandom.NextDouble(0, 2 * Math.PI)));
            z.Set<MovementComponent>(new MovementComponent(2));
            z.Set<HealthComponent>(new HealthComponent(100));
            if (GRandom.Probable(0.7)) {
                var w = new Sword();
                w.Firing = true;
                z.Set<WeaponComponent>(new WeaponComponent(w));
            }
            Model.Add(z);
        }

        double nextBroadcast = 0;
        List<Message> messagesToBroadcast = new List<Message>();
        public void AddToBroadcast(Message message) {
            messagesToBroadcast.Add(message);
        }

		void Update(double dt) {
			Handle();
			uncalcedTime += dt;
            double curTime = 0;
            int ticks = 0;
			while (uncalcedTime >= TICK_TIME && ticks < MAX_TICKS_PER_UPDATE) {
                curTime += TICK_TIME;
				uncalcedTime -= TICK_TIME;
				Model.Update(TICK_TIME);
                ++ticks;
			}
            Model.UpdateOnce(curTime);

            //if (System.Linq.Enumerable.Count(Model.Entities) < 20) {
            //    AddZombie();
            //}

            nextBroadcast -= dt;
            if (nextBroadcast < 0) {
                nextBroadcast = LAG;
                Broadcast(new MessageList(messagesToBroadcast));
                messagesToBroadcast = new List<Message>();
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

        public override void Disconnect(int who) {
            base.Disconnect(who);
            Model.RemoveOwnedBy(who);
            Broadcast(new Messages.UserDisconnected(who));
        }

        public void RemoveEntity(Entity e) {
            Model.Remove(e);
            Broadcast(new Messages.RemoveEntity(e));
        }

		public Message Login(string handle, int sender) {
			Entity e = Model.CreateEntity();
			e.OwnerId = sender;
			const double d = 2;
			e.Set<PositionComponent>(new PositionComponent(
				new Vec3(GRandom.NextDouble(-d, d), GRandom.NextDouble(-d, d), 0), 0));
			e.Set<MovementComponent>(new MovementComponent(3));
			e.Set<RenderComponent>(new BasicUnitRenderComponent(1, 1, new ResourcedTexture("Units/player.png")));
			e.Set<PhysicsComponent>(new PhysicsComponent(0.5));
			e.Set<HealthComponent>(new HealthComponent(75));
			e.Set<WeaponComponent>(new WeaponComponent(new Sword()));
			e.Get<HealthComponent>().Health *= 0.8;
			return new Messages.LogicSuccess(e);
		}

	}

}
