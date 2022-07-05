using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using TravelAgency.Annotations;

namespace TravelAgency.Services
{
    public class Notification : INotifyPropertyChanged
    {
        private string _title = "Sample Title";
        public string Title { get => _title;
            private set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _content = "Sample content. Zdarova Karova chi ne zdarova Karova";
        public string Content { get => _content;
            private set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        private bool _tabIsOpen;
        public bool TabIsOpen { get => _tabIsOpen;
            private set
            {
                _tabIsOpen = value;
                OnPropertyChanged();
            }
        }

        private readonly DispatcherTimer CloseTimer = new DispatcherTimer{ Interval = TimeSpan.FromSeconds(3)};

        public Notification()
        {
                CloseTimer.Tick += (s,e) => Close();
        }

        public void Close()
        {
            TabIsOpen = false;
            CloseTimer.Stop();
        }

        public void Show(string title, string content)
        {
            CloseTimer.Stop();
            Title = title;
            Content = content;
            TabIsOpen = true;
            CloseTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}