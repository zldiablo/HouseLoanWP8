using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HouseLoan;

namespace HouseLoadWP8
{
    public partial class ResultPage : PhoneApplicationPage
    {
        private static readonly double[,] Interest = { { 0.045, 0.04 }, { 0.0655, 0.064 } };

        #region private properties
        private int LoanType; // 0: 公积金 1:商业贷款
        private double LoanAmount;
        private int LoanMonths; // months
        private double InterestChange;
        private int PayType;
        #endregion

        public ResultPage()
        {
            InitializeComponent();
            ContentPanel.Children.Clear();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.LoanAmount = double.Parse(NavigationContext.QueryString["amount"]);
            this.LoanAmount *= 10000;
            this.LoanType = int.Parse(NavigationContext.QueryString["type"]);
            this.LoanMonths = int.Parse(NavigationContext.QueryString["years"]) * 12;
            this.InterestChange = double.Parse(NavigationContext.QueryString["interestChange"]);
            this.PayType = int.Parse(NavigationContext.QueryString["payType"]);

            LoadResultList();
        }

        private void LoadResultList()
        {
            double monthlyInterest;
            if (this.LoanMonths <= 60)
            {
                monthlyInterest = Interest[this.LoanType, 1];
            }
            else
            {
                monthlyInterest = Interest[this.LoanType, 0];
            }
            monthlyInterest /= 12.0;
            monthlyInterest *= (1 + this.InterestChange);
            if (PayType == 0)
            {
                //等额本息
                double pay = LoanCalculator.Calculate1(this.LoanAmount, monthlyInterest, this.LoanMonths);
                TextBlock subtitle = new TextBlock();
                subtitle.Text = "每月还款额度";
                subtitle.FontSize = 30;
                TextBlock tb = new TextBlock();
                tb.Text = string.Format("{0:f} 元", pay);
                ContentPanel.Children.Add(subtitle);
                tb.FontSize = 30;
                ContentPanel.Children.Add(tb);

                TextBlock summary = new TextBlock();
                summary.Text = string.Format("总还款 {0:f} 元", pay * this.LoanMonths);
                summary.FontSize = 30;
                ContentPanel.Children.Add(summary);
            }
            else
            {
                //等额本金
                double[] pay = LoanCalculator.Calculate2(this.LoanAmount, monthlyInterest, this.LoanMonths);
                TextBlock subtitle = new TextBlock();
                subtitle.Text = "每月还款额度";
                subtitle.FontSize = 30;
                ContentPanel.Children.Add(subtitle);
                double total = 0;
                for (int i = 0; i < pay.Length; i++)
                {
                    TextBlock tb = new TextBlock();
                    tb.Text = string.Format("第{0}月: {1:f} 元", i + 1, pay[i]);
                    tb.FontSize = 24;
                    ContentPanel.Children.Add(tb);
                    total += pay[i];
                }

                TextBlock summary = new TextBlock();
                summary.Text = string.Format("总还款 {0:f} 元", total);
                summary.FontSize = 30;
                ContentPanel.Children.Insert(1, summary);
            }
        }
    }
}