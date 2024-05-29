using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ACGManager.Pages.Search_Tools
{
    /// <summary>
    /// Vndb_Search.xaml 的交互逻辑
    /// </summary>
    public partial class Vndb_Search : Page
    {
        public Vndb_Search()
        {
            InitializeComponent();
        }
        public class Developers
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class VNData
        {
            public string id { get; set; }
            public string title { get; set; }
            public List<string> aliases { get; set; }
            // 添加一个内部类，用以匹配 JSON 中的 image 对象
            public class VNImage
            {
                public string url { get; set; }
            }
            // 添加一个属性，用来存储 VNImage 对象
            public VNImage image { get; set; }
            public List<string> languages { get; set; }
            public List<Developers> developers { get; set; }
            public int? length_minutes { get; set; }

            public string formattedUrl => $"https://vndb.org/{id}";
            public string aliasesnames => string.Join("\n", aliases);
            public string languagessupported => string.Join("\n", languages);
            public string developersnames => string.Join("\n", developers.Select(d => d.name));
            public string displaylength_minutes => length_minutes.HasValue
                                                ? TimeSpan.FromMinutes(length_minutes.Value).TotalHours >= 1
                                                ? $"{(int)TimeSpan.FromMinutes(length_minutes.Value).TotalHours} 小时 {TimeSpan.FromMinutes(length_minutes.Value).Minutes} 分钟"
                                                : $"{TimeSpan.FromMinutes(length_minutes.Value).Minutes} 分钟"
                                                : "无";
            public string Title_Aliases => $"{title}\n{aliasesnames}";
        }

        public class VNResponse
        {
            public List<VNData> results { get; set; }
            public bool more { get; set; }
        }

        public async Task<List<VNData>> FetchVNData(string searchText)
        {
            using (var httpClient = new HttpClient())
            {
                var postData = new
                {
                    filters = new object[] { "search", "=", searchText },
                    fields = "id,title,aliases,image.url,languages,developers.name,length_minutes"
                };
                var jsonContent = JsonConvert.SerializeObject(postData);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                try
                {
                    var response = await httpClient.PostAsync("https://api.vndb.org/kana/vn", httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var vnResponse = JsonConvert.DeserializeObject<VNResponse>(jsonResponse);
                        if (vnResponse != null && vnResponse.results != null && vnResponse.results.Any())
                        {
                            return vnResponse?.results;
                        }
                        else
                        {
                            ShowInfoBar("搜索结果为空 ", 0, 3);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowInfoBar("出现错误:" + ex.Message + " ", 3, 3);
                }
            }
            return null;
        }

        private async void Vndb_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Vndb_Text.Text == "")
            {
                ShowInfoBar("搜索框不能为空 ", 0, 3);
            }
            else
            {
                var searchText = Vndb_Text.Text;
                var vnDataList = await FetchVNData(searchText);
                Vndb_ListView.ItemsSource = vnDataList;
            }
        }

        private System.Timers.Timer _timer;
        private void ShowInfoBar(string message, int InfoBarSeverity, int durationInSeconds)
        {
            // ShowInfoBar(信息,错误类型,秒数)

            // Informational = 0 信息
            // Success = 1 成功
            // Warning = 2 警告
            // Error = 3 错误

            _timer = new System.Timers.Timer(1000)
            {
                AutoReset = true
            };

            vndb_null.Title = $"{message}";
            vndb_null.IsOpen = true;
            vndb_null.Severity = (iNKORE.UI.WPF.Modern.Controls.InfoBarSeverity)InfoBarSeverity;

            // 绑定计时器事件
            _timer.Elapsed += (s, e) =>
            {
                if (durationInSeconds > 0)
                {
                    durationInSeconds--;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // 不+1看着难受
                        vndb_null.Title = $"{message} {durationInSeconds + 1}s";
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        vndb_null.IsOpen = false;
                        StopAndDisposeTimer();
                    });
                }
            };

            vndb_null.Closed += (s, e) =>
            {
                StopAndDisposeTimer();
            };

            _timer.Start();
        }

        // 释放计时器
        private void StopAndDisposeTimer()
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }


        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void CopyTitle_Click(object sender, RoutedEventArgs e)
        {
            if (Vndb_ListView.SelectedItem is VNData selectedItem)
            {
                TextCopy.ClipboardService.SetText(selectedItem.title);
            }
        }

        private void CopyAliases_Click(object sender, RoutedEventArgs e)
        {
            if (Vndb_ListView.SelectedItem is VNData selectedItem)
            {
                TextCopy.ClipboardService.SetText(selectedItem.aliasesnames);
            }
        }

        private void CopyDeveloperse_Click(object sender, RoutedEventArgs e)
        {
            if (Vndb_ListView.SelectedItem is VNData selectedItem)
            {
                TextCopy.ClipboardService.SetText(selectedItem.developersnames);
            }
        }

        private void CopyUrl_Click(object sender, RoutedEventArgs e)
        {
            if (Vndb_ListView.SelectedItem is VNData selectedItem)
            {
                TextCopy.ClipboardService.SetText(selectedItem.formattedUrl);
            }
        }
    }
}
