using basketballUI.models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Windows.Media.AppBroadcasting;

namespace basketballUI;

public partial class ScheduleManagement : ContentPage
{
    string date;
    string time;
    string teamOne;
    string teamTwo;
    string city;
    string state;
    string zip;

	public ScheduleManagement()
	{
        InitializeComponent();
	}

    async private void Create_Schedule_Button_Click(object sender, EventArgs e) {
        string API_URL_1 = "http://localhost:5121/api/Schedules";
        string API_URL_2 = "http://localhost:5121/api/Games";

        using (HttpClient client = new HttpClient())
        {
            try {
                HttpResponseMessage response1 = await client.GetAsync(API_URL_1);
                HttpResponseMessage response2 = await client.GetAsync(API_URL_2);

                List<Team> teams;
                List<Game> games;

                if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {
                    string json1 = await response1.Content.ReadAsStringAsync();
                    teams = JsonConvert.DeserializeObject<List<Team>>(json1);

                    string json2 = await response2.Content.ReadAsStringAsync();
                    games = JsonConvert.DeserializeObject<List<Game>>(json2);

                    Player player = new Player();
                    int length1 = teams.Count;

                    TeamPlayer teamPlayer = new TeamPlayer();
                    int length2 = games.Count;

                    player.PlayerNo = teams[length1 - 1].PlayerNo + 1;
                    teamPlayer.Id = games[length2 - 1].Id + 1;
                }
                else
                {
                    CreateGameButton.Text = $"no success";
                }
            }
            catch (Exception ex) {
                CreateGameButton.Text = $"Clicked and failed " + ex;
            }
        }
    }

    private void DateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        date = e.NewTextValue;
    }

    private void TimeEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        time = e.NewTextValue;
    }

    private void TeamOneEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        teamOne = e.NewTextValue;
    }

    private void TeamTwoEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        teamTwo = e.NewTextValue;
    }

    private void CityEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        city = e.NewTextValue;
    }

    private void StateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        state = e.NewTextValue;
    }

    private void ZipEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        zip = e.NewTextValue;
    }

}