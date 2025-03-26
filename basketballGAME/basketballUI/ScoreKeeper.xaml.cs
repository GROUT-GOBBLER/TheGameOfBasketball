using CommunityToolkit.Maui.Views;
using Windows.Media.Playlists;
using basketballUI.models;
using Newtonsoft.Json;
namespace basketballUI;

public partial class ScoreKeeper : ContentPage
{
    private List<Player> playerList;

    public ScoreKeeper()
    {
        InitializeComponent();
    }
    public void UpdatePlayerList(List<Player> players)
    {
        playerList = players;
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
        }, SavedList.CurrentPlayerList);

        await this.ShowPopupAsync(popup);
    }

    private async void OnFreeThrowClicked(object sender, EventArgs e)
    {

        var popup = new PlayerSelect("Free Throw", selectedPlayer =>
        {

        }, SavedList.CurrentPlayerList);

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