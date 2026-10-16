using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{   
    /// <summary>
    /// Отвечает за воспроизведение звуков
    /// </summary>
    class AudioManager
    {
        private MidiOut _midiOut;
        public int _volume { get; set; }

        AudioManager() {
            _midiOut = new MidiOut(0);
            _volume = SettingsManager.settings.volume;
        }

        /// <summary>
        /// Воспроизводит ноту в заданном канале (канал по умолчанию - 0)
        /// </summary>
        /// <param name="note">Номер ноты</param>
        /// <param name="channel">Номер канала</param>
        public void PlayNote(int note, int channel = 0) {
            _midiOut.Send(MidiMessage.StartNote(note, _volume, channel).RawData);
        }

        /// <summary>
        /// Останавливает вопроизведение ноты в заданном канале (канал по умолчанию - 0)
        /// </summary>
        /// <param name="note">Номер ноты</param>
        /// <param name="channel">Номер канала</param>
        public void StopNote(int note, int channel = 0)
        {
            _midiOut.Send(MidiMessage.StopNote(note, _volume, channel).RawData);
        }
    }
}
