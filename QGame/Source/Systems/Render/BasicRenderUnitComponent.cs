using System;
using VitPro;
using VitPro.Engine;

namespace QGame {

    [Serializable]
    class BasicUnitRenderComponent : RenderComponent {

        double a, k;
        Vec3 prevPos;
		ResourcedTexture texture;

        double w, h;

		public BasicUnitRenderComponent(double w, double h, ResourcedTexture texture) {
			this.texture = texture;
            this.w = w;
            this.h = h;
        }

        public override void Update(double dt) {
            base.Update(dt);
			var len = (Entity.Get<PositionComponent>().Position - prevPos).Length;
            a += len * 5;
            k = GMath.Clamp(k + len * 10 - dt * 10, 0, 1);
			prevPos = Entity.Get<PositionComponent>().Position;
        }

        public override void Render() {
            RenderState.Push();
            RenderState.Push();
            double s = Math.Sin(a) * k / 10;
            RenderState.Color = Color.Black;
            var d = 0.2;
            Draw.Circle(-d, Math.Max(0, s), 0.08);
            Draw.Circle(d, Math.Max(0, -s), 0.08);
            RenderState.Pop();
            RenderState.Translate(0, 0.25);
			RenderState.Scale(w, h);
            RenderState.Origin(0.5, 0);
            RenderState.Set("texture", texture);
			RenderState.Set("rotation", (Entity.Model.Client.Camera.Rotation - 
				Entity.Get<PositionComponent>().Rotation - Math.PI / 2) / Math.PI / 2);
			RotatedShader.Instance.RenderQuad();
            RenderState.Color = Color.Black;
            Draw.Frame(0, 0, 1, 1, 0.05);
            RenderState.Pop();
        }
        
		static class RotatedShader {
			public static Shader Instance = new Shader(Resource.String("Shaders/Rotated.glsl"));
		}

    }

}