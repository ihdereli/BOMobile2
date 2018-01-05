using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BOMobile2.Services.Schema
{
    [DataContract]
    public class MiddlewareServiceRequest 
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
    
    [DataContract]
    public class MobileRequestData<T>
    {
        [DataMember(Name = "lang")]
        public string lang { get; set; }

        [DataMember(Name = "token")]
        public string token { get; set; }

        [DataMember(Name = "data")]
        public T data { get; set; }
    }

    [DataContract(Name = "MemberBalances", Namespace = "Frontend")]
    public class MemberBalancesRequest
    {
        [DataMember(Name = "CurrencyId")]
        public string CurrencyId { get; set; }
    }

    [DataContract(Name = "MemberInfo", Namespace = "Frontend")]
    public class MemberInfoRequest
    {

    }

    [DataContract(Name = "GetCountries", Namespace = "Global")]
    public class GetCountriesRequest
    {

    }

    [DataContract(Name = "GetCities", Namespace = "Global")]
    public class GetCitiesRequest
    {
        [DataMember(Name = "CountryId")]
        public string CountryId { get; set; }
    }

    [DataContract(Name = "MemberLogin", Namespace = "Frontend")]
    public class MemberLoginRequest
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }
    }

    [DataContract(Name = "MemberRegister", Namespace = "Frontend")]
    public class MemberRegisterRequest
    {
        [DataMember(Name = "Language")]
        public string Language { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Surname")]
        public string Surname { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "Gsm")]
        public string Gsm { get; set; }

        [DataMember(Name = "Password")]
        public string Password { get; set; }

        [DataMember(Name = "IsActive")]
        public bool IsActive { get; set; }

        [DataMember(Name = "Currency")]
        public string Currency { get; set; }

        [DataMember(Name = "LimitGroup")]
        public int LimitGroup { get; set; }

        [DataMember(Name = "Country")]
        public string Country { get; set; }

        [DataMember(Name = "CountryCodeTwoLetter")]
        public string CountryCodeTwoLetter { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "EmailVerified")]
        public bool EmailVerified { get; set; }

        [DataMember(Name = "IsSystem")]
        public bool IsSystem { get; set; }
    }

    [DataContract(Name = "MemberRegisterFirst", Namespace = "Frontend")]
    public class MemberRegisterFirstRequest
    {
        [DataMember(Name = "Language")]
        public string Language { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Surname")]
        public string Surname { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "Gsm")]
        public string Gsm { get; set; }

        [DataMember(Name = "Password")]
        public string Password { get; set; }

        [DataMember(Name = "Currency")]
        public string Currency { get; set; }

        [DataMember(Name = "Country")]
        public string Country { get; set; }

        [DataMember(Name = "CountryCodeTwoLetter")]
        public string CountryCodeTwoLetter { get; set; }
    }

    [DataContract(Name = "MemberRegisterVerification", Namespace = "Frontend")]
    public class MemberRegisterVerificationRequest
    {
        [DataMember(Name = "MemberId")]
        public int MemberId { get; set; }

        [DataMember(Name = "Code")]
        public string Code { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }
    }

    [DataContract(Name = "MemberRegisterSendVerificationSMS", Namespace = "Frontend")]
    public class MemberRegisterSendVerificationSMSRequest
    {
        [DataMember(Name = "MemberId")]
        public int MemberId { get; set; }

        [DataMember(Name = "GSM")]
        public string GSM { get; set; }
    }

    [DataContract(Name = "MemberRegisterFinish", Namespace = "Frontend")]
    public class MemberRegisterFinishRequest
    {
        [DataMember(Name = "MemberId")]
        public int MemberId { get; set; }

        [DataMember(Name = "IdentityNumber")]
        public string IdentityNumber { get; set; }

        [DataMember(Name = "BirthDate")]
        public DateTime BirthDate { get; set; }

        [DataMember(Name = "Country")]
        public string Country { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }
    }

    [DataContract(Name = "MemberSendActivationCode", Namespace = "Frontend")]
    public class MemberSendActivationRequest
    {
        [DataMember(Name = "VerificationIdentity")]
        public string VerificationIdentity { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }
    }

    [DataContract(Name = "MemberUpdatePassword", Namespace = "Frontend")]
    public class MemberUpdatePasswordRequest
    {
        [DataMember(Name = "MemberId")]
        public int MemberId { get; set; }

        [DataMember(Name = "Password")]
        public string Password { get; set; }
    }

    [DataContract(Name = "GetAllLanguages", Namespace = "Global")]
    public class GetLanguagesRequest
    {
        [DataMember(Name = "Id")]
        public string Id { get; set; }

        [DataMember(Name = "IsActive")]
        public bool IsActive { get; set; }
    }

    [DataContract(Name = "GetTextTranslationsByLang", Namespace = "Global")]
    public class GetTextTranslationsByLang
    {
        [DataMember(Name = "Language")]
        public string Language { get; set; }
    }
}
