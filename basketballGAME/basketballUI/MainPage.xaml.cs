using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using Windows.Media.Audio;
using Windows.Media.Protection.PlayReady;
using System.Collections.Generic;
using Windows.Media.PlayTo;
using System.Net.Http.Json;
using System.Linq;

namespace basketballUI
{
    public partial class MainPage: TabbedPage
    {
        string URL = $"http://localhost:5121/api";
        private string selectedPlayer;
        private string lastAction;
        private List<Player> playerList;

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

                    // List<Weather> w = JsonArray.JsonConvert.DeserializeObject<List<Person>>
                    
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var popup = new PlayerSelect("Select Player", selectedPlayer =>
            {

            }, SavedList.CurrentPlayerList);

            var result = await this.ShowPopupAsync(popup);

            if (result is string selectedPlayer && !string.IsNullOrEmpty(selectedPlayer))
            {
                await DisplayAlert("Player Selected", $"You selected: Player {selectedPlayer}", "OK");
                // await Navigation.PushAsync(new ScoreKeeper());

                if (Application.Current.MainPage is NavigationPage navPage && navPage.CurrentPage is TabbedPage tabbedPage)
                {
                    var scoreKeeperPage = tabbedPage.Children.FirstOrDefault(page => page.Title == "Score Keeping Mode");
                    if (scoreKeeperPage != null)
                    {
                        tabbedPage.CurrentPage = scoreKeeperPage;
                    }
                }
            }
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

