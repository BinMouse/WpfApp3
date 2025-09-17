using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;

namespace WpfApp3
{
    public class NotesToTimeExtractor
    {
        IList<MidiEvent> _midiEvents;
        public NotesToTimeExtractor(IList<MidiEvent> midiEvents)
        {
            _midiEvents = midiEvents;
        }

        private int _currentStep;

        private List<NoteOnEvent> _notesToDisplay = new();
        public IList<NoteOnEvent> GetNotesForTime(int time, int frameSize)
        {
            AddNewNotesToActiveTist(time, frameSize);
            RemoveOldNotes(time, frameSize);
            return _notesToDisplay.ToArray();
        }

        private void RemoveOldNotes(int time, int frameSize)
        {
            var notesToDelete = _notesToDisplay.Where(x=>((int)x.AbsoluteTime + x.NoteLength) < time).ToArray();
            _notesToDisplay.RemoveAll(match=> notesToDelete.Contains(match));
        }

        private void AddNewNotesToActiveTist(int time, int frameSize)
        {
            for (int i = _currentStep; i < _midiEvents.Count; i++)
            {
                if (_midiEvents[i] is NoteOnEvent noteOnEvent && noteOnEvent.Velocity > 0)
                {
                    if (noteOnEvent.AbsoluteTime <= time + frameSize)
                    {
                        _notesToDisplay.Add(noteOnEvent);
                        _currentStep = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
