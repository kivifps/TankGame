
using TankGame.GameLogic;
using TankGame.Utilities;

namespace TankGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TankGameLogic gameLogic = new TankGameLogic();
            var palette = gameLogic.CreatePalette();
            ConsoleRenderer currRenderer = new ConsoleRenderer(palette);
            ConsoleInput input = new ConsoleInput();
            gameLogic.InitializeInput(input);
            var lastFrameTime = DateTime.Now;
            while (true) 
            {
                var frameStartTime = DateTime.Now;
                float deltaTime = (float)(frameStartTime - lastFrameTime).TotalSeconds;
                input.Update();

                gameLogic.DrawNewState(deltaTime, currRenderer);
                lastFrameTime = frameStartTime;
                currRenderer.Render();


            } 



        }
    }
}
