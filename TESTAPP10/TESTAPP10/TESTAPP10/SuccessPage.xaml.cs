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
	public partial class SuccessPage : ContentPage
	{
		public SuccessPage ()
		{
			InitializeComponent ();
		}
        public SuccessPage(string str)
        {
            InitializeComponent();

            lblmsg.Text= str;
        }
    }
}