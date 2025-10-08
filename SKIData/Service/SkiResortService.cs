using Newtonsoft.Json;
using SKIData.Model;
using System.Runtime.CompilerServices;
using HtmlAgilityPack;
using NuGet.Protocol.Plugins;

namespace SKIData.Service
{
    public class SkiResortService
    {
        private readonly string skiResortLisUrl = "";//snowpak
        private readonly string userAgent = "My C# ski resort scrapper";
        private readonly int delaySeconds = 2;

        public async Task<List<SkiResort>> GetResortsAsync()
        {
            List<SkiResort> resorts = new List<SkiResort>();

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
                HttpResponseMessage response = await client.GetAsync(skiResortLisUrl);
                response.EnsureSuccessStatusCode();

                string htmlContent = await response.Content.ReadAsStringAsync();
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                //adjust XPath query based on actual site structure
                HtmlNodeCollection resortNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'resort-item')]");
                if (resortNodes != null)
                {
                    foreach (HtmlNode resortNode in resortNodes)
                    {
                        try
                        {
                            string name = "N/A";
                            string website = "N/A";

                            //Extract name
                            HtmlNode nameNode = resortNode.SelectSingleNode(".//h2[@class='resort-name']");
                            name = nameNode?.InnerText.Trim() ?? "N/A";

                            // Find the link to the resort's website
                            HtmlNode websiteNode = resortNode.SelectSingleNode(".//a[@class='resort-website']"); // Example: Look for an <a> tag with a class
                            website = websiteNode?.GetAttributeValue("href", "") ?? "N/A";  // Extract the href attribute (URL)

                            resorts.Add(new SkiResort { Name = name, WebsiteUrl = website });

                            await Task.Delay(TimeSpan.FromSeconds(delaySeconds)); // Respectful delay
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

            return resorts;
        }
    }
}
