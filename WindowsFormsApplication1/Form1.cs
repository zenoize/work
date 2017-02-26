using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Security.Permissions;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;
using System.IO.MemoryMappedFiles;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string serverIP = "8.8.8.8";
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Boolean result = false;       
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
                if (uniCast.Count > 0)
                {  
                    foreach (UnicastIPAddressInformation uni in uniCast)
                    {
                        if ((Convert.ToString(uni.DuplicateAddressDetectionState) == "Preferred") && (Convert.ToString(uni.Address).StartsWith("192.168.")))
                            result = true;   
                    }
                }
             }
            if (result == false) Console.WriteLine("Локальная сеть недоступна");
            if (result == true)
            {
             Ping pingSender = new Ping();
             PingReply reply = pingSender.Send(serverIP);
                if (reply.Status != IPStatus.Success)
                {
                    result = false;
                    Console.WriteLine("Сервер получения данных недоступен");
                }
            }
        }







        private void button4_Click(object sender, EventArgs e)
        {
            Boolean result = false;
            try
            {
                var writePermission = new FileIOPermission(FileIOPermissionAccess.Write, "conf.conf");
                result = true;
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка с работой конфигурационного файла, проверьте права доступа");
            }

        }








        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists("conf.conf"))
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                using (StreamReader reader = new StreamReader("conf.conf"))
                {
                    string row;
                    while ((row = reader.ReadLine()) != null)
                    {
                        string[] options = row.Split('=');
                        dictionary.Add(options[0], options[1]);
                    }
                    string penis = dictionary["penis"];

                    //drugie vhod znacheniya
                    Console.WriteLine(penis);
                }

            }
        }





        private void button3_Click(object sender, EventArgs e)
        {
            //на вход


            string datapath = "C:\\Users\\homePC\\Documents\\Visual Studio 2015\\Projects\\transfer\\transfer\\bin\\Debug";



            DateTime dateTime = DateTime.UtcNow.Date;
            string[] fileArray = Directory.GetFiles(datapath, "*.dat").Select(Path.GetFileName).ToArray();
            Console.Write(dateTime.ToString("yyMMdd"));
            if (fileArray.Length > 8)
            {
                int oldfiles = fileArray.Length - 8;
                for (int i = 0; i < oldfiles; i++)
                {
                    // shlem polnostju
                }

                for (int i = oldfiles; i < fileArray.Length; i++)
                {
                    if (fileArray[i].StartsWith(dateTime.ToString("yyMMdd")))
                    {
                        Console.Write("uspeh");
                        StreamReader reader = new StreamReader("conf.conf");
                        string content = reader.ReadToEnd();
                        reader.Close();
                        content = Regex.Replace(content, "lastdate=", "lastdate=" + dateTime.ToString("yyMMdd"));
                        StreamWriter writer = new StreamWriter("conf.conf");
                        writer.Write(content);
                        writer.Close();
                    }
                    else
                    {
                        //shlem polnostju
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //на вход 
            string readingfilename = "161203_06.dat";
            int Seekingnumber = 0;
            //

            int dataz = 0;
            double faza = 0d;
            using (var mmf = MemoryMappedFile.CreateFromFile(readingfilename, FileMode.Open))
            {
                using (var stream = mmf.CreateViewStream())
                {
                    using (BinaryReader binReader = new BinaryReader(stream))
                    {

                       binReader.BaseStream.Seek(Seekingnumber, SeekOrigin.Begin);
                        try
                        {
                            dataz = binReader.ReadInt32();
                            faza = binReader.ReadDouble();
                        }
                        catch(Exception X)
                        {
                            Console.WriteLine(X);
                        }

                        while ((dataz != 0) && (faza != 0) && (binReader.BaseStream.Position != binReader.BaseStream.Length))
                        {
                       //     Console.WriteLine(Convert.ToString(dataz) + "        |       " + Convert.ToString(faza));
                            dataz = binReader.ReadInt32();
                            faza = binReader.ReadDouble();


                            //отправка json



                        }

                    }
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
