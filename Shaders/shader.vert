#version 330 core // TODO check if can update

in vec3 aPosition;

out vec4 vertexColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    vertexColor = vec4(0.5, 0.0, 0.0, 1.0); // dark red
}