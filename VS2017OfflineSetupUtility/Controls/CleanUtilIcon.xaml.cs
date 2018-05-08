using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VS2017OfflineSetupUtility.Controls
{
    /// <summary>
    /// Interaction logic for CleanUtilIcon.xaml
    /// </summary>
    public partial class CleanUtilIcon : ContentControl
    {
        public CleanUtilIcon()
        {
            InitializeComponent();
            //Called to set dependency defaults from code
            Rebuild();
        }

        #region IconColor
        /// <summary>
        /// Identifies the <see cref="IconColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(CleanUtilIcon), new PropertyMetadata((SolidColorBrush)App.Current.Resources["GraySolidColorBrush"], OnPropertyChanged));

        /// <summary>
        /// Gets or sets the IconColor
        /// </summary>
        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }
        #endregion

        #region IsCheckedColor
        /// <summary>
        /// Identifies the <see cref="IsCheckedColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCheckedColorProperty = DependencyProperty.Register(nameof(IsCheckedColor), typeof(Brush), typeof(CleanUtilIcon), new PropertyMetadata((SolidColorBrush)App.Current.Resources["GreenSolidColorBrush"], OnPropertyChanged));

        /// <summary>
        /// Gets or sets the IsCheckedColor
        /// </summary>
        public Brush IsCheckedColor
        {
            get { return (Brush)GetValue(IsCheckedColorProperty); }
            set { SetValue(IsCheckedColorProperty, value); }
        }
        #endregion

        #region IsChecked
        /// <summary>
        /// Identifies the <see cref="IsChecked"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(CleanUtilIcon), new PropertyMetadata(false, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the IsCheckedColor
        /// </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        #endregion

        private static void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var icon = dependencyObject as CleanUtilIcon;
            icon?.Rebuild();
        }

        private void Rebuild()
        {
            if (IsChecked)
                IconPath.Fill = IsCheckedColor;
            else
                IconPath.Fill = IconColor;

            //Update any other depedency property here
        }
    }
}
