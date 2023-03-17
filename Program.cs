using Atol.Drivers10.Fptr;
using System.Threading.Tasks;

namespace AtolDriver
{
    internal class Program
    {

        
        static void Main(string[] args)
        {
            try
            {
                Task atolTask = Task.Run(() => PrintReport("Atol"));
                Task atolNetTask = Task.Run(() => PrintReport("AtolNet"));
                
                atolTask.Start();
                atolNetTask.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        private static int InitSettingsCOM(IFptr fptr, int comPort)
        {
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_MODEL, Constants.LIBFPTR_MODEL_ATOL_AUTO.ToString());
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_PORT, Constants.LIBFPTR_PORT_COM.ToString());
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_COM_FILE, $"COM{comPort}");
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_BAUDRATE, Constants.LIBFPTR_PORT_BR_115200.ToString());
            return fptr.applySingleSettings();
        }

        private static int InitSettingsTCP(IFptr fptr, string address, int tcpPort)
        {
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_MODEL, Constants.LIBFPTR_MODEL_ATOL_AUTO.ToString());
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_PORT, Constants.LIBFPTR_PORT_TCPIP.ToString());
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_IPADDRESS, address);
            fptr.setSingleSetting(Constants.LIBFPTR_SETTING_IPPORT, tcpPort.ToString());
            return fptr.applySingleSettings();
        }
        private static void PrintReport(string deviceName)
        {
 
                IFptr fptr_lib = new Fptr(deviceName, @"C:\Program Files (x86)\ATOL\Drivers10\KKT\bin\fptr10.dll");
                //IFptr fptr_lib = new Fptr(@"C:\Program Files (x86)\ATOL\Drivers10\KKT\bin\fptr10.dll");
                if (deviceName == "Atol")
                {
                    InitSettingsCOM(fptr_lib, 6);

                }
                else if(deviceName == "AtolNet")
                {
                    InitSettingsTCP(fptr_lib, "192.168.1.104", 5555);
                }

                if (fptr_lib.open() == 0)
                {
                    //fptr_lib.setParam(Constants.LIBFPTR_PARAM_REPORT_TYPE, Constants.LIBFPTR_RT_X);
                    //Console.WriteLine($"{deviceName} -> Результат fptr_lib.report() - {fptr_lib.report()}");
                }
                else
                {
                    Console.WriteLine($"{deviceName} -> Не удалось соединиться.");
                }
                //fptr_lib.report();


                fptr_lib.setParam(Constants.LIBFPTR_PARAM_JSON_DATA, File.ReadAllText(@"D:\yakov\testJsonTask\checkDotNet6.json"));
                int res = fptr_lib.processJson();
                if(res == 0)
                {
                    String result = fptr_lib.getParamString(Constants.LIBFPTR_PARAM_JSON_DATA);
                    Console.WriteLine($"{deviceName} Ответ - {result}");
                }
                else
                {
                    Console.WriteLine($"{deviceName} Ошибка - {res}");
                }

                fptr_lib.destroy();
            
        }

       
    }
}