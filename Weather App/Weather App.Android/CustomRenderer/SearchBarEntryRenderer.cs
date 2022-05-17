using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weather_App.Controls;
using Weather_App.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBarControl), typeof(SearchBarEntryRenderer))]
namespace Weather_App.Droid.CustomRenderer
{

    public class SearchBarEntryRenderer : SearchBarRenderer
    {
        public SearchBarEntryRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if(Control != null)
            {
                SearchView searchView = (base.Control as SearchView);
                searchView.SetInputType(InputTypes.ClassText | InputTypes.TextVariationNormal);

                int textViewId = searchView.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
                
                EditText textView = (searchView.FindViewById(textViewId) as EditText);

                textView.SetBackgroundColor(Android.Graphics.Color.Rgb(225, 225, 225));
                textView.SetTextColor(Android.Graphics.Color.Rgb(32, 32, 32));
                textView.SetHintTextColor(Android.Graphics.Color.Rgb(128, 128, 128));
                textView.Hint = "Search a City";
                


            }
        }
    }
}