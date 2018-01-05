using BOMobile2.Services.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BOMobile2.Services
{

    public class MobileDataService
    {
        private static string mwurl = "http://169.254.138.176:9100/CRequest?format=json&";

        public async Task<MobileResponseData<T1>> Post<T1, T2>(T2 data)
        {
            MobileResponseData<T1> responseObj = new MobileResponseData<T1>();

            try
            {
                HttpClient client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;

                MobileRequestData<T2> requestData = new MobileRequestData<T2>();
                requestData.lang = Global.Language;
                requestData.token = Global.MemberInfo.Token;
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
            }
            catch (Exception ex)
            {
                ex.ToString();

                responseObj.errorCode = ex.HResult.ToString();
                responseObj.errorDefiniton = ex.Message;
                responseObj.responseStatus = "ERROR";
            }

            return responseObj;
        }
    }
}
