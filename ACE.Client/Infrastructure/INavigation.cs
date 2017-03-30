using System.Windows.Navigation;

namespace ACE.Client.Infrastructure
{
    public interface INavigation
    {
        string ViewId { get; set; }
        string Header { get; }
        void OnNavigatedTo(object context);
        void OnNavigatedFrom(object context);

    }
}
