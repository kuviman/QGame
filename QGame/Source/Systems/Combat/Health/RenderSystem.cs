using System;
using VitPro;
using VitPro.Engine;

namespace QGame {

	class HealthRenderSystem : EntitySystem {

		public override bool Filter(Entity entity) {
			return entity.Get<HealthComponent>() != null;
		}

		public override void Render(Model model) {
			base.Render(model);
			foreach (var e in Entities) {
				RenderState.Push();
				RenderState.Translate(e.Get<PositionComponent>().Position);
				RenderState.FaceCam();
				RenderState.Translate(0, 1.7);
				RenderState.Scale(1, 0.1);
				RenderState.Origin(0.5, 0.5);
				RenderState.Color = Color.Red;
				Draw.Quad();
				RenderState.Scale(e.Get<HealthComponent>().Health / e.Get<HealthComponent>().MaxHealth, 1);
				RenderState.Color = Color.Green;
				Draw.Quad();
				RenderState.Pop();
			}
		}

	}

}
