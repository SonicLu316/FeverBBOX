using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json;
using RestSharp;

using Xamarin.Essentials;
using System.Collections.Generic;

namespace TYFoverBox
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();

            getData();
            MainPage = new NavigationPage(new MainSelect());
        }


        /// <summary>
        /// 取得資料作為全域變數
        /// </summary>
        public void getData()
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.None)
            {
                var client = new RestClient("http://monitor.focusit.com.tw/FeverBBOX/getStation");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                IRestResponse response = client.Execute(request);
                string sJSON = response.Content;

                List<Station> StationList = JsonConvert.DeserializeObject<List<Station>>(sJSON);
                Global._Station = StationList;
            }
            else
            {
                Global._Station = null;
            }


        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
