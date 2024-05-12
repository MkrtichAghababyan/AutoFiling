using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AutoFiling
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private string log { get; set; }
        private string url = "https://newsextractor20240506222851.azurewebsites.net/GetDataFromWeb";
        private Info info = new();
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string html = string.Empty;
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    html = await response.Content.ReadAsStringAsync();
                    info = JsonConvert.DeserializeObject<Info>(html);
                    if (info.Status == true)
                    {
                        var client = new HttpClient();
                        client.BaseAddress = new Uri("https://newsextractor20240506222851.azurewebsites.net/SendData");

                        var json = System.Text.Json.JsonSerializer.Serialize(info);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        response = client.PostAsync("/SendData", content).Result;
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = response.Content.ReadAsStringAsync().Result;

                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            };

                            var postResponse = System.Text.Json.JsonSerializer.Deserialize<Message>(responseContent, options);
                            Console.WriteLine("Post successful! ID: " + postResponse.Status);
                        }
                        else
                        {
                            Console.WriteLine("Error: " + response.StatusCode);
                        }
                    }
                }
                else
                {
                    await Console.Out.WriteLineAsync("Info Status False");
                }
            }

            _logger.LogInformation($"{log}: {DateTimeOffset.Now}");
            await Task.Delay(600000, stoppingToken); 
        }
    }
}

