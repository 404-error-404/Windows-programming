using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows_programming.view;
using MySQL_DLL;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Windows_programming
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        // TODO 还不知道这个变量干嘛用的，但似乎是上一步相关的，先标记着
        public Frame AppFrame => frame;

        // 一些常量
        // 菜单栏名称
        public readonly string StudentListLabel = "Student list";

        public readonly string OrderListLabel = "Order list";

        // 项目GitHub地址
        public readonly string GitHubUrl = "https://github.com/404-error-404/Windows-programming";

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            // 获取点击的菜单名称
            var label = args.InvokedItem as string;
            // 判断点击了哪个，容错处理返回null
            var pageType =
                args.IsSettingsInvoked ? typeof(SettingsPage) :
                label == StudentListLabel ? typeof(StudentListPage) : null;
            if (pageType != null && pageType != AppFrame.CurrentSourcePageType)
            {
                AppFrame.Navigate(pageType);
            }
        }

        /// <summary>
        /// 异步方法，在浏览器中打开github地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ViewCodeNavPaneButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(GitHubUrl));
        }

        /// <summary>
        /// 点击菜单栏触发对应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (e.SourcePageType == typeof(StudentListPage))
                {
                    NavView.SelectedItem = StudentListMenuItem;
                }
                else if (e.SourcePageType == typeof(SettingsPage))
                {
                    NavView.SelectedItem = NavView.SettingsItem;
                }
            }
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (AppFrame.CanGoBack)
            {
                AppFrame.GoBack();
            }
        }
    }
}
