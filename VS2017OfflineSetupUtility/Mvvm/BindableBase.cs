using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VS2017OfflineSetupUtility.Mvvm
{

    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event when property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">Storage reference property with getter setter</param>
        /// <param name="value">Property Value</param>
        /// <param name="propertyName">Name of the listener property</param>
        /// <returns>Return true if value was changed or else false</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// SetProperty if not same as previous set value
        /// </summary>
        /// <typeparam name="T">Property Type</typeparam>
        /// <param name="storage">Storage reference property with getter setter</param>
        /// <param name="value">Property Value</param>
        /// <param name="onChanged">Action onChanged</param>
        /// <param name="propertyName">Name of the listener property</param>
        /// <returns>Return true if value changed else false</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Raise Property Changed event
        /// </summary>
        /// <param name="propertyName">Name of the listener property<param>
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        
        /// <summary>
        /// Raise property changed
        /// </summary>
        /// <param name="args">Property Changed Arguments</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
    }
}
