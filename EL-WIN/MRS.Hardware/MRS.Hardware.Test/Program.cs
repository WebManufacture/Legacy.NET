using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HIDLibrary;
using MRS.Hardware.VUSB;

namespace MRS.Hardware.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var devID = "\\\\?\\hid#vid_4242&pid_e131#6&615a3ac&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}";
            //var manager = HidDevices.GetDevice(devID);
            //if (manager != null)
            //{
            //    manager.Open();
            //    while (!manager.IsOpen)
            //    {
            //        var input = Console.ReadLine().ToLower();
            //        if (input == "e") return;
            //        if (input == "s") break;
            //    }
            //    Console.WriteLine("Opened!");
            //    for (int i = 0; i < 100000; i++)
            //    {
            //        var report = manager.ReadReport();
            //        for (int j = 0; j < report.Data.Length; j++)
            //        {
            //            Console.Write(report.Data[j] + " ");
            //        }
            //        Console.WriteLine();
            //    }
            //    Console.WriteLine("end");
            //    manager.Close();
            //}

            VusbManager manager = new VusbManager(0x4242, 0xe131);
            if (manager.IsOpen())
            {
                Console.WriteLine("Opened!");
                Random rnd = new Random();
                string input = "";
                while ((input = Console.ReadLine().ToLower()) != "e")
                {
                    byte message = 0;
                    if (input == "1")
                    {
                        message = manager.SendMessage(1);
                        Console.WriteLine(message);
                    }
                    if (input == "0")
                    {
                        message = manager.SendMessage(0);
                        Console.WriteLine(message);
                    }
                    if (input == "l")
                    {
                        for (int i = 0; i < 10000; i++)
                        {
                            manager.SendMessage(rnd.Next(255));
                        }
                        Console.WriteLine("end");
                    }
                    if (input == "r")
                    {
                        manager.SendMessage(0);
                        for (int i = 0; i < 100000; i++)
                        {
                            Console.WriteLine(manager.GetMessage());
                        }
                        Console.WriteLine("end");
                    }
                    message = manager.GetMessage();
                    Console.WriteLine(message);
                }
            }
            manager.Close();
        }
    }
}
