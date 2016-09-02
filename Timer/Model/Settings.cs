using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace Timer.Model
{
    public sealed class Settings : INotifyPropertyChanged
    {
        private static volatile Settings instance;
        private static object syncRoot = new Object();

        public event PropertyChangedEventHandler PropertyChanged;

        private Settings()
        { }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Settings();
                    }
                }

                return instance;
            }
        }

        public Enums.MiningType Type
        {
            get { return Properties.Settings.Default.MiningType; }
            set
            {
                if (Properties.Settings.Default.MiningType != value)
                {
                    Properties.Settings.Default.MiningType = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged(null);
                }
            }
        }

        public int OreHold
        {
            get { return Properties.Settings.Default.OreHold; }
            set
            {
                if (Properties.Settings.Default.OreHold != value)
                {
                    Properties.Settings.Default.OreHold = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged(null);
                }
            }
        }

        public decimal CycleTime
        {
            get { return Properties.Settings.Default.CycleTime; }
            set
            {
                if (Properties.Settings.Default.CycleTime != value)
                {
                    Properties.Settings.Default.CycleTime = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged(null);
                }
            }
        }

        public int YieldPerHarvester
        {
            get { return Properties.Settings.Default.YieldPerHarvester; }
            set
            {
                if (Properties.Settings.Default.YieldPerHarvester != value)
                {
                    Properties.Settings.Default.YieldPerHarvester = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged(null);
                }
            }
        }

        public int WarningSeconds
        {
            get { return Properties.Settings.Default.WarningSeconds; }
            set
            {
                if (Properties.Settings.Default.WarningSeconds != value)
                {
                    Properties.Settings.Default.WarningSeconds = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged(null);
                }
            }
        }

        public string WarningSound
        {
            get { return Properties.Settings.Default.WarningSound; }
            set
            {
                if (Properties.Settings.Default.WarningSound != value)
                {
                    Properties.Settings.Default.WarningSound = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged(null);
                }
            }
        }

        public int NoOfHarvesters
        {
            get { return Properties.Settings.Default.NoOfHarvesters; }
            set
            {
                if (Properties.Settings.Default.NoOfHarvesters != value)
                {
                    Properties.Settings.Default.NoOfHarvesters = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged(null);
                }
            }
        }

        public int Volume
        {
            get { return Properties.Settings.Default.Volume; }
            set
            {
                if (Properties.Settings.Default.Volume != value)
                {
                    Properties.Settings.Default.Volume = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged(null);
                }
            }
        }
    }
}
