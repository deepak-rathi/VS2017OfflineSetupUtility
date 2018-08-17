using System.Windows.Input;

namespace VS2017OfflineSetupUtility.Mvvm
{
    public interface IChangedCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
