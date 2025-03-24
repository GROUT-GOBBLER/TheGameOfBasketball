using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;

namespace basketballUI
{
    public partial class MainPage: TabbedPage
    {
        private string _selectedAction;
        public MainPage()
        {
            InitializeComponent();
        }

        async private void Reload_Clicked(object sender, EventArgs e) {
            
            string API_URL = "http://localhost:5121/api/Players";

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
                        ReloadBtn.Text = $"Player num " + players[0].PlayerNo + " First name " + players[0].FName + " Last name " + players[0].LName;

                        statsList.ItemsSource = players;
                    }
                }
                catch (Exception ex)
                {
                    ReloadBtn.Text = $"Clicked and failed";
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayerSelect());
           
        }

        private async void GoToScoreKeeper_Clicked(object sender, EventArgs e)
        {
            if (Navigation != null)
            {
                await Navigation.PushAsync(new ScoreKeeper());
            }
            else
            {
                await DisplayAlert("Error", "Navigation is not available", "OK");
            }
        }

    }

}
