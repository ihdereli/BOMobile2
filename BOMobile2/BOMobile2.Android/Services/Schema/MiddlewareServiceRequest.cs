using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BOMobile.Services.Schema
{
    [Serializable]
    [Route("/CRequest")]
    [DataContract]
    public class MiddlewareServiceRequest : IReturn<MiddlewareServiceResponse>
    {
        [DataMember(Name = "service")]
        public string service { get; set; }

        [DataMember(Name = "modul")]
        public string modul { get; set; }

        [DataMember(Name = "action")]
        public string action { get; set; }

        [DataMember(Name = "data")]
        public string data { get; set; }
    }

    [Serializable]
    [DataContract]
    public class MobileRequestData<T>
    {
        [DataMember(Name = "lang")]
        public string lang { get; set; }

        [DataMember(Name = "userId")]
        public int userId { get; set; }

        [DataMember(Name = "data")]
        public T data { get; set; }
    }

    [Serializable]
    [DataContract(Name = "GetCities")]
    public class GetCitiesRequest
    {
        [DataMember(Name = "CountryId")]
        public string CountryId { get; set; }
    }
}
