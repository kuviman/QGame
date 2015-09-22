using System;
using VitPro;

namespace QGame {

	
	class MovementComponent : Component {

		MovementComponent() {}

		public MovementComponent(double speed, double acceleration = 20) {
			Speed = speed;
			Acceleration = acceleration;
		}

		[Serialize]
		public double Speed { get; set; }

		[Serialize]
		public Vec3 Velocity { get; set; }

		[Serialize]
		public Vec3 MoveDirection { get; set; }

		[Serialize]
		public double Acceleration { get; set; }

		public void Update(double dt) {
			Velocity += Vec3.Clamp(MoveDirection * Speed - Velocity, dt * Acceleration);
			Entity.Get<PositionComponent>().Position += Velocity * dt;
		}
	}

}
