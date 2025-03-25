using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;

namespace basketballUI;

public partial class PlayerSelect : ContentPage
{
    private string _lastAction;
    private Action<string> _onPlayerSelected;

    public PlayerSelect(string lastAction, Action<string> onPlayerSelected)
    {
        InitializeComponent();
        _lastAction = lastAction;
        _onPlayerSelected = onPlayerSelected;
        LoadPlayers();
    }

    private async void LoadPlayers()
    {
        string API_URL = "http://localhost:5121/api/Players";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(API_URL);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<Player> players = JsonConvert.DeserializeObject<List<Player>>(json);
                    PopulatePlayerGrid(players);
                }
                else
                {
                    await DisplayAlert("Error", "Failed to load players", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load players: {ex.Message}", "OK");
            }
        }
    }

    private void PopulatePlayerGrid(List<Player> players)
    {
        PlayerGrid.RowDefinitions.Clear();
        PlayerGrid.Children.Clear();

        int buttonsPerRow = 4;
        int rowCount = (int)Math.Ceiling((double)players.Count / buttonsPerRow);

        for (int i = 0; i < rowCount; i++)
        {
            PlayerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            int row = i / buttonsPerRow;
            int column = i % buttonsPerRow;

            var button = new Button
            {
                Text = $"Player {player.PlayerNo}",
                WidthRequest = PlayerGrid.Width,
                // HeightRequest = 10,
                Margin = 10
            };

            button.Clicked += async (sender, e) =>
            {
                string selectedPlayer = player.PlayerNo.ToString();
                _onPlayerSelected?.Invoke(selectedPlayer);

                if (_lastAction == "Free Throw")
                {
                    // Navigate to FreeThrow page
                    await Navigation.PushAsync(new FreeThrow(selectedPlayer));
                }
                else if (!string.IsNullOrEmpty(_lastAction))
                {
                    await DisplayAlert("Action Recorded", $"Player {selectedPlayer} performed: {_lastAction}", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await Navigation.PopAsync();
                }

                    
            };

            PlayerGrid.Children.Add(button);
            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}