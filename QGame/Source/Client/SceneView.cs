using System;
using VitPro;
using VitPro.Engine;

namespace QGame {

	partial class ClientView {

		class SceneView : State {

			ClientView clientView;
			Client client;
			Model model;
			Terrain terrain;

			Entity Player { get { return client.Player; } }

			Camera camera;

			public SceneView(ClientView clientView) {
				this.clientView = clientView;
				this.client = clientView.client;
				this.model = clientView.model;
				terrain = model.Terrain;
				camera = client.Camera;
			}

			public override void Update(double dt) {
				base.Update(dt);
				Vec2 v = Vec2.Zero;
				if (Key.W.Pressed())
					v.Y += 1;
				if (Key.S.Pressed())
					v.Y -= 1;
				if (Key.A.Pressed())
					v.X -= 1;
				if (Key.D.Pressed())
					v.X += 1;
				v = Vec2.Rotate(v, camera.Rotation);
				if (Player != null) {
					Player.Get<MovementComponent>().MoveDirection = new Vec3(v.X, v.Y, 0);
					var weapon = Player.Get<WeaponComponent>().Weapon;
					if (weapon != null)
						weapon.Firing = MouseButton.Left.Pressed();
					camera.Position += (Player.Get<PositionComponent>().Position - camera.Position) * Math.Min(dt * 10, 1);
				}
			}

			public override void MouseDown(MouseButton button, Vec2 position) {
				base.MouseDown(button, position);
			}

			public override void MouseMove(Vec2 position) {
				base.MouseMove(position);
				if (client.Player != null)
					client.Player.Get<PositionComponent>().Rotation = (Mouse.Position - new Vec2(App.Width, App.Height) / 2).Arg + camera.Rotation;
			}
			int dist = 14;
			int distY = 10;

			public override void KeyDown(Key key) {
				base.KeyDown(key);
				if (key == Key.KeypadPlus)
					++dist;
				if (key == Key.KeypadMinus)
					--dist;
                if (key == Key.H) {
                    if (Player != null) {
                        client.Send(new Messages.ChangeHealth(Player, 1000));
                    }
                }
				if (key == Key.I) {
					MovementPredictionComponent.Extrapolate = !MovementPredictionComponent.Extrapolate;
					App.Title = MovementPredictionComponent.Extrapolate.ToString();
				}
                if (key == Key.R) {
                    client.Send(new Messages.Login("kuviman"));
                }
			}

			public override void Render() {
				base.Render();
				Draw.Clear(Color.Sky);
				RenderState.Push();
				camera.Apply();
				int x = (int)camera.Position.X;
				int y = (int)camera.Position.Y;
				terrain.Render(x - dist, y - distY, x + dist, y + distY);
				RenderState.DepthTest = false;
				model.Render();
				RenderState.Pop();
			}

		}

	}

}
