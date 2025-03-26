using basketballUI.models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
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

    string gameID;

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

                List<Schedule> schedules;
                List<Game> games;

                if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {
                    string json1 = await response1.Content.ReadAsStringAsync();
                    schedules = JsonConvert.DeserializeObject<List<Schedule>>(json1);

                    string json2 = await response2.Content.ReadAsStringAsync();
                    games = JsonConvert.DeserializeObject<List<Game>>(json2);

                    Game game = new Game();
                    Schedule schedule = new Schedule();

                    //GAME
                    game.GameNo = games[games.Count - 1].GameNo + 1;
                    game.ScoreOne = 0;
                    game.ScoreTwo = 0;

                    if (teamOne != null)
                    {
                        game.TeamNoOne = int.Parse(teamOne);
                    }
                    else return;
                    if (teamTwo != null)
                    {
                        game.TeamNoTwo = int.Parse(teamTwo);
                    }
                    else return;

                    game.Stats = new List<Stat>();
                    game.TeamNoOneNavigation = null;
                    game.TeamNoTwoNavigation = null;

                    HttpResponseMessage response3 = await client.PostAsJsonAsync(API_URL_2, game);
                    //CreateGameButton.Text = JsonConvert.SerializeObject(game);

                    //SCHEDULE
                    if (date != null)
                    {
                        schedule.GameDate = date;
                    }
                    else return;
                    if (time != null)
                    {
                        schedule.GameTime = time;
                    }
                    else return;

                    schedule.GameNo = game.GameNo;

                    if (city != null)
                    {
                        schedule.City = city;
                    }
                    else return;
                    if (state != null)
                    {
                        schedule.State = state;
                    }
                    else return;
                    if (zip != null)
                    {
                        schedule.Zipcode = zip;
                    }
                    else return;

                    HttpResponseMessage response4 = await client.PostAsJsonAsync(API_URL_1, schedule);

                    if (response3.IsSuccessStatusCode)
                    {
                        if (response4.IsSuccessStatusCode)
                            CreateGameButton.Text = $"both success ";
                        else
                            CreateGameButton.Text = $"3 success 4 fail " + response4;
                    }
                    else
                    {
                        if (response4.IsSuccessStatusCode)
                            CreateGameButton.Text = $"3 fail 4 success " + response3;
                        else
                            CreateGameButton.Text = $"both fail " + response3 + "\n\n" + response4;
                    }
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

    async private void Delete_Schedule_Button_Click(object sender, EventArgs e)
    {
        string API_URL_2 = "http://localhost:5121/api/Games";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response3 = await client.DeleteAsync($"{API_URL_2}/{gameID}");

                if (response3.IsSuccessStatusCode)
                {
                    DeleteGameButton.Text = "Deleted";
                }
            }
            catch (Exception ex)
            {
                DeleteGameButton.Text = $"Clicked and failed " + ex;
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

    private void GameIDEdit_TextChanged(object sender, TextChangedEventArgs e)
    {
        gameID = e.NewTextValue;
    }

}