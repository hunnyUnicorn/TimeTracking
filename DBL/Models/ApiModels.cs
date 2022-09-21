using DBL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMobile.DBL.Models
{

    public class ApiRequestData
    {
        //[JsonProperty("tsp")]
        //public string Timestamp { get; set; }

        [JsonProperty("ver")]
        public int Version { get; set; }

        [JsonProperty("action")]
        public int Action { get; set; }

        [JsonProperty("content")]
        public dynamic Content { get; set; }


        //[JsonProperty("rdata")]
        //public ApiRequestInternalData ReservedData { get; set; }
    }

    public class ApiRequestInternalData
    {
        [JsonProperty("cft")]
        public int ContentDataFormat { get; set; }

        [JsonProperty("ucode")]
        public int UserCode { get; set; }

        [JsonProperty("keys")]
        public string[] SecurityKeys { get; set; }

        [JsonProperty("surl")]
        public string ServiceUrl { get; set; }

        [JsonProperty("suser")]
        public string ServiceUser { get; set; }

        [JsonProperty("spwd")]
        public string ServicePass { get; set; }

        [JsonProperty("sreq")]
        public string ServiceRequest { get; set; }

        [JsonProperty("sucode")]
        public string ServiceUserCode { get; set; }
        [JsonProperty("sname")]
        public string ServiceName { get; set; }
    }

    public class ApiResponseData
    {
        [JsonProperty("stat")]
        public int RespStatus { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("msg_code")]
        public string MessageCode { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }

    public class QueryDataItemList
    {
        [JsonProperty("items")]
        public ListModel[] DataItems { get; set; }
    }

    public class DataItemModel
    {
        [JsonProperty("name")]
        public string ItemName { get; set; }

        [JsonProperty("code")]
        public string ItemCode { get; set; }

        [JsonProperty("ext")]
        public string Extra { get; set; }
    }

    public class AgentCashWithTxnModel
    {
        [JsonProperty("refno")]
        public string RefNo { get; set; }

        [JsonProperty("cname")]
        public string CustName { get; set; }

        [JsonProperty("accno")]
        public string AccountNo { get; set; }

        [JsonProperty("pno")]
        public string PhoneNo { get; set; }

        [JsonProperty("amt")]
        public decimal Amount { get; set; }
    }

    public class AgentCashWithQueueModel
    {
        [JsonProperty("txns")]
        public AgentCashWithTxnModel[] Transactions { get; set; }
    }

}
