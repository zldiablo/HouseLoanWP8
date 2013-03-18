using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HouseLoadWP8.Resources;

namespace HouseLoadWP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        private int[] loanYears;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            loanYears = new int[30];
            for (int i = 0; i < 30; i++)
            {
                loanYears[i] = 30 - i;
            }
            LoanTime.ItemsSource = loanYears;

            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            string loanAmount = LoanAmount.Text;
            int loanType = LoanType.SelectedIndex;
            string loanMonths = LoanTime.SelectedItem.ToString();

            double[] interestChange = { 0, -0.15, 0.1 };
            int interestChangeSelect = InterestType.SelectedIndex;

            int payType = PayType.SelectedIndex;

            var frame = Application.Current.RootVisual as PhoneApplicationFrame;

            string url = "/ResultPage.xaml?amount={0}&type={1}&years={2}&interestChange={3}&payType={4}";

            Uri resultPageUri = new Uri(string.Format(url, loanAmount, loanType, loanMonths, interestChange[interestChangeSelect], payType), UriKind.Relative);
            frame.Navigate(resultPageUri);
        }

        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}