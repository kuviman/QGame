using System;
using System.Collections.Generic;
using VitPro;
using VitPro.Engine;

namespace QGame {

    [Serializable]
    class ResourcedTexture {
        static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public string Name { get; private set; }
        public Texture Texture {
            get {
                if (!textures.ContainsKey(Name)) {
                    var tex = new Texture(Resource.Stream(Name));
                    tex.Wrap = VitPro.Engine.Texture.WrapMode.Repeat;
                    textures[Name] = tex;
                }
                return textures[Name];
            }
        }

        public ResourcedTexture(string name) {
            this.Name = name;
        }

        public static implicit operator Texture(ResourcedTexture tex) {
            return tex.Texture;
        }

    }

}