using System;
using VitPro;

namespace QGame {

	
	class PhysicsComponent : Component {
		
		public PhysicsComponent(double size) {
			Size = size;
		}

		[Serialize]
		public double Size { get; set; }

	}

}
