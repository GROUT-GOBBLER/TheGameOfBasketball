using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;

namespace basketballUI;

public partial class FreeThrow : ContentPage
{
    private string selectedPlayer;
    private int shotCount;
    private int shotsTaken;
    private int shotsMade;

    public FreeThrow(string selectedPlayer)
    {
        InitializeComponent();
        this.selectedPlayer = selectedPlayer;
        shotCount = 0;
        shotsTaken = 0;
        shotsMade = 0;

        MissButton.IsEnabled = false;
        MadeButton.IsEnabled = false;
    }

    private void OnShotCountClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            shotCount = int.Parse(button.Text);
            shotsTaken = 0;
            shotsMade = 0;

            MissButton.IsEnabled = true;
            MadeButton.IsEnabled = true;

            OneShotButton.BackgroundColor = button == OneShotButton ? Color.FromArgb("#D3D3D3") : Color.FromArgb("#E0E0E0");
            TwoShotsButton.BackgroundColor = button == TwoShotsButton ? Color.FromArgb("#D3D3D3") : Color.FromArgb("#E0E0E0");
            ThreeShotsButton.BackgroundColor = button == ThreeShotsButton ? Color.FromArgb("#D3D3D3") : Color.FromArgb("#E0E0E0");
        }
    }

    private async void OnMissClicked(object sender, EventArgs e)
    {
        if (shotsTaken < shotCount)
        {
            shotsTaken++;
            await DisplayAlert("Free Throw Recorded", $"Player {selectedPlayer} missed free throw {shotsTaken} of {shotCount}", "OK");

            if (shotsTaken == shotCount)
            {
                await Navigation.PopAsync();
            }
        }
    }

    private async void OnMadeClicked(object sender, EventArgs e)
    {
        if (shotsTaken < shotCount)
        {
            shotsTaken++;
            shotsMade++;
            await DisplayAlert("Free Throw Recorded", $"Player {selectedPlayer} made free throw {shotsTaken} of {shotCount}", "OK");

            if (shotsTaken == shotCount)
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