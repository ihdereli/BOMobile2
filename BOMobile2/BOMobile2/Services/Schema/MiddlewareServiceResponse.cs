﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BOMobile2.Services.Schema
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

    [DataContract]
    public class Currency
    {
        [DataMember(Name = "Id")]
        public string Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Symbol")]
        public string Symbol { get; set; }

        [DataMember(Name = "DecimalPlace")]
        public int DecimalPlace { get; set; }

        [DataMember(Name = "CurrencyRate")]
        public decimal CurrencyRate { get; set; }
    }

    [DataContract]
    public class Bank
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Currency")]
        public string Currency { get; set; }

        [DataMember(Name = "AccountNo")]
        public string AccountNo { get; set; }

        public string AccountName
        {
            get
            {
                return Name + " - " + AccountNo;
            }
        }
    }

    [DataContract]
    public class MemberBalance
    {
        [DataMember(Name = "CurrencyId")]
        public string CurrencyId { get; set; }

        [DataMember(Name = "CurrencyName")]
        public string CurrencyName { get; set; }

        [DataMember(Name = "CurrencySymbol")]
        public string CurrencySymbol { get; set; }

        [DataMember(Name = "CurrencyRate")]
        public decimal CurrencyRate { get; set; }

        [DataMember(Name = "IsCoin")]
        public bool IsCoin { get; set; }

        [DataMember(Name = "Balance")]
        public decimal Balance { get; set; }
        public string BalanceText
        {
            get
            {
                return String.Format("{0:N" + (IsCoin ? "8" : "2") + "}", Balance);
            }
        }
    }

    [DataContract]
    public class DocumentConfirmation
    {
        [DataMember(Name = "Type")]
        public int Type { get; set; }

        [DataMember(Name = "TypeName")]
        public string TypeName { get; set; }

        [DataMember(Name = "Id")]
        public long Id { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }

        [DataMember(Name = "StatusText")]
        public string StatusText { get; set; }

        public string BGColor
        {
            get
            {
                if (Status == 3)
                    return "Green";
                else if (Status == 1)
                    return "Goldenrod";
                else if (Status == 2)
                    return "LightCoral";
                else
                    return "Chocolate";
            }
        }

        public bool Open
        {
            get
            {
                return Status == 0 || Status == 2;
            }
        }
    }
    

   [DataContract]
    public class MemberLoginInfo
    {
        [DataMember(Name = "Token")]
        public string Token { get; set; }

        [DataMember(Name = "Id")]
        public int? Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Surname")]
        public string Surname { get; set; }

        [DataMember(Name = "EMail")]
        public string EMail { get; set; }

        [DataMember(Name = "Gsm")]
        public string Gsm { get; set; }

        [DataMember(Name = "Language")]
        public string Language { get; set; }

        [DataMember(Name = "Currency")]
        public string Currency { get; set; }

        [DataMember(Name = "Country")]
        public string Country { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "LimitGroup")]
        public int LimitGroup { get; set; }

        [DataMember(Name = "IsActive")]
        public bool IsActive { get; set; }

        [DataMember(Name = "EmailVerified")]
        public bool EmailVerified { get; set; }

        [DataMember(Name = "GsmVerified")]
        public bool GsmVerified { get; set; }
    }

    [DataContract]
    public class Languages
    {
        [DataMember(Name = "Id")]
        public string Id { get; set; }

        [DataMember(Name = "Language")]
        public string Language { get; set; }

        [DataMember(Name = "Flag")]
        public string Flag { get; set; }

        [DataMember(Name = "IsActive")]
        public bool IsActive { get; set; }
    }

    [DataContract]
    public class Country
    {
        [DataMember(Name = "Id")]
        public string Id { get; set; }

        [DataMember(Name = "IsoCode")]
        public string IsoCode { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "NiceName")]
        public string NiceName { get; set; }

        [DataMember(Name = "PhoneCode")]
        public int PhoneCode { get; set; }

        [DataMember(Name = "Ordering")]
        public int Ordering { get; set; }
        public string PhoneCodeText
        {
            get
            {
                return Name + "( +" + PhoneCode + " )";
            }
        }
    }

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

    [DataContract]
    public class SendCoinOperation
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Address")]
        public string Address { get; set; }

        [DataMember(Name = "Amount")]
        public decimal Amount { get; set; }

        [DataMember(Name = "RequestDate")]
        public DateTime RequestDate { get; set; }

        [DataMember(Name = "ConfirmedDate")]
        public DateTime ConfirmedDate { get; set; }

        [DataMember(Name = "CompletedDate")]
        public DateTime CompletedDate { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }

        [DataMember(Name = "StatusText")]
        public string StatusText { get; set; }

        [DataMember(Name = "NoteText")]
        public string NoteText { get; set; }
    }
}
