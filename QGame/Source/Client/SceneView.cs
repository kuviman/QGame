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


			Vec2i startDragPos;

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
				double up = 0;
				if (Key.Space.Pressed())
					up += 1;
				if (Key.ShiftLeft.Pressed())
					up -= 1;
				v = Vec2.Rotate(v, camera.Rotation);
				if (Key.ControlLeft.Pressed())
					v *= 10;
				if (Player != null) {
					Player.Get<MovementComponent>().MoveDirection = new Vec3(v.X, v.Y, 0);
					camera.Distance += up * 3 * dt;
					camera.Position += (Player.Get<PositionComponent>().Position - camera.Position) * Math.Min(dt * 10, 1);
				}
			}

			public override void MouseDown(MouseButton button, Vec2 position) {
				base.MouseDown(button, position);
				if (button == MouseButton.Left)
					startDragPos = Mouse.Position;
			}

			public override void MouseMove(Vec2 position) {
				base.MouseMove(position);
				if (MouseButton.Left.Pressed()) {
					double sens = 1.0 / 100;
					Vec2i dv = Mouse.Position - startDragPos;
					camera.Rotation -= dv.X * sens;
					camera.UpAngle = GMath.Clamp(camera.UpAngle + dv.Y * sens, -Math.PI / 2, -Math.PI / 4);
					//					Mouse.Position = startDragPos;
				}
				startDragPos = Mouse.Position;

				if (client.Player != null)
					client.Player.Get<PositionComponent>().Rotation = (Mouse.Position - new Vec2(App.Width, App.Height) / 2).Arg + camera.Rotation;
			}
			int dist = 14;
			int distY = 10;

			public override void KeyDown(Key key) {
				base.KeyDown(key);
				if (key == Key.KeypadPlus)
					++dist;
				else if (key == Key.KeypadMinus)
					--dist;
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
