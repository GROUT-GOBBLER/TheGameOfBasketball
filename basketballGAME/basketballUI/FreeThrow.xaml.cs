using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;

namespace basketballUI;

public partial class FreeThrow : ContentPage
{
    private string _selectedPlayer;
    private int _shotCount;
    private int _shotsTaken;
    private int _shotsMade;

    public FreeThrow(string selectedPlayer)
    {
        InitializeComponent();
        _selectedPlayer = selectedPlayer;
        _shotCount = 0;
        _shotsTaken = 0;
        _shotsMade = 0;

        // Initially disable Miss and Made buttons until a shot count is selected
        MissButton.IsEnabled = false;
        MadeButton.IsEnabled = false;
    }

    private void OnShotCountClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            _shotCount = int.Parse(button.Text);
            _shotsTaken = 0;
            _shotsMade = 0;

            // Enable Miss and Made buttons
            MissButton.IsEnabled = true;
            MadeButton.IsEnabled = true;

            // Highlight the selected shot count button
            OneShotButton.BackgroundColor = button == OneShotButton ? Color.FromArgb("#D3D3D3") : Color.FromArgb("#E0E0E0");
            TwoShotsButton.BackgroundColor = button == TwoShotsButton ? Color.FromArgb("#D3D3D3") : Color.FromArgb("#E0E0E0");
            ThreeShotsButton.BackgroundColor = button == ThreeShotsButton ? Color.FromArgb("#D3D3D3") : Color.FromArgb("#E0E0E0");
        }
    }

    private async void OnMissClicked(object sender, EventArgs e)
    {
        if (_shotsTaken < _shotCount)
        {
            _shotsTaken++;
            await DisplayAlert("Free Throw Recorded", $"Player {_selectedPlayer} missed free throw {_shotsTaken} of {_shotCount}", "OK");

            if (_shotsTaken == _shotCount)
            {
                await Navigation.PopAsync();
            }
        }
    }

    private async void OnMadeClicked(object sender, EventArgs e)
    {
        if (_shotsTaken < _shotCount)
        {
            _shotsTaken++;
            _shotsMade++;
            await DisplayAlert("Free Throw Recorded", $"Player {_selectedPlayer} made free throw {_shotsTaken} of {_shotCount}", "OK");

            if (_shotsTaken == _shotCount)
            {
                await Navigation.PopAsync();
            }
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}