using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    /// <summary>
    /// Класс управления личными рекордами игроков.
    /// </summary>
    class ScoreManager
    {
        // Список рекордов
        private List<ScoreEntry> _scores;

        /// <summary>
        /// Сохраняет личный рекорд
        /// </summary>
        /// <param name="score">Экземпляр личного рекорда</param>
        public void SaveScore(ScoreEntry score) {
            if (_scores.Contains(score)) _scores.Remove(score);
            _scores.Add(score);
            _scores.Sort((e1,e2) => {
                if (e1 == e2) return 0;
                else if (e1 > e2) return 1;
                else return -1;
            });
        }

        /// <summary>
        /// Берёт первые count рекордов по уроню. Если count равняется 0, возвращает все рекорды по уровню.
        /// </summary>
        /// <param name="levelID">ID уровня</param>
        /// <param name="count">количество элементов</param>
        /// <returns></returns>
        public List<ScoreEntry> GetHighScores(String levelID, int count = 0) 
        {
            List <ScoreEntry> scoreEntries = _scores.FindAll(score => score._levelID == levelID);
            if (count > 0) scoreEntries = scoreEntries.Take(count).ToList();
            return scoreEntries;
        }
    }
}
