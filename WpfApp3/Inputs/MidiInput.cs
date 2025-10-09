using System;
using System.Windows.Forms;
using NAudio.Midi;

namespace WpfApp3
{
    internal class MiDIinput : IMidiIinput
    {
        public static int mInDeviceIndex = -1;
        public static int mOutDeviceIndex = -1;
        public static MidiIn mMidiIn = null;
        public static Boolean mReady = false;

        public static event EventHandler<int> KeyDown;
        public static event EventHandler<int> KeyUp;
        public static void ListDevices()
        {
            Console.WriteLine("MIDI In Devices");
            Console.WriteLine("===============");
            for (int device = 0; device < MidiIn.NumberOfDevices; device++)
            {
                mReady = true; //  some midi in device exists
                Console.WriteLine(MidiIn.DeviceInfo(device).ProductName);
            }

            mInDeviceIndex = 0;

            Console.WriteLine("\n\n\nMIDI Out Devices");
            Console.WriteLine("=====================");

            for (int device = 0; device < MidiOut.NumberOfDevices; device++)
            {
                Console.WriteLine(MidiOut.DeviceInfo(device).ProductName);
            }
            mOutDeviceIndex = 1;
        }

        public static void HandleMidiMessages()
        {
            mMidiIn = new MidiIn(mInDeviceIndex);
            mMidiIn.MessageReceived += midiIn_MessageReceived;
            mMidiIn.SysexMessageReceived += MMidiIn_SysexMessageReceived;
            mMidiIn.ErrorReceived += midiIn_ErrorReceived;
            mMidiIn.Start();
        }

        private static void MMidiIn_SysexMessageReceived(object? sender, MidiInSysexMessageEventArgs e)
        {
            Console.WriteLine(String.Format("Time {0} Message 0x{1:X8}",
            e.Timestamp, e.SysexBytes));
        }

        static void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(String.Format("Time {0} Message 0x{1:X8} Event {2}",
                e.Timestamp, e.RawMessage, e.MidiEvent));
        }

        static void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(String.Format("Time {0} Message 0x{1:X8} Event {2}",
                e.Timestamp, e.RawMessage, e.MidiEvent));
            if (e.MidiEvent is NoteOnEvent noteOnEvent)
            {
                if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOn)
                {
                    MiDIinput.KeyDown?.Invoke(null, noteOnEvent.NoteNumber);
                }
            }

            if (e.MidiEvent is NoteEvent noteEvent)
            {
                if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOff)
                {
                    MiDIinput.KeyUp?.Invoke(null, noteEvent.NoteNumber);
                }
            }
        }
    }
}
