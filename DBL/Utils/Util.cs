using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL
{
    public class Util
    {
        public static string GenerateSimplePassword()
        {
            var rdm = new Random();
            string myPass = rdm.Next(100000, 999999).ToString();
            string alpha = "0ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int idx = rdm.Next(1, 26);
            string char1 = alpha.Substring(idx, 1);
            idx = rdm.Next(1, 26);
            string char2 = alpha.Substring(idx, 1);

            myPass = myPass.Insert(4, char1);
            myPass = myPass.Insert(2, char2);

            return myPass;
        }
        public static string GeneratePin()
        {
            var rdm = new Random();
            string myPass = rdm.Next(1000, 9999).ToString();
            return myPass;
        }
        public static void SaveImage(string base64String,string filePath)
        {
            var bytes = Convert.FromBase64String(base64String);
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
        }
        
    }
}
