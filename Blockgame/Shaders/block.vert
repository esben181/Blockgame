#version 440 core

layout (location = 0) in vec3 a_pos;
layout (location = 1) in vec3 a_normal;
layout (location = 2) in float a_textureLayer;

uniform mat4 u_model;
uniform mat4 u_view;
uniform mat4 u_projection;

out vec3 v_normal;
out vec3 v_fragPos;
out float v_textureLayer;

void main()
{
	v_fragPos = vec3(u_model * vec4(a_pos, 1.0));
	v_normal = a_normal;
	v_textureLayer = a_textureLayer;
    gl_Position = u_projection * u_view * u_model * vec4(a_pos, 1.0);
}