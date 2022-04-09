using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimpleRESTServer;


    public class ScrapController : Controller
    {
        public event EventHandler<string> ScpMsg;


        [RoutingAttribute("/GetBalance", "POST", "Anonymous")]
        public void GetAmount(string uID)
        {
            ulong uid;
            int amt=0;
            if (ulong.TryParse(uID, out uid))
            {
               amt = ScrapAPI.GetBalance(uid);
                Ok(amt);
            }
            ScpMsg?.Invoke(this, $" Balance of {uid} is {amt}");
    }

        public ScrapController()
        {
        }

        [Routing("/SetBalance", "POST", "Anonymous")]
        public void SetAmount(string merged)
        {
            var prt = merged.Split('_');
            ulong uid;
            int amount = int.Parse(prt[1]);
            if (ulong.TryParse(prt[0], out uid))
            {
                
                ScrapAPI.SetBalance(uid, amount);

            }


            ScpMsg?.Invoke(this, $"Set Balance of {uid} to {amount}");
           
            Ok();

        }
    }
