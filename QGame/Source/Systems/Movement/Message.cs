using System;
using System.Collections.Generic;
using VitPro;

namespace QGame {

	partial class Messages {

		
		public class MovementUpdate : Message {

			[Serialize]
			public long id;

			[Serialize]
			public Vec3 newPosition;

			[Serialize]
			public Vec3 newVel;

			[Serialize]
			public double newRotation;

            MovementUpdate() { }
			public MovementUpdate(Entity entity) {
				id = entity.Id;
				newPosition = entity.Get<PositionComponent>().Position;
				newRotation = entity.Get<PositionComponent>().Rotation;
				newVel = entity.Get<MovementComponent>().Velocity;
			}
			public override IEnumerable<Message> HandleServer(Server server) {
				var e = server.Model.FindEntity(id);
				if (e != null) {
					e.Get<PositionComponent>().Position = newPosition;
					e.Get<PositionComponent>().Rotation = newRotation;
					e.Get<MovementComponent>().Velocity = newVel;
				} else
					yield return new GetEntity(id);
			}
			public override IEnumerable<Message> HandleClient(Client client) {
				var e = client.Model.FindEntity(id);
				if (e != null) {
					if (!e.Local) {
						var mp = e.Get<MovementPredictionComponent>();
						if (mp != null)
							mp.UpdatePosition(newPosition, newVel, newRotation);
						else {
							e.Get<PositionComponent>().Position = newPosition;
							e.Get<PositionComponent>().Rotation = newRotation;
							e.Get<MovementComponent>().Velocity = newVel;
						}
					}
				} else
					yield return new GetEntity(id);
			}
		}

	}

}
