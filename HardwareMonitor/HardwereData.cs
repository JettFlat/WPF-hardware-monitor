using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;

namespace HardwareMonitor
{
    public class HardwereData
    {
        static ObservableCollection<GraphicValues> _Mbanned = GraphicValues.Deserialize();
        public static ObservableCollection<GraphicValues> Mbanned
        {
            get => _Mbanned;
            set
            {
                _Mbanned = value;
            }
        }
        static public Monitor GetSysInfo(bool start=false)
        {
            var pc = new Computer() { CPUEnabled = true, RAMEnabled = true, GPUEnabled = true };
            try
            {
                pc.Open();
            }
            catch (Exception)
            { }
            string DataText = "";
            ObservableCollection<GraphicValuesContainer> tmp = new ObservableCollection<GraphicValuesContainer> { };
            foreach (var hard in pc.Hardware)
            {
                tmp.Add(new GraphicValuesContainer(hard, new List<SensorType> { SensorType.Load, SensorType.Clock, SensorType.Temperature }));
            }
            var mon = new Monitor(tmp);
            mon = Monitor.GetAvalible(mon, HardwereData.Mbanned);
            if(start)
                mon.StartAsync();
            return mon;
        }
       
    }
}
