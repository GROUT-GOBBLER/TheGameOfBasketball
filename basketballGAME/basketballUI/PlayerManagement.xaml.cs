using basketballUI.models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;

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
        playerID = null;
	}

    async private void EditPlayerButton_Clicked_1(object sender, EventArgs e)
    {
        string API_URL_1 = "http://localhost:5121/api/Players/";
        string API_URL_2 = "http://localhost:5121/api/TeamPlayers/";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response1 = await client.GetAsync(API_URL_1);//player
                HttpResponseMessage response2 = await client.GetAsync(API_URL_2);//teamplayer

                List<Player> players;
                List<TeamPlayer> teamsPlayers;

                if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {
                    // Fill json and teamPlayers with values.
                    string json = await response1.Content.ReadAsStringAsync();
                    players = JsonConvert.DeserializeObject<List<Player>>(json);
                    string json1 = await response1.Content.ReadAsStringAsync();
                    teamsPlayers = JsonConvert.DeserializeObject<List<TeamPlayer>>(json1);

                    Player player = new Player();
                    TeamPlayer teamPlayer = new TeamPlayer();

                    int indexOfThePlayerNoInTeamPlayers = -1;
                    int indexOfThePlayerNoInPlayers = -1;
                    Boolean wasFound = false; // does the player with player number exist?
                    int PLAYERnumber = int.Parse(playerID);
                    int TEAMnumber = -1;

                    if (teamID != null)
                        TEAMnumber = int.Parse(teamID); // otherwise, we set it equal to the teamID from the current player.

                    // Iterate through Players table in DB.
                    for(int x = 0; x < players.Count; x++)
                    {
                        if (players[x].PlayerNo == PLAYERnumber)
                        {
                            wasFound = true;
                            indexOfThePlayerNoInPlayers = x;
                            break;
                        }
                    }

                    // If entry exists, iterate through the TeamPlayers table in DB.
                    if(wasFound)
                    {
                        for(int x = 0; x < teamsPlayers.Count; x++)
                        {
                            if (players[x].PlayerNo == PLAYERnumber)
                            {
                                indexOfThePlayerNoInTeamPlayers = x;
                                break;
                            }
                        }
                    }

                    // Enter player values.
                    if(firstName != null)
                        player.FName = firstName;
                    if(lastName != null)
                        player.LName = lastName;
                    player.TeamPlayers = new List<TeamPlayer>();// use the list for any addition that references other connections

                    if (teamID != null)
                        teamPlayer.TeamId = int.Parse(teamID);

                    HttpResponseMessage response3 = await client.PutAsJsonAsync($"{API_URL_1}", player);//indexOfThePlayerNoInPlayers
                    HttpResponseMessage response4 = await client.PutAsJsonAsync($"{API_URL_2}", teamPlayer);//indexOfThePlayerNoInTeamPlayers

                    if (response3.IsSuccessStatusCode)
                    {
                        if (response4.IsSuccessStatusCode)
                            EditPlayerButton.Text = $"success";
                        else
                            EditPlayerButton.Text = $"3 worked 4 failed " + response4;
                    }
                    else
                    {
                        if (response4.IsSuccessStatusCode)
                            EditPlayerButton.Text = $"3 failed 4 worked " + response3;
                        else
                            EditPlayerButton.Text = $"no success  " + response3 + "\n" + response4;
                    }
                }
                else
                {
                    EditPlayerButton.Text = $"no success ...";
                }
            }
            catch (Exception ex)
            {
                EditPlayerButton.Text = $"Clicked and failed " + ex;
            }
        }

        firstName = null;
        lastName = null;
        teamID = null;
        playerID = null;
    }

    async private void DeletePlayerButton_Clicked(object sender, EventArgs e)
    {
        string API_URL = "http://localhost:5121/api/Players";
        string API_URL_2 = "http://localhost:5121/api/TeamPlayers";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response1 = await client.GetAsync(API_URL);
                HttpResponseMessage response2 = await client.GetAsync(API_URL_2);

                List<Player> playerLIST;
                List<TeamPlayer> teamPlayerLIST;

                if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {
                    string json1 = await response1.Content.ReadAsStringAsync();
                    playerLIST = JsonConvert.DeserializeObject<List<Player>>(json1);

                    string json2 = await response2.Content.ReadAsStringAsync();
                    teamPlayerLIST = JsonConvert.DeserializeObject<List<TeamPlayer>>(json2);

                    int indexPLAYER = playerLIST.Count;
                    int indexTEAMplayer = playerLIST.Count;
                    int PlayerIdValue;

                    bool playerInTeam = false;

                    if (playerID != null)
                        PlayerIdValue = int.Parse(playerID);
                    else
                    {
                        DeletePlayerButton.Text = "ENTER PLAYER ID.";
                        return;
                    }

                    for (int x = 0; x < playerLIST.Count; x++)
                    {
                        if (playerLIST[x].PlayerNo == PlayerIdValue)
                        {
                            indexPLAYER = x;

                            for (int y = 0; y < teamPlayerLIST.Count; y++)
                            {
                                if (teamPlayerLIST[y].PlayerId == PlayerIdValue)
                                {
                                    indexTEAMplayer = y;
                                    playerInTeam = true;
                                    break;
                                }
                            }
                        }
                    }

                    HttpResponseMessage response3 = await client.DeleteAsync($"{API_URL_2}/{}");
                    HttpResponseMessage response4 = await client.DeleteAsync($"{API_URL}/{}");

                    if (response3.IsSuccessStatusCode)
                    {
                        if (response4.IsSuccessStatusCode)
                            DeletePlayerButton.Text = $"success " + indexPLAYER + " " + indexTEAMplayer + " " + PlayerIdValue;
                        else
                            DeletePlayerButton.Text = $"3 success 4 fail " + response4 + "\n" + +indexPLAYER + " " + indexTEAMplayer + " " + PlayerIdValue;
                    }
                    else
                    {
                        if (response4.IsSuccessStatusCode)
                            DeletePlayerButton.Text = $"3 fail 4 success " + response3 + "\n" + +indexPLAYER + " " + indexTEAMplayer + " " + PlayerIdValue;
                        else
                            DeletePlayerButton.Text = $"TOTAL FAILURE " + response3 + "\n" + response4 + "\n" + indexPLAYER + " " + indexTEAMplayer + " " + PlayerIdValue;
                    }
                }
                else
                {
                    DeletePlayerButton.Text = $"no success";
                }
            }
            catch (Exception ex)
            {
                DeletePlayerButton.Text = $"Clicked and failed " + ex;
            }
        }

        firstName = null;
        lastName = null;
        teamID = null;
        playerID = null;
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

    private void PlayerIDEdit_TextChanged(object sender, TextChangedEventArgs e)
    {
        playerID = e.NewTextValue;
    }
}