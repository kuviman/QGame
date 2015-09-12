using System;
using VitPro;

namespace QGame {

	[Serializable]
	class HealthComponent : Component {

		public double MaxHealth { get; set; }

        double health;
        public double Health {
            get {
                return health;
            }
            set {
                health = GMath.Clamp(value, 0, MaxHealth);
            }
        }

		public HealthComponent(double maxHealth) {
			Health = MaxHealth = maxHealth;
		}
		
	}

}
