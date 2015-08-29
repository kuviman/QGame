using System;
using VitPro;

namespace QGame {

	[Serializable]
	class PhysicsComponent : Component {
		
		public PhysicsComponent(double size) {
			Size = size;
		}

		public double Size { get; set; }

	}

}
