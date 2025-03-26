using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;
using Windows.Media.Audio;
using Windows.Media.Protection.PlayReady;
using System.Collections.Generic;
using Windows.Media.PlayTo;
namespace basketballUI
{
    public partial class MainPage: TabbedPage
    {
        //http://localhost:5121/api/Schedules
        string URL = $"http://localhost:5121/api";

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += CurrentTime;
        }

        private void CurrentTime(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var timer = new System.Threading.Timer(
               obj =>
               {
                   MainThread.InvokeOnMainThreadAsync(() => { RefreshGameViewStats(); });
                   }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10)
               );
            });
           
                
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
                                if (player.FName.ToUpper() != "NULL" && player.FName != null && player.LName.ToUpper() != "NULL" && player.LName != null)
                                {
                                    String[] query = Searching.Text.ToLower().Split(" ");

                                    if (query.Length == 1 && query[0] != " " && (player.FName.ToLower().Contains(query[0]) || player.LName.ToLower().Contains(query[0]))) playerStrings.Add($"Player #: " + player.PlayerNo + "  Name: " + player.FName + " " + player.LName + "  |  Steals: " + steals + "  Turnovers: " + turnovers + "  Assists: " + assists + "  Blocks: " + blocks + "  Fouls: " + fouls + "  OffensiveRebounds: " + ofReb + "  DefensiveRebounds: " + dfReb);
                                    else if (query.Length == 2 && query[0] != " " && (player.FName.ToLower() == query[0] && player.LName.ToLower() == query[1])) playerStrings.Add($"Player #: " + player.PlayerNo + "  Name: " + player.FName + " " + player.LName + "  |  Steals: " + steals + "  Turnovers: " + turnovers + "  Assists: " + assists + "  Blocks: " + blocks + "  Fouls: " + fouls + "  OffensiveRebounds: " + ofReb + "  DefensiveRebounds: " + dfReb);
                                    else if (Searching.Text.Length == 0) playerStrings.Add($"Player #: " + player.PlayerNo + "  Name: " + player.FName + " " + player.LName + "  |  Steals: " + steals + "  Turnovers: " + turnovers + "  Assists: " + assists + "  Blocks: " + blocks + "  Fouls: " + fouls + "  OffensiveRebounds: " + ofReb + "  DefensiveRebounds: " + dfReb);
                                }
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

                                if (t.TeamName.ToUpper() != "NULL" && t.TeamName != null)
                                {
                                    if (Searching.Text.Length == 0) playerStrings.Add($"Team #: " + t.TeamNo + "  Name: " + t.TeamName + "  Abbreviation: " + t.TeamAbbreviation + "  |  Wins/Losses: " + t.Wins + "/" + t.Losses + "  Steals: " + steals + "  Turnovers: " + turnovers + "  Assists: " + assists + "  Blocks: " + blocks + "  Fouls: " + fouls + "  OffensiveRebounds: " + ofReb + "  DefensiveRebounds: " + dfReb);
                                    else if (t.TeamName.ToLower().Contains(Searching.Text.ToLower())) playerStrings.Add($"Team #: " + t.TeamNo + "  Name: " + t.TeamName + "  Abbreviation: " + t.TeamAbbreviation + "  |  Wins/Losses: " + t.Wins + "/" + t.Losses + "  Steals: " + steals + "  Turnovers: " + turnovers + "  Assists: " + assists + "  Blocks: " + blocks + "  Fouls: " + fouls + "  OffensiveRebounds: " + ofReb + "  DefensiveRebounds: " + dfReb);
                                }
                            }
                        }

                        statsList.ItemsSource = playerStrings;
                    }
                }
                catch (Exception ex)
                {
                    Searching.Text = $"Clicked and failed";
                }
            }
        }

        private void Button_Clicked (object sender, EventArgs e)
        {
            Navigation.PushAsync(new PlayerSelect());
        }


        private List<GameViewSearchResult> gvsr = new List<GameViewSearchResult>();
        async private void GameViewSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            gvsr.Clear();
            SearchBar searchBar = (SearchBar)sender;
            List<string> results = new List<string>();
            
            GameViewSearchResults.ClearLogicalChildren();
            int selectedIndex = GameViewPicker.SelectedIndex;

            if (selectedIndex != -1)
            {
                if (GameViewPicker.ItemsSource[selectedIndex].Equals("Date"))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            //http://localhost:5121/api/Schedules
                            HttpResponseMessage response = await client.GetAsync($"{URL}/{"Schedules"}");
                            // List<Weather> w = JsonArray.JsonConvert.DeserializeObject<List<Person>>
                            if (response.IsSuccessStatusCode)
                            {
                                string json = await response.Content.ReadAsStringAsync();
                                List<Schedule> schedule = JsonConvert.DeserializeObject<List<Schedule>>(json);

                                foreach (Schedule s in schedule)
                                {
                                    if (s.GameDate.Contains(GameViewSearch.Text))
                                    {
                                        response = await client.GetAsync($"{URL}/{"Games"}/{s.GameNo}");
                                        json = await response.Content.ReadAsStringAsync();
                                        Game game = JsonConvert.DeserializeObject<Game>(json);
                                        response = await client.GetAsync($"{URL}/{"Teams"}/{game.TeamNoOne}");
                                        json = await response.Content.ReadAsStringAsync();
                                        Team team1 = JsonConvert.DeserializeObject<Team>(json);
                                        response = await client.GetAsync($"{URL}/{"Teams"}/{game.TeamNoTwo}");
                                        json = await response.Content.ReadAsStringAsync();
                                        Team team2 = JsonConvert.DeserializeObject<Team>(json);
                                        GameViewSearchResult g = new GameViewSearchResult(game,s,team1,team2);
                                        gvsr.Add(g);
                                        results.Add(g.ToString);
                                        
                                        

                                    }

                                }












                            }
                        }
                        catch (Exception ex)
                        {
                            results.Add(" " + ex);
                            GameViewSearchResults.ItemsSource = results;
                            return;
                        }
                    }
                }
                else if (GameViewPicker.ItemsSource[selectedIndex].Equals("Team"))
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            try
                            {
                                HttpResponseMessage response = await client.GetAsync($"{URL}/{"games"}");
                                if (response.IsSuccessStatusCode)
                                {
                                    

                                   
                                    string json = await response.Content.ReadAsStringAsync();
                                    List<Game> games  = JsonConvert.DeserializeObject<List<Game>>(json);
                                    foreach(Game game in games)
                                    {
                                        response = await client.GetAsync($"{URL}/{"Teams"}/{game.TeamNoOne}");
                                       
                                        json = await response.Content.ReadAsStringAsync();
                                        Team team1 = JsonConvert.DeserializeObject<Team>(json);
                                        response = await client.GetAsync($"{URL}/{"Teams"}/{game.TeamNoTwo}");
                                       
                                        json = await response.Content.ReadAsStringAsync();
                                        Team team2 = JsonConvert.DeserializeObject<Team>(json);
                                        if (team1.TeamName.ToLower().Contains(GameViewSearch.Text.ToLower()) || team2.TeamName.ToLower().Contains(GameViewSearch.Text.ToLower()))
                                        {
                                            
                                            response = await client.GetAsync($"{URL}/{"Schedules"}");
                                            
                                            json = await response.Content.ReadAsStringAsync();
                                            List<Schedule> schedules = JsonConvert.DeserializeObject<List<Schedule>>(json);
                                            foreach (Schedule schedule in schedules)
                                            {
                                                if(schedule.GameNo == game.GameNo)
                                                {
                                                    GameViewSearchResult g = new GameViewSearchResult(game, schedule, team1, team2);
                                                    gvsr.Add(g);
                                                    results.Add(g.ToString);

                                                }
                                              

                                            }
                                            
                                        }

                                    }





















                                }
                            }
                            catch (Exception ex)
                            {
                                results.Add(" " + ex);
                                GameViewSearchResults.ItemsSource = results;
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        results.Add(" " + ex);
                        GameViewSearchResults.ItemsSource = results;
                        return;
                    }

                }
               
                
            }
            else
            {
                results.Add("Please select a search type");
                GameViewSearchResults.ItemsSource = results;
                return;
            }
            GameViewSearchResults.ItemsSource = results;
        }
        
      
        
        GameViewSearchResult selectedGameSearchResult = null;
        async private void RefreshGameViewStats()
        {
            using (HttpClient client = new HttpClient())
            {
                List<String> result1 = new List<String>();
                List<String> result2 = new List<String>();
                GameViewTeam1Stats.ClearLogicalChildren();
                GameViewTeam2Stats.ClearLogicalChildren();
                if (selectedGameSearchResult == null)
                {
                    GameViewTeam1.Text = "?";
                    GameViewTeam2.Text = "?";
                    return;
                }
                GameViewTeam1.Text = selectedGameSearchResult.GetTeam1().TeamName;
                GameViewTeam2.Text = selectedGameSearchResult.GetTeam2().TeamName;
                GameViewTeam1Score.Text = selectedGameSearchResult.GetGame().ScoreOne.ToString();
                GameViewTeam2Score.Text = selectedGameSearchResult.GetGame().ScoreTwo.ToString();


                HttpResponseMessage response = await client.GetAsync($"{URL}/{"Stats"}");

                String json = await response.Content.ReadAsStringAsync();
                List<Stat> stat = JsonConvert.DeserializeObject<List<Stat>>(json);
                foreach(Stat s in stat)
                {
                    if(s.GameId == selectedGameSearchResult.GetGame().GameNo)
                    {
                        response = await client.GetAsync($"{URL}/{"TeamPlayers"}/{s.PlayerTeamId}");
                        json = await response.Content.ReadAsStringAsync();
                        TeamPlayer tp = JsonConvert.DeserializeObject<TeamPlayer>(json);
                        response = await client.GetAsync($"{URL}/{"Players"}/{tp.PlayerId}");
                        json = await response.Content.ReadAsStringAsync();
                        Player p = JsonConvert.DeserializeObject<Player>(json);
                        response = await client.GetAsync($"{URL}/{"StatsTypes"}/{s.StatTypeId}");
                        json = await response.Content.ReadAsStringAsync();
                        StatsType st = JsonConvert.DeserializeObject<StatsType>(json);

                        String output = "" + p.FName + " " + p.LName + " " + st.StatName;
                        
                        if (tp.TeamId == selectedGameSearchResult.GetTeam1().TeamNo)
                        {

                            result1.Add(output);


                        }
                        if (tp.TeamId == selectedGameSearchResult.GetTeam2().TeamNo)
                        {

                            result2.Add(output);

                        }



                    }
                }

                GameViewTeam1Stats.ItemsSource = result1;
                GameViewTeam2Stats.ItemsSource = result2;



            }
        }

        private void GameViewSearchResults_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            GameViewTeam1.Text = "?";
            GameViewTeam2.Text = "?";
            int i = e.ItemIndex;
            selectedGameSearchResult = gvsr[i];
            RefreshGameViewStats();


        }

        private void GameViewReload_Clicked(object sender, EventArgs e)
        {
            RefreshGameViewStats();
        }
    }
  
}
