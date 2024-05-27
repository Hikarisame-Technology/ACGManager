using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ACGManager.Pages.Search_Tools
{
    /// <summary>
    /// RJ_Search.xaml 的交互逻辑
    /// </summary>
    public partial class RJ_Search : Page
    {
        public RJ_Search()
        {
            InitializeComponent();
        }

        public class Item
        {
            public string img { get; set; }
            public string rj { get; set; }
            public string title { get; set; }
            public string url { get; set; }
        }

        public async void Http_RJ_ListView(string keyword) 
        {
            try
            {
                string url = "https://rj.acgget.com/api.php"; // 替换成你的 API 地址

                // 准备要发送的表单数据
                var formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("keyword", keyword),
                });

                using (HttpClient client = new HttpClient())
                {
                    // 发送 POST 请求
                    HttpResponseMessage response = await client.PostAsync(url, formData);

                    // 确保响应成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // 解析 JSON
                    var items = JsonConvert.DeserializeObject<List<Item>>(responseBody);

                    // 将img字段转换为完整的HTTPS URL
                    foreach (var item in items)
                    {
                        item.img = "https:" + item.img.Replace("\\/", "/");
                        item.url = item.url.Replace("\\/", "/"); ;
                    }

                    // 将数据绑定到ListView
                    RJ_ListView.ItemsSource = items;

                }
            }
            catch (Exception ex)
            {
                rj_null.Title = "发送 POST 请求时出错:" + ex.Message;
                rj_null.IsOpen = true;
            }
        }

        private void RJ_Button_Click(object sender, RoutedEventArgs e)
        {
            if (RJ_Text.Text == "")
            {
                rj_null.IsOpen = true;
            }
            else
            { 
                Http_RJ_ListView(RJ_Text.Text);
            }
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                PopupImage.Source = image.Source;
                ImagePopup.IsOpen = true;
            }
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            ImagePopup.IsOpen = false;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void CopyRJ_Click(object sender, RoutedEventArgs e)
        {
            if (RJ_ListView.SelectedItem is Item selectedItem)
            {
                TextCopy.ClipboardService.SetText(selectedItem.rj);
            }
        }

        private void CopyTitle_Click(object sender, RoutedEventArgs e)
        {
            if (RJ_ListView.SelectedItem is Item selectedItem)
            {
                TextCopy.ClipboardService.SetText(selectedItem.title);
            }
        }

        private void CopyUrl_Click(object sender, RoutedEventArgs e)
        {
            if (RJ_ListView.SelectedItem is Item selectedItem)
            {
                TextCopy.ClipboardService.SetText(selectedItem.url);
            }
        }

    }
}
