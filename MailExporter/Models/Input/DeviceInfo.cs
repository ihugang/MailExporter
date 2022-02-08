using System;
using System.Management;
using Microsoft.VisualBasic.Devices;

namespace MailExporter.Models.Input
{
    public class DeviceInfo
    {
        public Guid ApplicationId => Program.ApplicationId;
        public Guid DeviceId { get; set; }

        public string Name { get; set; }

        public byte OS => 4; //windows

        public string OsVersion { get; set; }

        public string Model { get; set; }
        
        public string WechatVersion { get; set; }
        
        public long WechatSize { get; set; }

        public int PhotoNum { get; set; }

        public int VideoNum { get; set; }

        public int DocNum { get; set; }

        public string AppVersion { get; set; }

        public DeviceInfo()
        {
            string code = Guid.NewGuid().ToString();
            SelectQuery query = new SelectQuery("select * from Win32_ComputerSystemProduct");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (var item in searcher.Get())
                {
                    using (item) code = item["UUID"].ToString();
                }
            }

            var computer = new ComputerInfo();
            DeviceId = Guid.Parse(code);
            Name = Environment.MachineName;
            OsVersion = computer.OSFullName + " " + computer.OSVersion;


            var model = "Unkown";
            var model2 = "";
            var searcher2 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject wmi in searcher2.Get())
            {
                try
                {
                    model = wmi.GetPropertyValue("Manufacturer").ToString();
                    model2 = wmi.GetPropertyValue("Product").ToString();
                }
                catch
                {
                    model = "Unkown";
                }
            }

            Model = model + " " + model2;

        }
    }
}
