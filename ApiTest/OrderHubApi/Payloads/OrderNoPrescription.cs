using DataFromDb;
using System;
using System.Collections.Generic;
using OrderHubApi.Payloads.Helpers;

namespace OrderHubApi.Payloads
{
    public class OrderNoPrescription
    {
        public static string GetPaylaod(string pharmacyGln, List<Customer> customerList) 
        {
            //Generate OrderID
            string sourceOrderId = DataGen.GenSourceOrderId();

            var payload = $@"
                {{
                  ""sourceOrderId"": ""{sourceOrderId}"",
                  ""sourceSystemCreationDate"": ""{DataGen.GetNHoursBehind(5).ToString("yyyy-MM-ddTHH:mm:ssZ")}"",
                  ""orderType"": ""order"",
                  ""status"": ""created"",
                  ""dispensingPharmacyGln"": null,
                  ""customer"": {{
                    ""guest"": true,
                    ""microserviceUuid"": null,
                    ""triaPharmGuid"": null,
                    ""firstName"": ""{customerList[0].FirstName}"",
                    ""lastName"": ""{customerList[0].LastName}"",
                    ""contactPoints"": [
                      {{
                        ""system"": ""phone_number"",
                        ""priority"": 0,
                        ""value"": ""+410748563256""
                      }},
                      {{
                        ""system"": ""email"",
                        ""priority"": 1,
                        ""value"": ""test123@glanica123.com""
                      }}
                    ],
                    ""language"": ""fr"",
                    ""insuranceCardNumber"": null
                  }},
                  ""customerOnBehalf"": null,
                  ""addresses"": [
                    {{
                      ""type"": ""billing"",
                      ""sourceSystemAddressId"": ""string"",
                      ""firstName"": ""{customerList[0].FirstName}"",
                      ""lastName"": ""{customerList[0].LastName}"",
                      ""companyName"": null,
                      ""line1"": ""{customerList[0].Address1}"",
                      ""line2"": ""{customerList[0].Address2}"",
                      ""line3"": ""string"",
                      ""city"": ""{customerList[0].City}"",
                      ""postalCode"": ""{customerList[0].ZipCode}"",
                      ""state"": ""/"",
                      ""country"": ""CH"",
                      ""note"": ""string""
                    }},
                    {{
                      ""type"": ""shipping"",
                      ""sourceSystemAddressId"": ""string"",
                      ""firstName"": ""{customerList[0].FirstName}"",
                      ""lastName"": ""{customerList[0].LastName}"",
                      ""companyName"": null,
                      ""line1"": ""{customerList[0].Address1}"",
                      ""line2"": ""{customerList[0].Address2}"",
                      ""line3"": ""string"",
                      ""city"": ""{customerList[0].City}"",
                      ""postalCode"": ""{customerList[0].ZipCode}"",
                      ""state"": ""/"",
                      ""country"": ""CH"",
                      ""note"": ""string""
                    }}
                  ],
                  ""orderLines"": [
                    {{
                      ""itemNumber"": 1,
                      ""sourceSystemId"": ""{sourceOrderId}-0"",
                      ""type"": ""rayon"",
                      ""status"": ""created"",
                      ""pharmaCode"": ""58985"",
                      ""quantity"": 2,
                      ""paymentStatus"": false,
                      ""paymentTimestamp"": null,
                      ""baseSalesPrice"": 15.0,
                      ""actionPrice"": 13.99,
                      ""finalAmount"": 14,
                      ""vatRate"": 8.1,
                      ""priceModifiers"": []
                    }},
                    {{
                      ""itemNumber"": 2,
                      ""sourceSystemId"": ""{sourceOrderId}-1"",
                      ""type"": ""regular"",
                      ""status"": ""created"",
                      ""pharmaCode"": ""7412094"",
                      ""quantity"": 1,
                      ""paymentStatus"": false,
                      ""paymentTimestamp"": null,
                      ""baseSalesPrice"": 5.0,
                      ""actionPrice"": 0.0,
                      ""finalAmount"": 2.5,
                      ""vatRate"": 8.1,
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
                      ""provider"": ""pharmacy"",
                      ""shipmentType"": ""click_and_collect"",
                      ""pharmacyGln"": ""{pharmacyGln}"",
                      ""status"": ""created"",
                      ""shipmentCode"": null,
                      ""shipmentExpectedDateTime"": null,
                      ""shipmentDateTime"": null,
                      ""items"": [
                        {{
                          ""orderLinesItemNumber"": 1,
                          ""type"": ""rayon"",
                          ""sourceSystemId"": ""{sourceOrderId}-0"",
                          ""pharmaCode"": ""58985"",
                          ""status"": ""created"",
                          ""quantity"": 2,
                          ""paymentStatus"": false,
                          ""paymentTimestamp"": null,
                          ""baseSalesPrice"": 15.0,
                          ""actionPrice"": 13.99,
                          ""finalAmount"": 14,
                          ""priceModifiers"": []
                        }},
                        {{
                          ""orderLinesItemNumber"": 2,
                          ""sourceSystemId"": ""{sourceOrderId}-1"",
                          ""pharmaCode"": ""7412094"",
                          ""status"": ""created"",
                          ""quantity"": 1,
                          ""paymentStatus"": false,
                          ""paymentTimestamp"": null,
                          ""baseSalesPrice"": 5.0,
                          ""actionPrice"": 0.0,
                          ""finalAmount"": 2.5,
                          ""priceModifiers"": []
                        }}
                      ],
                      ""loyalty"": null
                    }}
                  ],
                  ""amounts"": {{
                    ""total"": 23.45,
                    ""totalTax"": 1.76
                  }}
                }}";

            return JsonFormatter.FlattenJson(payload);
            //return payload;
        }
    }
}
