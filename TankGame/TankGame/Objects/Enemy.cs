using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankGame.Utilities;

namespace TankGame.Objects
{
    internal class Enemy : TankUnit
    {
        public Enemy(Cell position, byte colorIndex) : base(position, colorIndex)
        {
            IsBulletVisible = true;
            HP = 3;
        }
        public void SetRotation(Cell dir)
        {
            _lookDir = (CrossDiration)Array.IndexOf(_moveDiraction, dir);
        }
    }
}