                                        GameViewSearchResult g = new GameViewSearchResult(game, s, team1, team2);
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
                                    List<Game> games = JsonConvert.DeserializeObject<List<Game>>(json);
                                    foreach (Game game in games)
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
                                                if (schedule.GameNo == game.GameNo)
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
                foreach (Stat s in stat)
                {
                    if (s.GameId == selectedGameSearchResult.GetGame().GameNo)
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








        private List<GameViewSearchResult> gvsrscore = new List<GameViewSearchResult>();
        async private void ScoreViewSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            gvsrscore.Clear();
            SearchBar searchBar = (SearchBar)sender;
            List<string> results = new List<string>();

            ScoreViewSearchResults.ClearLogicalChildren();
            int selectedIndex = ScoreViewPicker.SelectedIndex;

            if (selectedIndex != -1)
            {
                if (ScoreViewPicker.ItemsSource[selectedIndex].Equals("Date"))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            //http://localhost:5121/api/Schedules
                            HttpResponseMessage response = await client.GetAsync($"{URL}/{"Schedules"}");
                            if (response.IsSuccessStatusCode)
                            {
                                string json = await response.Content.ReadAsStringAsync();
                                List<Schedule> schedule = JsonConvert.DeserializeObject<List<Schedule>>(json);

                                foreach (Schedule s in schedule)
                                {
                                    if (s.GameDate.Contains(ScoreViewSearch.Text))
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

                                        GameViewSearchResult g = new GameViewSearchResult(game, s, team1, team2);
                                        gvsrscore.Add(g);
                                        results.Add(g.ToString);

                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            results.Add(" " + ex);
                            ScoreViewSearchResults.ItemsSource = results;
                            return;
                        }
                    }
                }
                else if (ScoreViewPicker.ItemsSource[selectedIndex].Equals("Team"))
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
                                    List<Game> games = JsonConvert.DeserializeObject<List<Game>>(json);
                                    foreach (Game game in games)
                                    {
                                        response = await client.GetAsync($"{URL}/{"Teams"}/{game.TeamNoOne}");

                                        json = await response.Content.ReadAsStringAsync();
                                        Team team1 = JsonConvert.DeserializeObject<Team>(json);


                                        response = await client.GetAsync($"{URL}/{"Teams"}/{game.TeamNoTwo}");

                                        json = await response.Content.ReadAsStringAsync();
                                        Team team2 = JsonConvert.DeserializeObject<Team>(json);

                                        if (team1.TeamName.ToLower().Contains(ScoreViewSearch.Text.ToLower()) || team2.TeamName.ToLower().Contains(ScoreViewSearch.Text.ToLower()))
                                        {

                                            response = await client.GetAsync($"{URL}/{"Schedules"}");

                                            json = await response.Content.ReadAsStringAsync();
                                            List<Schedule> schedules = JsonConvert.DeserializeObject<List<Schedule>>(json);
                                            foreach (Schedule schedule in schedules)
                                            {
                                                if (schedule.GameNo == game.GameNo)
                                                {
                                                    GameViewSearchResult g = new GameViewSearchResult(game, schedule, team1, team2);
                                                    gvsrscore.Add(g);
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
                                ScoreViewSearchResults.ItemsSource = results;
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        results.Add(" " + ex);
                        ScoreViewSearchResults.ItemsSource = results;
                        return;
                    }
                }
            }
            else
            {
                results.Add("Please select a search type");
                ScoreViewSearchResults.ItemsSource = results;
                return;
            }
            ScoreViewSearchResults.ItemsSource = results;
        }

        GameViewSearchResult selectedScoreGameSearchResult = null;//
        private async void ScoreViewSearchResults_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            GameViewTeam1.Text = "?";
            GameViewTeam2.Text = "?";
            int i = e.ItemIndex;
            selectedScoreGameSearchResult = gvsrscore[i];
            //  RefreshGameViewStats();

            string API_URL = "http://localhost:5121/api/TeamPlayers";
            string API_URL2 = "http://localhost:5121/api/Players";

            playerList = new List<Player>{};
           
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(API_URL);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<TeamPlayer> teamPlayers = JsonConvert.DeserializeObject<List<TeamPlayer>>(json);
                        List<TeamPlayer> ACTUALTEAMPLAYERS = new List<TeamPlayer> { };
                       
                        if (teamPlayers != null)
                        {
                            for (int index = 0; index < teamPlayers.Count; index++)
                            {
                               if (teamPlayers[index].TeamId == selectedScoreGameSearchResult.GetTeam1().TeamNo
                                   || teamPlayers[index].TeamId == selectedScoreGameSearchResult.GetTeam2().TeamNo)
                               {

                                    ACTUALTEAMPLAYERS.Add(teamPlayers[index]);
                                   
                               }
                            }

                            try
                            {
                                HttpResponseMessage response2 = await client.GetAsync($"{URL}/{"Players"}");
                                if (response2.IsSuccessStatusCode)
                                {
                                    string json2 = await response2.Content.ReadAsStringAsync();
                                    List<Player> t22eamPlayers = JsonConvert.DeserializeObject<List<Player>>(json2);

                                    foreach (Player player in t22eamPlayers)
                                    {
                                        response2 = await client.GetAsync($"{URL}/{"Players"}/{player.PlayerNo}");

                                        string json3 = await response2.Content.ReadAsStringAsync();
                                        Player something = JsonConvert.DeserializeObject<Player>(json3);
                                        if (ACTUALTEAMPLAYERS != null)
                                        {
                                            for (int index = 0; index < ACTUALTEAMPLAYERS.Count; index++)
                                            {
                                                if (something.PlayerNo == ACTUALTEAMPLAYERS[index].PlayerId)
                                                {
                                                    playerList.Add(something);
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Error", "Cannot Connect to API!!!", "OK");
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Cannot Connect to API!!!", "OK");
                }
            }

            SavedList.CurrentPlayerList = playerList;

            // Update the ScoreKeeper tab with the new playerList
            var scoreKeeperPage = Children.FirstOrDefault(page => page is ScoreKeeper) as ScoreKeeper;
            if (scoreKeeperPage != null)
            {
                scoreKeeperPage.UpdatePlayerList(playerList);
            }


            var popup = new PlayerSelect("Select Player", selectedPlayer =>
            {
                this.selectedPlayer = selectedPlayer;
                if (!string.IsNullOrEmpty(this.selectedPlayer))
                {
                    DisplayAlert("Action Recorded", $"{this.selectedPlayer} performed: {this.lastAction}", "OK");
                }
            }, SavedList.CurrentPlayerList);

            await this.ShowPopupAsync(popup);

        }
        private async void addStat(String selectedPlayer, String selectedAction)
        {
              /*      "id": 12,
                "playerTeamId": 1,
                "gameId": 1,
                "statTypeId": 1,
                "statValue": 3,
                "game": null,
                "playerTeam": null,
                "statType": null*/
            Stat stat = new Stat();
            
            stat.GameId = selectedScoreGameSearchResult.GetGame().GameNo;
            //GameViewTeam1.Text = "" + selectedScoreGameSearchResult.GetGame().TeamNoTwo;
            
            if (stat.GameId == null)
            {
                return;
            }
            stat.StatTypeId = -1;
            if (selectedAction.Contains("2PT shot made")) {

                stat.StatTypeId = 2;
            }
            else if (selectedAction.Contains("3PT shot made"))
            {
                stat.StatTypeId = 4;

            }
            else if (selectedAction.Contains("2PT shot missed"))
            {
                stat.StatTypeId = 1;

            }
            else if (selectedAction.Contains("3PT shot missed"))
            {
                stat.StatTypeId = 3;

            }
            else if (selectedAction.Contains("Free throw"))
            {
                stat.StatTypeId = 1;

            }
            else if (selectedAction.Contains("OffRebound"))
            {
                stat.StatTypeId = 10;

            }
            else if (selectedAction.Contains("DefRebound"))
            {
                stat.StatTypeId = 11;

            }
            else if (selectedAction.Contains("Assist"))
            {
                stat.StatTypeId = 7;

            }
            else if (selectedAction.Contains("TOver"))
            {
                stat.StatTypeId = 6;

            }
            else if (selectedAction.Contains("Block"))
            {
                stat.StatTypeId = 8;

            }
            else if (selectedAction.Contains("Steal"))
            {
                stat.StatTypeId = 5;

            }
            else if (selectedAction.Contains("Foul"))
            {
                stat.StatTypeId = 9;

            }
            if(stat.StatTypeId == -1)
            {
                return;
            }
            Player p = new Player();//SavedList.CurrentPlayerList[index];
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response2 = await client.GetAsync($"{URL}/{"Players"}");

                string json2 = await response2.Content.ReadAsStringAsync();
                int index = int.Parse(selectedPlayer);
                List<Player> Players = JsonConvert.DeserializeObject<List<Player>>(json2);
                foreach (Player player in Players)
                {
                    if( index == player.PlayerNo)
                    {
                        p = player;
                        break;
                    }
                }

               
               

            }
                
            
            if (p.FName == null)
            {
                GameViewTeam1.Text = "player not found";
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{URL}/{"TeamPlayers"}");

                String json = await response.Content.ReadAsStringAsync();
                List<TeamPlayer> teamPlayer = JsonConvert.DeserializeObject<List<TeamPlayer>>(json);
                foreach (TeamPlayer t in teamPlayer) {
                    if (t.PlayerId == p.PlayerNo)
                    {

                        if (t.Id == selectedScoreGameSearchResult.GetGame().TeamNoOne)
                        {
                            stat.PlayerTeamId = selectedScoreGameSearchResult.GetGame().TeamNoOne;
                            break;
                        }
                        else if (t.Id == selectedScoreGameSearchResult.GetGame().TeamNoTwo)
                        {
                            stat.PlayerTeamId = selectedScoreGameSearchResult.GetGame().TeamNoTwo;
                            break;
                        }










                    }


                }

                if (selectedScoreGameSearchResult.GetGame() == null)
                {
                    return;
                }
                if (stat.PlayerTeamId == null)
                {
                    return;
                }
                response = await client.GetAsync($"{URL}/{"Stats"}");

                json = await response.Content.ReadAsStringAsync();
                List<Stat> S = JsonConvert.DeserializeObject<List<Stat>>(json);
                stat.Id = S[S.Count - 1].Id + 1 ;







                stat.Game = null;
                stat.StatType = null;
                stat.PlayerTeam = null;
                foreach (Stat s in S)
                {
                    if (s.GameId == stat.GameId && s.PlayerTeamId == stat.PlayerTeamId && s.StatTypeId == stat.StatTypeId){

                        stat.Id = s.Id;
                        short i = 1;
                        stat.StatValue = (short)(s.StatValue + 1);

                        

                        response = await client.PutAsJsonAsync($"{URL}/{"Stats"}/{stat.Id}", stat);
                       // GameViewTeam1.Text = "" M response;

                        return;
                    }
                }
                if (stat.StatValue == null)
                {
                    stat.StatValue = 1;
                }


                response = await client.PostAsJsonAsync($"{URL}/{"Stats"}",stat);
                //GameViewTeam1.Text = "" + response;
            }





        }
        private async void GoToScoreKeeper_Clicked(object sender, EventArgs e)
        {
            if (Navigation != null)
            {
                await Navigation.PushAsync(new ScoreKeeper());
            }
            else
            {
                await DisplayAlert("Error", "Navigation is not available", "OK");
            }
        }

        private async void OnActionButtonClicked(object sender, EventArgs e)//stat info
        {
            var button = sender as Button;
            this.lastAction = button?.Text;

            if (playerList != null)
            {
                var popup = new PlayerSelect(this.lastAction, selectedPlayer =>
                {
                    this.selectedPlayer = selectedPlayer;
                    if (!string.IsNullOrEmpty(this.selectedPlayer))
                    {
                        addStat(this.selectedPlayer, this.lastAction);
                        DisplayAlert("Action Recorded", $"{this.selectedPlayer} performed: {this.lastAction}", "OK");
                    }
                }, SavedList.CurrentPlayerList);

                await this.ShowPopupAsync(popup);
            }
            else
            {
                await DisplayAlert("NOT WORKING", "NO PLAYERS DETECTED: RELOAD THE PLAYERS", "OK");
            }
        }

        private async void OnFreeThrowClicked(object sender, EventArgs e)
        {
            if (playerList != null)
            {
              
                var popup = new PlayerSelect("Free Throw", selectedPlayer =>
                {
                    this.selectedPlayer = selectedPlayer;
                }, SavedList.CurrentPlayerList);

                var result = await this.ShowPopupAsync(popup);

                if (result is string selectedPlayer && !string.IsNullOrEmpty(selectedPlayer))
                {
                    await Navigation.PushAsync(new FreeThrow(selectedPlayer));
                }
            }
            else
            {
                await DisplayAlert("NOT WORKING", "NO PLAYERS DETECTED: RELOAD THE PLAYERS", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Schedule_Management_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ScheduleManagement());
        }

        private void Player_Management_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PlayerManagement());
        }

        private void Team_Management_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TeamManagement());
        }
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
