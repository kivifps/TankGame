using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using TankGame.Utilities;

namespace TankGame.Objects
{
    internal class Player : TankUnit
    {
        public Player(Cell position, byte colorIndex) : base(position, colorIndex)
        {
            IsBulletVisible = true;
            HP = 3;
            Speed = 2;
        }

    }
}
