using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using SimpleRESTServer;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;
using System.Timers;


internal class ScrapAPI
    {
        public static ScrapAPI _instance;
        public static Dictionary<ulong, int> BankPlayers = new Dictionary<ulong, int>();
        private static Server ScpServer;
        private static string paath = "Bank.json";
        private static Timer _timer;

        #region Api Server
        public ScrapAPI()
        {
            _instance = this;

            if (File.Exists(paath))
                BankPlayers = JsonConvert.DeserializeObject<Dictionary<ulong, int>>(File.ReadAllText(paath));
            else
            {
                BankPlayers = new Dictionary<ulong, int>();
                SaveData();
            }
               
            _timer = new Timer();
            _timer.Interval =  1000 * 60 * 60;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            
            StartApi();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string tstamp = DateTime.Now.ToString();
            File.WriteAllText("/backups/"+tstamp+"_"+paath, JsonConvert.SerializeObject(BankPlayers));

        }

        public static void stopAPI()
        {
            ScpServer.Stop();
        }

        private void StartApi()
        {
            ScpServer = new Server(EAuthenticationTypes.eNone, new DummyUserMgr(), new[] { "http://*:8967/" });
            ScrapController contl = new ScrapController();

            contl.ScpMsg += (s, e) => { Console.WriteLine(e); };
            ScpServer.AddController(contl);

            new Thread(
                delegate () { ScpServer.Run(); }).Start();
            Console.WriteLine("ScpServer Started on port : 8967");
        }
        private static void SaveData()
        {
            File.WriteAllText(paath, JsonConvert.SerializeObject(BankPlayers));
        }
    #endregion


    #region Main

    public static int GetBalance(ulong id)
    {
        if (!BankPlayers.ContainsKey(id))
        {
            BankPlayers[id] = 0;
            SaveData();
            return 0;
        }
        else
        {
            return BankPlayers[id];
        }
        
    }

    public static void SetBalance(ulong id,int amount)
    {

        if (BankPlayers.ContainsKey(id))
        {
            BankPlayers[id] = amount;
        }
        SaveData();

    }

    #endregion


}
