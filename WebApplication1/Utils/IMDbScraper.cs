using HtmlAgilityPack;
using WebApplication1.Models;

public class IMDbScraper
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public IMDbScraper(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<Actor>> ScrapeActorsAsync()
    {
        var actors = new List<Actor>();

        var imdbUrl = _configuration["IMDb:Url"];
        var imdbHtml = await _httpClient.GetStringAsync(imdbUrl);
        var doc = new HtmlDocument();
        doc.LoadHtml(imdbHtml);

        var actorNodes = doc.DocumentNode.SelectNodes(_configuration["Selectors:ActorNode"]);
        if (actorNodes != null)
        {
            foreach (var node in actorNodes)
            {
                var actor = new Actor();

                var id = node.SelectSingleNode(_configuration["Selectors:IdNode"])?.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(id))
                {
                    actor.Id = id.Split('/')[2];
                    actor.Source = "IMDb";
                    actor.Name = node.SelectSingleNode(_configuration["Selectors:NameNode"])?.InnerText.Trim();
                    actor.Details = node.SelectSingleNode(_configuration["Selectors:DetailsNode"])?.InnerText.Trim();

                    var typeNode = node.SelectSingleNode(_configuration["Selectors:TypeNode"]);
                    if (typeNode != null)
                    {
                        actor.Type = string.Join(", ", typeNode.InnerText.Trim().Split(',').Select(s => s.Trim()));
                    }

                    var rankNode = node.SelectSingleNode(_configuration["Selectors:RankNode"]);
                    if (rankNode != null)
                    {
                        var rankText = rankNode.InnerText.Trim().Replace(".", "");
                        if (int.TryParse(rankText, out int rank))
                        {
                            actor.Rank = rank;
                        }
                    }
                }

                actors.Add(actor);
            }
        }

        return actors;
    }
}
