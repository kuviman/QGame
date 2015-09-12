using System;
using VitPro;

namespace QGame {

	[Serializable]
	class PositionComponent : Component {

		public PositionComponent(Vec3 position, double rotation) {
			Position = position;
			Rotation = rotation;
		}

		public Vec3 Position { get; set; }
		public double Rotation { get; set; }

	}

}
