using System;
using System.Collections.Generic;

using System.IO;

using System.Reflection;

using System.Threading.Tasks;
using Acr.UserDialogs;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json;
using Plugin.SimpleAudioPlayer;
using RestSharp;


namespace TYFoverBox.view.videoPage
{

    /// <summary>
    /// 聲音類型
    /// </summary>
    public enum AlertType
    {
        OK,
        Fail,
        count
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Video : ContentPage
    {

        /// <summary>
        /// 聲音檔初始化
        /// </summary>
        ISimpleAudioPlayer[] players = new ISimpleAudioPlayer[(int)AlertType.count];

        private readonly VideoModel _viewModel;

        /// <summary>
        /// 入口名
        /// </summary>
        public string _stationName
        {
            get;
            set;
        }

        /// <summary>
        /// 影像網址
        /// </summary>
        public string _url
        {
            get;
            set;
        }

        /// <summary>
        /// 裝置
        /// </summary>
        public string _device
        {
            get;
            set;
        }

        public Video(string url, string StationName, string Device)
        {
            //音效實體化
            for (int i = 0; i < (int)AlertType.count; i++)
            {
                players[i] = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                players[i].Loop = false;
            }

            InitializeComponent();

            //載入聲音
            LoadSound();

            _viewModel = BindingContext as VideoModel;



            _url = url;

            _device = Device;

            _stationName = StationName;

            getdata();
        }

        /// <summary>
        /// 開啟影像
        /// </summary>
        protected override async void OnAppearing()
        {

            base.OnAppearing();
            _viewModel.OnAppearing(_url);
            mediaPlayer.PlaybackControls = _viewModel.PlaybackControls;
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;

            Title = _stationName;

            using (var dlg = UserDialogs.Instance.Loading("影像初始化中"))
            {
                await Task.Delay(13000);
            }
        }

        /// <summary>
        /// 關閉影像並釋放
        /// </summary>
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.OnDisappearing();
            DeviceDisplay.MainDisplayInfoChanged -= OnMainDisplayInfoChanged;
        }



        void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (e.DisplayInfo.Orientation == DisplayOrientation.Landscape)
                {
                    await Navigation.PushAsync(new OnlyVideoPage(_viewModel.MediaPlayer));
                }
            });
        }

        /// <summary>
        /// 取得即時資料
        /// </summary>
        public async void getdata()
        {
            var a = new RestClient();

            while (true)
            {
                string url = string.Format("http://monitor.focusit.com.tw/FeverBBOX/getData?Device={0}", _device);
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                IRestResponse response = client.Execute(request);
                string sJSON = response.Content;
                List<RealTimeData> realTimeData = JsonConvert.DeserializeObject<List<RealTimeData>>(sJSON);
                RealTimeData real = realTimeData[0];

                if (real.TPDAlertDistanceStatus.Equals("1"))
                {
                    TemperatureMSG.Text = string.Format("{0} 度", real.TPDDeviceTemperature);
                    if (Convert.ToDouble(real.TPDDeviceTemperature) >= Convert.ToDouble(real.ThresholdFever))
                    {
                        //EnterMSG.Source = "NOEnter.png";

                        TemperatureMSG.TextColor =Color.Red;

                        lbMsg.Text = "疑似";
                        lbMsg.TextColor = Color.Red;
                        AlertType at = AlertType.Fail;
                        players[(int)at]?.Play();
                    }
                    else
                    {

                        TemperatureMSG.TextColor = Color.DodgerBlue;

                        lbMsg.Text = "通過";
                        lbMsg.TextColor = Color.Green;
                        AlertType at = AlertType.OK;
                        players[(int)at]?.Play();
                    }

                    //var curTime = real.Updatetime.ToString("yyyy/MM/dd HH:mm:ss.fff");
                    //string json = string.Format("{\r\n  \"DeviceSN\": \"{0}\",\r\n  \"Insertdatetime\": \"{1}\"\r\n}", real.TPDDevice, curTime);
                    //var client_up = new RestClient("http://monitor.focusit.com.tw/FeverBBOX/SetisOKStatus");
                    //var request_up = new RestRequest(Method.POST);
                    //request_up.AddParameter("application/json", json, ParameterType.RequestBody);
                    //IRestResponse response_up = client_up.Execute(request_up);

                    string urlUp = string.Format("http://monitor.focusit.com.tw/FeverBBOX/SetisOKStatusBySN?SN={0}", real.TPDSN);
                    var client_up = new RestClient(urlUp);
                    var request_up = new RestRequest(Method.POST);
                    request_up.AddHeader("cache-control", "no-cache");
                    IRestResponse response_up = client_up.Execute(request_up);
                }
                else
                {
                    //EnterMSG.Source = "waitCheck.png";
                    TemperatureMSG.Text = string.Empty;
                    lbMsg.Text = "等待偵測";
                    lbMsg.TextColor = Color.DodgerBlue;
                }


                //將 Task 暫停 0.25 秒
                await Task.Delay(1500);
            }

        }

        /// <summary>
        /// 取得該程式目錄下的實體檔案
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            //NOTE: 在專案中的檔案右鍵屬性，建置動作選擇EmbebdeResouce(編譯至資源檔)
            var stream = assembly.GetManifestResourceStream("TYFoverBox." + filename);

            return stream;
        }

        /// <summary>
        /// 載入聲音檔至記憶體
        /// </summary>
        public void LoadSound()
        {

            players[(int)AlertType.OK].Load(GetStreamFromFile("Audio.OK.mp3"));
            players[(int)AlertType.Fail].Load(GetStreamFromFile("Audio.Fail.mp3"));

        }

        

    }
}
