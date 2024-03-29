﻿#version 330 core
layout (location = 0) in vec3 a_pos;

out vec3 v_texCoord;

uniform mat4 u_projection;
uniform mat4 u_view;

void main()
{
    v_texCoord = a_pos;
    vec4 pos = u_projection * u_view * vec4(a_pos, 1.0);
	gl_Position = pos.xyww;
}  