using System;
using VitPro;

namespace QGame {

	[Serializable]
	class MovementPredictionComponent : Component {

		const double LAG = 0.2;

		double prevRot = 0;
		double nextRot = 0;

		Vec3 a, b, c, d;

		internal Vec3 GetPos(double k) {
			if (k > 1)
				return a + b + c + d + Velocity * (k - 1) * LAG;
			else
				return a * Math.Pow(k, 3) + b * GMath.Sqr(k) + c * k + d;
		}

		internal Vec3 GetVel(double k) {
			if (k > 1)
				return (3 * a + 2 * b + c) / LAG;
			else
				return (3 * a * GMath.Sqr(k) + 2 * b * k + c) / LAG;
		}

		public double K { get; set; }

		public Vec3 Position {
			get { return GetPos(K); }
		}

		public Vec3 Velocity {
			get { return GetVel(K); }
		}

		public double Rotation {
			get {
				return prevRot + GMath.AngleDifference(nextRot, prevRot) * Math.Min(K, 1);
			}
		}

		public void Update(double dt) {
			K += dt / LAG;
			Entity.Get<PositionComponent>().Position = Position;
			Entity.Get<PositionComponent>().Rotation = Rotation;
			Entity.Get<MovementComponent>().Velocity = Velocity;
		}

		internal static bool Extrapolate = false;

		public void UpdatePosition(Vec3 pos, Vec3 vel, double rot) {
			if (Extrapolate)
				pos += vel * LAG;
			d = Position;
			c = Velocity * LAG;
			// a = pos - b - c - d
			// 3a + 2b + c = vel * LAG
			// 3pos - 3b - 3c - 3d + 2b + c = vel * LAG
			b = 3 * pos - 2 * c - 3 * d - vel * LAG;
			a = pos - b - c - d;
			prevRot = Rotation;
			nextRot = rot;
			K = 0;
		}
	}

}
