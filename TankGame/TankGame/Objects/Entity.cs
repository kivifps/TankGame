using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankGame.Utilities;

namespace TankGame.Objects
{
    internal abstract class Entity
    {
        public int HP {  get; private protected set; }
        public int Speed {  get; private protected set; } 
        public Cell Position {  get; private protected set; }
        public bool IsBulletVisible {  get; private protected set; }
        public virtual char[,] Texture { get; private protected set; }
        public byte ColorIndex { get; private set; }
        

        public Entity(Cell position, byte colorIndex)
        {
            Position = position;
            ColorIndex = colorIndex;
        }
        public abstract void GetDamage();
        public void ChangePosition(Cell pos)
        {
            Position = pos;
        }
    }
}
