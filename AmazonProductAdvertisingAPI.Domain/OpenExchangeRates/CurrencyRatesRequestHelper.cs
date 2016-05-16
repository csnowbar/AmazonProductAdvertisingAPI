using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using AmazonProductAdvertisingAPI.Domain.Concrete;
namespace AmazonProductAdvertisingAPI.Domain.OpenExchangeRates
{
    public class CurrencyRatesRequestHelper
    {
        private const string url = "https://openexchangerates.org/api/latest.json?app_id=9a62ab2de4244281b1fcfeae45d33cad";

        private static T _download_serialized_json_data<T>(string url) where T : new()
        {
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                // if string with JSON data is not empty, deserialize it to class and return its instance 
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
            }
        }

        public CurrencyRates getCurrencyRates
        {
            get { return _download_serialized_json_data<CurrencyRates>(url); }
        }

    }
}
