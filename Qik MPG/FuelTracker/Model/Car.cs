﻿/*
Karan Thaker
karan.thaker@hotmail.com
*/

using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Linq;

namespace FuelTracker.Model
{
    public class Car : INotifyPropertyChanged
    {
        private string _name;
        private BitmapImage _picture;
        private float _initialOdometerReading;
        private ObservableCollection<Fillup> _fillupHistory;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        [System.Runtime.Serialization.IgnoreDataMemberAttribute]
        public BitmapImage Picture
        {
            get { return _picture; }
            set
            {
                if (_picture == value) return;
                _picture = value;
                NotifyPropertyChanged("Picture");
            }
        }

        public float InitialOdometerReading
        {
            get { return _initialOdometerReading; }
            set
            {
                var roundedValue = (float)Math.Round(value, 0);
                if (_initialOdometerReading.Equals(roundedValue)) return;
                _initialOdometerReading = roundedValue;
                NotifyPropertyChanged("InitialOdometerReading");
            }
        }

        public float AverageFuelEfficiency
        {
            get
            {
                if (FillupHistory == null) return 0;
                float totalFuel = FillupHistory.Sum(fillup => fillup.FuelQuantity);
                float totalDistance = FillupHistory.Sum(fillup => fillup.DistanceDriven);
                if (totalDistance.Equals(0)) return 0;
                return totalDistance / totalFuel;
            }
        }

        public ObservableCollection<Fillup> FillupHistory
        {
            get { return _fillupHistory; }
            set
            {
                if (_fillupHistory == value) return;
                _fillupHistory = value;
                if (_fillupHistory != null)
                {
                    _fillupHistory.CollectionChanged += delegate
                    {
                        NotifyPropertyChanged("AverageFuelEfficiency");
                        NotifyPropertyChanged("LastFillup");
                    };
                }
                NotifyPropertyChanged("FillupHistory");
                NotifyPropertyChanged("AverageFuelEfficiency");
                
            }
        }

        public Fillup LastFillup
        {
            get
            {
                if(FillupHistory != null && FillupHistory.Count > 0)
                {
                    return FillupHistory[0];
                }
                
                return null;
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
