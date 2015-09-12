using System;
using VitPro;

namespace QGame {

	[Serializable]
	class MovementComponent : Component {

		public MovementComponent(double speed, double acceleration = 20) {
			Speed = speed;
			Acceleration = acceleration;
		}

		public double Speed { get; set; }
		public Vec3 Velocity { get; set; }
		public Vec3 MoveDirection { get; set; }
		public double Acceleration { get; set; }

		public void Update(double dt) {
			Velocity += Vec3.Clamp(MoveDirection * Speed - Velocity, dt * Acceleration);
			Entity.Get<PositionComponent>().Position += Velocity * dt;
		}
	}

}
