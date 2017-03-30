using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace ACE.Client.Infrastructure
{
    public static class NavigationServices
    {

        private static TabControl _navigationFrame { get; set; }
        public static void Initialize(TabControl navigationFrame)
        {
            _navigationFrame = navigationFrame;
            _navigationFrame.SelectionChanged += _navigationFrame_SelectionChanged;
        }

        private static void _navigationFrame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var view = (_navigationFrame.SelectedItem as TabItem)?.Content;
            if (view == null) return;
            Console.WriteLine(@"_navigationFrame_SelectionChanged: " + view.GetHashCode());
            var navigatingView = NavigatingViews;
        }

        public static Dictionary<string, Type> RegisteredViews { get; } = new Dictionary<string, Type>();
        public static void Register(string viewId, Type contentControlType)
        {
            if (!typeof(ContentControl).IsAssignableFrom(contentControlType)) throw  new ArgumentException(@"Type must be or derive from ContentControl", nameof(contentControlType));
            RegisteredViews.Add(viewId, contentControlType);
        }

        public static ViewViewModel ActiveView { get; private set; }

        private static Queue<ContentControl> _initializedViews;
        public static Queue<ContentControl> InitializedViews => _initializedViews ?? (_initializedViews = new Queue<ContentControl>());

        public static void NavigateTo(string viewId, object context = null)
        {
            if (!RegisteredViews.ContainsKey(viewId)) throw new ArgumentException(@"View not registered", nameof(viewId));
            var vvm = new ViewViewModel(viewId);
            var viewType = vvm.View.GetType();
            var maxNumberOfViewInstances = vvm.View.GetType().GetCustomAttributes(true).OfType<NavigationInstancesAttribute>().FirstOrDefault()?.MaximumInstances;
            var numberOfViewInstances = NavigatingViews.CircularArray.Count(v => v?.View?.GetType() == viewType);
            Console.WriteLine(@"# instances: " + numberOfViewInstances + @".    Max instances: " + maxNumberOfViewInstances);
            if (numberOfViewInstances >= maxNumberOfViewInstances)
            {
                var existingView = NavigatingViews.CircularArray.FirstOrDefault(v => v.ViewModel.ViewId == viewId);
                if (existingView != null)
                {
                    existingView.ViewModel.OnNavigatedTo(null);
                    return;
                }
            }
            NavigateToANewView(vvm, context);
        }

        private static void NavigateToANewView(ViewViewModel vvm, object context)
        {
            string viewId = vvm.ViewModel.ViewId;
            var item = new TabItem();
            SetupHeader(viewId, vvm.ViewModel, item);
            AddViewToNavigationRegion(item, vvm.View);
            NavigatingViews.Peek()?.ViewModel.OnNavigatedFrom(null);
            NavigatingViews.Add(vvm);
            ActiveView = NavigatingViews.Peek();
            NavigatingViews.Peek()?.ViewModel.OnNavigatedTo(context);
        }

        private static void AddViewToNavigationRegion(TabItem item, ContentControl view)
        {
            item.Content = view;
            _navigationFrame.Items.Add(item);
            _navigationFrame.SelectedItem = item;
        }

        private static void SetupHeader(string viewId, INavigation navigatingToViewModel, TabItem item)
        {
            if (navigatingToViewModel == null) item.Header = viewId;
            else
            {
                var nameBinding = new Binding()
                {
                    Path = new PropertyPath("Header"),
                    Source = navigatingToViewModel
                };
                BindingOperations.SetBinding(item, TabItem.HeaderProperty, nameBinding);
            }
        }

        public static CircularList<ViewViewModel> NavigatingViews = new CircularList<ViewViewModel>(10); 

        public static List<string> OpenViews()
        {
            return null;
        }
        private static DelegateCommand _closeTabItemCommand;

        public static DelegateCommand CloseTabItemCommand
        {
            get { return _closeTabItemCommand ?? (_closeTabItemCommand = new DelegateCommand(new Action<object>((a) => CloseSelectedView()))); }
            set { _closeTabItemCommand = value; }
        }

        public static void CloseSelectedView()
        {
            //TODO: Must dispose View
            if (_navigationFrame.SelectedItem == null) return;
            _navigationFrame.Items.Remove(_navigationFrame.SelectedItem);
            if (ActiveView != null && NavigatingViews.Find(ActiveView) >= 0)
            {
                NavigatingViews.Peek()?.ViewModel.OnNavigatedFrom(null);
                NavigatingViews.Remove(ActiveView);
                ActiveView = NavigatingViews.Peek();
            }
        }

        public class ViewViewModel
        {
            public ContentControl View;
            public INavigation ViewModel;
            public ViewViewModel(string viewId)
            {
                var viewType = RegisteredViews[viewId];
                var view = Activator.CreateInstance(viewType) as ContentControl;
                if (view == null) throw  new ArgumentException(@"invalid View", nameof(viewId));
                this.View = view;
                var viewModel = view.DataContext;
                ViewModel = viewModel as INavigation;
                if (ViewModel == null) throw new ArgumentException(@"invalid ViewModel", nameof(viewId));
                ViewModel.ViewId = viewId;
            }
        }
    }
}
