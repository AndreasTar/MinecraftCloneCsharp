#version 330 core

in vec4 vertColor;

out vec4 fColor; //  the output of the shader

void main()
{
    fColor = vertColor;
}