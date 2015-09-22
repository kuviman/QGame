using System;
using VitPro;

namespace QGame {

	
	class HealthComponent : Component {

		[Serialize]
		public double MaxHealth { get; set; }

        double health;

		[Serialize]
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
