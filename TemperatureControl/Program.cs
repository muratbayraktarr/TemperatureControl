using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TemperatureControl
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        static void Main(string[] args)
        {
            
            if (Environment.UserInteractive) // Kontrol servisin interaktif olarak çalıştırılıp çalıştırılmadığını belirler
            {
                Service1 service = new Service1();

                // Hizmeti hata ayıklama modunda çalıştırma
                service.OnDebug();

                // Konsolun kapanmaması için bir tuşa basılmasını bekleyin
                Console.WriteLine("Servis konsol modunda çalışıyor...");
                while (Console.ReadLine() != "q") ;


            }
            else
            {
                // Servisiniz normal olarak çalıştırılacaksa burada başlatma kodu yer alacak
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Service1()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
        static void StartService()
        {
            var service = new Service1(); // YourService, oluşturduğunuz servisin adıdır
            service.OnDebug();
        }
    }
}
