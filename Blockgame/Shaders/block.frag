#version 440 core
 
out vec4 f_color;

uniform sampler2DArray u_texture;
uniform vec3 u_lightPos;
uniform vec3 u_viewPos;


in vec3 v_fragPos;
in vec3 v_normal;
in float v_textureLayer;
in vec3 v_color;

vec2 GetTextureCoord(vec3 normal);

void main()
{
	
	vec3 lightColor = vec3(1.0);
	vec3 objectColor = vec3(v_color);

	// ambient
	float ambientStrength = 0.75;
	vec3 ambient = ambientStrength * lightColor;

	// diffuse
	vec3 norm = normalize(v_normal);
	vec3 lightDir = normalize(u_lightPos - v_fragPos);
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;

	// specular
	vec3 viewDir = normalize(u_viewPos - v_fragPos);
	vec3 reflectDir = reflect(-lightDir, v_normal);
	vec3 halfwayDir = normalize(lightDir + viewDir);
	float spec = pow(max(dot(v_normal, halfwayDir), 0.0), 32.0);
	vec3 specular = vec3(0.3) * spec;

	vec3 result = (ambient + diffuse + specular) * objectColor;
	f_color  = texture(u_texture, vec3(GetTextureCoord(v_normal), v_textureLayer)) * vec4(result, 1.0);
	//f_color = vec4(v_color, 1.0) * vec4(result, 1.0);
}

vec2 GetTextureCoord(vec3 normal)
{
	if (abs(normal.y) == 1)
	{
		return -v_fragPos.xz;
	}
	else if(abs(normal.x) == 1)
	{
		return -v_fragPos.zy;
	}
	else if (normal.z == 1)
	{
		return -v_fragPos.xy;
	}
	else
	{
		return -v_fragPos.xy;
	}
}
