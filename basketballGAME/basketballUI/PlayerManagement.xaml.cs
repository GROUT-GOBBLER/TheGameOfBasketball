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

                List<Player> players;

                if (response1.IsSuccessStatusCode)
                {
                    string json1 = await response1.Content.ReadAsStringAsync();
                    players = JsonConvert.DeserializeObject<List<Player>>(json1);

                    
                    Player player = new Player();
                    int length1 = players.Count;

                    player.PlayerNo = players[length1 - 1].PlayerNo + 1;

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

                    player.TeamPlayers = new List<TeamPlayer>();

                    HttpResponseMessage response3 = await client.PostAsJsonAsync(API_URL_1, player);

                    if (response3.IsSuccessStatusCode)
                    {
                        CreatePlayerButton.Text = $"success ";
                    }
                    else
                    {
                        CreatePlayerButton.Text = $"fail " + response3;
                    }
                }
                else
                {
                    CreatePlayerButton.Text = $"no success ";
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
        string API_URL_1 = "http://localhost:5121/api/Players";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response1 = await client.GetAsync(API_URL_1);//player
                List<Player> players;

                if (response1.IsSuccessStatusCode)
                {
                    // Fill json and teamPlayers with values.
                    string json = await response1.Content.ReadAsStringAsync();
                    players = JsonConvert.DeserializeObject<List<Player>>(json);
                    
                    Player player = new Player();
                    int PLAYERnumber = int.Parse(playerID);

                    // Enter player values.
                        player.PlayerNo = PLAYERnumber;
                        player.FName = firstName;
                        player.LName = lastName;
                        player.TeamPlayers = new List<TeamPlayer>();// use the list for any addition that references other connections

                    HttpResponseMessage response3 = await client.PutAsJsonAsync($"{API_URL_1}/{PLAYERnumber}", player);

                    if (response3.IsSuccessStatusCode)
                    {
                            EditPlayerButton.Text = $"success";
                    }
                    else
                    { 
                        EditPlayerButton.Text = $"no success  " + response3 + "\n" + player.PlayerNo + " " + player.FName + " " + player.LName;
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

                    int PlayerIdValue;

                    if (playerID != null)
                        PlayerIdValue = int.Parse(playerID);
                    else
                    {
                        DeletePlayerButton.Text = "ENTER PLAYER ID.";
                        return;
                    }

                    TeamPlayer tp = teamPlayerLIST.Find(e => e.PlayerId == PlayerIdValue);
                    int PlayerTeamIdValue = tp.Id;

                    HttpResponseMessage response3 = await client.DeleteAsync($"{API_URL_2}/{PlayerTeamIdValue}");
                    HttpResponseMessage response4 = await client.DeleteAsync($"{API_URL}/{PlayerIdValue}");

                    if (response3.IsSuccessStatusCode)
                    {
                        if (response4.IsSuccessStatusCode)
                            DeletePlayerButton.Text = $"success ";
                        else
                            DeletePlayerButton.Text = $"3 success 4 fail " + response4;
                    }
                    else
                    {
                        if (response4.IsSuccessStatusCode)
                            DeletePlayerButton.Text = $"3 fail 4 success " + response3;
                        else
                            DeletePlayerButton.Text = $"TOTAL FAILURE " + response3 + "\n" + response4;
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

    async private void ViewPlayerButton_Clicked(object sender, EventArgs e)
    {
        string API_URL = "http://localhost:5121/api/Players";
        
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(API_URL);
                
                List<Player> players;
                if (response.IsSuccessStatusCode)
                {
                    ViewPlayerButton.Text = "success";

                    string json = await response.Content.ReadAsStringAsync();
                    players = JsonConvert.DeserializeObject<List<Player>>(json);
                    
                    int indexOfPlayerInPlayers = -1;
                    int PlayerIDValue = int.Parse(this.playerID);

                    for (int x = 0; x < players.Count; x++)
                    {
                        if (players[x].PlayerNo == PlayerIDValue)
                        {
                            indexOfPlayerInPlayers = x;
                            break;
                        }
                    }
                    Player p;

                    if (indexOfPlayerInPlayers != -1)
                        p = players[indexOfPlayerInPlayers];
                    else
                    {
                        ViewPlayerButton.Text = $"ENTER VALID PLAYER ID.";
                        PLAYERinfoLABEL.Text = "*** waiting ^-^ for player ^-^ number *** :3";
                        return;
                    }

                    PLAYERinfoLABEL.Text = p.PlayerNo + " " + p.FName + " " + p.LName + ".";
                }
                else
                {
                    ViewPlayerButton.Text = $"no success";
                }
            }
            catch (Exception ex)
            {
                ViewPlayerButton.Text = $"Clicked and failed " + ex;
            }

        }

        string firstName = null;
        string lastName = null;
        string teamID = null;
        string playerID = null;
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