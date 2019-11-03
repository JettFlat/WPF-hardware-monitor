using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HardwareMonitor
{
    [Serializable]
    public class GraphicValues
    {
        public bool Remove { get; set; } = false;
        [NonSerialized]
        public ISensor Sensor;
        public SensorType SensorType { get; set; }
        public float CurrentValue { get; set; }
        public float Max { get; set; }
        public float Min { get; set; }
        public string HardName { get; set; }
        public string UnitType { get; set; }
        [NonSerialized]
        public static int MaxGraphValues = 10;
        [NonSerialized]
        public static int MaxGraphPixels = 30;
        public List<float> PrevValues { get; set; }
        public PointCollection Points { get { return _Points; } set { _Points = value; } }
        [NonSerialized]
        private PointCollection _Points;

        public GraphicValues()
        { }
        public GraphicValues(ISensor sensor, GraphicValues Prev = null)
        {
            if (sensor == null || sensor.Value == null || sensor.Max == null || sensor.Min == null || sensor.Hardware == null)
                return;
            Sensor = sensor;
            SensorType = sensor.SensorType;
            CurrentValue = (float)Math.Round(sensor.Value.Value, 2);
            Max = (float)Math.Round(sensor.Max.Value, 2);
            //Min = (float)Math.Round(sensor.Min.Value, 2);
            Min = 0;
            HardName = sensor.Name;
            if (Prev != null)
            {
                if (Prev.PrevValues.Count > MaxGraphValues)
                    Prev.PrevValues.RemoveAt(0);
                PrevValues = Prev.PrevValues;
                PrevValues.Add(Prev.CurrentValue);
                Max = PrevValues.Max();
                //Min = PrevValues.Min();
            }
            else
            {
                PrevValues = new List<float> { };
                PrevValues.Add(CurrentValue);
            }
            Points = GetPoints(MaxGraphPixels);
            GetUnitType();
        }
        public PointCollection GetPoints(int MaxPixelSpan)
        {
            if (PrevValues == null)
                return null;
            if (Max == Min)
                return null;
            var k = (Max - Min) / MaxPixelSpan;
            var Points = PrevValues.Select((x, index) => new Point(index * 10, (MaxPixelSpan - ((x - Min) / k)))).ToList();
            var pc = new PointCollection(Points);
            pc.Freeze();
            return pc;
        }
        void GetUnitType()
        {
            //TODO для остальных типов
            var type = Sensor.SensorType;
            if (type == SensorType.Clock)
            {
                UnitType = "MHz";
                return;
            }
            if (type == SensorType.Load)
            {
                UnitType = "%";
                return;
            }
            if (type == SensorType.Temperature)
            {
                UnitType = "℃";
                return;
            }
            UnitType = "";
        }
        public static ObservableCollection<GraphicValues> Deserialize()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            ObservableCollection<GraphicValues> file = null;
            try
            {
                if (File.Exists("Data.dt"))
                    using (FileStream fs = new FileStream("Data.dt", FileMode.OpenOrCreate))
                        file = (ObservableCollection<GraphicValues>)formatter.Deserialize(fs);
            }
            catch (Exception exc)
            {

            }
            return file;
        }
        public static void Serialize(ObservableCollection<GraphicValues> obj)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream("Data.dt", FileMode.Create))
                {
                    formatter.Serialize(fs, obj);
                }
            }
            catch (Exception exc)
            {
            }
        }
    }
    [Serializable]
    public class GraphicValuesContainer : INotifyPropertyChanged
    {
        public IHardware Hardware { get; set; }
        public List<SensorType> ReqSensors { get; set; }
        ObservableCollection<GraphicValues> _GraphicValues;
        public ObservableCollection<GraphicValues> GraphicValues
        {
            get => _GraphicValues;
            set
            {
                _GraphicValues = value;
                OnPropertyChanged("GraphicValues");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName]string PropertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public GraphicValuesContainer(IHardware Hardware, List<SensorType> ReqSensors)
        {
            this.Hardware = Hardware;
            this.ReqSensors = ReqSensors;
            SetGraphicValues();
        }
        public GraphicValuesContainer(IHardware Hardware, ObservableCollection<GraphicValues> GraphicValues, List<SensorType> ReqSensors)
        {
            this.Hardware = Hardware;
            this.ReqSensors = ReqSensors;
            this.GraphicValues = GraphicValues;
        }
        public void UpdateValues()
        {
            //if (Hardware == null || ReqSensors == null)
            //    return;
            Hardware.Update();
            ObservableCollection<GraphicValues> tmp = new ObservableCollection<GraphicValues> { };
            foreach (var gv in GraphicValues)
                tmp.Add(new GraphicValues(gv.Sensor, gv));
            GraphicValues = tmp;
        }
        void SetGraphicValues()
        {
            ObservableCollection<GraphicValues> tmp = new ObservableCollection<GraphicValues> { };
            foreach (var sensor in Hardware.Sensors)
                if (!(sensor == null || sensor.Value == null || sensor.Max == null || sensor.Min == null || sensor.Hardware == null) && ReqSensors.Contains(sensor.SensorType) )
                    tmp.Add(new HardwareMonitor.GraphicValues(sensor));
            GraphicValues = tmp;
        }
    }
    [Serializable]
    public class Monitor
    {
        public ObservableCollection<GraphicValuesContainer> Hards { get; set; }
        public int Timespan { get; set; }
        public bool IsRun { get; set; }
        public Monitor(ObservableCollection<GraphicValuesContainer> Hards, int Timespan = 1000)
        {
            this.Hards = Hards;
            this.Timespan = Timespan;
        }
        public async void StartAsync()
        {
            IsRun = true;
            await Task.Run(() =>
            {
                while (IsRun)
                {
                    System.Threading.Thread.Sleep(Timespan);
                    Hards.AsParallel().ForAll(x =>
                    {
                        x.UpdateValues();
                    });
                }
            });
        }
        public void Stop()
        {
            IsRun = false;
        }
        public static Monitor GetAvalible(Monitor obj, ObservableCollection<GraphicValues> Banned)
        {
            if (Banned == null || Banned.Count < 1)
                return obj;
            ObservableCollection<GraphicValuesContainer> avaliblehards = new ObservableCollection<GraphicValuesContainer> { };
            foreach (var hard in obj.Hards)
            {
                ObservableCollection<GraphicValues> gvs = new ObservableCollection<GraphicValues> { };
                foreach (var gvc in hard.GraphicValues)
                {
                    if (!Banned.Any(x => x.HardName == gvc.HardName && x.SensorType == gvc.SensorType))
                        gvs.Add(gvc);
                }
                if (gvs.Count > 0)
                    avaliblehards.Add(new GraphicValuesContainer(hard.Hardware, gvs, hard.ReqSensors));
            }
            var mon = new Monitor(new ObservableCollection<GraphicValuesContainer>(avaliblehards));
            return mon;
        }
    }
}
