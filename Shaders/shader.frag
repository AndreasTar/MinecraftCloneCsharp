#version 330 core

in vec4 vertexColor;

out vec4 FragColor; //  the output of the shader

uniform vec4 ourColor;

void main()
{
    FragColor = vertexColor;
}