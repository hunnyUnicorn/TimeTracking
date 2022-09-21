using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DBL
{
    public class LogUtil
    {
        public static void Infor(string logFile, string functioName, string message)
        {
            WriteLog(logFile, functioName, new Exception(message), false);
        }

        public static void Error(string logFile, string functioName, Exception ex)
        {
            WriteLog(logFile, functioName, ex);
        }

        public static void Error(string logFile, string functioName, string message)
        {
            WriteLog(logFile, functioName, new Exception(message));
        }

        private static void WriteLog(string logFile, string functioName, Exception ex, bool isError = true)
        {
            Task.Run(async () =>
            {
                try
                {
                    //--- Create folder if it does not exists
                    var fi = new FileInfo(logFile);
                    if (!fi.Directory.Exists)
                        fi.Directory.Create();

                    //--- Delete log if it more than 500Kb
                    if (fi.Exists)
                    {
                        if ((fi.Length / 1000) > 100)
                            fi.Delete();
                    }
                    //--- Create stream writter
                    StreamWriter stream = new StreamWriter(logFile, true);
                    await stream.WriteLineAsync(string.Format("{0}|{1:dd-MMM-yyyy HH:mm:ss}|{2}|{3}",
                        isError ? "ERROR" : "INFOR",
                        DateTime.Now,
                        functioName,
                        isError ? ex.ToString() : ex.Message));
                    stream.Close();
                }
                catch (Exception) { }
            });
        }

        public static void WriteMsg(string logFile, StringBuilder builder, bool isRequest)
        {
            Task.Run(async () =>
            {
                try
                {
                    //--- Create folder if it does not exists
                    var fi = new FileInfo(logFile);
                    if (!fi.Directory.Exists)
                        fi.Directory.Create();

                    //--- Delete log if it more than 1Mb
                    if (fi.Exists)
                    {
                        if ((fi.Length / 1000) > 1024)
                            fi.Delete();
                    }

                    //--- Create stream writter
                    using (var stream = new StreamWriter(logFile, true))
                    {
                        await stream.WriteLineAsync("----------------------- Message Start ------------------------------");
                        await stream.WriteLineAsync((isRequest ? "REQUEST" : "RESPONSE") + " MESSAGE: => Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        await stream.WriteLineAsync("-----------------------------------------------------");
                        await stream.WriteAsync(builder.ToString());
                        await stream.WriteLineAsync("------------------------- Message End ------------------------------");
                    }
                }
                catch (Exception) { }
            });
        }
    }
}
