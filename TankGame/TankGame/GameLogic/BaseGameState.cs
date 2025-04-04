using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame.GameLogic
{
    internal abstract class BaseGameState
    {
        public abstract void Update(float deltaTime);
        public abstract void Reset();
        public abstract void Draw(ConsoleRenderer renderer);
        public abstract bool IsDone();
    }
}
