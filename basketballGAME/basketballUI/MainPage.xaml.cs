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
                        HttpResponseMessage response1 = await client.PostAsJsonAsync(API_URL, player);
                        if (response1.IsSuccessStatusCode)
                        { 
                            CounterBtn2.Text = $"success ";
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

        async private void CounterBtn3_Clicked(object sender, EventArgs e)
        {
            string API_URL = "http://localhost:5121/api/Players";
            //copy the url given when running the api
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(API_URL);
                    List<Player> players;
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        players = JsonConvert.DeserializeObject<List<Player>>(json);
                        Player player = new Player();
                        

                        int length = players.Count -1;
                        player.PlayerNo = 13  ;
                        player.FName = "Editoria";
                        player.LName = "Ami";
                        player.TeamPlayers = new List<TeamPlayer>();// use the list for any addition that references other connections
                        HttpResponseMessage response1 = await client.PutAsJsonAsync($"{API_URL}/{13}", player);// "/{13}" is the id
                        if (response1.IsSuccessStatusCode)
                        {
                     
                            CounterBtn3.Text = $"success" ;
                        }
                        else
                        {
                            CounterBtn3.Text = $"no success  " + response1;
                        }
                    }
                    else
                    {
                        CounterBtn3.Text = $"no success";
                    }
                }
                catch (Exception ex)
                {
                    CounterBtn2.Text = $"Clicked and failed " + ex;
                }

            }

        }

        private async void CounterBtn4_Clicked(object sender, EventArgs e)
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


                        int length = players.Count - 1;
                        player.PlayerNo = 13;
                        player.FName = "Editoria";
                        player.LName = "Ami";
                        player.TeamPlayers = new List<TeamPlayer>();
                        HttpResponseMessage response1 = await client.GetAsync($"{API_URL}/{13}");// "/{13}" is the id
                        if (response1.IsSuccessStatusCode)
                        {

                            


                            CounterBtn4.Text = $"success";
                        }
                        else
                        {
                            CounterBtn4.Text = $"no success  " + response1;
                        }
                    }
                    else
                    {
                        CounterBtn4.Text = $"no success";
                    }
                }
                catch (Exception ex)
                {
                    CounterBtn4.Text = $"Clicked and failed " + ex;
                }

            }
        }

        async private void CounterBtn5_Clicked(object sender, EventArgs e)
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
                        HttpResponseMessage response1 = await client.DeleteAsync($"{API_URL}/{13}");
                        if (response1.IsSuccessStatusCode)
                        {




                            CounterBtn5.Text = $"success";
                        }
                        else
                        {
                            CounterBtn5.Text = $"no success  " + response1;
                        }
                    }
                    else
                    {
                        CounterBtn5.Text = $"no success";
                    }
                }
                catch (Exception ex)
                {
                    CounterBtn5.Text = $"Clicked and failed " + ex;
                }

            }
        }
    }
    }
  
