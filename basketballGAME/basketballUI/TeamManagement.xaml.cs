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
        string API_URL = "http://localhost:5121/api/Teams";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(API_URL);
                List<Team> teams;

                if (response.IsSuccessStatusCode)
                {
                    string json1 = await response.Content.ReadAsStringAsync();
                    teams = JsonConvert.DeserializeObject<List<Team>>(json1);

                    Team team = new Team();

                    int teamNumber = int.Parse(teamID);
                    team.TeamName = teamName;
                    team.TeamAbbreviation = teamAbbreviation;
                    team.Wins = short.Parse(wins);
                    team.Losses = short.Parse(losses);
                    team.GameTeamNoOneNavigations = new List<Game>();
                    team.GameTeamNoTwoNavigations = new List<Game>();
                    team.TeamPlayers = new List<TeamPlayer>();

                    HttpResponseMessage updateResponse = await client.PutAsJsonAsync($"{API_URL}/{teamNumber}", team);

                    if (updateResponse.IsSuccessStatusCode)
                    {
                        EditTeamButton.Text = "Team updated successfully.";
                    }
                    else
                    {
                        EditTeamButton.Text = $"Failed to update team: " + updateResponse;
                    }
                }
                else
                {
                    EditTeamButton.Text = $"Failed to fetch team: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                EditTeamButton.Text = $"Error: {ex.Message}";
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
        string API_URL = "http://localhost:5121/api/Teams/";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                if (string.IsNullOrEmpty(teamID) || !int.TryParse(teamID, out int teamNo))
                {
                    ViewTeamButton.Text = "Invalid Team ID.";
                    return;
                }

                HttpResponseMessage response = await client.GetAsync($"{API_URL}{teamNo}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Team team = JsonConvert.DeserializeObject<Team>(json);

                    if (team != null)
                    {
                        ViewTeamButton.Text = $"Team: {team.TeamName}, Abbr: {team.TeamAbbreviation}, W: {team.Wins}, L: {team.Losses}";
                    }
                    else
                    {
                        ViewTeamButton.Text = "Team not found.";
                    }
                }
                else
                {
                    ViewTeamButton.Text = $"Failed to fetch team: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                ViewTeamButton.Text = $"Error: {ex.Message}";
            }
        }

   
        teamID = null;
    }

 
    async private void AddPlayerToTeamButton_Clicked(object sender, EventArgs e)
    {
        string API_URL_TEAMS = "http://localhost:5121/api/Teams/";
        string API_URL_PLAYERS = "http://localhost:5121/api/Players/";
        string API_URL_TEAMPLAYERS = "http://localhost:5121/api/TeamPlayers/";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                
                if (string.IsNullOrEmpty(teamIDAddPlayer) || !int.TryParse(teamIDAddPlayer, out int teamNo))
                {
                    AddPlayerToTeamButton.Text = "Invalid Team ID.";
                    return;
                }

                if (string.IsNullOrEmpty(playerIDAddToTeam) || !int.TryParse(playerIDAddToTeam, out int playerNo))
                {
                    AddPlayerToTeamButton.Text = "Invalid Player ID.";
                    return;
                }

               
                HttpResponseMessage teamResponse = await client.GetAsync($"{API_URL_TEAMS}{teamNo}");
                if (!teamResponse.IsSuccessStatusCode)
                {
                    AddPlayerToTeamButton.Text = $"Team not found: {teamResponse.StatusCode}";
                    return;
                }

                
                HttpResponseMessage playerResponse = await client.GetAsync($"{API_URL_PLAYERS}{playerNo}");
                if (!playerResponse.IsSuccessStatusCode)
                {
                    AddPlayerToTeamButton.Text = $"Player not found: {playerResponse.StatusCode}";
                    return;
                }

                
                HttpResponseMessage teamPlayersResponse = await client.GetAsync(API_URL_TEAMPLAYERS);
                if (teamPlayersResponse.IsSuccessStatusCode)
                {
                    string json = await teamPlayersResponse.Content.ReadAsStringAsync();
                    List<TeamPlayer> teamPlayers = JsonConvert.DeserializeObject<List<TeamPlayer>>(json);

                    foreach (var teamPlayer in teamPlayers)
                    {
                        if (teamPlayer.TeamId == teamNo && teamPlayer.PlayerId == playerNo)
                        {
                            AddPlayerToTeamButton.Text = "Player is already in this team.";
                            return;
                        }
                    }

                   
                    TeamPlayer newTeamPlayer = new TeamPlayer
                    {
                        Id = teamPlayers.Count > 0 ? teamPlayers[teamPlayers.Count - 1].Id + 1 : 1,
                        TeamId = teamNo,
                        PlayerId = playerNo,
                        Player = null,
                        Team = null,
                        Stats = new List<Stat>()
                    };

                    HttpResponseMessage addResponse = await client.PostAsJsonAsync(API_URL_TEAMPLAYERS, newTeamPlayer);

                    if (addResponse.IsSuccessStatusCode)
                    {
                        AddPlayerToTeamButton.Text = "Player added to team successfully.";
                    }
                    else
                    {
                        AddPlayerToTeamButton.Text = $"Failed to add player to team: {addResponse.StatusCode}";
                    }
                }
                else
                {
                    AddPlayerToTeamButton.Text = $"Failed to fetch team players: {teamPlayersResponse.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                AddPlayerToTeamButton.Text = $"Error: {ex.Message}";
            }
        }


        teamIDAddPlayer = null;
        playerIDAddToTeam = null;
    }


    async private void RemovePlayerFromTeamButton_Clicked(object sender, EventArgs e)
    {
        string API_URL_TEAMPLAYERS = "http://localhost:5121/api/TeamPlayers/";

        using (HttpClient client = new HttpClient())
        {
            try
            {

                if (string.IsNullOrEmpty(teamIDRemovePlayer) || !int.TryParse(teamIDRemovePlayer, out int teamNo))
                {
                    RemovePlayerFromTeamButton.Text = "Invalid Team ID.";
                    return;
                }

                if (string.IsNullOrEmpty(playerIDRemoveFromTeam) || !int.TryParse(playerIDRemoveFromTeam, out int playerNo))
                {
                    RemovePlayerFromTeamButton.Text = "Invalid Player ID.";
                    return;
                }


                HttpResponseMessage teamPlayersResponse = await client.GetAsync(API_URL_TEAMPLAYERS);
                if (teamPlayersResponse.IsSuccessStatusCode)
                {
                    string json = await teamPlayersResponse.Content.ReadAsStringAsync();
                    List<TeamPlayer> teamPlayers = JsonConvert.DeserializeObject<List<TeamPlayer>>(json);

                    TeamPlayer teamPlayerToRemove = null;
                    foreach (var teamPlayer in teamPlayers)
                    {
                        if (teamPlayer.TeamId == teamNo && teamPlayer.PlayerId == playerNo)
                        {
                            teamPlayerToRemove = teamPlayer;
                            break;
                        }
                    }

                    if (teamPlayerToRemove == null)
                    {
                        RemovePlayerFromTeamButton.Text = "Player is not in this team.";
                        return;
                    }

                    // Delete the TeamPlayer entry
                    HttpResponseMessage deleteResponse = await client.DeleteAsync($"{API_URL_TEAMPLAYERS}{teamPlayerToRemove.Id}");

                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        RemovePlayerFromTeamButton.Text = "Player removed from team successfully.";
                    }
                    else
                    {
                        RemovePlayerFromTeamButton.Text = $"Failed to remove player from team: {deleteResponse.StatusCode}";
                    }
                }
                else
                {
                    RemovePlayerFromTeamButton.Text = $"Failed to fetch team players: {teamPlayersResponse.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                RemovePlayerFromTeamButton.Text = $"Error: {ex.Message}";
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