﻿using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Net;

namespace QGame {

    partial class Messages {

        [Serializable]
        public class QueryTerrain : Message {
            int fromX, fromY, toX, toY;
            public QueryTerrain(int fromX, int fromY, int toX, int toY) {
                this.fromX = fromX;
                this.fromY = fromY;
                this.toX = toX;
                this.toY = toY;
            }
			public override IEnumerable<Message> Handle(Model model) {
                UpdateTerrain reply = new UpdateTerrain();
                for (int i = fromX; i <= toX; i++)
                    for (int j = fromY; j <= toY; j++) {
                        reply[i, j] = model.Terrain[i, j];
                    }
                yield return reply;
            }
        }

        [Serializable]
        public class UpdateTerrain : Message {
//            Dictionary<Vec2i, Terrain.Vertex> q = new Dictionary<Vec2i, Terrain.Vertex>();
			List<Vec2i> pos = new List<Vec2i>();
			List<Terrain.Vertex> vertex = new List<Terrain.Vertex>();
            public Terrain.Vertex this[int i, int j] {
//                get { return q[new Vec2i(i, j)]; }
//                set { q[new Vec2i(i, j)] = value; }
				set { pos.Add(new Vec2i(i, j)); vertex.Add(value); }
            }
			public override IEnumerable<Message> Handle(Model model) {
//                foreach (var entry in q) {
//                    model.Terrain[entry.Key.X, entry.Key.Y] = entry.Value;
//                }
				for (int i = 0; i < pos.Count; i++)
					model.Terrain[pos[i].X, pos[i].Y] = vertex[i];
                return null;
            }
        }

    }

}