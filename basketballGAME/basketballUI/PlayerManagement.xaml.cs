using basketballUI.models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace basketballUI;

public partial class PlayerManagement : ContentPage
{
	public PlayerManagement()
	{
		InitializeComponent();
	}

    async private void Create_Player_Button_Click(object sender, EventArgs e)
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
                        CreatePlayerButton.Text = $"success ";
                    }
                    else
                    {
                        CreatePlayerButton.Text = $"no success  " + response1;
                    }
                }
                else
                {
                    CreatePlayerButton.Text = $"no success";
                }
            }
            catch (Exception ex)
            {
                CreatePlayerButton.Text = $"Clicked and failed " + ex;
            }
        }
	}

    private void entry_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void entry_Completed(object sender, EventArgs e)
    {

    }
}