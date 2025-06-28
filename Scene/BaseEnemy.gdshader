shader_type canvas_item;
uniform float Outline = 1.;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 tex = texture(TEXTURE,UV).rgba;
	COLOR = vec4(mix(vec3(1.),tex.rgb,tex.a),texture(TEXTURE,(UV-0.5)*Outline+0.5).a+tex.a*2.);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
