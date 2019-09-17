using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using static TESTAPP10.ShipmentDetails;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace TESTAPP10
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cargo : ContentPage
    {
        public Cargo()
        {
            InitializeComponent();


            var HAWB = Application.Current.Properties.ContainsKey("carHAWB") ? Application.Current.Properties["carHAWB"] as string : "";
            var MoveType = Application.Current.Properties.ContainsKey("carMoveType") ? Application.Current.Properties["carMoveType"] as string : "";
            var ServiceDate = Application.Current.Properties.ContainsKey("carManifestNo") ? Application.Current.Properties["carManifestNo"] as string : "";


            LoadValues(HAWB, MoveType, ServiceDate);


        }
        public Cargo(string HAWB, string MoveType, string ServiceDate)
        {
            InitializeComponent();
            //date.Text = DateTime.Now.ToString("MMM dd yyyy");

            //lblhawb.Text = HAWB;
            //lblmovetype.Text = MoveType;
            //lblservicedate.Text = ServiceDate;
            //string Type = Application.Current.Properties["Type"].ToString();
            //if (Type.ToLower() == "m")
            //{
            //    lblmanif.IsVisible = true;
            //    lblship.IsVisible = false;
            //}
            //else
            //{
            //    lblmanif.IsVisible = false;
            //    lblship.IsVisible = true;
            //}
            //var username = Application.Current.Properties["username"];
            //lblwlcm.Text = "Welcome " + username;



            LoadValues(HAWB, MoveType, ServiceDate);
        }

        public void LoadValues(string HAWB, string MoveType, string ServiceDate)
        {

            Application.Current.Properties.Remove("ImageStramList");


            Application.Current.Properties["carHAWB"] = HAWB;
            Application.Current.Properties["carMoveType"] = MoveType;
            Application.Current.Properties["carManifestNo"] = ServiceDate;


            date.Text = DateTime.Now.ToString("MMM dd yyyy");

            lblhawb.Text = "Shipment # " + HAWB;
            lblmovetype.Text = "Move Type : " + MoveType;
            lblservicedate.Text = "Service Date : " + ServiceDate;
            string Type = Application.Current.Properties["Type"].ToString();
            if (Type.ToLower() == "m")
            {
                lblmanif.IsVisible = true;
                lblship.IsVisible = false;
            }
            else
            {
                lblmanif.IsVisible = false;
                lblship.IsVisible = true;
            }
            var username = Application.Current.Properties["username"];
            lblwlcm.Text = "Welcome " + username;


            var resp = Application.Current.Properties.ContainsKey("LoadResponse") ? Application.Current.Properties["LoadResponse"] as string : "";

            SD_RootObject response = JsonConvert.DeserializeObject<SD_RootObject>(resp);

            foreach (var a in response.Details)
            {
                lbltkpicno.Text = "Total Files Uploaded (" + a.Pic + ")";
                notes.Text = a.DCargoNotes;
            }
        }


        private async void Sigout_Clicked(object sender, EventArgs e)
        {
            App.CloseLoginBackThread();
            await Navigation.PushAsync(new MainPage());
        }

        private async void Signout_Tapped(object sender, EventArgs e)
        {
            App.CloseLoginBackThread();
            await Navigation.PushAsync(new MainPage());
        }

  

        private async void Signin_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void Manifests_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MBoardItemDetails());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShipmentDetails());
        }

        private async void TGlblship_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SBoardDataDetails());
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShipmentDetails());
        }



        private async void Btnsave_Clicked(object sender, EventArgs e)
        {
            try
            {
                var note = notes.Text.Trim();

                var HAWB = Application.Current.Properties.ContainsKey("carHAWB") ? Application.Current.Properties["carHAWB"] as string : "";
                var MoveType = Application.Current.Properties.ContainsKey("carMoveType") ? Application.Current.Properties["carMoveType"] as string : "";
                var ServiceDate = Application.Current.Properties.ContainsKey("carManifestNo") ? Application.Current.Properties["carManifestNo"] as string : "";
                var inprogress = Application.Current.Properties.ContainsKey("inProgess") ? Application.Current.Properties["inProgess"] as string : "";

                var customer = from s in App.SqlLiteCon().Table<Customer>() select s;

                var username = "";
                var CompanyId = "";
                var InviteCode = "";
                var Url = "";


                var DGargo = "";
                var RefNo = "";
                
                foreach (var c in customer)
                {
                    username = c.UserId;
                    InviteCode = c.XCode;
                    CompanyId = c.CompanyID;
                    Url = c.TransactURL;
                    break;
                }

                var resp = Application.Current.Properties.ContainsKey("LoadResponse") ? Application.Current.Properties["LoadResponse"] as string : "";

                SD_RootObject response = JsonConvert.DeserializeObject<SD_RootObject>(resp);

                foreach (var a in response.Details)
                {
                    DGargo = a.DCargo;
                    RefNo = a.RefNo;
                   
                }
                if (inprogress.ToLower() != "y")
                {
                    await DisplayAlert("", "This shipment is not In-progress, cannot save any updates.", "OK");
                    return;
                }
                if (!String.IsNullOrEmpty(note))
                {
                    var UCresp = App.SOAP_Request.UpdateDCargoNotes(RefNo, HAWB, note, username.Trim(), CompanyId, InviteCode, Url);

                    Cargo_RootObject Btnshipresponse = JsonConvert.DeserializeObject<Cargo_RootObject>(UCresp);

                    if (Btnshipresponse.Details[0].Message.ToLower().Contains("ok"))
                        await DisplayAlert("", "Notes Updated.", "OK");
                    else
                        await DisplayAlert("", "Notes could not be Updated.", "OK");
                }

                List<string> ISL = new List<string>();

                if (Application.Current.Properties.ContainsKey("ImageStramList"))
                    ISL = Application.Current.Properties["ImageStramList"] as List<string>;
                string imgcount = ISL.Count.ToString();


                foreach (var val in ISL)
                {
                    var UCIresp = App.SOAP_Request.Uploadimgs(RefNo, HAWB, val, username.Trim(), CompanyId, InviteCode, Url);

                    Cargo_RootObject Btnshipresp = JsonConvert.DeserializeObject<Cargo_RootObject>(UCIresp);

                    if (Btnshipresp.Details[0].Message.ToLower().Contains("ok"))
                    {
                        if (Btnshipresp.Details[0].Count != "0")
                            lbltkpicno.Text = "Total Files Uploaded (" + Btnshipresp.Details[0].Count + ")";
                    }
                }

                Application.Current.Properties.Remove("ImageStramList");

            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
            

        }

        private void Btnreset_Clicked(object sender, EventArgs e)
        {
            notes.Text = string.Empty;
            Application.Current.Properties.Remove("ImageStramList");

        }

        private  void Btncamera_Clicked(object sender, EventArgs e)
        {
            loadimg();

        }
        public async void loadimg()
        {


            try
            {

                var inprogress = Application.Current.Properties.ContainsKey("inProgess") ? Application.Current.Properties["inProgess"] as string : "";
                if (inprogress.ToLower() != "y")
                {
                    await DisplayAlert("", "This shipment is not In-progress, cannot save any updates.", "OK");
                    return;
                }



                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await DisplayAlert("Camera Permission", "Allow SavR to access your camera", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
                    status = results[Permission.Camera];
                }

                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "uploadImage",
                    SaveToAlbum = false,
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    SaveMetaData = false,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Rear
                });
              
                if (file == null)
                    return;


                string imgBase64String = GetBase64StringForImage(file.Path);

                try
                {
                    string OriginalFilePath = file.Path;
                    file.Dispose();
                    if (File.Exists(OriginalFilePath))
                    {
                        System.IO.File.Delete(OriginalFilePath);
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("", ex.Message, "OK");
                }



                List<string> ISL = new List<string>();

                if (Application.Current.Properties.ContainsKey("ImageStramList"))
                    ISL = Application.Current.Properties["ImageStramList"] as List<string>;


                ISL.Add(imgBase64String);

                Application.Current.Properties["ImageStramList"] = ISL;

            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");

            }
          
        }
        protected string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        private void Imgpic_Tapped(object sender, EventArgs e)
        {
            loadimg();
        }
       
       
    }

    public class Cargo_Detail
    {
        public string Message { get; set; }
        public string ID { get; set; }
        public string UserId { get; set; }
        public string Count { get; set; }
    }

    public class Cargo_RootObject
    {
        public List<Cargo_Detail> Details { get; set; }
    }

   
}
