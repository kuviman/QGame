﻿using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Net;

namespace QGame {

    partial class Messages {

        
        public class QueryTerrain : Message {

			[Serialize]
			public int fromX;

			[Serialize]
			public int fromY;

			[Serialize]
			public int toX;

			[Serialize]
			public int toY;

			QueryTerrain() { }

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

        
        public class UpdateTerrain : Message {

			[Serialize]
            public Dictionary<Vec2i, Terrain.Vertex> q = new Dictionary<Vec2i, Terrain.Vertex>();

            public Terrain.Vertex this[int i, int j] {
                get { return q[new Vec2i(i, j)]; }
                set { q[new Vec2i(i, j)] = value; }
            }
			public override IEnumerable<Message> Handle(Model model) {
                foreach (var entry in q) {
                    model.Terrain[entry.Key.X, entry.Key.Y] = entry.Value;
                }
                return null;
            }
        }

    }

}