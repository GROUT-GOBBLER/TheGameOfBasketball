using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using basketballUI.models;
using Newtonsoft.Json;
namespace basketballUI
{
    public partial class MainPage: TabbedPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked (object sender, EventArgs e)
        {
            Navigation.PushAsync(new PlayerSelect());
        }

    }

}
