using Newtonsoft.Json;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using ErrorEventArgs = Sanford.Multimedia.ErrorEventArgs;

namespace MidiWatcher
{
    public partial class Form1 : Form
    {
        private InputDevice _device;
        private SynchronizationContext _context;
        readonly Stopwatch _stopWatch = Stopwatch.StartNew();
        private double _lastMillis;
        private int _counter;
        private int _lastTimestamp;
        private Dictionary<string, string> _sounds = new Dictionary<string, string>();
        private readonly MediaPlayer _mediaPlayer;
        private float _volume = 15.0f / 127.0f;

        public Form1()
        {
            InitializeComponent();
            _mediaPlayer = new MediaPlayer();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (InputDevice.DeviceCount == 0)
            {
                MessageBox.Show(@"No MIDI input devices available.", @"Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();

                return;
            }

            try
            {
                _context = SynchronizationContext.Current;

                _device = new InputDevice(0);
                _device.ChannelMessageReceived += HandleChannelMessageReceived;
                _device.SysRealtimeMessageReceived += HandleSysRealtimeMessageReceived;
                _device.Error += inDevice_Error;

                ReloadMapping();
                ListEffects();

                _device.StartRecording();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }

            base.OnLoad(e);
        }            

        protected override void OnClosed(EventArgs e)
        {
            _device?.Close();

            base.OnClosed(e);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            channelListBox.Items.Clear();

            try
            {               
                _device.StartRecording();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                _device.StopRecording();
                _device.Reset();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private static void inDevice_Error(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.Error.Message, @"Error!",
                   MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void ListEffects()
        {
            var n = 0;
            foreach (var sound in _sounds)
            {
                var name = sound.Value.Replace('-', ' ').Replace('_', ' ').Replace(".wav", "");
                var upNext = true;
                var sb = new StringBuilder();
                foreach (var ch in name)
                {
                    var c = ch;
                    if (c == ' ')
                        upNext = true;
                    else if (upNext)
                    {
                        c = char.ToUpper(c);
                        upNext = false;
                    }
                    sb.Append(c);
                }

                _soundList.Items.Add(
                    sound.Key + ": "  +
                    sb.ToString());

                if (++n == 4)
                {
                    _soundList.Items.Add("--------------------");
                    n = 0;
                }
            }
        }

        public void Play(string filename)
        {
            _mediaPlayer.Open(new Uri(filename));
            _mediaPlayer.Volume = _volume;
            _mediaPlayer.Play();
        }

        private void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            _context.Post(delegate
            {
                channelListBox.Items.Add(
                    e.Message.Command.ToString() + '\t' + '\t' +
                    e.Message.MidiChannel + '\t' +
                    e.Message.Data1 + '\t' +
                    e.Message.Data2);
                channelListBox.TopIndex = channelListBox.Items.Count - 1;

                // first keyboard key (lower C)
                if (e.Message.Data1 == 48 && e.Message.Command == ChannelCommand.NoteOn)
                {
                    ReloadMapping();
                    return;
                }

                // just testing
                if (e.Message.Command == ChannelCommand.PitchWheel)
                {
                    Console.WriteLine($"SpeedRatio: {e.Message.Data2} {e.Message.Data1}");
                    return;
                }

                // first dial on set of 8 dials on top right of AKAI MPL mini
                if (e.Message.Data1 == 1)
                {
                    _volume = e.Message.Data2 / 127.0f;
                    _mediaPlayer.Volume = _volume;
                    return;
                }

                if (e.Message.Data1 == 2)
                {
                    _volume = e.Message.Data2 / 127.0f;
                    _mediaPlayer.Pause();
                    _mediaPlayer.SpeedRatio = e.Message.Data2 / 127.0f;
                    _mediaPlayer.Play();

                    return;
                }

                if (e.Message.Command != ChannelCommand.NoteOff)
                    return;

                if (_sounds.TryGetValue(e.Message.Data1.ToString(), out var soundFile))
                {
                    var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var effects = Path.Combine(docs, "SoundBoard");
                    var sfx = Path.Combine(effects, soundFile);
                    Play(sfx);
                }

                channelListBox.SelectedIndex = channelListBox.Items.Count - 1;
            }, null);
        }

        private void ReloadMapping()
        {
            _sounds = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("Sounds.json"));
        }

        private void HandleSysRealtimeMessageReceived(object sender, SysRealtimeMessageEventArgs e)
        {
            _counter++;
            if (_counter % 24 == 0)
            {
                var millis = _stopWatch.Elapsed.TotalMilliseconds;
                _lastMillis = millis;

                var timestamp = e.Message.Timestamp;
                _lastTimestamp = timestamp;
            }
        }
    }
}