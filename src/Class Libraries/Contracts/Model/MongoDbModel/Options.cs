using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Contracts.Model.MongoDbModel
{
    public class Options
    {
        public class ChatItem : INotifyPropertyChanged
        {
            [JsonProperty("ID")]
            public string ID { get; set; }
            //stores content ID from content field for after the Content field is replaced with actual content.
            [JsonProperty("ContentId")]
            public string ContentId { get; set; }
            [JsonProperty("Content")]
            public string Content { get; set; }
            [JsonProperty("Description")]
            public string Description { get; set; }
            [JsonProperty("Question")]
            public string Question { get; set; }
            [JsonProperty("Score")]
            public int Score { get; set; }
            [JsonProperty("Rating")]
            public int Rating { get; set; }
            [JsonProperty("Type")]
            public MessageType Type { get; set; }
            [JsonProperty("MinVersion")]
            public Version MinVersion { get; set; }
            [JsonProperty("GroupID")]
            public int GroupID { get; set; }
            [JsonProperty("Entities")]
            public Dictionary<string, string> Entities { get; set; }
            public List<string> OptionsList { get; set; }
            public Byte[] Image { get; set; }

            private ObservableCollection<ChatItem> _groupItems;

            public ObservableCollection<ChatItem> GroupItems
            {
                get => _groupItems;
                set
                {
                    _groupItems = value;
                    OnPropertyChanged("GroupItems");
                    OnPropertyChanged("CanExpanded");
                }
            }


            public bool CanExpanded => GroupItems?.Any() ?? false;

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }


        public enum MessageType
        {
            URL = 1,
            Text = 2,
            Plugin = 3,
            Options = 4,
            Flow = 5
        }

    }
}
