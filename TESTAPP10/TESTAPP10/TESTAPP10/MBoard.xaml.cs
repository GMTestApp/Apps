using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TESTAPP10
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MBoard : ContentPage
	{
		public MBoard ()
		{
			InitializeComponent ();
            date.Text = DateTime.Now.ToString("MMMM dd yyyy");
            Nextpage();

        }
        public MBoard(string str)
        {
            InitializeComponent();
            date.Text = DateTime.Now.ToString("MMMM dd yyyy");
            Nextpage();
        }
        public MBoard(string username, string password)
        {
            InitializeComponent();
            date.Text = DateTime.Now.ToString("MMMM dd yyyy");
            lblwlcm.Text = "Welcome " + username;
            contentload(username, password);
            Nextpage();
        }

        public async void Nextpage()
        {
            try
            {
                ListMBoard.ItemTapped += async (sender, e) =>
            {

                MB_Manifest details = e.Item as MB_Manifest;
                string values = details.ID.ToString();
                await Navigation.PushAsync(new SuccessPage(values));

            };

            }
            catch (Exception empty)
            {
                await DisplayAlert("", empty.Message, "OK");
                return;
            }
        }

        public async void contentload(string username, string password)
        {
            try
            {

                var customer = from s in App.SqlLiteCon().Table<Customer>().Where(s => s.UserId == username).Where(s => s.Password == password) select s;

                var InviteCode = "";
                var Type = "";
                var CompanyId = "";

                foreach (var c in customer)
                {
                    Type = c.Type;
                    InviteCode = c.XCode;
                    CompanyId = c.CompanyID;
                    break;
                }
                var resp = App.SOAP_Request.MBoardData(username.Trim(), InviteCode, Type, CompanyId);

               //resp= "{ \"Manifest\": [{ \"ID\": \"12345685\", \"SeqNo\": \"1\", \"Count\": \"10\" }, { \"ID\": \"12552185\", \"SeqNo\": \"2\", \"Count\": \"5\" }, { \"ID\": \"122225\", \"SeqNo\": \"3\", \"Count\": \"3\" } ] }";

                if (resp.Contains("\"Manifest\":"))
                {

                    MB_RootObject response = JsonConvert.DeserializeObject<MB_RootObject>(resp);
                    ListMBoard.ItemsSource = response.Manifest;

                }
                else
                {
                    await DisplayAlert("", resp, "OK");
                }
            }
            catch(Exception f)
            {
                await DisplayAlert("", f.Message, "OK");
            }

        }
        private async void Signout_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void Sigout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void Lnksignin_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

       

        private void ListMBoard_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }

    public class AlternateColorDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EvenTemplate { get; set; }
        public DataTemplate UnevenTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // TODO: Maybe some more error handling here
            return ((List<string>)((ListView)container).ItemsSource).IndexOf(item as string) % 2 == 0 ? EvenTemplate : UnevenTemplate;
        }
    }

    public class MB_Manifest
    {
        public string ID { get; set; }
        public string SeqNo { get; set; }
        public string Count { get; set; }
    }

    public class MB_RootObject
    {
        public List<MB_Manifest> Manifest { get; set; }
    }
}