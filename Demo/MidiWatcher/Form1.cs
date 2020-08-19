using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Text;
using Newtonsoft.Json;
using ErrorEventArgs = Sanford.Multimedia.ErrorEventArgs;

namespace MidiWatcher
{
    public partial class Form1 : Form
    {
        private InputDevice _device;
        private SynchronizationContext _context;
        readonly Stopwatch _stopWatch = Stopwatch.StartNew();
        private double _lastMillis;
        private double _diffMills;
        private double _diffTimeStamp;
        private int _counter;
        private int _lastTimestamp;
        private Dictionary<string, string> _sounds = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
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
                //_device.SysCommonMessageReceived += HandleSysCommonMessageReceived;
                //_device.SysExMessageReceived += HandleSysExMessageReceived;
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

        private void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            _context.Post(delegate
            {
                channelListBox.Items.Add(
                    e.Message.Command.ToString() + '\t' + '\t' +
                    e.Message.MidiChannel + '\t' +
                    e.Message.Data1 + '\t' +
                    e.Message.Data2);

                if (e.Message.Data1 == 72)
                {
                    ReloadMapping();
                }

                if (_sounds.TryGetValue(e.Message.Data1.ToString(), out var soundFile))
                {
                    var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var effects = Path.Combine(docs, "SoundBoard");
                    var sfx = Path.Combine(effects, soundFile);
                    var player = new SoundPlayer(sfx );
                    player.Play();

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
                _diffMills = 60000 / (millis - _lastMillis);
                _lastMillis = millis;

                var timestamp = e.Message.Timestamp;
                _diffTimeStamp = 60000.0 / (timestamp - _lastTimestamp);
                _lastTimestamp = timestamp;
            }
        }
    }
}