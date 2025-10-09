using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using SKIData.Data;
using SKIData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SKIData.Services
{
    public class SkiResortService
    {
        private readonly string _skiResortListUrl = "https://www.colorado.com/articles/colorado-ski-resorts-snows-perfect-state"; // Replace with your URL
        private readonly string _userAgent = "My C# Ski Resort Scraper";
        private readonly int _delaySeconds = 2;
        private readonly SkiResortContext _context;

        public SkiResortService(SkiResortContext context)
        {
            _context = context;
        }

        public async Task ScrapeAndSaveSkiResortsAsync()
        {
            List<SkiResort> scrapedResorts = await ScrapeSkiResortDataAsync();

            if (scrapedResorts != null && scrapedResorts.Any())
            {
                Console.WriteLine($"Successfully scraped {scrapedResorts.Count} resorts."); // Debugging

                try
                {
                    foreach (var resort in scrapedResorts)
                    {
                        if (!_context.SkiResorts.Any(r => r.Name == resort.Name))
                        {
                            _context.SkiResorts.Add(resort);
                            Console.WriteLine($"Adding resort to database: {resort.Name}"); // Debugging
                        }
                        else
                        {
                            Console.WriteLine($"Resort already exists in database: {resort.Name}"); // Debugging
                        }
                    }

                    await _context.SaveChangesAsync();
                    Console.WriteLine("Ski resorts scraped and saved to the database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving to database: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No data scraped or an error occurred during scraping.");
            }
        }

        private async Task<List<SkiResort>> ScrapeSkiResortDataAsync()
        {
            List<SkiResort> skiResorts = new List<SkiResort>();

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);

                HttpResponseMessage response = await client.GetAsync(_skiResortListUrl);
                response.EnsureSuccessStatusCode();

                string htmlContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"HTML content length: {htmlContent.Length}"); // Debugging

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                //Adjust XPath queries to match structure of website
                HtmlNodeCollection resortNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='field-item even']/p");

                if (resortNodes != null)
                {
                    Console.WriteLine($"Found {resortNodes.Count} resort nodes."); // Debugging

                    foreach (HtmlNode resortNode in resortNodes)
                    {
                        try
                        {
                            string name = "N/A";
                            string description = "N/A";

                            //Extract name
                            HtmlNode nameNode = resortNode.SelectSingleNode(".//strong");
                            name = nameNode?.InnerText.Trim() ?? "N/A";

                            // Get the description
                            description = "";
                            HtmlNodeCollection descriptionNodes = resortNode.SelectNodes("./text()");

                            if (descriptionNodes != null && descriptionNodes.Count > 1)
                            {
                                description = descriptionNodes[1].InnerText.Trim();
                            }
                            else
                            {
                                description = "N/A";
                            }

                            Console.WriteLine($"Extracted Name: {name}, Description: {description}"); // Debugging

                            skiResorts.Add(new SkiResort { Name = name, Description = description });

                            await Task.Delay(TimeSpan.FromSeconds(_delaySeconds)); // Respectful delay
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error extracting data from resort node: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No resort items found on the page.");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

            return skiResorts;
        }
    }
}
