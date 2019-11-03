using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace HardwareMonitor
{
    public class VM : VMBase
    {
        Monitor _SysData = HardwereData.GetSysInfo(true);// HardwereData.GetSysInfo(true);
        public Monitor SysData
        {
            get => _SysData;
            set
            {
                _SysData = value;
                OnPropertyChanged("SysData");
            }
        }
        int _GraphWidth = 100;
        public int GraphWidth
        {
            get => _GraphWidth;
        }
        int _GraphHeight = 100;
        public int GraphHeight
        {
            get => _GraphHeight;
        }
        ObservableCollection<GraphicValues> _Banned = HardwereData.Mbanned;
        public ObservableCollection<GraphicValues> Banned
        {
            get => _Banned;
            set
            {
                _Banned = value;
                OnPropertyChanged();
                GraphicValues.Serialize(Banned);
            }
        }
        public RelayCommand Delete => new RelayCommand(o =>
        {
            var gv = o as GraphicValues;
            gv.Remove = true;
            foreach (var i in SysData.Hards)
            {
                var list = i.GraphicValues.Where(x => x.Remove == true).ToList();
                list.All(y => i.GraphicValues.Remove(y));
                var newbl = new ObservableCollection<GraphicValues> { };
                if (Banned != null && Banned.Count > 0)
                    newbl = new ObservableCollection<GraphicValues>(Banned);
                list.ForEach(y => newbl.Add(y));
                Banned = newbl;
            }
            SysData.Hards.Where(x => x.GraphicValues.Count < 1).ToList().All(y => SysData.Hards.Remove(y));
            OnPropertyChanged("SysData");
        });
        public RelayCommand ToDefault => new RelayCommand(o => {
            Banned = new ObservableCollection<GraphicValues> { };
            HardwereData.Mbanned = GraphicValues.Deserialize();
            SysData.Stop();
            SysData = HardwereData.GetSysInfo(true);
        });
    }
}
