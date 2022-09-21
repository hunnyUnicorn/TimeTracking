using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.JWT
{
    public class AuthenticationManager
    {
        private readonly string key;
        Bl bl;
        public AuthenticationManager(string key,string connString)
        {
            this.key = key;
            bl = new Bl(connString);
        }
        //public string AuthenticateUser(string username,string password)
        //{

        //}
    }
}
