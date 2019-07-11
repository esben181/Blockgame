#version 440 core

layout (location = 0) in vec3 a_pos;
layout (location = 1) in vec3 a_normal;
layout (location = 2) in vec2 a_texCoord;
layout (location = 3) in float a_textureId;

uniform mat4 u_model;
uniform mat4 u_view;
uniform mat4 u_projection;

out vec3 v_normal;
out vec3 v_fragPos;
out vec2 v_texCoord;
out float v_textureId;

void main()
{
	v_fragPos = vec3(u_model * vec4(a_pos, 1.0));
	v_normal = a_normal;
	v_texCoord = a_texCoord;
	v_textureId = a_textureId;
    gl_Position = u_projection * u_view * u_model * vec4(a_pos, 1.0);
}