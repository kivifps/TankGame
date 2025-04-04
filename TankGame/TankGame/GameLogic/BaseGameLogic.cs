using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame.GameLogic
{
    internal abstract class BaseGameLogic : ConsoleInput.IArrowListener
    {
        protected BaseGameState? currentState { get; private set; }
        protected float time { get; private set; }
        protected int screenWidth { get; private set; }
        protected int screenHeight { get; private set; }


        public abstract void OnArrowUp();
        public abstract void OnArrowDown();
        public abstract void OnArrowLeft();
        public abstract void OnArrowRight();
        public abstract void OnSpacebar();


        public abstract void Update(float deltaTime);

        public abstract ConsoleColor[] CreatePalette();


        public void DrawNewState(float deltaTime, ConsoleRenderer renderer)
        {
            time += deltaTime;
            screenWidth = renderer.width;
            screenHeight = renderer.height;
            
            currentState?.Update(deltaTime);
            currentState?.Draw(renderer);

            Update(deltaTime);
        }

        public void InitializeInput(ConsoleInput input)
        {
            input.Subscribe(this);
        }
        protected void ChangeState(BaseGameState? state)
        {
            currentState?.Reset();
            currentState = state;
        }

        
    }
}
