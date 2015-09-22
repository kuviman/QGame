using System;
using VitPro;
using VitPro.Engine;

namespace QGame {

    
    abstract class RenderComponent : Component {
        public virtual void Update(double dt) { }
        public abstract void Render();
    }

}