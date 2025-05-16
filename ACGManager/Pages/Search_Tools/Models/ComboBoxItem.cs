using System;

namespace ACGManager.Pages.Search_Tools.Models
{
    public class ComboBoxItem
    {
        public string DisplayText { get; set; }
        
        public string Value { get; set; }
        
        public ComboBoxItem(string displayText, string value)
        {
            DisplayText = displayText;
            Value = value;
        }
        
        public override string ToString()
        {
            return DisplayText;
        }
    }
} 