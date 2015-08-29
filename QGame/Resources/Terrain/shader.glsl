uniform sampler2D texture00, texture01, texture11, texture10;
uniform sampler2D texture;
uniform vec4 color00, color01, color11, color10;

varying vec3 modelPos;

void main() {
	if (true) {
		vec4 c11 = texture2D(texture00, modelPos.xy) * color00;
		vec4 c10 = texture2D(texture01, modelPos.xy) * color01;
		vec4 c00 = texture2D(texture11, modelPos.xy) * color11;
		vec4 c01 = texture2D(texture10, modelPos.xy) * color10;
		float x = fract(modelPos.x);
		float y = fract(modelPos.y);
		gl_FragColor = (c00 * x + c10 * (1.0 - x)) * y
    		+ (c01 * x + c11 * (1.0 - x)) * (1.0 - y);
	} else {
		vec4 c11 = color00;
		vec4 c10 = color01;
		vec4 c00 = color11;
		vec4 c01 = color10;
		float x = fract(modelPos.x);
		float y = fract(modelPos.y);
		gl_FragColor = ((c00 * x + c10 * (1.0 - x)) * y
    		+ (c01 * x + c11 * (1.0 - x)) * (1.0 - y)) * texture2D(texture, modelPos.xy);
	}
}