using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankGame.Utilities;

namespace TankGame.Objects
{
    internal class Bullet : Entity
    {
        private char[,] _texture = new char[,]
        {
            {'A','A','☻','A'},
            {'A','A','A','A'}
        };
        private Cell _direction;
        public override char[,] Texture => _texture;
        public Bullet(Cell position, Cell direction, byte colorIndex) : base(position, colorIndex)
        {
            _direction = direction;
        }
        public Cell Move()
        {
            return Position.Sum(_direction);
        } 
        public override void GetDamage()
        {
        }
    }
}
