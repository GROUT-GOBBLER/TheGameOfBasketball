using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;
namespace basketballUI
{
    public partial class MainPage: TabbedPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        async private void Reload_Clicked(object sender, EventArgs e) {
            string API_URL = "http://localhost:5121/api/Players";
            string API_URL2 = "http://localhost:5121/api/TeamPlayers";
            string API_URL3 = "http://localhost:5121/api/Stats";
            string API_URL4 = "http://localhost:5121/api/Teams";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(API_URL);
                    HttpResponseMessage response2 = await client.GetAsync(API_URL2);
                    HttpResponseMessage response3 = await client.GetAsync(API_URL3);
                    HttpResponseMessage response4 = await client.GetAsync(API_URL4);

                    if (response.IsSuccessStatusCode && response2.IsSuccessStatusCode && response3.IsSuccessStatusCode)
                    {
                        List<string> playerStrings = new List<string>();

                        string json = await response.Content.ReadAsStringAsync();
                        string json2 = await response2.Content.ReadAsStringAsync();
                        string json3 = await response3.Content.ReadAsStringAsync();
                        string json4 = await response4.Content.ReadAsStringAsync();

                        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(json);
                        List<TeamPlayer> teamPlayers = JsonConvert.DeserializeObject<List<TeamPlayer>>(json2);
                        List<Stat> stats = JsonConvert.DeserializeObject<List<Stat>>(json3);
                        List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(json4);

                        if (modeSwitch.IsToggled == false)
                        {
                            foreach (Player player in players)
                            {
                                int steals = 0;
                                int turnovers = 0;
                                int assists = 0;
                                int blocks = 0;
                                int fouls = 0;
                                int ofReb = 0;
                                int dfReb = 0;

                                foreach (Stat s in stats)
                                {
                                    TeamPlayer tp = teamPlayers.Find(e => e.PlayerId == player.PlayerNo);

                                    if (tp == null) { }
                                    else if (s.PlayerTeamId.Equals(tp.Id))
                                    {
                                        switch (s.StatTypeId)
                                        {
                                            case 5:
                                                steals = (int)s.StatValue;
                                                break;
                                            case 6:
                                                turnovers = (int)s.StatValue;
                                                break;
                                            case 7:
                                                assists = (int)s.StatValue;
                                                break;
                                            case 8:
                                                blocks = (int)s.StatValue;
                                                break;
                                            case 9:
                                                fouls = (int)s.StatValue;
                                                break;
                                            case 10:
                                                ofReb = (int)s.StatValue;
                                                break;
                                            case 11:
                                                dfReb = (int)s.StatValue;
                                                break;
                                        }
                                    }
                                }

                                playerStrings.Add($"Player #: " + player.PlayerNo + "  Name: " + player.FName + " " + player.LName + "  |  Steals: " + steals + "  Turnovers: " + turnovers + "  Assists: " + assists + "  Blocks: " + blocks + "  Fouls: " + fouls + "  OffensiveRebounds: " + ofReb + "  DefensiveRebounds: " + dfReb);
                            }
                        }

                        else
                        {
                            foreach (Team t in teams)
                            {
                                int steals = 0;
                                int turnovers = 0;
                                int assists = 0;
                                int blocks = 0;
                                int fouls = 0;
                                int ofReb = 0;
                                int dfReb = 0;

                                foreach (Player p in players)
                                {
                                    foreach (Stat s in stats)
                                    {
                                        TeamPlayer tp = teamPlayers.Find(e => e.PlayerId == p.PlayerNo);

                                        if (tp == null) { }
                                        else if (s.PlayerTeamId.Equals(tp.Id) && t.TeamNo.Equals(tp.TeamId))
                                        {
                                            switch (s.StatTypeId)
                                            {
                                                case 5:
                                                    steals += (int)s.StatValue;
                                                    break;
                                                case 6:
                                                    turnovers += (int)s.StatValue;
                                                    break;
                                                case 7:
                                                    assists += (int)s.StatValue;
                                                    break;
                                                case 8:
                                                    blocks += (int)s.StatValue;
                                                    break;
                                                case 9:
                                                    fouls += (int)s.StatValue;
                                                    break;
                                                case 10:
                                                    ofReb += (int)s.StatValue;
                                                    break;
                                                case 11:
                                                    dfReb += (int)s.StatValue;
                                                    break;
                                            }
                                        }

                                    }
                                }

                                playerStrings.Add($"Team #: " + t.TeamNo + "  Name: " + t.TeamName + "  Abbreviation: " + t.TeamAbbreviation + "  |  Wins/Losses: " + t.Wins + "/" + t.Losses + "  Steals: " + steals + "  Turnovers: " + turnovers + "  Assists: " + assists + "  Blocks: " + blocks + "  Fouls: " + fouls + "  OffensiveRebounds: " + ofReb + "  DefensiveRebounds: " + dfReb);
                            }
                        }

                        statsList.ItemsSource = playerStrings;
                    }
                }
                catch (Exception ex)
                {
                    Seaching.Text = $"Clicked and failed";
                }
            }
        }

        private void Button_Clicked (object sender, EventArgs e)
        {
            Navigation.PushAsync(new PlayerSelect());
        }

    }

}
