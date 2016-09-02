using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Timer.Enums;

namespace Timer
{
    public partial class Settings : Telerik.WinControls.UI.RadForm
    {
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
            }
            else
            {
                lblYieldPerHarvester.Visible = true;
                txtYieldPerHarvester.Visible = true;
            }
        }

        private void rdioMiningTypeOre_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (args.ToggleState == ToggleState.On)
            {
                lblYieldPerHarvester.Visible = true;
                txtYieldPerHarvester.Visible = true;
            }
            else
            {
                lblYieldPerHarvester.Visible = false;
                txtYieldPerHarvester.Visible = false;
            }
        }
    }
}
