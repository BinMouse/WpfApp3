using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Interfaces;

namespace WpfApp3
{   
    /// <summary>
    /// Класс, отвечающий за состояния игры
    /// </summary>
    class GameManager
    {
        private IGameState _currentState;
        private ScoreManager _scoreManager;

        public void SwitchState(IGameState newState)
        {
            _currentState = newState;
        }

        public void Update()
        {
            _currentState.Update();
        }
    }
}
