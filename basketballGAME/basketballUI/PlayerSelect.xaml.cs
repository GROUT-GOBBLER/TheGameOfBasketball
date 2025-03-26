using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;

using CommunityToolkit.Maui.Views;
using System;

namespace basketballUI;

public partial class PlayerSelect : Popup
{
    private string lastAction;
    private Action<string> onPlayerSelected;
   // private List<Player> players;


    public PlayerSelect(string lastAction, Action<string> onPlayerSelected, List<Player> players = null)
    {
        InitializeComponent();
        this.lastAction = lastAction;
        this.onPlayerSelected = onPlayerSelected;

        var widthOfScreen = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        var heightOfScreen = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        Size = new Size(widthOfScreen * 0.5, heightOfScreen * 0.6);

        if (players != null)
        {
            PopulatePlayerGrid(players);
        }
        else
        {
            LoadPlayers();
        }
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
                    Close();
                }
            }
            catch (Exception ex)
            {
                Close();
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
                Text = $"{player.FName}",
                WidthRequest = 180,
                HeightRequest = 100,
                FontSize = 22,
                Margin = 0,
                Padding = 0,
                BorderWidth = 3,
                BorderColor = Color.FromArgb("#000000"),
                BackgroundColor = Color.FromArgb("#A9A9A9")
            };


            button.Clicked += async (sender, e) =>
            {
                string selectedPlayer = player.PlayerNo.ToString();
                onPlayerSelected?.Invoke(selectedPlayer);

                if (lastAction == "Free Throw")
                {
                    Close(selectedPlayer);
                }
                else
                {
                    Close(selectedPlayer);
                }
                

            };

            PlayerGrid.Children.Add(button);
            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);
        }
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        Close();
    }
}