using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TankGame.Objects;
using TankGame.Utilities;
using static TankGame.Objects.TankUnit;

namespace TankGame.GameLogic
{
    public enum ControlButton
    {
        Up, Down, Left, Right, Attack, None
    }
    internal class TankGamePlayState : BaseGameState
    {
        private int _frameCounter = 0;
        private int _mapSize = 10;

        private Dictionary<Cell, Entity> _allEntity = new Dictionary<Cell, Entity>();
        private HashSet<Cell> _clearQueue = new HashSet<Cell>();

        private Player _player;

        private HashSet<Enemy> _enemys = new HashSet<Enemy>();
        private int _enemyFrameCounter = 0;

        private HashSet<Bullet> _bullets = new HashSet<Bullet>();

        private ControlButton _button = ControlButton.None;

        private float _timeToMove = 0f;
        private bool _firstFrame = false;

        public bool GameOver { get; private set; }
        public bool HasWon { get; private set; }
        public int Level { get; set; }
        public override void Reset()
        {
            _timeToMove = 0;
            GameOver = false;
            HasWon = false;
            _clearQueue.UnionWith(_allEntity.Keys);
            _allEntity.Clear();
            _enemys.Clear();
            _enemyFrameCounter = 0;
            _bullets.Clear();
            _player = null;
            _button = ControlButton.None;

            _firstFrame = true;
            WormMap mapCreator = new WormMap(_mapSize + 1, _mapSize + 1);
            mapCreator.GenerateMap();
            mapCreator.AddWater();
            mapCreator.AddUnit(1, 'P');
            int enemyCount = Level;
            mapCreator.AddUnit(Level, 'E');

            var map = mapCreator.Map;
            for (int w = 0; w < map.GetLength(0); w++)
            {
                for (int h = 0; h < map.GetLength(1); h++)
                {
                    char symbol = map[w, h];
                    Entity? obj = Factory.Create(symbol, w, h);
                    if (obj != null)
                    {
                        _allEntity[new Cell(w, h)] = obj;
                        if (obj is Player)
                            _player = (Player)obj;
                        else if (obj is Enemy)
                            _enemys.Add((Enemy)obj);
                    }

                }
            }

        }
        public override void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;

            if (_timeToMove > 0f) return;
            _timeToMove = 1f / 8f;
            _frameCounter++;

            ActivateInput(_button, _player);
            _player._coldown++;

            if (_enemyFrameCounter + 3 < _frameCounter)
            {
                EnemyBrain();
                _enemyFrameCounter = _frameCounter;
            }
            foreach (var bullet in _bullets)
            {
                if (bullet == null) continue;
                var targetPos = bullet.Move();
                if (!TryChangePosition(bullet, targetPos))
                {
                    _bullets.Remove(bullet);
                    _clearQueue.Add(bullet.Position);
                    continue;
                }
                _clearQueue.Add(bullet.Position);
                bullet.ChangePosition(targetPos);
            }
            _button = ControlButton.None;
            if (_enemys.Count <= 0)
            {
                HasWon = true;
                return;
            }
            if (_player.HP <= 0)
            {
                GameOver = true;
                return;
            }
        }
        public void OnButtonPress(ControlButton button)
        {
            _button = button;
        }
        public bool TryChangePosition(Entity entity, Cell targetPos)
        {
            if (!targetPos.InRange(10, 10))
            {
                return false;
            }
            if (_allEntity.ContainsKey(targetPos))
            {
                if (entity is Bullet)
                {
                    if (_allEntity[targetPos].IsBulletVisible)
                    {
                        var block = _allEntity[targetPos];
                        block.GetDamage();
                        if (block.HP <= 0)
                        {
                            _clearQueue.Add(targetPos);
                            _allEntity.Remove(targetPos);
                            if (block is Enemy)
                            {
                                _enemys.Remove((Enemy)block);
                            }
                        }
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }
        public void EnemyBrain()
        {
            foreach (var enemy in _enemys)
            {
                enemy._coldown++;
                var dirToPlayer = enemy.Position.DirToPoint(_player.Position);
                bool playerOnFire = false;
                Cell zero = new Cell(0, 0);
                if (!dirToPlayer.Equals(zero))
                {
                    Cell stepCell = enemy.Position.Sum(dirToPlayer);
                    while (!stepCell.Equals(_player.Position))
                    {
                        playerOnFire = false;
                        if (!_allEntity.ContainsKey(stepCell))
                        {
                            playerOnFire = true;
                            stepCell = stepCell.Sum(dirToPlayer);
                            continue;
                        }
                        break;
                    }
                }
                if (playerOnFire)
                {
                    enemy.SetRotation(dirToPlayer);
                    _bullets.Add(enemy.Shoot());
                    return;
                }
                Collection<Cell> availablePath = new Collection<Cell>();
                foreach (var dir in enemy._moveDiraction)
                {
                    Cell lookDir = enemy.Position.Sum(dir);
                    if (_allEntity.ContainsKey(lookDir)) continue;
                    availablePath.Add(lookDir);
                }
                if (availablePath.Count == 0) return;
                Cell direction = availablePath[new Random().Next(0, availablePath.Count)];
                enemy.SetRotation(direction.Sub(enemy.Position));
                if (TryChangePosition(enemy, direction))
                {
                    _allEntity.Add(direction, enemy);
                    _clearQueue.Add(enemy.Position);
                    _allEntity.Remove(enemy.Position);
                    enemy.ChangePosition(direction);
                }
            }
        }
        public void ActivateInput(ControlButton? button, Player player)
        {
            switch (button)
            {
                case (ControlButton.Left):
                    _clearQueue.Add(player.Position);
                    player.Rotate(false);
                    break;
                case (ControlButton.Right):
                    _clearQueue.Add(player.Position);
                    player.Rotate(true);
                    break;
                case (ControlButton.Up):
                    var targetPosUp = _player.Move(true);
                    if (TryChangePosition(_player, targetPosUp))
                    {
                        _allEntity.Add(targetPosUp, _player);
                        _clearQueue.Add(_player.Position);
                        _allEntity.Remove(_player.Position);
                        _player.ChangePosition(targetPosUp);
                    }
                    break;
                case (ControlButton.Down):
                    var targetPosDown = _player.Move(false);
                    if (TryChangePosition(_player, targetPosDown))
                    {
                        _allEntity.Add(targetPosDown, _player);
                        _clearQueue.Add(_player.Position);
                        _allEntity.Remove(_player.Position);
                        _player.ChangePosition(targetPosDown);
                    }
                    break;
                case (ControlButton.Attack):

                    _bullets.Add(player.Shoot());
                    break;
                case (ControlButton.None):
                    break;
            }
        }
        public override void Draw(ConsoleRenderer renderer)
        {
            foreach (var cell in _clearQueue)
            {
                renderer.ClearPixels(cell.X + 1, cell.Y + 1);
            }
            foreach (var cell in _allEntity.Values)
            {
                renderer.SetPixels(cell.Position.X + 1, cell.Position.Y + 1, cell.Texture, cell.ColorIndex);
            }
            foreach (var bullet in _bullets)
            {
                if (bullet != null)
                    renderer.SetPixels(bullet.Position.X + 1, bullet.Position.Y + 1, bullet.Texture, bullet.ColorIndex);
            }
            if (_firstFrame)
            { 
                _firstFrame = false;
                Console.Clear();
                renderer.Clear();
                renderer.SetFrame(13, 13, '█', 4);
            }
        }

        public override bool IsDone()
        {
            return GameOver || HasWon;
        }
        private void EntityDie(Entity entity)
        {
            _allEntity.Remove(entity.Position);
        }
    }
}
