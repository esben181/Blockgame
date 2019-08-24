﻿#version 330 core
out vec4 f_color;

in vec3 v_texCoord;

uniform samplerCube u_skybox;

void main()
{    
    f_color = texture(u_skybox, v_texCoord);
}