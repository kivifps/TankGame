using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankGame.Utilities;

namespace TankGame.Objects
{
    internal class Block : Entity
    {
        private char[] _symbolSet;

        public Block(Cell position, bool isBulletVisible, char[] symbolSet, byte colorIndex) : base(position, colorIndex)
        {
            IsBulletVisible = isBulletVisible;
            _symbolSet = symbolSet;
            HP = symbolSet.Length;
            SetSimpleTexture(HP - 1);
        }

        public override void GetDamage()
        {
            if(--HP > 0)
            {
                SetSimpleTexture(HP - 1);
            }
        }

        private void SetSimpleTexture(int symbolSetID)
        {
            char[,] texture = new char[2, 4];
            for(int w = 0; w < 2; w++)
            {
                for(int h = 0; h < 4; h++)
                {
                    texture[w, h] = _symbolSet[symbolSetID];
                }
            }
            Texture = texture; 
        }
    }
}
