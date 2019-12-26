using MySQL_DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Windows_programming.view
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class StudentListPage : Page
    {
        public StudentListPage()
        {
            this.InitializeComponent();
        }

        public class TabName    //数据结构,教程 https://blog.csdn.net/www89574622/article/details/76187254
        {
            public string student_name { get; set; }
            public string student_id { get; set; }
            public string student_phone { get; set; }
            public string student_email { get; set; }
            public string student_github { get; set; }
            public string student_remarks { get; set; }
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            MySQL_Helper mysqlConnector = new MySQL_Helper();
            // 测试连接
            if (mysqlConnector.setConnection("root", "root", "windows_programmng"))
            {
                Console.WriteLine("<!---------------连接成功！--------------->");
            }
            else
            {
                Console.WriteLine("<!---------------连接失败！--------------->");
                return;
            }
            // 查询数据
            DataSet ds = mysqlConnector.Query("studentinfo");
            List<TabName> mlist = DataSetToList<TabName>(ds, 0);
            dataGrid.ItemsSource = mlist;
        }

        /// <summary>
        /// DataSet转换成List
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="dataSet">数据源</param>
        /// <param name="tableIndex">需要转换表的索引</param>
        /// <returns></returns>
        public List<TabName> DataSetToList<T>(DataSet dataSet, int tableIndex)
        {
            //确认参数有效
            if (dataSet == null || dataSet.Tables.Count <= 0 || tableIndex < 0)
                return null;

            DataTable dt = dataSet.Tables[tableIndex];

            List<TabName> list = new List<TabName>();

            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TabName tabName_cache = new TabName();
                tabName_cache.student_name = dt.Rows[i][0].ToString();
                tabName_cache.student_id = dt.Rows[i][1].ToString();
                tabName_cache.student_phone = dt.Rows[i][2].ToString();
                tabName_cache.student_email = dt.Rows[i][3].ToString();
                tabName_cache.student_github = dt.Rows[i][4].ToString();
                tabName_cache.student_remarks = dt.Rows[i][5].ToString();
                list.Add(tabName_cache);
            }
            return list;
        }

        private async void groupButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

            ContentDialog content_dialog = new ContentDialog()
            {
                Title = "删除",
                Content = "确认删除这条数据？",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消",
                FullSizeDesired = false,
            };

            content_dialog.PrimaryButtonClick += (_s, _e) => { };

            ContentDialogResult dialogResult = await content_dialog.ShowAsync();
            if(dialogResult == ContentDialogResult.Primary)
            {
                TabName tabName = dataGrid.SelectedItem as TabName;
                
                MySQL_Helper mysqlConnector = new MySQL_Helper();

                mysqlConnector.setConnection("root", "root", "windows_programmng");
                string condition = "学号='" + tabName.student_id.ToString() + "';";
                int x = mysqlConnector.deleteSql("studentinfo", condition);
                if(x == 1)
                {
                    // 删除成功后更新数据
                    DataSet ds = mysqlConnector.Query("studentinfo");
                    List<TabName> mlist = DataSetToList<TabName>(ds, 0);
                    dataGrid.ItemsSource = mlist;
                }
                else
                {
                    ContentDialog deleteFailed = new ContentDialog()
                    {
                        Title = "删除失败",
                        Content = "出现未知错误，请在控制台查看错误信息。",
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };

                    deleteFailed.PrimaryButtonClick += (_s, _e) => { };
                    await deleteFailed.ShowAsync();

                }
            }

        }

    }
}
