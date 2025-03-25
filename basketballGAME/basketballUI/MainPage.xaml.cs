using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;

namespace basketballUI
{
    public partial class MainPage: TabbedPage
    {
        private string _selectedPlayer;
        private string _lastAction;
        private List<Player> playerList;
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
                        // ReloadBtn.Text = $"Player num " + players[0].PlayerNo + " First name " + players[0].FName + " Last name " + players[0].LName;
                        

                        statsList.ItemsSource = players;
                        playerList = players;
                        ReloadBtn.Text = $"Number of players in the list: " + playerList.Count;
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
            //await Navigation.PushAsync(new PlayerSelect());
            await Navigation.PushAsync(new PlayerSelect("Select Player", selectedPlayer =>
            {
                // Callback when a player is selected
                // You can add logic here if needed
            }));
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

        private async void OnActionButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            _lastAction = button?.Text;

            if (playerList != null)
            {
                // Navigate to PlayerSelect page, passing the last action and a callback
                await Navigation.PushAsync(new PlayerSelect(_lastAction, selectedPlayer =>
                {
                    _selectedPlayer = selectedPlayer;
                    // Callback when a player is selected
                    if (!string.IsNullOrEmpty(_selectedPlayer))
                    {
                        DisplayAlert("Action Recorded", $"{_selectedPlayer} performed: {_lastAction}", "OK");
                    }
                }));
            }
            else
            {
                await DisplayAlert("NOT WORKING", "NO PLAYERS DETECTED: RELOAD THE PLAYERS", "OK");
            }
        }

        private async void OnFreeThrowClicked(object sender, EventArgs e)
        {
            if (playerList != null)
            {
                // Navigate to PlayerSelect page for free throw
                await Navigation.PushAsync(new PlayerSelect("Free Throw", selectedPlayer =>
                {
                    _selectedPlayer = selectedPlayer;
                    // Callback when a player is selected
                    if (!string.IsNullOrEmpty(_selectedPlayer))
                    {
                        // Navigate to FreeThrow page (as already handled in PlayerSelect.xaml.cs)
                        // No additional action needed here since PlayerSelect handles it
                    }
                }));
            }
            else
            {
                await DisplayAlert("NOT WORKING", "NO PLAYERS DETECTED: RELOAD THE PLAYERS", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }

}
