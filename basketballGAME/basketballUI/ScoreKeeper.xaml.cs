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
        // Create a new PlayerSelect instance with the required parameters
        await Navigation.PushAsync(new PlayerSelect(lastAction, selectedPlayer =>
        {
            // Callback when a player is selected
            // For non-free-throw actions, we can log the action here if needed
        }));
    }

    private async void OnFreeThrowClicked(object sender, EventArgs e)
    {
        // await Navigation.PushAsync(new PlayerSelect("Free Throw"));
        // Create a new PlayerSelect instance for free throw with the required parameters
        await Navigation.PushAsync(new PlayerSelect("Free Throw", selectedPlayer =>
        {
            // Callback when a player is selected
            // The free throw logic is handled in PlayerSelect, which navigates to FreeThrow
        }));
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}