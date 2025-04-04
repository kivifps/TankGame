using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame.Utilities
{
    internal class GameData
    {
        private static GameData _instance;
        public char[,] Map =
        {
            {' ', ' ',' ', 'R', ' '},
            {' ', 'P',' ', 'R', 'R'},
            {' ', ' ',' ', ' ', ' '},
            {' ', ' ','W', ' ', ' '},
            {' ', 'W','W', ' ', ' '},
            {' ', ' ',' ', ' ', ' '},
        };
        public Dictionary<char, byte> _colors = new Dictionary<char, byte>
        {
            {'W', (byte)0},
            {'P', (byte)1},
            {'R', (byte)2},
            {'E', (byte)5},
        };
        public char[] _wallSymbolSet = new char[] { '╬', '▓' };
        public char[] _waterSymbolSet = new char[] { '░' };
        public static GameData GetInstance()
        {
            if( _instance == null )
                _instance = new GameData();
            return _instance;
        }
    }
}
