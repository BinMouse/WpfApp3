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
        public int volume { get; set; }
        public GameSettings(int volume)
        {
            this.volume = volume;
        }
    }
}
