using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankGame.Objects;
using TankGame.Utilities;

namespace TankGame.GameLogic
{
    internal class Factory
    {
        private static readonly Dictionary<char, Func<int, int, Entity>> _factoryMenu = new()
        {
            {'W', (w, h) => new Block(new Cell(w, h), true, GameData.GetInstance()._wallSymbolSet, GameData.GetInstance()._colors['W']) },
            {'R', (w, h) => new Block(new Cell(w, h), false, GameData.GetInstance()._waterSymbolSet, GameData.GetInstance()._colors['R']) },
            {'P', (w, h) => new Player(new Cell(w, h),GameData.GetInstance()._colors['P']) },
            {'E', (w, h) => new Enemy(new Cell(w, h),GameData.GetInstance()._colors['E']) }
        };

        public static Entity? Create(char symbol, int w, int h)
        {
            return _factoryMenu.TryGetValue(symbol, out var createObject) ? createObject(w, h) : null;
        }
    }
}
