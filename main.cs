/* 
decided to use OpenTK cause it has handles for graphics, audio and input all together.
This is a project to learn, so i could bother trying to find libraries and stuff
for audio and window opening etc.
Another candidate was Vulkan with Vortice.Vulkan : https://github.com/amerkoleci/Vortice.Vulkan
but that doesnt offer any audio etc as i said, and i also couldnt find any docs etc
so i discarded that idea.

very useful : https://opentk.net/learn/index.html
aka how i learned to use this whole thing
*/

// small reminder for comment highlights :
// * important
// ! deprecated
// ? question
// TODO uhm... todo
//// this is strikethrough

using WindowMain;

public class main{
    


    public static void Main(){

        using(WindowMain.WindowCreator window = new WindowCreator(1600, 900, "First Window")){
            window.Run(1); // double : how many fps. if null, updates at hardware limit
        }


    }
}