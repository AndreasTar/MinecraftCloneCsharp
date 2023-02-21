/* 
decided to use OpenTK cause it has handles for graphics, audio and input all together.
This is a project to learn, so i could bother trying to find libraries and stuff
for audio and window opening etc.
Another candidate was Vulkan with Vortice.Vulkan : https://github.com/amerkoleci/Vortice.Vulkan
but that doesnt offer any audio etc as i said, and i also couldnt find any docs etc
so i discarded that idea.

very useful : https://opentk.net/learn/index.html
aka how i learned to use this whole thing
also some slight help from : http://neokabuto.blogspot.com/p/tutorials.html
for some small stuff that the official tutorial doesnt cover
also : https://github.com/jdah/minecraft-again
good for getting an idea for the general structure of the source code
*/

// small reminder for comment highlights :
// * important
// ! deprecated
// ? question
// TODO uhm... todo
//// this is strikethrough

using GameMain;

public class main{
    
    public static void Main(){

        using(GameMain.Game game = new Game(1600, 900, "First Window", 90, 500)){
            // double FPS , int TPS
            // if either is zero, renders|updates at hardware limit respectively
            game.Run();
            
        }
    }
}