using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ServiceChecker : IDisposable
{
    private readonly HttpClient _httpClient;

    public ServiceChecker()
    {
        var handler = new HttpClientHandler
        {
            //ignore ssl issue
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        _httpClient = new HttpClient(handler);
    }

    public async Task<string> IsHttpServiceAvailable(string url, string login, string password)
    {
        try
        {
            // If login is provided, add Basic Authentication header
            if (!string.IsNullOrEmpty(login))
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{login}:{password}"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            }

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            return response.StatusCode.ToString();
        }
        catch
        {
            return "N/A";
        }
        finally
        {
            // Clear the Authorization header after the request to avoid reuse in subsequent calls
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    public bool IsTcpServiceAvailable(string host, int port)
    {
        try
        {
            using (var tcpClient = new TcpClient())
            {
                tcpClient.Connect(host, port);
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
