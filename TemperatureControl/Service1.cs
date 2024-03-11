using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using System.Timers;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.Uwp.Notifications;
using System.IO;
using System.Threading;


namespace TemperatureControl
{
    public partial class Service1 : ServiceBase
    {
        private Computer computer;
        private System.Timers.Timer timer;

        public Service1()
        {
            InitializeComponent();
            

        }
        public void OnDebug()
        {
            computer = new Computer();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.RAMEnabled = true;
            computer.HDDEnabled = true;
            computer.MainboardEnabled = true;
            computer.FanControllerEnabled = true;
            
            computer.Open();
            timer = new System.Timers.Timer();
            timer.Interval = 5000; // 5 saniye (5000 milisaniye)
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
            Console.WriteLine("aa");

        }
        protected override void OnStart(string[] args)
        {
            computer = new Computer();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.RAMEnabled = true;
            computer.HDDEnabled = true;
            computer.MainboardEnabled = true;
            computer.FanControllerEnabled = true;

            computer.Open();
            timer = new System.Timers.Timer();
            timer.Interval = 3000; // 5 saniye (5000 milisaniye)
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();

        }
        string textYolu = null;
        protected override void OnStop()
        {
            timer.Stop();
            timer.Dispose();

            computer.Close();
            
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Zamanlayıcı tetiklendiğinde sıcaklıkları kontrol et
            CheckTemperatures();
        }
        int CPUCounter = 0;
            int GPUCounter = 0;
        private void CheckTemperatures()
        {
            List<float?> CpuTemperatures = new List<float?>();
            float? CurrentCpuTemperatures = null;
            float? CurrentGpuTemperatures = null;
            float? CurrentGpuFanSpeed = null;
            
            foreach (var hardware in computer.Hardware)
            {
                hardware.Update();
                if (hardware.HardwareType == HardwareType.HDD || hardware.HardwareType == HardwareType.TBalancer || hardware.HardwareType == HardwareType.Heatmaster || hardware.HardwareType == HardwareType.Mainboard || hardware.HardwareType == HardwareType.RAM || hardware.HardwareType == HardwareType.SuperIO ) {
                continue;
                }
                foreach (var sensor in hardware.Sensors)
                {
                    //Console.WriteLine($"{hardware.HardwareType} - {hardware.Name}: {sensor.Name} - {sensor.Value}°C");

                    if(hardware.HardwareType == HardwareType.CPU)
                    {
                        // CPU İÇİN İŞLEMLER
                        CpuTemperatures.Add(sensor.Value);
                    }
                    else if(hardware.HardwareType == HardwareType.GpuAti || hardware.HardwareType == HardwareType.GpuNvidia)
                    {
                        // GPU İÇİN İŞLEMLER
                        if(sensor.SensorType == SensorType.Temperature)
                        {
                            // GPU SICAKLIĞI İLE İLGİLİ İŞLEMLER
                            CurrentGpuTemperatures = sensor.Value;
                        }
                        else if(sensor.SensorType == SensorType.Fan)
                        {
                            // GPU FANI İLE İLGİLİ İŞLEMLER
                            CurrentGpuFanSpeed = sensor.Value;
                        }
                    }
                   

                }
            }
            CurrentCpuTemperatures = CpuTemperatures.Max();
            Console.WriteLine("CPU °C DEGERI : " + CurrentCpuTemperatures);
            Console.WriteLine("GPU °C DEGERI : " + CurrentGpuTemperatures);
            Console.WriteLine("GPU FAN HIZI : " + CurrentGpuFanSpeed);
            Console.WriteLine("CPU COUNTER : " + CPUCounter);
            Console.WriteLine("GPU COUNTER : " + GPUCounter);
            string dosyaYolu = AppDomain.CurrentDomain.BaseDirectory + "/Logs";
            if(!Directory.Exists(dosyaYolu))
            {
                Directory.CreateDirectory(dosyaYolu);
            }

            textYolu = AppDomain.CurrentDomain.BaseDirectory + "/Logs/servisim.txt";
            if(!File.Exists(textYolu))
            {
                using (StreamWriter sw = File.CreateText(textYolu))
                {
                    sw.WriteLine("CPU °C DEGERI : " + CurrentCpuTemperatures);
                    sw.WriteLine("GPU °C DEGERI : " + CurrentGpuTemperatures);
                    sw.WriteLine("GPU FAN HIZI : " + CurrentGpuFanSpeed);
                    sw.WriteLine("CPU COUNTER : " + CPUCounter);
                    sw.WriteLine("GPU COUNTER : " + GPUCounter);

                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(textYolu))
                {
                    sw.WriteLine("CPU °C DEGERI : " + CurrentCpuTemperatures);
                    sw.WriteLine("GPU °C DEGERI : " + CurrentGpuTemperatures);
                    sw.WriteLine("GPU FAN HIZI : " + CurrentGpuFanSpeed);
                    sw.WriteLine("CPU COUNTER : " + CPUCounter);
                    sw.WriteLine("GPU COUNTER : " + GPUCounter);

                }
            }


            if (CurrentCpuTemperatures > 95)
            {
                CPUCounter++;
            }
            else
            {
                CPUCounter = 0;
            }

            if (CurrentGpuTemperatures > 95)
            {
                GPUCounter++;
            }
            else
            {
                GPUCounter = 0;
            }

            if(CPUCounter >= 5)
            {
                String message = "İşlemci Sıcaklığı 85 Dereceyi Geçti !!";
                CPUCounter = 0;
                Notification(message);
            }
            if(GPUCounter >= 5)
            {
                String message = "Ekran Kartı Sıcaklığı 85 Dereceyi Geçti !!";
                GPUCounter = 0;
                if(CurrentGpuFanSpeed < 1000)
                {
                    message = "Ekran Kartı Sıcaklığı 85 Dereceyi Geçti !! Muhtemelen fanında bir problem var !!";
                }
                Notification(message);
            }
        }

        private void Notification(String message)
        {
            // Bilgisayarı kapatma işlemi
            //System.Diagnostics.Process.Start("shutdown", "/s /t 0");
            /*
            try
            {
                new ToastContentBuilder()
               .AddArgument("action", "viewConversation")
               .AddArgument("conversationId", 9813)
               .AddText("Dikkat !!")
               .AddText(message)
               .Show();
            }
            catch (Exception)
            {

            }
            */
            // Yeni bir iş parçacığı oluştur
            Thread notificationThread = new Thread(() =>
            {
                try
                {

                     Process.Start("shutdown", "/s /t 0");
                    
                }
                catch (Exception ex)
                {
                    using (StreamWriter sw = File.AppendText(textYolu))
                    {
                        sw.WriteLine("Exception : " + ex);
                      

                    }
                    Console.WriteLine("Hata: " + ex.Message);
                }
                
            });

            // İş parçacığını başlat
            notificationThread.Start();

        }

    }
}
