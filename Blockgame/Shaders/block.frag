#version 440 core
 
out vec4 f_color;

uniform sampler2DArray u_texture;
uniform vec3 u_lightPos;


in vec3 v_normal;
in vec3 v_fragPos;
in vec2 v_texCoord;
in float v_textureId;


void main()
{
	vec3 lightColor = vec3(1.0);
	vec3 objectColor = vec3(0.75);

	// ambient
	float ambientStrength = 0.75;
	vec3 ambient = ambientStrength * lightColor;

	// diffuse
	vec3 norm = normalize(v_normal);
	vec3 lightDir = normalize(u_lightPos - v_fragPos);
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;

	vec3 result = (ambient + diffuse) * objectColor;
	f_color  = texture(u_texture, vec3(v_texCoord, v_textureId))* vec4(result, 1.0);
}
