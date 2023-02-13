#version 330 core // TODO check if can update

in vec3 vPosition;
in vec3 vColor;

out vec4 vertColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(vPosition, 1.0) * model * view * projection;
    vertColor = vec4(vColor, 1.0); // color per vertex
}