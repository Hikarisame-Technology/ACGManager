using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ACGManager.Pages.Search_Tools.Models;

namespace ACGManager.Pages.Search_Tools.ViewModels
{
    public class SearchViewModel : ObservableObject
    {
        private ObservableCollection<ComboBoxItem> _searchTypes;
        public ObservableCollection<ComboBoxItem> SearchTypes
        {
            get => _searchTypes;
            set => SetProperty(ref _searchTypes, value);
        }

        private ComboBoxItem _selectedSearchType;
        public ComboBoxItem SelectedSearchType
        {
            get => _selectedSearchType;
            set => SetProperty(ref _selectedSearchType, value);
        }

        public SearchViewModel()
        {
            // 初始化搜索类型集合，键为显示文本(中文)，值为实际值(英文)
            SearchTypes = new ObservableCollection<ComboBoxItem>
            {
                new ComboBoxItem("同人", "maniax"),
                new ComboBoxItem("漫画", "books"),
                new ComboBoxItem("游戏", "pro"),
                new ComboBoxItem("软件", "appx")
            };

            // 默认选择第一项
            SelectedSearchType = SearchTypes[0];
        }
    }
} 