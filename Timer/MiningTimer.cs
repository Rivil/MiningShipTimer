using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DevExpress.Data.Helpers;
using Timer.Properties;

namespace Timer
{
    public class MiningTimer : IDisposable
    {
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        NotifyIcon ni;
        private System.Timers.Timer fullTimer;
        private System.Timers.Timer alarmTimer;

        private DateTime startTime;

        private void SetVolume()
        {
            // Calculate the volume that's being set. BTW: this is a trackbar!
            int NewVolume = ((ushort.MaxValue / 10) * Model.Settings.Instance.Volume);
            // Set the same volume for both the left and the right channels
            uint NewVolumeAllChannels = (((uint) NewVolume & 0x0000ffff) | ((uint) NewVolume << 16));
            // Set the volume
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        }

        public MiningTimer()
        {
            ni = new NotifyIcon();
            alarmTimer = new System.Timers.Timer();
            fullTimer = new System.Timers.Timer();
            SetVolume();
        }

        public void Display()
        {
            //ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            ni.Icon = Resources.AppIcon;
            ni.Text = "Move you shit out of mining ship alarmer!";
            ni.Visible = true;

            ni.ContextMenuStrip = CreateMenu();

            alarmTimer.Interval = GetTimerTime(true, true);
            alarmTimer.AutoReset = true;
            alarmTimer.Elapsed += OnAlarmCall;
            
            fullTimer.Interval = GetTimerTime(false, true);
            fullTimer.AutoReset = true;
            fullTimer.Elapsed += OnFullTimerCall;
        }

        private void OnAlarmCall(object sender, EventArgs eventArgs)
        {

            alarmTimer.Interval = GetTimerTime(true, false);
            WriteDebugMessage("Alarm call. Next call in: " + (alarmTimer.Interval / 1000));
            PlaySound();
            ChangeIcon(Resources.Alarm);
        }

        private void OnFullTimerCall(object sender, EventArgs eventArgs)
        {

            fullTimer.Interval = GetTimerTime(false, false);
            WriteDebugMessage("Full call. Next call in: " + (alarmTimer.Interval/1000));
            ChangeIcon(Resources.AppIcon);
        }

        private void PlaySound()
        {
            SoundPlayer simpleSound = new SoundPlayer(@Model.Settings.Instance.WarningSound);
            simpleSound.Play();
        }

        private void ChangeIcon(System.Drawing.Icon icon)
        {
            ni.Icon = icon;
        }

        private int GetTimerTime(bool isAlarm, bool isFirst)
        {
            int miliseconds = 1000;

            decimal cycles =
                Math.Ceiling((decimal)Model.Settings.Instance.OreHold/
                             (Model.Settings.Instance.NoOfHarvesters*Model.Settings.Instance.YieldPerHarvester));

            miliseconds =
                decimal.ToInt32(miliseconds*
                                (((cycles - (isFirst ? 0 : 1))*Model.Settings.Instance.CycleTime) -
                                 (isAlarm && isFirst ? Model.Settings.Instance.WarningSeconds : 0)));
            return (miliseconds <= 0) ? 1 : miliseconds;
        }

        private ContextMenuStrip CreateMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            item = new ToolStripMenuItem();
            item.Text = "Start Timer";
            item.Click += StartTimerOnCLick;
            item.Image = Resources.Play.ToBitmap();
            menu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Text = "Stop Timer";
            item.Click += StopTimerOnClick;
            item.Image = Resources.Stop.ToBitmap();
            item.Enabled = false;
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            item = new ToolStripMenuItem();
            item.Text = "Settings";
            item.Click += ItemOnClick;
            item.Image = Resources.Settings.ToBitmap();
            menu.Items.Add(item);

            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            item = new ToolStripMenuItem();
            item.Text = "Quit";
            item.Click += QuitOnClick;
            item.Image = Resources.Quit.ToBitmap();
            menu.Items.Add(item);

            return menu;
        }

        private void QuitOnClick(object sender, EventArgs eventArgs)
        {
            Application.Exit();
        }

        private void StopTimerOnClick(object sender, EventArgs eventArgs)
        {
            foreach (var menuItem in ni.ContextMenuStrip.Items)
            {
                if (menuItem.GetType() == typeof(ToolStripMenuItem))
                {
                    if (((ToolStripMenuItem) menuItem).Text == "Stop Timer")
                    {
                        ((ToolStripMenuItem) menuItem).Enabled = false;
                    }
                    if (((ToolStripMenuItem) menuItem).Text == "Settings")
                    {
                        ((ToolStripMenuItem) menuItem).Enabled = true;
                    }
                }
            }

            fullTimer.Stop();
            alarmTimer.Stop();

        }

        private void StartTimerOnCLick(object sender, EventArgs eventArgs)
        {
            startTime = DateTime.Now;
            WriteDebugMessage("Timer Start");
            foreach (var menuItem in ni.ContextMenuStrip.Items)
            {
                if (menuItem.GetType() == typeof(ToolStripMenuItem))
                {
                    if (((ToolStripMenuItem) menuItem).Text == "Stop Timer")
                    {
                        ((ToolStripMenuItem) menuItem).Enabled = true;
                    }
                    if (((ToolStripMenuItem) menuItem).Text == "Settings")
                    {
                        ((ToolStripMenuItem) menuItem).Enabled = false ;
                    }
                }
            }

            alarmTimer.Start();
            fullTimer.Start();
        }

        private void WriteDebugMessage(string message)
        {
            Debug.Write(DateTime.Now + " - " + new TimeSpan((DateTime.Now - startTime).Ticks).Seconds + " - " + message + Environment.NewLine);
        }

        private void ItemOnClick(object sender, EventArgs eventArgs)
        {
            Model.Settings.Instance.PropertyChanged += InstanceOnPropertyChanged;
            new Settings().ShowDialog();

        }

        private void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            StopTimerOnClick(null, null);
            alarmTimer.Interval = GetTimerTime(true, true);
            fullTimer.Interval = GetTimerTime(false, true);
            SetVolume();
        }

        public void Dispose()
        {
            alarmTimer.Stop();
            fullTimer.Stop();
            ni.Dispose();
        }
    }
}
