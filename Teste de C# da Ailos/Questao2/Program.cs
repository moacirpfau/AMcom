using System.Text.Json;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year).GetAwaiter().GetResult();

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year).GetAwaiter().GetResult();

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        int page = 1;
        bool hasMorePages = true;

        using (HttpClient client = new HttpClient())
        {
            // Loop para pegar os gols como team1
            while (hasMorePages)
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={page}";
                HttpResponseMessage response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var matchData = JsonSerializer.Deserialize<MatchResponse>(content);

                if (matchData?.data == null || matchData.data.Count == 0)
                {
                    break;
                }

                totalGoals += matchData.data.Sum(x => int.Parse(x.team1goals));
                page++;

                hasMorePages = page <= matchData.total_pages;
            }

            // Resetar a página e buscar como team2
            page = 1;
            hasMorePages = true;

            while (hasMorePages)
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={page}";
                HttpResponseMessage response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var matchData = JsonSerializer.Deserialize<MatchResponse>(content);

                if (matchData?.data == null || matchData.data.Count == 0)
                {
                    break;
                }

                totalGoals += matchData.data.Sum(m => int.Parse(m.team2goals));
                page++;

                hasMorePages = page <= matchData.total_pages;
            }
        }

        return totalGoals;
    }
}

public class MatchResponse
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<Match>? data { get; set; }
}

public class Match
{
    public string? team1 { get; set; }
    public string? team2 { get; set; }
    public string? team1goals { get; set; }
    public string? team2goals { get; set; }
}