using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{   
    /// <summary>
    /// Класс хранения информации о личном рекорде
    /// </summary>
    class ScoreEntry
    {   
        /// <summary>
        /// Имя игрока
        /// </summary>
        public String _playerName { get; private set; }
        
        /// <summary>
        /// Уровень
        /// </summary>
        public String _levelID { get; private set; }

        /// <summary>
        /// Рекорд
        /// </summary>
        public int _score { get; private set;  }

        ScoreEntry(String playerName, String levelID, int score)
        {
            _playerName = playerName;
            _levelID = levelID;
            _score = score;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ScoreEntry other = (ScoreEntry)obj;
            return (_playerName == other._playerName && 
                    _levelID == other ._levelID &&
                    _score == other._score);
        }

        public static bool operator ==(ScoreEntry left, ScoreEntry right) 
        {            
            return left.Equals(right);
        }

        public static bool operator !=(ScoreEntry left, ScoreEntry right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(ScoreEntry left, ScoreEntry right)
        {
            return left._score > right._score;
        }

        public static bool operator <(ScoreEntry left, ScoreEntry right)
        {
            return left._score < right._score;
        }
    }
}
