﻿using ACGManager.Pages.Search_Tools.ViewModels;
using ACGManager.Pages.Search_Tools.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ACGManager.Pages.Search_Tools
{
    /// <summary>
    /// RJ_Search.xaml 的交互逻辑
    /// </summary>
    public partial class RJ_Search : Page
    {
        private SearchViewModel _viewModel;
        
        public RJ_Search()
        {
            InitializeComponent();
            
            // 初始化ViewModel并设置为DataContext
            _viewModel = new SearchViewModel();
            this.DataContext = _viewModel;
        }

        public class Item
        {
            public string Title { get; set; }
            public string Link { get; set; }
            public string Image { get; set; }
            public string Maker { get; set; }
            public string Maker_Url { get; set; }
            
            // 添加FormattedUrl属性，将Link转换为Uri对象
            public Uri FormattedUrl
            {
                get
                {
                    if (string.IsNullOrEmpty(Link))
                        return null;
                    
                    return new Uri(Link);
                }
            }
            
            // 添加FormattedImg属性，用于ToolTip中的图片显示
            public string FormattedImg
            {
                get
                {
                    return Image;
                }
            }
        }

        public async void Http_RJ_ListView(string search,string query, string results)
        {
            try
            {
                string url = "https://api.hksts.eu.org/dlsite"; // 替换成你的 API 地址
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);

                    string jsonData = @"{'search':'', 'query':'', 'results':''}";
                    JObject jsonObject = JObject.Parse(jsonData);
                    jsonObject["search"] = search;
                    jsonObject["query"] = query;
                    jsonObject["results"] = results;
                    string modify_jsonData = jsonObject.ToString();
                    // 发送 POST 请求
                    HttpContent content = new StringContent(modify_jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    // 确保响应成功
                    response.EnsureSuccessStatusCode();
                    
                    // 读取响应内容
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // 解析 JSON
                    var items = JsonConvert.DeserializeObject<List<Item>>(responseBody);
                    if (items.Any() == true)
                    {
                        RJ_ListView.ItemsSource = items;
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

        private void RJ_Button_Click(object sender, RoutedEventArgs e)
        {
            if (DL_query_Text.Text == "")
            {
                ShowInfoBar("搜索框不能为空 ", 0, 3);
            }
            else
            {
                // 获取选中项的英文Value属性值
                string searchTypeValue = _viewModel.SelectedSearchType.Value;
                Http_RJ_ListView(searchTypeValue, DL_query_Text.Text, DL_results.Text);
            }
        }

        private System.Timers.Timer _timer;
        // 倒计时关闭InfoBar
        private void ShowInfoBar(string message, int InfoBarSeverity, int durationInSeconds)
        {
            // ShowInfoBar(信息,错误类型,秒数)

            // Informational = 0
            // Success = 1
            // Warning = 2
            // Error = 3

            // 初始化Timer
            _timer = new System.Timers.Timer(1000) // 设置时间间隔为1秒，每秒更新一次
            {
                AutoReset = true // 重复触发
            };

            rj_null.Title = $"{message}";
            rj_null.IsOpen = true;
            rj_null.Severity = (iNKORE.UI.WPF.Modern.Controls.InfoBarSeverity)InfoBarSeverity;

            // 绑定计时器事件
            _timer.Elapsed += (s, e) =>
            {
                if (durationInSeconds > 0)
                {
                    // 减少剩余时间
                    durationInSeconds--;
                    // 使用Dispatcher在UI线程上更新InfoBar
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // 不+1看着难受
                        rj_null.Title = $"{message} {durationInSeconds + 1}s";
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        rj_null.IsOpen = false;
                        StopAndDisposeTimer();
                    });
                }
            };

            // 如果InfoBar有一个关闭事件，这里是进行绑定的地方
            rj_null.Closed += (s, e) =>
            {
                // 停止并释放计时器
                StopAndDisposeTimer();
            };

            // 启动计时器
            _timer.Start();
        }
        // 释放计时器
        private void StopAndDisposeTimer()
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                // 停止计时器
                _timer.Stop();
                // 释放计时器资源
                _timer.Dispose();
                _timer = null; // 确保计时器被释放
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void CopyTitle_Click(object sender, RoutedEventArgs e)
        {
            if (RJ_ListView.SelectedItem is Item selectedItem)
            {
                TextCopy.ClipboardService.SetText(selectedItem.Title);
            }
        }

        private void CopyUrl_Click(object sender, RoutedEventArgs e)
        {
            if (RJ_ListView.SelectedItem is Item selectedItem)
            {
                TextCopy.ClipboardService.SetText(selectedItem.Link);
            }
        }

        private void searchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 可以在这里添加额外的处理逻辑
        }

        private void resultsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
