using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame.GameLogic
{
    internal class TankGameLogic : BaseGameLogic
    {
        private TankGamePlayState _gameplayState = new TankGamePlayState();
        private ShowTextState _showTextState = new(2f);
        private bool newGamePending = false;
        private int currLevel = 0;
        public void GoToGameplay()
        {
            Console.Clear();
            ChangeState(_gameplayState);
            _gameplayState.Level = currLevel;
            _gameplayState.Reset();
        }
        private void GotoGameOver()
        {
            Console.Clear();
            currLevel = 0;
            newGamePending = true;
            _showTextState.text = $"Game Over!";
            ChangeState(_showTextState);
        }
        private void GotoNextLevel()
        {
            Console.Clear();
            currLevel++;
            newGamePending = false;
            _showTextState.text = $"Level {currLevel}";
            ChangeState(_showTextState);
        }
        public override void OnArrowDown()
        {
            if (currentState != _gameplayState) return;
            _gameplayState.OnButtonPress(ControlButton.Down);
        }

        public override void OnArrowLeft()
        {
            if (currentState != _gameplayState) return;
            _gameplayState.OnButtonPress(ControlButton.Left);
        }

        public override void OnArrowRight()
        {
            if (currentState != _gameplayState) return;
            _gameplayState.OnButtonPress(ControlButton.Right);
        }

        public override void OnArrowUp()
        {
            if(currentState != _gameplayState) return;
            _gameplayState.OnButtonPress(ControlButton.Up);
        }

        public override void OnSpacebar()
        {
            if (currentState != _gameplayState) return;
            _gameplayState.OnButtonPress(ControlButton.Attack);
        }

        public override void Update(float deltaTime)
        {
            if (currentState != null && !currentState.IsDone())
                return;
            if (currentState == null || currentState == _gameplayState && !_gameplayState.GameOver)
            {
                GotoNextLevel();
            }
            else if (currentState == _gameplayState && _gameplayState.GameOver)
            {
                GotoGameOver();
            }
            else if (currentState != _gameplayState && newGamePending)
            {
                GotoNextLevel();
            }
            else if (currentState != _gameplayState && !newGamePending)
            {
                GoToGameplay();
            }
        }
        public override ConsoleColor[] CreatePalette()
        {
            return 
                [
                    ConsoleColor.Red, 
                    ConsoleColor.Green, 
                    ConsoleColor.DarkBlue, 
                    ConsoleColor.Yellow, 
                    ConsoleColor.DarkGray,
                    ConsoleColor.DarkRed,
                    ConsoleColor.White

                ];
        }
    }
}
