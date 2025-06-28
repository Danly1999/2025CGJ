shader_type canvas_item;
uniform float _Outline = 1.;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 tex = texture(TEXTURE,UV).rgba;
	COLOR = vec4(mix(vec3(0.),tex.rgb,tex.a),texture(TEXTURE,(UV-0.5)*_Outline+0.5).a+tex.a);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
