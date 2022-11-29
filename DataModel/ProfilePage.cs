using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class ProfilePage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Image ProfilePicture;

        public void OnProfilePictureChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
