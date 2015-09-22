using System;
using VitPro;
using VitPro.Engine;

namespace QGame {

    
    class StaticModelComponent : RenderComponent {
		
		[Serialize]
		public double w;

		[Serialize]
		public double h;

		[Serialize]
        public ResourcedTexture texture;

        public StaticModelComponent(double w, double h, ResourcedTexture texture) {
            this.w = w;
            this.h = h;
            this.texture = texture;
        }

        public override void Render() {
            RenderState.Push();
            RenderState.Scale(w, h);
            RenderState.Origin(0.5, 0);
            texture.Texture.Render();
            RenderState.Pop();
        }

    }

}