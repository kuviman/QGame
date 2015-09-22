using System;
using VitPro;

namespace QGame {

	
	class PositionComponent : Component {

		PositionComponent() {}

		public PositionComponent(Vec3 position, double rotation) {
			Position = position;
			Rotation = rotation;
		}

		[Serialize]
		public Vec3 Position { get; set; }

		[Serialize]
		public double Rotation { get; set; }

	}

}
