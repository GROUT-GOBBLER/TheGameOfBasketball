using basketballUI.models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Reflection;

namespace basketballUI;

public partial class PlayerManagement : ContentPage
{
    string firstName = null;
    string lastName = null;
    string teamID = null;
    string playerID = null;

    public PlayerManagement()
	{
		InitializeComponent();
	}

    async private void Create_Player_Button_Click(object sender, EventArgs e)
	{
		string API_URL_1 = "http://localhost:5121/api/Players";
        string API_URL_2 = "http://localhost:5121/api/TeamPlayers";


        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response1 = await client.GetAsync(API_URL_1);
                HttpResponseMessage response2 = await client.GetAsync(API_URL_2);

                List<Player> players;
                List<TeamPlayer> teamsPlayers;

                if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {
                    string json1 = await response1.Content.ReadAsStringAsync();
                    players = JsonConvert.DeserializeObject<List<Player>>(json1);

                    string json2 = await response2.Content.ReadAsStringAsync();
                    teamsPlayers = JsonConvert.DeserializeObject<List<TeamPlayer>>(json2);

                    Player player = new Player();
                    int length1 = players.Count;

                    TeamPlayer teamPlayer = new TeamPlayer();
                    int length2 = teamsPlayers.Count;
                    
                    player.PlayerNo = players[length1 - 1].PlayerNo + 1;
                    teamPlayer.Id = teamsPlayers[length2 - 1].Id + 1;

                    // PLAYER ... first name.
                    if (firstName != null)
                        player.FName = firstName;
                    else
                        return;
                    
                    // PLAYER ... last name.
                    if (lastName != null)
                        player.LName = lastName;
                    else
                        return;

                    // TEAMPLAYER ... PlayerId.
                    teamPlayer.PlayerId = player.PlayerNo;

                    // TEAMPLAYER ... TeamId.
                    if (teamID != null)
                        teamPlayer.TeamId = int.Parse(teamID);
                    else
                        return;

                    // Various.
                    teamPlayer.Player = null;
                    teamPlayer.Stats = new List<Stat>();
                    teamPlayer.Team = null;
                    
                    HttpResponseMessage response3 = await client.PostAsJsonAsync(API_URL_1, player);
                    HttpResponseMessage response4 = await client.PostAsJsonAsync(API_URL_2, teamPlayer);

                    if (response3.IsSuccessStatusCode)
                    {
                        if (response4.IsSuccessStatusCode)
                            CreatePlayerButton.Text = $"both success ";
                        else
                            CreatePlayerButton.Text = $"3 success 4 fail " + response4;
                    }
                    else
                    {
                        if (response4.IsSuccessStatusCode)
                            CreatePlayerButton.Text = $"3 fail 4 success " + response3;
                        else
                            CreatePlayerButton.Text = $"both fail " + response3 + "\n\n" + response4;
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

    //async private void EditPlayerButton_Clicked_1(object sender, EventArgs e)
    //{
    //    string API_URL_1 = "http://localhost:5121/api/Players";
    //    string API_URL_2 = "http://localhost:5121/api/TeamPlayers";

    //    using (HttpClient client = new HttpClient())
    //    {
    //        try
    //        {
    //            HttpResponseMessage response1 = await client.GetAsync(API_URL_1);
    //            HttpResponseMessage response2 = await client.GetAsync(API_URL_2);

    //            List<Player> players;
    //            List<TeamPlayer> teamsPlayers;

    //            if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
    //            {
    //                string json1 = await response1.Content.ReadAsStringAsync();
    //                players = JsonConvert.DeserializeObject<List<Player>>(json1);
    //                string json2 = await response2.Content.ReadAsStringAsync();
    //                teamsPlayers = JsonConvert.DeserializeObject<List<TeamPlayer>>(json2);

    //                int PlayerIdValue = int.Parse(playerID);
    //                int teamIdValue;

    //                // Set player equal to specified player number.
    //                // If player number does not exist make a new player.
    //                    int index = -1;
    //                    foreach (Player pl in players)
    //                    {
    //                        if(pl.PlayerNo == PlayerIdValue)
    //                        {
    //                            index = pl.PlayerNo;
    //                            break;  
    //                        }
    //                    }

    //                    Player player;

    //                    if (index == -1)
    //                    {
    //                        player = new Player();
    //                    }
    //                    else
    //                    {
    //                        player = players[index - 1];
    //                    }

    //                // Find team number from TeamPlayers.
    //                for (int x = 0; x < teamsPlayers.Count; x++)
    //                {
                        
    //                }

    //                TeamPlayer teamPlayer = new TeamPlayer();

    //                int length = players.Count - 1;
    //                player.PlayerNo = 10;
    //                player.FName = "Editoria";
    //                player.LName = "Ami";
    //                player.TeamPlayers = new List<TeamPlayer>();// use the list for any addition that references other connections

    //                HttpResponseMessage response3 = await client.PutAsJsonAsync($"{API_URL_1}/{10}", player);// "/{13}" is the id.
    //                HttpResponseMessage response4 = await client.PutAsJsonAsync($"{API_URL_1}/{10}", player);// "/{13}" is the id.

    //                if (response3.IsSuccessStatusCode)
    //                {
    //                    if (response4.IsSuccessStatusCode)
    //                        EditPlayerButton.Text = $"both success ";
    //                    else
    //                        EditPlayerButton.Text = $"3 success 4 fail " + response4;
    //                }
    //                else
    //                {
    //                    if (response4.IsSuccessStatusCode)
    //                        EditPlayerButton.Text = $"3 fail 4 success " + response3;
    //                    else
    //                        EditPlayerButton.Text = $"both fail " + response3 + "\n\n" + response4;
    //                }
    //            }
    //            else
    //            {
    //                EditPlayerButton.Text = $"no success";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            EditPlayerButton.Text = $"Clicked and failed " + ex;
    //        }

    //    }
    //}

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

    private void PlayerIDEdit_TextChanged(object sender, TextChangedEventArgs e)
    {
        playerID = e.NewTextValue;
    }
}