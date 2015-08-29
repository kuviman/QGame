using System;
using VitPro;
using VitPro.Engine;

namespace QGame {

	static class Misc {

		static Misc() {
			UnknownTexture = new Texture(Resource.Stream("Misc/unknown_texture.png"));
			UnknownTexture.Wrap = Texture.WrapMode.Repeat;
		}
		
		public static Texture UnknownTexture { get; private set; }

	}

}
