using System;
using VitPro;
using VitPro.Engine;

namespace QGame {
	
	class Sword : Weapon {
		
		double swing;

		public Sword() {
			Damage = 10;
		}

		public override void Update(double dt) {
			base.Update(dt);
            swing += dt * 3;
		}

		public override void Render() {
			base.Render();
			RenderState.Push();
            RenderState.DepthTest = true;
			RenderState.Translate(Entity.Get<PositionComponent>().Position);
            RenderState.FaceCam();
            RenderState.Translate(0, 0.5);
            RenderState.RotateX(-0.5);
            RenderState.Rotate(Entity.Get<PositionComponent>().Rotation);
			if (Firing)
				RenderState.Rotate(Math.Sin(swing * 2 * Math.PI) * 0.8);
			RenderState.Translate(0.5, 0);
			RenderState.Scale(1, 0.5);
			RenderState.Origin(0, 0.5);
			Resources.texture.Render();
			RenderState.Pop();
		}

		[Serialize]
		public double Damage { get; set; }

        public override void CheckFire() {
            base.CheckFire();
            if (Firing) {
                if (swing > 1) {
                    swing = GMath.Frac(swing);
                    // TODO use system
                    foreach (var e in Entity.Model.Entities) {
                        if (e == Entity)
                            continue;
                        if (e.Get<HealthComponent>() == null)
                            continue;
                        var myPos = Entity.Get<PositionComponent>().Position;
                        var ePos = e.Get<PositionComponent>().Position;
                        if (Math.Abs(GMath.AngleDifference((ePos - myPos).XY.Arg, Entity.Get<PositionComponent>().Rotation)) > Math.PI / 4)
                            continue;
                        if ((myPos - ePos).Length > 2)
                            continue;
                        if (Entity.Model.Client == null)
                            e.Get<HealthComponent>().Health -= Damage;
                        else
                            Entity.Model.Client.Send(new Messages.ChangeHealth(e, -Damage));
                    }
                }
            }
        }

		static class Resources {
			public static Texture texture = new Texture(Resource.Stream("Weapons/sword.png"));
		}

	}

}
