using basketballUI.models;
using Newtonsoft.Json;
using System.Net.Http.Json;
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

	public ScheduleManagement()
	{
        InitializeComponent();
	}

    async private void Create_Schedule_Button_Click(object sender, EventArgs e) { }

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

}