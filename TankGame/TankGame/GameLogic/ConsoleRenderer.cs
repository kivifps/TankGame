using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankGame.Utilities;

namespace TankGame
{
    internal class ConsoleRenderer
    {
        public int width { get; private set; }
        public int height { get; private set; }

        private const int MaxColors = 8;
        private readonly ConsoleColor[] _colors;
        private readonly char[,] _pixels;
        private readonly char[,] _previousPixels;
        private readonly byte[,] _pixelColors;
        private readonly int _maxWidth;
        private readonly int _maxHeight;
        public ConsoleColor bgColor { get; set; }

        public char this[int w, int h]
        {
            get { return _pixels[w, h]; }
            set { _pixels[w, h] = value; }
        }

        public ConsoleRenderer(ConsoleColor[] colors)
        {
            if (colors.Length > MaxColors)
            {
                var tmp = new ConsoleColor[MaxColors];
                Array.Copy(colors, tmp, tmp.Length);
                colors = tmp;
            }

            _colors = colors;

            _maxWidth = Console.LargestWindowWidth;
            _maxHeight = Console.LargestWindowHeight;
            width = Console.WindowWidth;
            height = Console.WindowHeight;


            _pixels = new char[_maxWidth, _maxHeight];
            _previousPixels = new char[_maxWidth, _maxHeight];
            _pixelColors = new byte[_maxWidth, _maxHeight];
        }

        public void SetFrame(int w, int h, char symbol, byte colorIdx)
        {
            for (int i = 0; i < w; i++)
            {
                SetPixel(i, 0, symbol, colorIdx);
                SetPixel(i, h - 1, symbol, colorIdx);
            }

            for (int i = 0; i < h; i++)
            {
                SetPixel(0, i, symbol, colorIdx);
                SetPixel(w - 1, i, symbol, colorIdx);
            }
        }
        public void SetPixels(int w, int h, char[,] val, byte colorIdx)
        {
            for (int y = 0; y < val.GetLength(0); y++)
            {
                for (int x = 0; x < val.GetLength(1); x++)
                {
                    if (val[y, x] != 'A')
                    {

                        _pixels[x + (h * 4), y + (w * 2)] = val[y, x];
                        _pixelColors[x + (h * 4), y + (w * 2)] = colorIdx;
                    }
                }
            }
        }
        public void SetPixel(int w, int h, char val, byte colorIdx)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 4; x++)
                {

                    _pixels[x + (h * 4), y + (w * 2)] = val;
                    _pixelColors[x + (h * 4), y + (w * 2)] = colorIdx;

                }
            }
        }
        public void ClearPixels(int w, int h)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    _pixels[x + (h * 4), y + (w * 2)] = ' ';
                }
            }
        }
        public void Render()
        {
            Console.BackgroundColor = bgColor;
            for (var w = 0; w < width; w++)
                for (var h = 0; h < height; h++)
                {
                    if (_previousPixels[w, h] == _pixels[w, h])
                        continue;
                    var colorIdx = _pixelColors[w, h];
                    var color = _colors[colorIdx];
                    var symbol = _pixels[w, h];

                    if (symbol == 0 || color == bgColor)
                        continue;

                    Console.ForegroundColor = color;

                    Console.SetCursorPosition(w, h);
                    Console.Write(symbol);

                    _previousPixels[w, h] = _pixels[w, h];
                }

            Console.ResetColor();
            Console.CursorVisible = false;
        }

        public void DrawString(string text, int atWidth, int atHeight, ConsoleColor color)
        {
            var colorIdx = Array.IndexOf(_colors, color);
            if (colorIdx < 0)
                return;

            for (int i = 0; i < text.Length; i++)
            {
                _pixels[atWidth + i, atHeight] = text[i];
                _pixelColors[atWidth + i, atHeight] = (byte)colorIdx;
            }
        }

        public void Clear()
        {
            for (int w = 0; w < width; w++)
                for (int h = 0; h < height; h++)
                {
                    _pixelColors[w, h] = 0;
                    _pixels[w, h] = (char)0;
                    _previousPixels[w, h] = (char)0;
                }
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ConsoleRenderer casted)
                return false;

            if (_maxWidth != casted._maxWidth || _maxHeight != casted._maxHeight ||
                width != casted.width || height != casted.height ||
                _colors.Length != casted._colors.Length)
            {
                return false;
            }


            for (int i = 0; i < _colors.Length; i++)
            {
                if (_colors[i] != casted._colors[i])
                    return false;
            }

            for (int w = 0; w < width; w++)
                for (var h = 0; h < height; h++)
                {
                    if (_pixels[w, h] != casted._pixels[w, h] ||
                                    _pixelColors[w, h] != casted._pixelColors[w, h])
                    {
                        return false;
                    }
                }

            return true;
        }

        public override int GetHashCode()
        {
            var hash = HashCode.Combine(_maxWidth, _maxHeight, width, height);

            for (int i = 0; i < _colors.Length; i++)
            {
                hash = HashCode.Combine(hash, _colors[i]);
            }

            for (int w = 0; w < width; w++)
                for (var h = 0; h < height; h++)
                {
                    hash = HashCode.Combine(hash, _pixelColors[w, h], _pixels[w, h]);
                }

            return hash;
        }
    }
}
