using NAudio.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Models
{
    /// <summary>
    /// Класс хранения данных об игровых настройках
    /// </summary>
    class GameSettings
    {   
        /// <summary>
        /// Громкость игры
        /// </summary>
        public int volume { get; set; }

        public GameSettings(int volume = 100)
        {
            this.volume = volume;
        }
    }
}
