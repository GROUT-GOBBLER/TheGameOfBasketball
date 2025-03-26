using basketballUI.models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Windows.Networking;

namespace basketballUI;

public partial class TeamManagement : ContentPage
{
    string teamName = null;
    string teamAbbreviation = null;
    string wins = null;
    string losses = null;
    string teamID = null;
    string teamIDAddPlayer = null;
    string playerIDAddToTeam = null;
    string teamIDRemovePlayer = null;
    string playerIDRemoveFromTeam = null;

    public TeamManagement()
    {
        InitializeComponent();
    }

    async private void CreateTeamButton_Clicked(object sender, EventArgs e)
    {
        // Button called: CreateTeamButton
        // Entry called: TeamNameEntry, TeamAbbreviationEntry, TeamWinsEntry, TeamLossesEntry
        // Entry changed: TeamNameEntry_TextChanged, TeamAbbreviationEntry_TextChanged, TeamLossesEntry_TextChanged, TeamWinsEntry_TextChanged
        // Variables used teamName, teamAbbreviation, wins, losses.

        string API_URL = "http://localhost:5121/api/Teams/";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(API_URL);
                List<Team> teams;

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    teams = JsonConvert.DeserializeObject<List<Team>>(json);

                    Team newTeam = new Team();
                    int length = teams.Count;

                    newTeam.TeamNo = teams[length - 1].TeamNo + 1;
                    newTeam.TeamName = teamName;
                    newTeam.TeamAbbreviation = teamAbbreviation;
                    newTeam.Wins = short.Parse(wins);
                    newTeam.Losses = short.Parse(losses);

                    // Various.
                    newTeam.GameTeamNoOneNavigations = new List<Game>();
                    newTeam.GameTeamNoTwoNavigations = new List<Game>();
                    newTeam.TeamPlayers = new List<TeamPlayer>();

                    HttpResponseMessage createResponse = await client.PostAsJsonAsync(API_URL, newTeam);

                    if (createResponse.IsSuccessStatusCode)
                    {
                        CreateTeamButton.Text = "Team created successfully.";
                    }
                    else
                    {
                        CreateTeamButton.Text = $"Failed to create team: " + createResponse + "\n" + length + " " + newTeam.TeamNo + " "
                            + newTeam.TeamName + " " + newTeam.TeamAbbreviation + " " + newTeam.Wins + " " + newTeam.Losses;
                        return; ;
                    }
                }
                else
                {
                    CreateTeamButton.Text = $"Failed to fetch teams: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                CreateTeamButton.Text = $"Error: {ex.Message}";
            }
        }
        teamName = null;
        teamAbbreviation = null;
        wins = null;
        losses = null;
        teamID = null;
    }
  
    async private void EditTeamButton_Clicked(object sender, EventArgs e)
    {
        string API_URL_1 = "http://localhost:5121/api/Teams";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response1 = await client.GetAsync(API_URL_1);//player
                List<Team> teams;

                if (response1.IsSuccessStatusCode)
                {
                    // Fill json and teams with values.
                    string json = await response1.Content.ReadAsStringAsync();
                    teams = JsonConvert.DeserializeObject<List<Team>>(json);

                    Team team = new Team();
                    int TEAMnumber = int.Parse(teamID);

                    // Enter team values.
                    team.TeamNo = TEAMnumber;
                    team.TeamName = teamName;
                    team.TeamAbbreviation = teamAbbreviation;
                    team.Wins = short.Parse(wins);
                    team.Losses = short.Parse(losses);
                    // Various.
                    team.GameTeamNoOneNavigations = new List<Game>();
                    team.GameTeamNoTwoNavigations = new List<Game>();
                    team.TeamPlayers = new List<TeamPlayer>();

                    HttpResponseMessage response3 = await client.PutAsJsonAsync($"{API_URL_1}/{team.TeamNo}", team);

                    if (response3.IsSuccessStatusCode)
                    {
                        EditTeamButton.Text = $"success";
                    }
                    else
                    {
                        EditTeamButton.Text = $"no success  " + response3 + "\n" + team.TeamNo + " " + team.TeamAbbreviation + " " + team.Wins
                            + " " + team.Losses;
                    }
                }
                else
                {
                    EditTeamButton.Text = $"no success ...";
                }
            }
            catch (Exception ex)
            {
                EditTeamButton.Text = $"Clicked and failed " + ex;
            }
        }

        teamName = null;
        teamAbbreviation = null;
        wins = null;
        losses = null;
        teamID = null;
    }

    async private void DeleteTeamButton_Clicked(object sender, EventArgs e)
    {
        string API_URL = "http://localhost:5121/api/Teams";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response3 = await client.DeleteAsync($"{API_URL}/{TeamIDDelete.Text}");

                if (response3.IsSuccessStatusCode)
                {
                    DeleteTeamButton.Text = "Deleted";
                }
            }
            catch (Exception ex)
            {
                DeleteTeamButton.Text = $"Clicked and failed " + ex;
            }
        }

        teamID = null;
    }

    async private void ViewTeamButton_Clicked(object sender, EventArgs e)
    {
        string API_URL = "http://localhost:5121/api/Teams";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(API_URL);
                List<Team> teams;

                if (response.IsSuccessStatusCode)
                {
                    ViewTeamButton.Text = "success";

                    string json = await response.Content.ReadAsStringAsync();
                    teams = JsonConvert.DeserializeObject<List<Team>>(json);

                    int indexOfTeamInTeams = -1;
                    int TeamIDValue = int.Parse(this.teamID);

                    for (int x = 0; x < teams.Count; x++)
                    {
                        if (teams[x].TeamNo == TeamIDValue)
                        {
                            indexOfTeamInTeams = x;
                            break;
                        }
                    }
                    Team t;

                    if (indexOfTeamInTeams != -1)
                        t = teams[indexOfTeamInTeams];
                    else
                    {
                        ViewTeamButton.Text = $"ENTER VALID PLAYER ID.";
                        TEAMinfoLABEL.Text = "*** waiting ^-^ for player ^-^ number *** :3";
                        return;
                    }

                    TEAMinfoLABEL.Text = t.TeamNo + " " + t.TeamName + " " + t.TeamAbbreviation + " " + t.Wins + " " + t.Losses;
                }
                else
                {
                    ViewTeamButton.Text = $"no success";
                }
            }
            catch (Exception ex)
            {
                ViewTeamButton.Text = $"Clicked and failed " + ex;
            }

        }

        string firstName = null;
        string lastName = null;
        string teamID = null;
        string playerID = null;
    }

    async private void AddPlayerToTeamButton_Clicked(object sender, EventArgs e)
    {

        string API_URL_TEAMPLAYERS = "http://localhost:5121/api/TeamPlayers/";
        string API_URL_1 = "http://localhost:5121/api/Players";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response1 = await client.GetAsync(API_URL_TEAMPLAYERS);
                HttpResponseMessage response2 = await client.GetAsync(API_URL_1);

                List<TeamPlayer> teamPlayers;
                List<Player> playerList;

                if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {
                    string json1 = await response1.Content.ReadAsStringAsync();
                    teamPlayers = JsonConvert.DeserializeObject<List<TeamPlayer>>(json1);

                    string json2 = await response1.Content.ReadAsStringAsync();
                    playerList = JsonConvert.DeserializeObject<List<Player>>(json2);

                    TeamPlayer tp = new TeamPlayer();

                    tp.Id = teamPlayers[teamPlayers.Count - 1].Id + 1;

                    if (PlayerIDAddToTeam != null)
                    {
                        tp.PlayerId = int.Parse(PlayerIDAddToTeam.Text);
                    }
                    else return;
                    if (TeamIDAddPlayer != null)
                    {
                        tp.TeamId = int.Parse(TeamIDAddPlayer.Text);
                    }
                    else return;

                    TeamPlayer tp2 = teamPlayers.Find(e => e.PlayerId == int.Parse(PlayerIDAddToTeam.Text));
                    if (tp2 != null)
                    {
                        return;
                    }

                    HttpResponseMessage response3 = await client.PostAsJsonAsync(API_URL_TEAMPLAYERS, tp);

                    if (response3.IsSuccessStatusCode)
                    {
                        AddPlayerToTeamButton.Text = $"success ";
                    }
                    else
                    {
                        AddPlayerToTeamButton.Text = $" fail " + response3;

                    }
                }
            }
            catch (Exception ex)
            {
                AddPlayerToTeamButton.Text = $"Error: {ex.Message}";
            }
        }
    }

    async private void RemovePlayerFromTeamButton_Clicked(object sender, EventArgs e)
    {
        string API_URL_1 = "http://localhost:5121/api/TeamPlayers";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(API_URL_1);
                List<TeamPlayer> teamPlayerLIST;

                if (response.IsSuccessStatusCode)
                {
                    string json2 = await response.Content.ReadAsStringAsync();
                    teamPlayerLIST = JsonConvert.DeserializeObject<List<TeamPlayer>>(json2);

                    int PlayerIDValue = int.Parse(playerIDRemoveFromTeam);
                    TeamPlayer tp = teamPlayerLIST.Find(e => e.PlayerId == PlayerIDValue);

                    if (tp == null)
                    {
                        RemovePlayerFromTeamButton.Text = $"Player is not in team.";
                    }
                    else
                    {
                        int PlayerTeamIDValue = tp.Id;
                        HttpResponseMessage response2 = await client.DeleteAsync($"{API_URL_1}/{PlayerTeamIDValue}");

                        if (response2.IsSuccessStatusCode)
                        {
                            RemovePlayerFromTeamButton.Text = $"success ";
                        }
                        else
                        {
                            RemovePlayerFromTeamButton.Text = $"fail " + response2;
                        }
                    }
                }
                else
                {
                    RemovePlayerFromTeamButton.Text = $"no success";
                }
            }
            catch (Exception ex)
            {
                RemovePlayerFromTeamButton.Text = $"Clicked and failed " + ex;
            }
        }

        teamIDRemovePlayer = null;
        playerIDRemoveFromTeam = null;
    }

    // Event Handlers for Text Changes
    private void TeamNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        teamName = e.NewTextValue;
    }

    private void TeamAbbreviationEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        teamAbbreviation = e.NewTextValue;
    }

    private void TeamWinsEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        wins = e.NewTextValue;
    }

    private void TeamLossesEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        losses = e.NewTextValue;
    }

    private void TeamIDEdit_TextChanged(object sender, TextChangedEventArgs e)
    {
        teamID = e.NewTextValue;
    }

    private void TeamIDAddPlayer_TextChanged(object sender, TextChangedEventArgs e)
    {
        teamIDAddPlayer = e.NewTextValue;
    }

    private void PlayerIDAddToTeam_TextChanged(object sender, TextChangedEventArgs e)
    {
        playerIDAddToTeam = e.NewTextValue;
    }

    private void TeamIDRemovePlayer_TextChanged(object sender, TextChangedEventArgs e)
    {
        teamIDRemovePlayer = e.NewTextValue;
    }

    private void PlayerIDRemoveFromTeam_TextChanged(object sender, TextChangedEventArgs e)
    {
        playerIDRemoveFromTeam = e.NewTextValue;
    }
}