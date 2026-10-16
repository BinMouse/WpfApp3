using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Interfaces
{
    interface IGameState
    {
        public void Exit();
        public void Update();
        public void Render();
    }
}
