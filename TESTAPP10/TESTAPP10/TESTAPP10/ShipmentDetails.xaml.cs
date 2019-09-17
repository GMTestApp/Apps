using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using Xamarin.Essentials;
using System.Threading;
using System.ComponentModel;

namespace TESTAPP10
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShipmentDetails : ContentPage
    {
        public ShipmentDetails()
        {
            InitializeComponent();

            var Hawb = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
            var Mtype = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
            var Date = Application.Current.Properties.ContainsKey("ShipDate") ? Application.Current.Properties["ShipDate"] as string : "";


           

            Loadvalues(Hawb, Mtype, Date);
            date.Text = DateTime.Now.ToString("MMM dd yyyy");

            
        }
        public ShipmentDetails(string hawb, string Mtype, string Date)
        {
            InitializeComponent();
            Loadvalues(hawb, Mtype, Date);
            date.Text = DateTime.Now.ToString("MMM dd yyyy");
            
        }
        
        private async void Loadvalues(string hawb, string Mtype, string Date)
        {

            try
            {
                Application.Current.Properties["ShipHawb"] = hawb;
                Application.Current.Properties["ShipMtype"] = Mtype;
                Application.Current.Properties["ShipDate"] = Date;


                date.Text = DateTime.Now.ToString("MMM dd yyyy");
                lblhawb.Text = "Shipment # " + hawb;
                lblmovetype.Text = "Move Type : " + Mtype;
                lblservicedate.Text = "Service Date : " + Date;

                var customer = from s in App.SqlLiteCon().Table<Customer>() select s;

                var username = "";
                var CompanyId = "";
                var type = "";
                var InviteCode = "";
                var Url = "";
                var Type = "";
                var Lat = "";
                var Long = "";
                var Status = "";
                var RefNo = "";
               

                foreach (var c in customer)
                {

                    username = c.UserId;
                    Type = c.Type;
                    lblwlcm.Text = "Welcome " + username;
                    Application.Current.Properties["username"] = username;
                    InviteCode = c.XCode;
                    CompanyId = c.CompanyID;
                    Url = c.TransactURL;
                    type = c.Type;
                    break;
                }

                var resp = App.SOAP_Request.LoadDetails(hawb, username.Trim(), Mtype, InviteCode, CompanyId, Url);

                //var resp = "{ \"Details\": [{ \"Message\": \"OK\", \"ID\": \"1253001\", \"Date\": \"01/01/2017\", \"Mtype\": \"Direct\", \"NoPieces\": \"10\", \"Wgt\": \"25.50 lbs\", \"Haz\": \"No\", \"SLine1\": \"2020 EXHIBITS\", \"SLine2\": \"10550 S. SAM HOUSTON PKWY W HOUSTON TX,77071\", \"CLine1\": \"36 CS MSG COMM-F1C344\", \"CLine2\": \"ARACELI PATAGUE APO,AP GUAM ,96543\", \"SP\": \"\", \"DCargo\": \"No\", \"DCargo\": \"No\", \"Pic\": \"5\" ,\"Status\": \"Out For Delivery\" ,\"Sign\": \"Ox920191jd200001j11010i11010111113f\" ,\"inProgress\": \"Y\" ,\"Action\": \"Out for Pickup, Arrived at Shipper, Picked Up, Delivered\" }] }";

                if (resp.Contains("\"Details\":"))
                {

                    SD_RootObject response = JsonConvert.DeserializeObject<SD_RootObject>(resp);
                    bool isdataexist = true;
                    foreach (var a in response.Details)
                    {
                       
                        if (a.Message.ToLower() != "ok")
                        {
                            isdataexist = false;
                            break;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(a.Date)))
                            a.Date = a.Date.Split(' ')[0];

                        Lat = a.SLine2;
                        Long = a.CLine2;
                        Status = a.Status;
                        RefNo = a.RefNo;

                        Application.Current.Properties["ShipRef"] = RefNo;

                        if ((!string.IsNullOrEmpty(a.inProgress)) && (a.inProgress.ToLower() == "y"))
                            btnship.BackgroundColor = (Color.FromHex("#B6D7A8"));
                        else
                            btnship.BackgroundColor = (Color.FromHex("#DDDDDD"));

                        if (a.DCargo.ToLower() == "y")
                            a.DCargo = "true";
                        else
                            a.DCargo = "false";

                        a.CLine2 = a.CLine2.Trim().Trim(',').Trim(' ');
                        a.SLine2 = a.SLine2.Trim().Trim(',').Trim(' ');

                        if (!string.IsNullOrEmpty(a.SP.Trim()))
                            a.SPBtnClr = "#B6D7A8";
                        else
                            a.SPBtnClr = "#DDDDDD";

                        Application.Current.Properties["inProgess"] = a.inProgress;
                        Application.Current.Properties["SP"] = a.SP;
                        Application.Current.Properties["ShipDCargoNotes"] = a.DCargoNotes;
                        if (!string.IsNullOrEmpty(a.Sign))
                        {
                            var im = Convert.FromBase64String(a.Sign);
                            Stream stream = new MemoryStream(im);
                            imgsign.Source = ImageSource.FromStream(() => { return stream; });
                            imgsign.IsVisible = true;
                        }
                    }

                    Application.Current.Properties["LoadResponse"] = resp;

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
                    Application.Current.Properties["Type"] = Type;
                    if (isdataexist)
                        shipdetails.ItemsSource = response.Details;
                }
                else
                {
                    await DisplayAlert("", resp, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }

        }


        public async void SendProgressCall(string RefNo, string HAWB, string lattitude, string longtitude, string UserId, string COMPANYID, string InviteCode, string Status, string Url, string TrackDateTime,string Seconds)
        {

         
        start:
            //var StopSend = Application.Current.Properties.ContainsKey("StopSend") ? Application.Current.Properties["StopSend"] as string : "";

            //if (StopSend.ToLower() == "true")
            //{
            //    Application.Current.Properties["StopSend"] = "";
            //    return;
            //}

            string shipmentid = "";
            var customer = from s in App.SqlLiteCon().Table<Customer>().Where(s => s.UserId == UserId) select s;

            foreach (var v in customer)
            {
                shipmentid = v.ActiveShipmentNo;
            }

            if(HAWB.Trim()!= shipmentid.Trim())
            {
                return;
            }
         

            try
            {


                try
                {
                    var requestsss = new GeolocationRequest(GeolocationAccuracy.Medium);
                    var locationsss = await Geolocation.GetLocationAsync(requestsss);


                    if (locationsss != null)
                    {
                        lattitude = locationsss.Latitude.ToString();
                        longtitude = locationsss.Longitude.ToString();
                    }

                }
                catch
                {
                    var locationd = await Geolocation.GetLastKnownLocationAsync();

                    if (locationd != null)
                    {
                        lattitude = locationd.Latitude.ToString();
                        longtitude = locationd.Longitude.ToString();
                    }
                    
                }




                //var locationd = await Geolocation.GetLastKnownLocationAsync();

                if ((lattitude!="0")&& (longtitude != "0"))
                {
                    //lattitude = locationd.Latitude.ToString();
                    //longtitude = locationd.Longitude.ToString();


                 

                     TrackDateTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-", "/");

                    var Sendresponse = App.SOAP_Request.SendProgress(RefNo, HAWB, lattitude, longtitude, UserId.Trim(), COMPANYID, InviteCode, Status, Url, TrackDateTime);
                    Btnship_RootObject Btnshipresponse = JsonConvert.DeserializeObject<Btnship_RootObject>(Sendresponse);
                    foreach (var re in Btnshipresponse.Details)
                    {
                        if (re.Message.ToLower() == "ok")
                        {
                            //await Task.Delay(60000);
                            await Task.Delay(Convert.ToInt32(!String.IsNullOrEmpty(Seconds)?Seconds: "60000"));
                            goto start;
                        }
                    }
                }
                else
                    return;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        private async void Signout_Tapped(object sender, EventArgs e)
        {
            App.CloseLoginBackThread();
            await Navigation.PushAsync(new MainPage());
        }

       

        private async void Sigout_Clicked(object sender, EventArgs e)
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

        private void Pickup_Tapped(object sender, EventArgs e)
        {
            var button = sender as Grid;
            if (button != null)
            {
                var childgrid = (Grid)button.Children[0];
                if (childgrid != null)
                {
                    Label label = (Label)childgrid.Children[0];

                    var clicked = label.Text;
                    if (!string.IsNullOrEmpty(clicked))
                        openmap(clicked.Trim());
                }
            }
        }

        private void Destination_Tapped(object sender, EventArgs e)
        {
            var button = sender as Grid;
            if (button != null)
            {
                var childgrid = (Grid)button.Children[0];
                if (childgrid != null)
                {
                    Label label = (Label)childgrid.Children[0];
                    var clicked = label.Text;
                    if (!string.IsNullOrEmpty(clicked))
                        openmap(clicked.Trim());
                }
            }
        }

        private void openmap(string address)
        {
            Uri url = null;
            Device.OnPlatform(iOS: () =>
            {
                url = new Uri(string.Format("http://maps.apple.com/maps?q={0}", address.Replace(" ", "+").Replace("\r", "+").Replace("\n", "+")));
            },
            Android: () =>
            {
                url = new Uri(string.Format("http://maps.google.com/maps?q={0}", address.Replace(" ", "+").Replace("\r", "+").Replace("\n", "+")));
            });

            Device.OpenUri(url);
        }

        private async void Spl_ins_Tapped(object sender, EventArgs e)
        {

            string HAWB = lblhawb.Text, MoveType = lblmovetype.Text, ServiceDate = lblservicedate.Text;


            //var HAWB = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
            //var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
            //var ServiceDate = Application.Current.Properties.ContainsKey("ShipDate") ? Application.Current.Properties["ShipDate"] as string : "";
            var SP = Application.Current.Properties.ContainsKey("SP") ? Application.Current.Properties["SP"] as string : "";
            await Navigation.PushAsync(new Spl_Instruction(HAWB, MoveType, ServiceDate, SP));
        }

        private async void Btnship_Clicked(object sender, EventArgs e)
        {

            try
            {
                var Hawb = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
                var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
                
                var customer = from s in App.SqlLiteCon().Table<Customer>() select s;
                var username = "";
                var CompanyId = "";
                var InviteCode = "";
                var Url = "";
                var ActiveShipmentNo = "";
                var ActiveShipmentStatus = "";

                //var Lat = "";
                //var Long = "";
                var Status = "";
                var RefNo = "";
                var Seconds = "";
               

                var resp = Application.Current.Properties.ContainsKey("LoadResponse") ? Application.Current.Properties["LoadResponse"] as string : "";

                SD_RootObject response = JsonConvert.DeserializeObject<SD_RootObject>(resp);

                foreach (var a in response.Details)
                {
                    //Lat = a.SLine2;
                    //Long = a.CLine2;
                    Status = a.Status;
                    RefNo = a.RefNo;
                    ActiveShipmentNo = a.ID;
                    ActiveShipmentStatus = a.Status;
                    Seconds = a.Seconds;
                }

                foreach (var c in customer)
                {
                    username = c.UserId;
                    InviteCode = c.XCode;
                    CompanyId = c.CompanyID;
                    Url = c.TransactURL;
                    if ((string.IsNullOrEmpty(c.ActiveShipmentNo)) && (string.IsNullOrEmpty(c.ActiveShipmentStatus)))
                    {

                        //c.ActiveShipmentNo = ActiveShipmentNo;
                        //c.ActiveShipmentStatus = ActiveShipmentStatus;
                        //App.SqlLiteCon().Update(c);

                        //var announcementToUpdate = App.SqlLiteCon().Query<Customer>($"SELECT * FROM Announcement WHERE Id = '{originalId}'").

                             App.SqlLiteCon().Execute("update Customer set ActiveShipmentNo='" + ActiveShipmentNo.Trim() + "' ,ActiveShipmentStatus='" + ActiveShipmentStatus.Trim() + "' where ActiveShipmentNo = ?", c.ActiveShipmentNo);


                    }
                    else if ((!string.IsNullOrEmpty(c.ActiveShipmentNo)) && (!string.IsNullOrEmpty(c.ActiveShipmentStatus)))
                    {
                        if (c.ActiveShipmentNo.Trim() != ActiveShipmentNo.Trim())
                        {
                            var action = await DisplayAlert("", "You are currently working on shipment #" + c.ActiveShipmentNo + ". Would you like to stop working on that shipment and start working on this one?", "Yes", "No");
                            if (action)
                            {
                                var TrackDateTimes = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-", "/");
                                var ChangeShipment = App.SOAP_Request.ChangeShipment(c.ActiveShipmentNo, ActiveShipmentNo, username.Trim(), CompanyId, "Y", c.ActiveShipmentStatus, TrackDateTimes, InviteCode, Url);
                                Btnship_RootObject ChngShipresponse = JsonConvert.DeserializeObject<Btnship_RootObject>(ChangeShipment);
                                foreach (var m in ChngShipresponse.Details)
                                {
                                    if(m.Message.ToLower()=="ok")
                                    {
                                        //c.ActiveShipmentNo = ActiveShipmentNo;
                                        //c.ActiveShipmentStatus = ActiveShipmentStatus;
                                        //App.SqlLiteCon().Update(c);
                                        App.SqlLiteCon().Execute("update Customer set ActiveShipmentNo='" + ActiveShipmentNo.Trim() + "' ,ActiveShipmentStatus='" + ActiveShipmentStatus.Trim() + "' where ActiveShipmentNo = ?", c.ActiveShipmentNo);

                                    }
                                }

                            }
                            else
                                return;
                        }
                    }
                    break;
                }

                
                //var locator = CrossGeolocator.Current;
                //locator.DesiredAccuracy = 50;
                //TimeSpan ts = TimeSpan.FromTicks(10000);
                //var position = await locator.GetPositionAsync(ts);

            
                
                string lattitude = "0";
                string longtitude = "0";


                try
                {
                    try
                    {
                     
                        var requestsss = new GeolocationRequest(GeolocationAccuracy.Medium);
                        var locationsss = await Geolocation.GetLocationAsync(requestsss);


                        if(locationsss!=null)
                        {
                            lattitude = locationsss.Latitude.ToString();
                            longtitude = locationsss.Longitude.ToString();
                        }
                        else
                            await DisplayAlert("", "Cannot access Location. Please enable the location.", "OK");

                    }
                    catch
                    {
                        var locationd = await Geolocation.GetLastKnownLocationAsync();

                        if (locationd != null)
                        {
                            lattitude = locationd.Latitude.ToString();
                            longtitude = locationd.Longitude.ToString();
                        }
                        else
                            await DisplayAlert("", "Cannot access Location. Please enable the location.", "OK");
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    await DisplayAlert("", "Cannot access Location.", "OK");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    await DisplayAlert("", "Cannot access Location.", "OK");
                }
                catch (PermissionException pEx)
                {
                    await DisplayAlert("", "Cannot access Location.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("", "Cannot access Location.", "OK");
                }
                var TrackDateTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-", "/");
                var Sendresponse = App.SOAP_Request.SendProgress(RefNo, Hawb, lattitude, longtitude, username.Trim(), CompanyId, InviteCode, "I", Url,TrackDateTime);
                Btnship_RootObject Btnshipresponse = JsonConvert.DeserializeObject<Btnship_RootObject>(Sendresponse);
                foreach (var re in Btnshipresponse.Details)
                {
                    if (re.Message.ToLower() == "ok")
                        btnship.BackgroundColor = (Color.FromHex("#B6D7A8"));
                    else
                        await DisplayAlert("", re.Message, "OK");
                }

                resp = App.SOAP_Request.LoadDetails(Hawb, username.Trim(), MoveType, InviteCode, CompanyId, Url);
                response = JsonConvert.DeserializeObject<SD_RootObject>(resp);

                string inProgress = "";
                string TrackUser = "";
                foreach (var a in response.Details)
                {
                    inProgress = a.inProgress;
                    TrackUser = a.TrackUser;
                }
                Application.Current.Properties["inProgess"] = inProgress;

                if ((inProgress.ToLower() == "y") && (TrackUser.ToLower() == "y"))
                {

                    //await Task.Run(() => SendProgressCall(RefNo, Hawb, lattitude, longtitude, username.Trim(), CompanyId, InviteCode.Trim(), "I", Url)); //does not block UI

                    Thread t = new Thread(() =>
                    {
                        Console.WriteLine("executing ThreadProc");
                        try
                        {
                            TrackDateTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-", "/");
                            SendProgressCall(RefNo, Hawb, lattitude, longtitude, username.Trim(), CompanyId, InviteCode.Trim(), "I", Url,TrackDateTime, Seconds);
                        }
                        finally
                        {
                            // Console.WriteLine("finished executing ThreadProc");
                        }
                    });
                    t.IsBackground = true;
                    t.Start();

                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
        }

        private async void TGlblship_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SBoardDataDetails());
        }

        private async void Arrow_Clicked(object sender, EventArgs e)
        {
            string HAWB = lblhawb.Text, MoveType = lblmovetype.Text, ServiceDate = lblservicedate.Text;


            //var HAWB = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
            //var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
            //var ServiceDate = Application.Current.Properties.ContainsKey("ShipDate") ? Application.Current.Properties["ShipDate"] as string : "";

            var SP = Application.Current.Properties.ContainsKey("SP") ? Application.Current.Properties["SP"] as string : "";

            await Navigation.PushAsync(new Spl_Instruction(HAWB, MoveType, ServiceDate, SP));
        }
     
        private DateTime timeUtc;
      


        private async void XamlSwitch_Toggled(object sender, ToggledEventArgs e)
        {

            try
            {
               
                var swtich = sender as Switch;

              
                var inprogress = Application.Current.Properties.ContainsKey("inProgess") ? Application.Current.Properties["inProgess"] as string : "";
               
           

                var HAWB = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
                    var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
                    var ServiceDate = Application.Current.Properties.ContainsKey("ShipDate") ? Application.Current.Properties["ShipDate"] as string : "";
                  
                    var username = "";
                    var CompanyId = "";
                    var InviteCode = "";
                    var Url = "";
                    var RefNo = "";

                    var DGargo = swtich.IsToggled ? "Y" : "N";

                    var resp = Application.Current.Properties.ContainsKey("LoadResponse") ? Application.Current.Properties["LoadResponse"] as string : "";

                    SD_RootObject response = JsonConvert.DeserializeObject<SD_RootObject>(resp);

                    string RDGargo = "";
                
                    foreach (var a in response.Details)
                    {
                        RDGargo = a.DCargo;
                        RefNo = a.RefNo;
                      
                    }
                   
                    if (DGargo.ToLower() == RDGargo.ToLower())
                        return;
                if (inprogress.ToLower() != "y")
                {

                    if (swtich.IsToggled)
                        swtich.IsToggled = false;
                    else
                        swtich.IsToggled = true;


                    await DisplayAlert("", "This shipment is not In-progress, cannot save any updates.", "OK");
                    return;
                }

                var customer = from s in App.SqlLiteCon().Table<Customer>() select s;
                    foreach (var c in customer)
                    {
                        username = c.UserId;
                        InviteCode = c.XCode;
                        CompanyId = c.CompanyID;
                        Url = c.TransactURL;
                        break;
                    }


                    var UCresp = App.SOAP_Request.UpdateDCargo(RefNo, HAWB, DGargo, username.Trim(), CompanyId, InviteCode, Url);

                    Cargo_RootObject Btnshipresponse = JsonConvert.DeserializeObject<Cargo_RootObject>(UCresp);

                    if (Btnshipresponse.Details[0].Message.ToLower().Contains("ok"))
                    {
                        // await DisplayAlert("", "Notes Updated.", "OK");
                        var updresp = App.SOAP_Request.LoadDetails(HAWB, username.Trim(), MoveType, InviteCode, CompanyId, Url);

                        Application.Current.Properties["LoadResponse"] = updresp;
                    }
                    else
                    {
                        // await DisplayAlert("", "Notes could not be Updated.", "OK");

                        if (swtich.IsToggled)
                            swtich.IsToggled = false;
                        else
                            swtich.IsToggled = true;
                    }
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }

        }


        private void Btncamera_Clicked(object sender, EventArgs e)
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



                var HAWB = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
                var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
                var ServiceDate = Application.Current.Properties.ContainsKey("ShipDate") ? Application.Current.Properties["ShipDate"] as string : "";


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
               

                var UCIresp = App.SOAP_Request.Uploadimgs(RefNo, HAWB, imgBase64String, username.Trim(), CompanyId, InviteCode, Url);

                Cargo_RootObject Btnshipresponse = JsonConvert.DeserializeObject<Cargo_RootObject>(UCIresp);

                if (Btnshipresponse.Details[0].Message.ToLower().Contains("ok"))
                {

                    if (Btnshipresponse.Details[0].Count != "0")
                    {
                        //lbltkpicno.Text = "Total Files Uploaded (" + Btnshipresponse.Details[0].Count + ")";
                        await Navigation.PushAsync(new ShipmentDetails(HAWB, MoveType, ServiceDate));
                    }

                }

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

        public class SD_ShipmentDetails
        {
            public string Message { get; set; }
            public string ID { get; set; }
            public string RefNo { get; set; }
            public string Date { get; set; }
            public string Mtype { get; set; }
            public string NoPieces { get; set; }
            public string Wgt { get; set; }
            public string Haz { get; set; }
            public string SLine1 { get; set; }
            public string SLine2 { get; set; }
            public string CLine1 { get; set; }
            public string CLine2 { get; set; }
            public string SP { get; set; }

            public string DCargo { get; set; }
            
            public string Pic { get; set; }
            public string Status { get; set; }
            public string Sign { get; set; }
            public string inProgress { get; set; }
            public string Action { get; set; }
            public string SPBtnClr { get; set; }
            public string DCargoNotes { get; set; }
            public string TrackUser { get; set; }

            public string Seconds { get; set; }

        }

        public class SD_RootObject
        {
            public List<SD_ShipmentDetails> Details { get; set; }
        }


        /*********************************************************************************************/

        public class Btnship_Detail
        {
            public string Message { get; set; }
            public string ID { get; set; }
            public string UserId { get; set; }
        }

        public class Btnship_RootObject
        {
            public List<Btnship_Detail> Details { get; set; }
        }
        private async void Actions_Tapped(object sender, EventArgs e)
        {
            // string HAWB = lblhawb.Text, MoveType = lblmovetype.Text, ServiceDate = lblservicedate.Text;


            var HAWB = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
            var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
            var ServiceDate = Application.Current.Properties.ContainsKey("ShipDate") ? Application.Current.Properties["ShipDate"] as string : "";

            var resp = Application.Current.Properties.ContainsKey("LoadResponse") ? Application.Current.Properties["LoadResponse"] as string : "";

            await Navigation.PushAsync(new Actions(HAWB, MoveType, ServiceDate, resp));
        }

        private async void Imgactions_Clicked(object sender, EventArgs e)
        {
            var resp = Application.Current.Properties.ContainsKey("LoadResponse") ? Application.Current.Properties["LoadResponse"] as string : "";

            //string HAWB = lblhawb.Text, MoveType = lblmovetype.Text, ServiceDate = lblservicedate.Text;

            var HAWB = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
            var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
            var ServiceDate = Application.Current.Properties.ContainsKey("ShipDate") ? Application.Current.Properties["ShipDate"] as string : "";


            await Navigation.PushAsync(new Actions(HAWB, MoveType, ServiceDate, resp));
        }
        private async void Notesarrow_Clicked(object sender, EventArgs e)
        {

            var HAWB = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
            var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
            var ServiceDate = Application.Current.Properties.ContainsKey("ShipDate") ? Application.Current.Properties["ShipDate"] as string : "";


            await Navigation.PushAsync(new Cargo(HAWB, MoveType, ServiceDate));
        }
      

      
        private async void Notes_Tapped(object sender, EventArgs e)
        {

            var HAWB = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
            var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
            var ServiceDate = Application.Current.Properties.ContainsKey("ShipDate") ? Application.Current.Properties["ShipDate"] as string : "";


            await Navigation.PushAsync(new Cargo(HAWB, MoveType, ServiceDate));
        }

        private async void Btnsign_Clicked(object sender, EventArgs e)
        {
            var HAWB = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
            var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
            var ServiceDate = Application.Current.Properties.ContainsKey("ShipDate") ? Application.Current.Properties["ShipDate"] as string : "";
            var ShipRef = Application.Current.Properties.ContainsKey("ShipRef") ? Application.Current.Properties["ShipRef"] as string : "";
            var inprogress = Application.Current.Properties.ContainsKey("inProgess") ? Application.Current.Properties["inProgess"] as string : "";
            if (inprogress.ToLower() != "y")
            {
                await DisplayAlert("", "This shipment is not In-progress, cannot save any updates.", "OK");
                return;
            }

            await Navigation.PushAsync(new Signature(HAWB, MoveType, ServiceDate, ShipRef));
        }

        private void Imgpic_Tapped(object sender, EventArgs e)
        {
            loadimg();
        }

        private async void Btnmarkship_Clicked(object sender, EventArgs e)
        {

            try
            {
                var Hawb = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
                var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
                var customer = from s in App.SqlLiteCon().Table<Customer>() select s;
                var inprogress = Application.Current.Properties.ContainsKey("inProgess") ? Application.Current.Properties["inProgess"] as string : "";
                if (inprogress.ToLower() != "y")
                {
                    await DisplayAlert("", "This shipment is not In-progress, cannot save any updates.", "OK");
                    return;
                }
                var username = "";
                var CompanyId = "";
                var InviteCode = "";
                var Url = "";
             
                var Lat = "0";
                var Long = "0";
                var Status = "";
                var RefNo = "";
                var Type = "";
               

                foreach (var c in customer)
                {
                    username = c.UserId;
                    InviteCode = c.XCode;
                    CompanyId = c.CompanyID;
                    Url = c.TransactURL;
                    Type = c.Type;
                    //if ((!string.IsNullOrEmpty(c.ActiveShipmentNo)) && (!string.IsNullOrEmpty(c.ActiveShipmentStatus)))
                    //{
                    //    c.ActiveShipmentNo = "";
                    //    c.ActiveShipmentStatus = "";
                    //    App.SqlLiteCon().Update(c);
                    //}
                    break;
                }

                var resp = Application.Current.Properties.ContainsKey("LoadResponse") ? Application.Current.Properties["LoadResponse"] as string : "";

                SD_RootObject response = JsonConvert.DeserializeObject<SD_RootObject>(resp);

                foreach (var a in response.Details)
                {
                    // Lat = a.SLine2;
                    //Long = a.CLine2;
                    Status = a.Status;
                    RefNo = a.RefNo;
                  
                }
               

                try
                {
                    //var locationd = await Geolocation.GetLastKnownLocationAsync();

                    //if (locationd != null)
                    //{
                    //    Lat = locationd.Latitude.ToString();
                    //    Long = locationd.Longitude.ToString();
                    //}
                    //else
                    //{
                    //    await DisplayAlert("", "Cannot access Location ? Please enable the location.", "OK");

                    //}

                    try
                    {
                        var requestsss = new GeolocationRequest(GeolocationAccuracy.Medium);
                        var locationsss = await Geolocation.GetLocationAsync(requestsss);


                        if (locationsss != null)
                        {
                            Lat = locationsss.Latitude.ToString();
                            Long = locationsss.Longitude.ToString();
                        }
                        else
                            await DisplayAlert("", "Cannot access Location ? Please enable the location.", "OK");
                    }
                    catch
                    {
                        var locationd = await Geolocation.GetLastKnownLocationAsync();

                        if (locationd != null)
                        {
                            Lat = locationd.Latitude.ToString();
                            Long = locationd.Longitude.ToString();
                        }
                        else
                            await DisplayAlert("", "Cannot access Location ? Please enable the location.", "OK");
                    }

                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    await DisplayAlert("", fnsEx.Message, "OK");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    await DisplayAlert("", fneEx.Message, "OK");
                }
                catch (PermissionException pEx)
                {
                    await DisplayAlert("", pEx.Message, "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("", ex.Message, "OK");
                }
                string TrackDateTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-", "/");

                var Sendresponse = App.SOAP_Request.SendProgress(RefNo, Hawb, Lat, Long, username.Trim(), CompanyId, InviteCode, "C", Url,TrackDateTime);


                Btnship_RootObject Btnshipresponse = JsonConvert.DeserializeObject<Btnship_RootObject>(Sendresponse);
                foreach (var re in Btnshipresponse.Details)
                {
                    if (re.Message.ToLower() == "ok")
                    {

                        var resp2 = App.SOAP_Request.LoadDetails(Hawb, username.Trim(), MoveType, InviteCode, CompanyId, Url);

                        if (resp2.Contains("\"Details\":"))
                        {
                            SD_RootObject response2 = JsonConvert.DeserializeObject<SD_RootObject>(resp2);
                            foreach (var a in response2.Details)
                            {
                                Application.Current.Properties["inProgess"] = a.inProgress;
                                if ((!string.IsNullOrEmpty(a.inProgress)) && (a.inProgress.ToLower() != "y"))
                                {
                                    Application.Current.Properties["StopSend"] = "true";

                                    App.SqlLiteCon().Execute("update Customer set ActiveShipmentNo='' ,ActiveShipmentStatus='' where ActiveShipmentNo = ?", Hawb);

                                    //ThreadAbortList TAs = new ThreadAbortList();
                                    //TAs.ShipmentId = Hawb;

                                    //App.SqlLiteCon().Insert(TAs);
                                }
                            }

                            Application.Current.Properties["LoadResponse"] = resp2;

                        }


                        await DisplayAlert("","Mark Shipment Completed", "OK");

                        if(Type.ToLower()=="m")
                            await Navigation.PushAsync(new MBoardItemDetails());
                        else
                            await Navigation.PushAsync(new SBoardDataDetails());


                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
                return;
            }


        }

      

        private async void Ximage_Tapped(object sender, EventArgs e)
        {
            try
            {
                var Hawb = Application.Current.Properties.ContainsKey("ShipHawb") ? Application.Current.Properties["ShipHawb"] as string : "";
                var MoveType = Application.Current.Properties.ContainsKey("ShipMtype") ? Application.Current.Properties["ShipMtype"] as string : "";
             

                var customer = from s in App.SqlLiteCon().Table<Customer>() select s;
                var ActiveShipmentStatus = "";
            
                var username = "";
                var CompanyId = "";
                var InviteCode = "";
                var Url = "";
                var Type = "";
                var newrefno = "0";
                var Status = "";
                var RefNo = "";



                var resp = Application.Current.Properties.ContainsKey("LoadResponse") ? Application.Current.Properties["LoadResponse"] as string : "";

                SD_RootObject response = JsonConvert.DeserializeObject<SD_RootObject>(resp);

                foreach (var a in response.Details)
                {

                    Status = a.Status;
                    RefNo = a.RefNo;

                }

                foreach (var c in customer)
                {
                    username = c.UserId;
                    InviteCode = c.XCode;
                    CompanyId = c.CompanyID;
                    Url = c.TransactURL;
                    ActiveShipmentStatus = c.ActiveShipmentStatus;
                  
                    Type = c.Type;


                    break;
                }
                var ddate = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-", "/");

                var ChangeShipment = App.SOAP_Request.ChangeShipment(RefNo, newrefno, username.Trim(), CompanyId, "Y", ActiveShipmentStatus, ddate, InviteCode, Url);
                Btnship_RootObject Xresponse = JsonConvert.DeserializeObject<Btnship_RootObject>(ChangeShipment);
                foreach (var re in Xresponse.Details)
                {
                    if (re.Message.ToLower() == "ok")
                    {
                        foreach (var c in customer)
                        {
                            //c.ActiveShipmentNo = "";
                            //c.ActiveShipmentStatus = "";
                            //App.SqlLiteCon().Update(c);
                            App.SqlLiteCon().Execute("update Customer set ActiveShipmentNo='' ,ActiveShipmentStatus='' where ActiveShipmentNo = ?", Hawb);

                        }
                        if (Type.ToLower() == "m")
                            await Navigation.PushAsync(new MBoardItemDetails());
                        else
                            await Navigation.PushAsync(new SBoardDataDetails());

                    }
                    else
                        await DisplayAlert("", re.Message, "OK");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
                return;
            }

        }
    }
}