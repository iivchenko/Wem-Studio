﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WemManagementStudio.Wpf.ViewModels
{
    public sealed class MainViewModel : IMainViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        ////private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        ////{
        ////    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        ////}
        public void TestMethod()
        {
            throw new System.NotImplementedException();
        }
    }
}