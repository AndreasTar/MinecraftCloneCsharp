#version 330 core

in vec4 vertColor;

out vec4 fColor; //  the output of the shader

uniform vec4 ourFragColor;

void main()
{
    fColor = vertColor;
}