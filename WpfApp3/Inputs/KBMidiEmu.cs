using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace WpfApp3
{
    internal class KBMidiEmu : IMidiIinput
    {
        public static event EventHandler<int> KeyDown;
        public static event EventHandler<int> KeyUp;
        
        public static void AttachToWindow(System.Windows.Window window)
        {
            window.PreviewKeyDown += KeyboardKeyDown;
            window.PreviewKeyUp += KeyboardKeyUp;
        }

        private static readonly Dictionary<int, int> KbToMidi = new Dictionary<int, int>
        {
            { (int)Key.D0, 36 }, { (int)Key.D1, 37 }, { (int)Key.D2, 38 }, { (int)Key.D3, 39 },
            { (int)Key.D4, 40 }, { (int)Key.D5, 41 }, { (int)Key.D6, 42 }, { (int)Key.D7, 43 },
            { (int)Key.D8, 44 }, { (int)Key.D9, 45 },

            { (int)Key.Q, 46 }, { (int)Key.W, 47 }, { (int)Key.E, 48 }, { (int)Key.R, 49 },
            { (int)Key.T, 50 }, { (int)Key.Y, 51 }, { (int)Key.U, 52 }, { (int)Key.I, 53 },
            { (int)Key.O, 54 }, { (int)Key.P, 55 },

            { (int)Key.A, 56 }, { (int)Key.S, 57 }, { (int)Key.D, 58 }, { (int)Key.F, 59 },
            { (int)Key.G, 60 }, { (int)Key.H, 61 }, { (int)Key.J, 62 }, { (int)Key.K, 63 },
            { (int)Key.L, 64 }, { (int)Key.OemSemicolon, 65 },

            { (int)Key.Z, 66 }, { (int)Key.X, 67 }, { (int)Key.C, 68 }, { (int)Key.V, 69 },
            { (int)Key.B, 70 }, { (int)Key.N, 71 }, { (int)Key.M, 72 }, { (int)Key.OemComma, 73 },
            { (int)Key.OemPeriod, 74 }, { (int)Key.OemQuestion, 75 },

            { (int)Key.NumPad0, 76 }, { (int)Key.NumPad1, 77 }, { (int)Key.NumPad2, 78 },
            { (int)Key.NumPad3, 79 }, { (int)Key.NumPad4, 80 }, { (int)Key.NumPad5, 81 },
            { (int)Key.NumPad6, 82 }, { (int)Key.NumPad7, 83 }, { (int)Key.NumPad8, 84 },
            { (int)Key.NumPad9, 85 },

            //{ (int)Key.OemPlus, 86 }, { (int)Key.OemMinus, 87 }, { (int)Key.OemOpenBrackets, 88 },
            //{ (int)Key.Oem6, 89 }, { (int)Key.OemQuotes, 90 }, { (int)Key.Oem1, 91 },
            //{ (int)Key.Oem5, 92 }, { (int)Key.Oem3, 93 }, { (int)Key.OemBackslash, 94 },
            //{ (int)Key.Oem102, 95 }, { (int)Key.Space, 96 },
        };


        private static void KeyboardKeyDown(object sender, KeyEventArgs e)
        {
            int keycode = ((int)e.Key);
            int note = KbToMidi[keycode];
            KeyDown?.Invoke(null, note);
        }

        private static void KeyboardKeyUp(object sender, KeyEventArgs e)
        {
            int keycode = ((int)e.Key);
            int note = KbToMidi[keycode];
            KeyUp?.Invoke(null, note);
        }
    }
}
