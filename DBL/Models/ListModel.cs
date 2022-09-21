using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Models
{
    public class ListModel
    {
        public ListModel() { }

        public ListModel(string text, string value)
        {
            Text = text;
            Value = value;
        }

        [JsonProperty("itxt")]
        public string Text { get; set; }

        [JsonProperty("ival")]
        public string Value { get; set; }
    }
}
