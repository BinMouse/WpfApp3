using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    internal interface IMidiIinput
    {
        public static event EventHandler<int> KeyDown;
        public static event EventHandler<int> KeyUp;
    }
}
