using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;
using System.Net.Http.Json;
namespace basketballUI
{
    public partial class MainPage : ContentPage
    {
    
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        async private void OnCounterClicked(object sender, EventArgs e)
        {

            string API_URL = "http://localhost:5121/api/Players";
            //copy the url given when running the api
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
                        CounterBtn.Text = $"Player num " + players[0].PlayerNo + " First name " + players[0].FName + " Last name " + players[0].LName;

                    }
                    




                }
                catch (Exception ex)
                {
                    CounterBtn.Text = $"Clicked and failed";
                }









            }
        }

        async private void CounterBtn2_Clicked(object sender, EventArgs e)
        {
            string API_URL = "http://localhost:5121/api/Players";
            //copy the url given when running the api
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(API_URL);
                    // List<Weather> w = JsonArray.JsonConvert.DeserializeObject<List<Person>>
                    List<Player> players;
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        players = JsonConvert.DeserializeObject<List<Player>>(json);
                        Player player = new Player();
                        Console.WriteLine("here");

                        int length = players.Count;
                        player.PlayerNo = players[length - 1].PlayerNo + 1;
                        player.FName = "Calamity";
                        player.LName = "Bisteria";
                        player.TeamPlayers = new List<TeamPlayer>();
                        //String p = JsonConvert.SerializeObject(player);
                        Console.WriteLine("here2");
                        HttpResponseMessage response1 = await client.PostAsJsonAsync(API_URL, player);
                        //HttpResponseMessage response1 = await client.PostAsJsonAsync($"{API_URL}$/{13}", player);
                        if (response1.IsSuccessStatusCode)
                        {
                          
                            json = await response.Content.ReadAsStringAsync();
                            players = JsonConvert.DeserializeObject<List<Player>>(json);

                            int l = players.Count();
                            CounterBtn2.Text = $"Player num " + players[l - 1].PlayerNo + " First name " + players[l- 1].FName + " Last name " + players[l- 1].LName;
                        }
                        else
                        {
                            CounterBtn2.Text = $"no success  " + response1;
                        }
                    }
                    else
                    {
                        CounterBtn2.Text = $"no success";
                    }
                }
                catch (Exception ex)
                {
                    CounterBtn2.Text = $"Clicked and failed " + ex;
                }
            }
            }
    }

}
