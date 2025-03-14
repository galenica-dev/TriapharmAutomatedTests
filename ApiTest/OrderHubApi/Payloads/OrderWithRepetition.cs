using DataFromDb;
using OrderHubApi.Payloads.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHubApi.Payloads
{
    public class OrderWithRepetition
    {
        public static string GetPaylaod(string pharmacyGln, List<Customer> customerList, string ouCode = "SUN007") 
        {
            //Generate OrderID
            string sourceOrderId = DataGen.GenSourceOrderId(); 

            //This will work only with SUN brand in N+2 (SUN007 and SUN008)
            var payload = $@"
                {{
                    ""prescription"": {{
                        ""genericReplacement"": true,
                        ""ouCode"": ""{ouCode}""
                    }},
                    ""sourceOrderId"": ""{sourceOrderId}"",
                    ""sourceSystemCreationDate"": ""{DataGen.GetNHoursBehind(5).ToString("yyyy-MM-ddTHH:mm:ssZ")}"",
                    ""orderType"": ""order"",
                    ""status"": ""created"",
                    ""dispensingPharmacyGln"": null,
                    ""customer"": {{
                        ""guest"": false,
                        ""microserviceUuid"": ""a37cc619cfb3b497e652ad13525de411c1547a5373d0c94ba0b2508ca804473d"",
                        ""triaPharmGuid"": ""9cff48e0-7d65-4955-bc2a-627eb5ea1961"",
                        ""firstName"": ""Test Samy"",
                        ""lastName"": ""BLAEUER"",
                        ""contactPoints"": [
                            {{
                                ""system"": ""email"",
                                ""priority"": 1,
                                ""value"": ""samy.kacem@galenica.com""
                            }},
                            {{
                                ""system"": ""phone_number"",
                                ""priority"": 2,
                                ""value"": ""+41791767145""
                            }}
                        ],
                        ""language"": ""fr"",
                        ""insuranceCardNumber"": null
                    }},
                    ""addresses"": [
                        {{
                            ""type"": ""shipping"",
                            ""sourceSystemAddressId"": ""421"",
                            ""firstName"": ""Test Samy"",
                            ""lastName"": ""BLAEUER"",
                            ""companyName"": null,
                            ""line1"": ""Avenue Adrien-Lachenal 56"",
                            ""line2"": null,
                            ""line3"": null,
                            ""city"": ""VERSOIX"",
                            ""postalCode"": ""1290"",
                            ""state"": """",
                            ""country"": ""CH"",
                            ""note"": """"
                        }},
                        {{
                            ""type"": ""billing"",
                            ""sourceSystemAddressId"": ""421"",
                            ""firstName"": ""Test Samy"",
                            ""lastName"": ""BLAEUER"",
                            ""companyName"": null,
                            ""line1"": ""Avenue Adrien-Lachenal 56"",
                            ""line2"": null,
                            ""line3"": null,
                            ""city"": ""VERSOIX"",
                            ""postalCode"": ""1290"",
                            ""state"": """",
                            ""country"": ""CH"",
                            ""note"": """"
                        }}
                    ],
                    ""orderLines"": [
                        {{
                            ""itemNumber"": 1,
                            ""sourceSystemId"": ""{sourceOrderId}-0"",
                            ""type"": ""regular"",
                            ""status"": ""processing"",
                            ""pharmaCode"": ""2391909"",
                            ""quantity"": 1,
                            ""paymentStatus"": false,
                            ""paymentTimestamp"": null,
                            ""baseSalesPrice"": 0.0,
                            ""actionPrice"": 0.0,
                            ""finalAmount"": 0.0,
                            ""vatRate"": 0.0,
                            ""priceModifiers"": []
                        }}
                    ],
                    ""fees"": [
                        {{
                            ""type"": ""other"",
                            ""description"": ""Other fee"",
                            ""amount"": 6.95,
                            ""vatRate"": 8.1
                        }}
                    ],
                    ""fulfillment"": [
                        {{
                            ""sourceSystemId"": null,
                            ""provider"": ""pharmacy"",
                            ""status"": ""created"",
                            ""shipmentType"": ""click_and_collect"",
                            ""pharmacyGln"": ""{pharmacyGln}"",
                            ""shipmentCode"": null,
                            ""shipmentExpectedDateTime"": null,
                            ""shipmentDateTime"": null,
                            ""items"": [
                                {{
                                    ""orderLinesItemNumber"": 1,
                                    ""sourceSystemId"": ""{sourceOrderId}-0"",
                                    ""pharmaCode"": ""2391909"",
                                    ""status"": ""created"",
                                    ""quantity"": 1,
                                    ""paymentStatus"": false,
                                    ""paymentTimestamp"": null,
                                    ""baseSalesPrice"": null,
                                    ""actionPrice"": null,
                                    ""finalAmount"": null,
                                    ""priceModifiers"": [],
                                    ""type"": ""regular"",
                                    ""description"": ""SPEDIFEN cpr pell 400 mg 12 pce"",
                                    ""shipmentCode"": null,
                                    ""shipmentDateTime"": null,
                                    ""prescriptionRepetitionId"": ""bdb1d25c-3abb-425c-9d7e-04cc9d68cd25""
                                }}
                            ],
                            ""loyalty"": null
                        }}
                    ],
                    ""amounts"": {{
                        ""total"": 23.45,
                        ""totalTax"": 1.76
                    }}
                }}

            ";

            return JsonFormatter.FlattenJson(payload);
        }
    }
}
