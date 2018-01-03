using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Collections.Generic;
using BOMobile2.Services.Schema;
using System.Reflection;
using System.Threading.Tasks;

namespace BOMobile2.Services
{
    public delegate void DataEventHandler<T, U>(T data, U eventArgs);

    public class MobileService<T1, T2> 
    {
        public class DataEventArgs : EventArgs { }
        public event DataEventHandler<MobileResponseData<T1>, DataEventArgs> OnData;

        private static string mwurl = "http://169.254.138.176:9100/CRequest?format=json&";
        private readonly HttpClient client;
        public string Language { get; set; }
        public int UserId { get; set; }

        public MobileService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async void Post(T2 data)
        {
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            MobileRequestData<T2> requestData = new MobileRequestData<T2>();
            requestData.lang = Language;
            requestData.memberId = UserId;
            requestData.data = data;

            var jsonData = JsonConvert.SerializeObject(requestData);

            var dcAttribute = typeof(T2).GetTypeInfo().GetCustomAttribute(typeof(DataContractAttribute), true) as DataContractAttribute;

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("service", "CoinServer"));
            postData.Add(new KeyValuePair<string, string>("modul", dcAttribute.Namespace));
            postData.Add(new KeyValuePair<string, string>("action", dcAttribute.Name));
            postData.Add(new KeyValuePair<string, string>("data", jsonData));

            var content = new FormUrlEncodedContent(postData);

            var response = await client.PostAsync(mwurl, content);

            var jsonResult = response.Content.ReadAsStringAsync().Result;

            var mwResponse = JsonConvert.DeserializeObject<MiddlewareServiceResponse>(jsonResult);

            MobileResponseData<T1> responseObj = new MobileResponseData<T1>();

            if (mwResponse.response != null)
            {
                responseObj = JsonConvert.DeserializeObject<MobileResponseData<T1>>(mwResponse.response);
            }
            else
            {
                responseObj.errorCode = "9999";
                responseObj.errorDefiniton = "null data";
                responseObj.responseStatus = "ERROR";
            }

            OnData(responseObj, new DataEventArgs());
        }
    }
}
