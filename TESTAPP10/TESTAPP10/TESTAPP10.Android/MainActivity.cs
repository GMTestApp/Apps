using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using Android.Support.V4.Content;
using System.Collections.Generic;
using Android;
using Xamarin.Forms;
using Android.Support.V4.App;

namespace TESTAPP10.Droid
{
    [Activity(Label = "TESTAPP10", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        string[] PermissionsArray = null;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
                base.OnCreate(savedInstanceState);
                global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
                App.SOAP_Request = new SOAP_Request(new SoapService());
                await CrossMedia.Current.Initialize();
                //CrossCurrentActivity.Current.Init(this, bundle);
                LoadApplication(new App());

                updateNonGrantedPermissions();
                //Initializer.Initialize();
                try
                {
                    if (PermissionsArray != null && PermissionsArray.Length > 0)
                    {
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                        {
                            ActivityCompat.RequestPermissions(this, PermissionsArray, 0);
                        }
                    }
                }
                catch (Exception oExp)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



        private void updateNonGrantedPermissions()
        {
            try
            {
                List<string> PermissionList = new List<string>();
                PermissionList.Add(Manifest.Permission.MediaContentControl);
                //if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.RecordAudio) != (int)Android.Content.PM.Permission.Granted)
                //{
                //    PermissionList.Add(Manifest.Permission.RecordAudio);
                //}
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.WriteExternalStorage) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.WriteExternalStorage);
                }
                //if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.ReadPhoneState) != (int)Android.Content.PM.Permission.Granted)
                //{
                //    PermissionList.Add(Manifest.Permission.ReadPhoneState);
                //}

                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.LocationHardware) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.LocationHardware);
                }

                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.AccessCoarseLocation) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.AccessCoarseLocation);
                }
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.AccessFineLocation) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.AccessFineLocation);
                }
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.AccessMockLocation) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.AccessMockLocation);
                }
                if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.Camera) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.Camera);
                }

                PermissionsArray = new string[PermissionList.Count];
                for (int index = 0; index < PermissionList.Count; index++)
                {
                    PermissionsArray.SetValue(PermissionList[index], index);
                }
            }
            catch (Exception oExp)
            {

            }
        }





    }
}