using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NAudio.Midi;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Channels;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MidiOut midiOut = new MidiOut(0);
        private IMidiIinput midiInput;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;

            MyImage.Source = _writeableBitmap;

            MiDIinput.ListDevices();
            if (MiDIinput.mReady)
            {
                MiDIinput.HandleMidiMessages();
                MiDIinput.KeyDown += MiDIinput_KeyDown;
                MiDIinput.KeyUp += MiDIinput_KeyUp;
            }            
            KBMidiEmu.AttachToWindow(this);
            KBMidiEmu.KeyDown += MiDIinput_KeyDown;
            KBMidiEmu.KeyUp += MiDIinput_KeyUp;
        }

        private void MiDIinput_KeyUp(object? sender, int e)
        {
            midiOut.Send(MidiMessage.StopNote(e, 127, 2).RawData);
            Dispatcher.Invoke(()=> DrawNote(e - 36, 0));
        }

        private void MiDIinput_KeyDown(object? sender, int e)
        {
            Console.WriteLine(e);
            midiOut.Send(MidiMessage.StartNote(e, 127, 2).RawData);
            Dispatcher.Invoke(() => DrawNote(e - 36, 127));
        }

        WriteableBitmap _writeableBitmap = new WriteableBitmap(1040, 400, 96, 96, PixelFormats.Bgra32, null);
        private NotesToTimeExtractor _notesToTimeExtractor;

        private SolidColorBrush _blackKey = new SolidColorBrush(Colors.Black); 
        private SolidColorBrush _whiteKey = new SolidColorBrush(Colors.White);
        private SolidColorBrush _grayKey = new SolidColorBrush(Colors.Gray);
        private int _yScale = FrameSize / 400;
        private int[] _keys = new int[] { 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0 }; // { 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, };
        private const int FrameSize = 500;

        private const int _keyscount = 61;
        private int[] _keyWidth = new int [_keyscount];
        private int[] _keyLeftPositiont = new int[_keyscount];

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var leftShift = 0;            
            for (int i = 0; i < _keyscount; i++)
            {
                Rectangle key = new Rectangle();
                
                if (_keys[i % _keys.Length] == 0)
                {
                    _keyLeftPositiont[i] = leftShift+3;
                    leftShift += 20;
                    _keyWidth[i] = 14;
                    key.Width = 20;
                    key.Height = 100;
                    key.Fill = new SolidColorBrush(Colors.White);
                    key.StrokeThickness = 1;
                    key.Stroke = Brushes.Black;
                }
                else
                {
                    _keyLeftPositiont[i] = leftShift-2;
                    _keyWidth[i] = 6;
                    key.Width = 10;
                    key.Height = 60;
                    key.Fill = new SolidColorBrush(Colors.Black);
                    key.Margin = new Thickness(-5, 0, -5, 0);
                    key.VerticalAlignment = VerticalAlignment.Top;
                    Canvas.SetZIndex(key, 2);
                }
                
                //key.Name= i.ToString();
                // Set properties of the key (width, height, fill color)
                keyboard.Children.Add(key);
            }
        }

        private void PlayChannels(IEnumerable<IList<MidiEvent>> channels)
        {
            foreach(var channel in channels)
            {

            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            midiOut.Volume= 65535;
            //MidiFile file = new MidiFile("D:/Work/Karaoke for hands/Midi/К Элизе 6 (Фортепиано).mid");
            MidiFile file = new MidiFile("midi.mid");

            TickGenerator tg = new TickGenerator(file);

            tg.Tick += Tg_Tick;
            tg.Finished += Tg_Finished;
            tg.SoloMidiOnEvent += Tg_SoloMidiOnEvent;
            tg.Start();
            var eventsToPlay = file.Events
                .Where(x => x.Any(y => y is PatchChangeEvent e && e.Patch == 2))
                .SelectMany(x => x)
                .OrderBy(x=>x.AbsoluteTime)
                .ToList();

            _notesToTimeExtractor = new NotesToTimeExtractor(eventsToPlay);
        }

        private void Tg_SoloMidiOnEvent(object? sender, MidiEvent e)
        {
            if (e is NoteOnEvent evt)
            {
                //DrawNote(evt.NoteNumber - 21, evt.Velocity);

                //RenderFlow(file.Events[0], (int)evt2.AbsoluteTime);
                if (evt.Velocity > 0)
                {
                    midiOut.Send(MidiMessage.StartNote(evt.NoteNumber, 127, evt.Channel).RawData);
                }
                else
                {
                    midiOut.Send(MidiMessage.StopNote(evt.NoteNumber, 127, evt.Channel).RawData);
                }
            }
        }

        private void Tg_Finished(object? sender, bool e)
        {
            Console.WriteLine($"Finished with status {e}");
        }

        private void Tg_Tick(object? sender, int e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                RenderFlow(_notesToTimeExtractor.GetNotesForTime(e, FrameSize), e);
            });
        }

        private void DrawNote(int noteNumber, int velocity)
        {
            if (noteNumber > _keyscount) return;
            var key = keyboard.Children[noteNumber] as Rectangle;
            if (velocity == 0)
            {
                key.Fill = _keys[noteNumber % _keys.Length] == 0 ? _whiteKey : _blackKey;
            }
            else
            {
                key.Fill = _grayKey;
            }
        }

        
        public void RenderFlow(IList<NoteOnEvent> events, int time)
        {
            Dispatcher.Invoke(() =>
            {
                _writeableBitmap.Lock();
                _writeableBitmap.Clear(Colors.Black);

                for(int i = 40; i < 1040; i += 140)
                {
                    _writeableBitmap.DrawLine(i, 0, i, 400, Colors.Gray);
                }

                foreach (var item in events)
                {
                    if (item.NoteNumber-21 > _keyscount+1) continue;

                    var x1 = _keyLeftPositiont[item.NoteNumber-21];
                    var x2 = x1 + _keyWidth[item.NoteNumber-21];

                    var y1 = SoundTimeToyChords((int)item.AbsoluteTime + item.NoteLength - time);
                    var y2 = SoundTimeToyChords((int)item.AbsoluteTime - time);
                    _writeableBitmap. DrawRectangle(x1, y1, x2, y2, System.Windows.Media.Colors.Red);
                }

                _writeableBitmap.Unlock();
            });
        }

        private int SoundTimeToyChords(int y)
        {
            //Debug.WriteLine($"{y} => {400 - y/_yScale}");
            return 400 - (y < 0 ? 0 : y / _yScale);
        }
    }
}
