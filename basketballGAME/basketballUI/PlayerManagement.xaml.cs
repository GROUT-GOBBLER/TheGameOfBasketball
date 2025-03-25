using basketballUI.models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace basketballUI;

public partial class PlayerManagement : ContentPage
{
    string firstName = null;
    string lastName = null;
    String teamID = null;

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

                List<Player> players;
                List<TeamPlayer> playerTeams;

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    
                    // Make a player.
                        players = JsonConvert.DeserializeObject<List<Player>>(json);

                        Player player = new Player();
                            int length = players.Count;
                    
                        // PLAYER NUMBER.
                            player.PlayerNo = players[length - 1].PlayerNo + 1;

                        // FIRST NAME.
                            if (firstName != null)
                                player.FName = firstName;
                            else
                                player.FName = "NULL";

                        // LAST NAME.
                            if (lastName != null)
                                player.LName = lastName;
                            else
                                player.LName = "NULL";

                    // Make a team player.
                        playerTeams = JsonConvert.DeserializeObject<List<TeamPlayer>>(json);

                        TeamPlayer playerTeam = new TeamPlayer();
                            length = playerTeams.Count;

                        // ID.
                            playerTeam.Id = playerTeams[length - 1].Id + 1;

                        // PLAYER ID.
                            playerTeam.PlayerId = player.PlayerNo;

                        // TEAM ID.
                            if (teamID != null)
                                playerTeam.TeamId = int.Parse(teamID);
                            else
                                playerTeam.TeamId = 0;

                        // Assign team player to player.
                            player.TeamPlayers = playerTeams;
                    
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

        firstName = null;
        lastName = null;
        teamID = null;
	}

    private void PlayerFirstNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        firstName = e.NewTextValue;
    }

    private void PlayerLastNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        lastName = e.NewTextValue;
    }

    private void PlayerTeamEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
            teamID = e.NewTextValue;
    }
}