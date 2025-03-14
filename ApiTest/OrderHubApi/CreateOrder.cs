using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataFromDb;
using OrderHubApi.Payloads;

namespace OrderHubApi
{
    public class CreateOrder
    {
        private readonly HttpClient _client;
        private LogEvent _log = new LogEvent("CreateOrder_Log_" + DateTime.Now.ToString("yyyyMMdd"), @"C:\QaTools\logs\WebApiClient\OrderHub\CreateOrder");

        public CreateOrder(string hostname, ApiType apiType = ApiType.Standard)
        {
            var config = ConfigurationManager.GetConfiguration(hostname);

            _client = new HttpClient();
            TokenHandling token = new TokenHandling();
            // Set up the headers
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.GetAccessToken(hostname, apiType)}");
            _client.DefaultRequestHeaders.Add("accept", "application/json");
            _client.DefaultRequestHeaders.Add("Host", $"{new Uri(config.UrlPostRequest).Host}");
            if ( apiType == ApiType.Rx )
                _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{config.SubKeyRx}");
            else
                _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{config.SubKey}");
        }

        public async Task<HttpResponseMessage> SendPostRequest(
            string pharmacyGln, 
            List<Customer> customerList, 
            string hostname, 
            OrderType orderType = OrderType.NoPrescription)
        {
            _log.WriteLog($"SendPostRequest - OrderType : {orderType}");
            var config = ConfigurationManager.GetConfiguration(hostname);
            _log.WriteLog($"hostname : {hostname}");
            string payload = string.Empty;

            //Need to service to get current GLN
            if (orderType == OrderType.NoPrescription)
            {
                _log.WriteLog("Generate payload OrderType.NoPrescription");
                payload = OrderNoPrescription.GetPaylaod(pharmacyGln, customerList);
            }
            else if (orderType == OrderType.WithPrescription)
            {
                _log.WriteLog("Generate payload OrderType.WithPrescription");
                //TODO: Implement payload generation for OrderType.WithPrescription
            }
            else if (orderType == OrderType.WithRepetition)
            {
                _log.WriteLog("Generate payload OrderType.WithRepetition");
                payload = OrderWithRepetition.GetPaylaod(pharmacyGln, customerList);
            }

            _log.WriteLog($"pharmacyGln : {pharmacyGln}");
            _log.WriteLog( $"customer first name : {customerList[0].FirstName}");
            _log.WriteLog($"customer last name : {customerList[0].LastName}");
            _log.WriteLog($"payload : {payload}");


            // Convert the payload to StringContent and set the content type
            var content = new StringContent(payload, Encoding.UTF8, "text/plain");

            // Send the POST request and return the response
            HttpResponseMessage response = await _client.PostAsync(config.UrlPostRequest, content);
            _log.WriteLog( $"response status code : {response.StatusCode}");
            _log.WriteLog($"response headers : {response.Headers}");
            _log.WriteLog( $"response : {response}");

            return response;
        }

        //enum OrderType

        public enum OrderType
        {
            NoPrescription,
            WithPrescription,
            WithRepetition
        }

        //enum ApiType
        public enum ApiType
        { 
            Standard,
            Rx
        }
    }
}
