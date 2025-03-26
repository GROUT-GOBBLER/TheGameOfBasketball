using CommunityToolkit.Maui.Views;

namespace basketballUI;

public partial class ScoreKeeper : ContentPage
{
    public ScoreKeeper()
    {
        InitializeComponent();
    }

    private async void OnActionButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        string lastAction = button?.Text;
        //await Navigation.PushAsync(new PlayerSelect(lastAction));
        var popup = new PlayerSelect(lastAction, selectedPlayer =>
        {
            if (!string.IsNullOrEmpty(selectedPlayer))
            {
                DisplayAlert("Action Recorded", $"Player {selectedPlayer} performed: {lastAction}", "OK");
            }
        });

        await this.ShowPopupAsync(popup);
    }

    private async void OnFreeThrowClicked(object sender, EventArgs e)
    {

        var popup = new PlayerSelect("Free Throw", selectedPlayer =>
        {

        });

        var result = await this.ShowPopupAsync(popup);

        if (result is string selectedPlayer && !string.IsNullOrEmpty(selectedPlayer))
        {
            await Navigation.PushAsync(new FreeThrow(selectedPlayer));
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}