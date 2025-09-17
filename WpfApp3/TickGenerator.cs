using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Midi;

namespace WpfApp3
{
    internal class TickGenerator
    {
        private MidiFile _midiFile;
        private int[] _channelCursors;
        private int _channelCount;
        private bool _isActive;
        private int _currentTime = 0;
        private int _duration = 0;
        private float _currentMicroSecondsPerTick = 1;
        private const int SmallestDelay = 50; 

        public event EventHandler<int> Tick;
        public event EventHandler<bool> Finished;
        public event EventHandler<MidiEvent> SoloMidiOnEvent;
        public event EventHandler<MidiEvent> AccompanimentMidiOnEvent;

        public TickGenerator(MidiFile midiFile)
        {
            _midiFile = midiFile;
            _channelCount = _midiFile.Events.Count();
            _duration = (int)_midiFile.Events.SelectMany(x => x).Max(x => x.AbsoluteTime);

            Console.WriteLine($"DeltaTicksPerQuarterNote: {midiFile.DeltaTicksPerQuarterNote}");
            Console.WriteLine($"Duration: {_duration}");
            var tt = _midiFile.Events.SelectMany(x => x).Where(x => x is TempoEvent).ToList();
        }

        public IEnumerable<IList<MidiEvent>> GetAccompaniment()
        {
            return _midiFile.Events.Where(x => !x.Any(y => y is PatchChangeEvent e && e.Patch != 2));
        }

        public IEnumerable<MidiEvent> GetSolo()
        {
            return _midiFile.Events
                .Where(x => x.Any(y => y is PatchChangeEvent e && e.Patch == 2))
                .SelectMany(x => x)
                .OrderBy(x => x.AbsoluteTime);
        }

        public void Start()
        {
            if (_isActive)
            {
                throw new Exception("Alredy playing.");
            }
            _channelCursors = new int[_channelCount];
            _isActive = true;
            Task.Factory.StartNew(MainLoop, TaskCreationOptions.LongRunning);
        }

        public void stop()
        {
            _isActive = false;
        }

        private void MainLoop()
        {
            while (_isActive && _currentTime < _duration)
            {
                int delayInTicks = (int)GetNextDelayAndShiftChennalIndex();

                var delayInMilliSeconds = delayInTicks * _currentMicroSecondsPerTick / 1000;

                do
                {
                    delayInTicks--;
                    _currentTime++;
                    Thread.Sleep((int)_currentMicroSecondsPerTick / 1000);
                    Tick?.Invoke(this, _currentTime);
                }
                while (delayInTicks > 0);

            }
            Finished?.Invoke(this, _isActive);
            _isActive = false;
        }

        private int GetNextDelayAndShiftChennalIndex()
        {
            long closestEventTime = int.MaxValue;
            // get SmolestDelay
            for (int chennalIndex = 0; chennalIndex < _channelCount; chennalIndex++)
            {
                if (_channelCursors[chennalIndex] == _midiFile.Events[chennalIndex].Count())
                {
                    continue;
                }

                var nextAbsoluteTIme = _midiFile.Events[chennalIndex][_channelCursors[chennalIndex]].AbsoluteTime;
                closestEventTime = Math.Min(closestEventTime, nextAbsoluteTIme);
            }

            if (closestEventTime - _currentTime > SmallestDelay)
            {
                return SmallestDelay;
            }

            // Move cursors to this delay
            for (int channelIndex = 0; channelIndex < _channelCount; channelIndex++)
            {
                if (_channelCursors[channelIndex] == _midiFile.Events[channelIndex].Count())
                {
                    continue;
                }

                if (_midiFile.Events[channelIndex][_channelCursors[channelIndex]] is TempoEvent tempoEvent)
                {
                    _currentMicroSecondsPerTick = (float)tempoEvent.MicrosecondsPerQuarterNote / (float)_midiFile.DeltaTicksPerQuarterNote;
                }

                if (_midiFile.Events[channelIndex][_channelCursors[channelIndex]].AbsoluteTime == closestEventTime)
                {
                    SoloMidiOnEvent?.Invoke(this, _midiFile.Events[channelIndex][_channelCursors[channelIndex]]);
                    _channelCursors[channelIndex]++;
                };
            }

            return (int)(closestEventTime - _currentTime);
        }
    }
}
