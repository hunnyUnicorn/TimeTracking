using DBL.Entities;
using DBL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Models
{
    public class Notification
    {
        public int NotifCode { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string ItemName { get; set; }
        public int ItemCode { get; set; }
        public int CustCode { get; set; }
        public int StatCode { get; set; }
        public USER_TYPE UserType { get; set; }
        public DateTime NotifDate { get; set; }
    }
}
