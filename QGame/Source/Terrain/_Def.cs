using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {

	partial class Terrain {

        const int CHUNK_SIZE = 16;

        [Serializable]
		public struct Vertex {
            public ResourcedTexture Texture { get; set; }
			public double Height { get; set; }
            public double WaterHeight { get; set; }
		}

		Dictionary<Vec2i, Vertex[,]> map = new Dictionary<Vec2i,Vertex[,]>();

        Vertex[,] GetChunk(int x, int y) {
            Vec2i v = new Vec2i(x, y);
            if (!map.ContainsKey(v)) {
                var chunk = new Vertex[CHUNK_SIZE, CHUNK_SIZE];
                for (int i = 0; i < CHUNK_SIZE; i++)
                    for (int j = 0; j < CHUNK_SIZE; j++)
                        chunk[i, j] = new Vertex();
                map[v] = chunk;
            }
            return map[v];
        }

		public Vertex this[int i, int j] {
			get {
                int cx = GMath.DivDown(i, CHUNK_SIZE);
                int cy = GMath.DivDown(j, CHUNK_SIZE);
                return GetChunk(cx, cy)[i - cx * CHUNK_SIZE, j - cy * CHUNK_SIZE];
            }
            set {
                int cx = GMath.DivDown(i, CHUNK_SIZE);
                int cy = GMath.DivDown(j, CHUNK_SIZE);
                GetChunk(cx, cy)[i - cx * CHUNK_SIZE, j - cy * CHUNK_SIZE] = value;
            }
		}

        public double GetHeight(double x, double y) {
            int i = GMath.Floor(x);
            int j = GMath.Floor(y);
            x -= i;
            y -= j;
            Vec3 v1, v2;
            if (x > y) {
                v1 = GetVec(i + 1, j) - GetVec(i, j);
                v2 = GetVec(i + 1, j + 1) - GetVec(i + 1, j);
            } else {
                v1 = GetVec(i + 1, j + 1) - GetVec(i, j + 1);
                v2 = GetVec(i, j + 1) - GetVec(i, j);
            }
            return GetVec(i, j).Z + v1.Z * x + v2.Z * y;
        }

        public void Render(int fromX, int fromY, int toX, int toY) {
            RenderState.Push();
            shader.Prepare();
            RenderState.Push();
            for (int i = fromX; i < toX; i++)
                for (int j = fromY; j < toY; j++) {
                    RenderState.Set("texture00", GetTexture(i, j).Texture);
                    RenderState.Set("texture10", GetTexture(i + 1, j).Texture);
                    RenderState.Set("texture11", GetTexture(i + 1, j + 1).Texture);
                    RenderState.Set("texture01", GetTexture(i, j + 1).Texture);
                    RenderState.Set("color00", GetColor(i, j));
                    RenderState.Set("color10", GetColor(i + 1, j));
                    RenderState.Set("color11", GetColor(i + 1, j + 1));
                    RenderState.Set("color01", GetColor(i, j + 1));
                    shader.RenderPolygon(
                        GetVec(i, j),
                        GetVec(i + 1, j),
                        GetVec(i + 1, j + 1),
                        GetVec(i, j + 1));
                }
            RenderState.Pop();
            RenderState.Pop();
            RenderWater(fromX, fromY, toX, toY);
            Draw.ClearDepth();
        }

        void RenderWater(int fromX, int fromY, int toX, int toY) {
            RenderState.Push();
            RenderState.Set("color", new Color(1, 1, 1, 0.5));
            RenderState.Set("texture", waterTexture);
            Shader.Std.Texture.Prepare();
            for (int i = fromX; i < toX; i++)
                for (int j = fromY; j < toY; j++) {
                    Shader.Std.Texture.RenderPolygon(
                        new Vec3(i, j, this[i, j].WaterHeight),
                        new Vec3(i + 1, j, this[i + 1, j].WaterHeight),
                        new Vec3(i + 1, j + 1, this[i + 1, j + 1].WaterHeight),
                        new Vec3(i, j + 1, this[i, j + 1].WaterHeight));
                }
            RenderState.Pop();
        }

        static ResourcedTexture waterTexture = new ResourcedTexture("Terrain/water.png");
        static Shader shader = new Shader(Resource.String("Terrain/shader.glsl"));

		Color GetColor(int i, int j) {
			return Color.White;
		}

		Vec3 GetVec(int i, int j) {
			return new Vec3(i, j, this[i, j].Height);
		}

		ResourcedTexture GetTexture(int i, int j) {
			var texture = this[i, j].Texture;
			if (texture == null)
				return Textures.Unknown;
			return texture;
		}

		public Vec2i? ClosestUnloaded(int i, int j, int maxDist) {
			int dist = maxDist;
			Vec2i? res = null;
			for (int x = i - maxDist; x <= i + maxDist; x++)
				for (int y = j - maxDist; y <= j + maxDist; y++) {
					int curDist = Math.Max(Math.Abs(x - i), Math.Abs(y - j));
					if (this[x, y].Texture == null && curDist < dist) {
						dist = curDist;
						res = new Vec2i(x, y);
					}
				}
			return res;
		}

	}

}
