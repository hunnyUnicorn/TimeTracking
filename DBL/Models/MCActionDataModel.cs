using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Models
{
    public class MCActionDataModel
    {
        public MCActionDataHeader Header { get; set; }
        public List<MCActionGroupData> GroupData { get; set; }
        public List<MCActionData> ActionData { get; set; }

        public MCActionDataModel()
        {
            Header = new MCActionDataHeader();
            GroupData = new List<MCActionGroupData>();
            ActionData = new List<MCActionData>();
        }
    }

    public class MCActionDataHeader
    {
        public int ActionCode { get; set; }
        public DateTime CreateTime { get; set; }
        public string ActionType { get; set; }
        public string GroupName { get; set; }
        public string Maker { get; set; }
    }

    public class MCActionGroupData
    {

        public string ItemName { get; set; }
        public string ItemValue { get; set; }
    }

    public class MCActionData
    {

        public string ItemName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
