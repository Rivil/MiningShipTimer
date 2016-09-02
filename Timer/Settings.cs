using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Timer.Enums;

namespace Timer
{
    public partial class Settings : Telerik.WinControls.UI.RadForm
    {
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        private bool warningShown;

        public Settings()
        {
            InitializeComponent();
            OpenFileDialog dialog = (OpenFileDialog) fileWarningSound.Dialog;
            dialog.Filter = "(*.wav)|*.wav";
            
            if (Model.Settings.Instance.Type == Enums.MiningType.Ice)
            {
                rdioMiningTypeIce.IsChecked = true;
                rdioMiningTypeOre.IsChecked = false;
            }
            else
            {
                rdioMiningTypeIce.IsChecked = false;
                rdioMiningTypeOre.IsChecked = true;
            }

            txtOreHold.Value = Model.Settings.Instance.OreHold;
            txtCycletime.Value = Model.Settings.Instance.CycleTime;
            txtYieldPerHarvester.Value = Model.Settings.Instance.YieldPerHarvester;
            txtWarnBefore.Value = Model.Settings.Instance.WarningSeconds;
            fileWarningSound.Value = Model.Settings.Instance.WarningSound;
            txtNoOfHarvesters.Value = Model.Settings.Instance.NoOfHarvesters;
            rtbVolume.Value = Model.Settings.Instance.Volume;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool hasChanged = false;

            if (rdioMiningTypeIce.IsChecked)
            {
                if (Model.Settings.Instance.Type == MiningType.Ore)
                Model.Settings.Instance.Type = Enums.MiningType.Ice;
            }
            if (rdioMiningTypeOre.IsChecked)
            {
                if (Model.Settings.Instance.Type == MiningType.Ice)
                Model.Settings.Instance.Type = Enums.MiningType.Ore;
            }
                
            Model.Settings.Instance.OreHold = Decimal.ToInt32(Decimal.Parse(txtOreHold.Value.ToString().Replace(",", "").Replace(" ", "").Replace("-", "").Replace("_", "")));
            Model.Settings.Instance.CycleTime = Decimal.Parse(txtCycletime.Value.ToString().Replace(",", "").Replace(" ", "").Replace("-", "").Replace("_", ""));
            if (rdioMiningTypeIce.ToggleState == ToggleState.On)
                Model.Settings.Instance.YieldPerHarvester = 1000;
            else
                Model.Settings.Instance.YieldPerHarvester = Int32.Parse(txtYieldPerHarvester.Value.ToString().Replace(",", "").Replace(" ", "").Replace("-", "").Replace("_", ""));
            Model.Settings.Instance.WarningSeconds = Int32.Parse(txtWarnBefore.Value.ToString().Replace(",", "").Replace(" ", "").Replace("-", "").Replace("_", ""));
            Model.Settings.Instance.WarningSound = fileWarningSound.Value;
            Model.Settings.Instance.NoOfHarvesters = Int32.Parse(txtNoOfHarvesters.Value.ToString().Replace(",", "").Replace(" ", "").Replace("-", "").Replace("_", ""));
            Model.Settings.Instance.Volume = Convert.ToInt32(rtbVolume.Value);
            this.DialogResult = DialogResult.OK;
        }

        private void rdioMiningTypeIce_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (args.ToggleState == ToggleState.On)
            {
                lblYieldPerHarvester.Visible = false;
                txtYieldPerHarvester.Visible = false;
                radLabel2.Visible = false;
            }
            else
            {
                lblYieldPerHarvester.Visible = true;
                txtYieldPerHarvester.Visible = true;
                radLabel2.Visible = false;
            }
        }

        private void rdioMiningTypeOre_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (args.ToggleState == ToggleState.On)
            {
                lblYieldPerHarvester.Visible = true;
                txtYieldPerHarvester.Visible = true;
                radLabel2.Visible = true;
            }
            else
            {
                lblYieldPerHarvester.Visible = false;
                txtYieldPerHarvester.Visible = false;
                radLabel2.Visible = false;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            SetVolume();
            PlaySound();
        }

        private void SetVolume()
        {
            // Calculate the volume that's being set. BTW: this is a trackbar!
            int NewVolume = ((ushort.MaxValue / 10) * Convert.ToInt32(rtbVolume.Value));
            // Set the same volume for both the left and the right channels
            uint NewVolumeAllChannels = (((uint) NewVolume & 0x0000ffff) | ((uint) NewVolume << 16));
            // Set the volume
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        }

        private void PlaySound()
        {
            SoundPlayer simpleSound = new SoundPlayer(@Model.Settings.Instance.WarningSound);
            simpleSound.Play();
        }
    }
}
