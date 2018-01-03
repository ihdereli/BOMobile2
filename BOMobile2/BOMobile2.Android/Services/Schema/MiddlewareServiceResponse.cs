using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BOMobile.Services.Schema
{
    [DataContract]
    public class MiddlewareServiceResponse
    {
        [DataMember(Name = "response")]
        public string response { get; set; }
    }

    [DataContract]
    public class MobileResponseData<T>
    {
        [DataMember(Name = "data")]
        public T data { get; set; }

        [DataMember(Name = "responseStatus")]
        public string responseStatus { get; set; }

        [DataMember(Name = "errorCode")]
        public string errorCode { get; set; }

        [DataMember(Name = "errorDefiniton")]
        public string errorDefiniton { get; set; }
    }

    [Serializable]
    [DataContract]
    public class City
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "CountryId")]
        public string CountryId { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }
    }
}
