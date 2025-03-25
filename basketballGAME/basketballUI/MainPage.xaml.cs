using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;
using Windows.Media.Audio;
using Windows.Media.Protection.PlayReady;
namespace basketballUI
{
    public partial class MainPage: TabbedPage
    {
        //http://localhost:5121/api/Schedules
        string URL = $"http://localhost:5121/api";

        public MainPage()
        {
            InitializeComponent();
        }

        async private void Reload_Clicked(object sender, EventArgs e) {
            string API_URL = "http://localhost:5121/api/Players";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(API_URL);
                    // List<Weather> w = JsonArray.JsonConvert.DeserializeObject<List<Person>>
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(json);
                       // ReloadBtn.Text = $"Player num " + players[0].PlayerNo + " First name " + players[0].FName + " Last name " + players[0].LName;

                       // statsList.ItemsSource = players;
                    }
                }
                catch (Exception ex)
                {
                   // ReloadBtn.Text = $"Clicked and failed";
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
                                    
        private void GameViewSearchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            int i = e.SelectedItemIndex;


    }
    }

}
