using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame.Utilities
{
    internal class WormMap
    {
        public enum Direction
        {
            UP, RIGHT, DOWN, LEFT
        }
        private Random _random = new Random();
        private char[,] _currentMap;
        private Dictionary<Cell, Cell> _wayMap = new Dictionary<Cell, Cell>();
        private char[] symbols = { ' ', '▓', '░' };
        private Cell[] CrossDirection =
        {
            new Cell(1, 0),
            new Cell(0, 1),
            new Cell(-1, 0),
            new Cell(0, -1)
        };
        private int Width;
        private int Height;

        public char[,] Map => _currentMap;
        public WormMap(int w, int h)
        {
            _currentMap = GetSimpleMap(w, h, 'W');
            Width = w;
            Height = h;
        }
        public void GenerateMap()
        {
            Cell worm = new Cell(_random.Next(1, _currentMap.GetLength(0) - 1), _random.Next(1, _currentMap.GetLength(1) - 1));
            CheckWay(worm);
            while (_wayMap.Count > 0)
            {
                var twoStepPoint = _wayMap.Keys.ElementAt(_random.Next(_wayMap.Count));
                var point = twoStepPoint.Sub(_wayMap[twoStepPoint]);
                _currentMap[point.X, point.Y] = ' ';
                _currentMap[twoStepPoint.X, twoStepPoint.Y] = ' ';
                worm = twoStepPoint;
                CheckWay(worm);
                _wayMap.Remove(twoStepPoint);
            }
        }
        public void AddWater()
        {
            var length = _random.Next(_currentMap.Length / 10, (_currentMap.Length / 10) * 2);
            Cell fish = new Cell(_random.Next(1, _currentMap.GetLength(0) - 1), _random.Next(1, _currentMap.GetLength(1) - 1));
            while (length > 0)
            {
                _currentMap[fish.X, fish.Y] = 'R';
                foreach (var dir in CrossDirection)
                {
                    Cell lookPos = fish.Sum(dir);
                    if (lookPos.InRange(Width - 1, Height - 1))
                    {
                        _wayMap.Add(lookPos, dir);
                    }
                }
                if (_wayMap.Count > 0)
                    fish = _wayMap.Keys.ElementAt(_random.Next(_wayMap.Count));
                _wayMap.Clear();
                length--;
            }

        }
        public void AddUnit(int count, char symbole)
        {
            while (count > 0)
            {
                var isFindingPlace = true;
                while (isFindingPlace)
                {
                    Cell player = new Cell(_random.Next(1, _currentMap.GetLength(0) - 1), _random.Next(1, _currentMap.GetLength(1) - 1));
                    if (_currentMap[player.X, player.Y] == ' ')
                    {
                        isFindingPlace = false;
                        _currentMap[player.X, player.Y] = symbole;
                    }
                }
                count--;
            }
        }
        public void CheckWay(Cell pos)
        {
            foreach (var dir in CrossDirection)
            {
                Cell lookDir = pos.Sum(dir);
                Cell twoStepDir = lookDir.Sum(dir);
                if (_wayMap.ContainsKey(twoStepDir)) continue;
                if (twoStepDir.InRange(Width - 1, Height - 1) && _currentMap[twoStepDir.X, twoStepDir.Y] == 'W')
                {
                    _wayMap.Add(twoStepDir, dir);
                }
            }
        }
        public char[,] GetSimpleMap(int sizeW, int sizeH, char symbole)
        {
            char[,] map = new char[sizeW, sizeH];
            for (int w = 0; w < sizeW; w++)
            {
                for (int h = 0; h < sizeH; h++)
                {
                    map[w, h] = symbole;
                }
            }
            return map;
        }
    }
}
