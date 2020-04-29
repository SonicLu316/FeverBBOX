using System;
using System.Collections.Generic;
using TYFoverBox.view.videoPage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TYFoverBox
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSelect : ContentPage
    {
        public MainSelect()
        {
            InitializeComponent();

            GetData();
        }


        /// <summary>
        /// 取得入口List
        /// </summary>
        private void GetData()
        {
            List<Station> StationList = Global._Station;

            if (StationList != null)
            {
                foreach (Station station in StationList)
                {

                    if (station.TPSValid.Equals("0"))
                    {
                        continue;
                    }

                    Button b = new Button();
                    b.Text = station.TPSDeviceAlias;

                    b.BackgroundColor = Color.CadetBlue;
                    b.FontSize = 20;
                    b.WidthRequest = 250;
                    b.TextColor = Color.Black;

                    b.Clicked += (s, e) => Navigation.PushAsync(new Video(station.TPSVlcUrl, station.TPSDeviceAlias, station.TPSDevice));

                    StackLayoutMaster.Children.Add(b);
                }
            }
            else
            {
                Label lb = new Label();
                lb.Text = "請連接網路並重新開啟程式";
                lb.FontSize = 25;
                lb.TextColor = Color.Red;
                StackLayoutMaster.Children.Add(lb);
            }
        }

    }

}
