using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankGame.Utilities;

namespace TankGame.Objects
{
    internal class TankUnit : Entity
    {
        public enum CrossDiration
        {
            Up, Right, Down, Left
        }
        protected const int ColdownTime = 3;
        public int _coldown = 0;
        protected Dictionary<CrossDiration, char[,]> TextureSet = new Dictionary<CrossDiration, char[,]>
        {
            {
                CrossDiration.Up,
                new char[,]
                {
                     {' ','╔','╩','╗'},
                     {' ','╚','═','╝'}
                }
            },
            {
                CrossDiration.Right,
                new char[,]
                {
                     {'╔','═','╗','_'},
                     {'╚','═','╝','T'}
                }
            },
            {
                CrossDiration.Down,
                new char[,]
                {
                     {' ','╔','═','╗'},
                     {' ','╚','╦','╝'}
                }
            },
            {
                CrossDiration.Left,
                new char[,]
                {
                     {'_','╔','═','╗'},
                     {'T','╚','═','╝'}
                }
            }
        };
        public Cell[] _moveDiraction = new Cell[]
        {
            new Cell(-1, 0),
            new Cell(0, 1),
            new Cell(1 , 0),
            new Cell(0 , -1),
        };
        protected CrossDiration _lookDir = CrossDiration.Up;
        public override char[,] Texture
        {
            get
            {
                return TextureSet[_lookDir];
            }
        }

        public TankUnit(Cell position, byte colorIndex) : base(position, colorIndex)
        {
        }
        public Cell Move(bool isFrontMove)
        {
            if (isFrontMove)
            {
                return Position.Sum(_moveDiraction[(int)_lookDir]);
            }
            else
                return Position.Sum(_moveDiraction[((int)_lookDir + 2) % 4]);
        }
        public void Rotate(bool isRightRotate)
        {
            if (isRightRotate)
            {
                _lookDir = (CrossDiration)(((int)_lookDir + 1) % 4);
            }
            else
                _lookDir = (CrossDiration)(((int)_lookDir - 1 + 4) % 4);
        }
        public Bullet Shoot()
        {
            if (_coldown > ColdownTime)
            {
                _coldown = 0;
                return new Bullet(Position, _moveDiraction[(int)_lookDir], 3);
            }
            return null;
        }
        public override void GetDamage()
        {
            HP--;
        }
    }
}
