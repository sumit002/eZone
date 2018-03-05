using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using MyElectronicZoneWpf.Modules.Masters;
using MahApps.Metro.Controls;
using System.Data;
using MyElectronicZoneWpf.DataLayer;
using MyElectronicZoneWpf.Modules.Reports;
using MyElectronicZoneWpf.Utils;
using System.Globalization;
using MyElectronicZoneWpf.Modules.Screen;

namespace MyElectronicZoneWpf
{
    /// <summary>
    /// Interaction logic for UserDashboard.xaml
    /// </summary>
    public partial class UserDashboard : MetroWindow
    {
        #region properties
        string fromDate = string.Empty;
        string toDate = string.Empty;
        ILogger logger = new Logger(typeof(UserDashboard));
        #endregion

        public UserDashboard()
        {
            InitializeComponent();
            InitializeSettings();
            //set dates for dashboard result
            DateTimeUtility dtUtility = new DateTimeUtility();
            dpDashboardFromDate.SelectedDate = dtUtility.getMonthStartDate();
            dpDashboardToDate.SelectedDate = DateTime.Now;

            SetDate();

            loadDashboardComponrnts();
        }

        #region functions
        private void SetDate()
        {
            fromDate = string.IsNullOrEmpty(dpDashboardFromDate.Text) ? "" : (DateTime.Parse(dpDashboardFromDate.Text).ToString("yyyy-MM-dd HH:mm:ss"));
            toDate = string.IsNullOrEmpty(dpDashboardToDate.Text) ? "" : (DateTime.Parse(dpDashboardToDate.Text).ToString("yyyy-MM-dd HH:mm:ss"));
        }
        private void loadDashboardComponrnts()
        {
            try
            {
                getPaymentIncome();
                getPaymentInvest();

                // load top 5 Pending Payments
                loadTopPendingPayment();

                // load top 5 Sales
                loadTopSales();

                // load top 5 Purchases
                loadTopPurchases();

                // load top 5 Sellers
                loadTopSellers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                logger.LogException(ex);
            }
        }

        private void getPaymentInvest()
        {
            DataAccess da = new DataAccess();
            DataTable dtPurchaseInvest = da.SearchPaymentInvest(fromDate, toDate, PaymentTransaction.PaymentStatus.PURCHASE_PAYMENT.ToString());

            tbTotalPurchasePayment.Text = Common.getSum(dtPurchaseInvest, "Amount").ToString("F", CultureInfo.InvariantCulture);
        }

        private void getPaymentIncome()
        {
            DataAccess da = new DataAccess();
            DataTable dtSupportIncome = da.SearchPaymentIncome(fromDate, toDate, PaymentTransaction.PaymentStatus.SUPPORT_PAYMENT.ToString());
            tbTotalSupportIncome.Text = Common.getSum(dtSupportIncome, "Amount").ToString("F", CultureInfo.InvariantCulture);

            DataTable dtSaleIncome = da.SearchPaymentIncome(fromDate, toDate, PaymentTransaction.PaymentStatus.SALE_PAYMENT.ToString());
            tbTotalSaleIncome.Text = Common.getSum(dtSaleIncome, "Amount").ToString("F", CultureInfo.InvariantCulture);

            tbTotalIncome.Text = (Common.getSum(dtSupportIncome, "Amount") + Common.getSum(dtSaleIncome, "Amount")).ToString("F", CultureInfo.InvariantCulture);
        }

        private void loadTopPendingPayment()
        {
            DataTable dtPending = new DataTable();
            DataAccess da = new DataAccess();
            dtPending = da.SearchPendingPayment(null, null, fromDate, toDate, string.Empty, 0);
            // soprt Column and Select top 5
            dtPending = Common.sortTable(dtPending, "PendingAmount", true);
            dtPending = Common.GetTopRow(dtPending);
            // bind to dataGrid
            dataGridPendingPayment.ItemsSource = dtPending.DefaultView;
        }

        private void loadTopSales()
        {
            DataTable dtSales = new DataTable();
            DataAccess da = new DataAccess();
            dtSales = da.SearchSales(string.Empty, string.Empty, string.Empty, string.Empty, null, null, fromDate, toDate, string.Empty);
            // sort Column and Select top 5
            dtSales = Common.sortTable(dtSales, "Total", true);
            dtSales = Common.GetTopRow(dtSales);
            // bind to dataGrid
            dataGridTopSales.ItemsSource = dtSales.DefaultView;
        }

        private void loadTopPurchases()
        {
            DataTable dtPurchase = new DataTable();
            DataAccess da = new DataAccess();
            dtPurchase = da.SearchStocks(string.Empty, string.Empty, string.Empty, string.Empty, null, null, fromDate, toDate);
            // sort Column and Select top 5
            dtPurchase = Common.sortTable(dtPurchase, "PurchasePrice", true);
            dtPurchase = Common.GetTopRow(dtPurchase);
            // bind to dataGrid
            dataGridTopPurchases.ItemsSource = dtPurchase.DefaultView;
        }

        private void loadTopSellers()
        {

        }

        private void validateCalendarMinMaxDate()
        {
            try
            {
                this.dpDashboardFromDate.DisplayDateEnd = string.IsNullOrEmpty(dpDashboardToDate.Text) ? (DateTime?)null : DateTime.Parse(dpDashboardToDate.Text);
                this.dpDashboardToDate.DisplayDateStart = string.IsNullOrEmpty(dpDashboardFromDate.Text) ? (DateTime?)null : DateTime.Parse(dpDashboardFromDate.Text);
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
            }
        }

        private void InitializeSettings()
        {
            menuReports.Visibility = chkbShowReports.IsChecked == true ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }
        #endregion

        #region events

        private void btnDashboardResult_Click(object sender, RoutedEventArgs e)
        {
            loadDashboardComponrnts();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            //SoundPlayer asd = new SoundPlayer(@"e:\3D_drums.wav");// it requires wmv files only
            //asd.Play();
            Info info = new Info();
            info.ShowDialog();
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            AboutUs about = new AboutUs();
            about.ShowDialog();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }
        
        private void RegisterProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Registration will be available soon!", "Coming Soon!", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Update will be available soon!", "Coming Soon!", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void TechnicalSupport_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Technical Support will be available soon!", "Coming Soon!", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void Invoice_Click(object sender, RoutedEventArgs e)
        {
            SalesInvoice salesInvoice = new SalesInvoice();
            salesInvoice.ShowDialog();
        }
        

        // Masters Section
        private void ProductMaster_Click(object sender, RoutedEventArgs e)
        {
            ProductMaster product = new ProductMaster();
            product.ShowDialog();
        }

        private void BrandMaster_Click(object sender, RoutedEventArgs e)
        {
            BrandMaster brand = new BrandMaster();
            brand.ShowDialog();
        }

        private void StockMaster_Click(object sender, RoutedEventArgs e)
        {
            StockMaster stock = new StockMaster();
            stock.ShowDialog();
        }

        private void Sale_Click(object sender, RoutedEventArgs e)
        {
            SaleMaster sale = new SaleMaster();
            sale.ShowDialog();
        }

        private void Support_Click(object sender, RoutedEventArgs e)
        {
            SupportMaster supportPayment = new SupportMaster();
            supportPayment.ShowDialog();
        }

        // Reports Section
        private void SalesReport_Click(object sender, RoutedEventArgs e)
        {
            SalesReport salesReport = new SalesReport();
            salesReport.ShowDialog();
        }

        private void PurchaseReport_Click(object sender, RoutedEventArgs e)
        {
            PurchaseReport purchaseReport = new PurchaseReport();
            purchaseReport.ShowDialog();
        }

        private void SupportPaymentReport_Click(object sender, RoutedEventArgs e)
        {
            SupportPaymentReport supportPaymentReport = new SupportPaymentReport();
            supportPaymentReport.ShowDialog();
        }

        private void ContactsReport_Click(object sender, RoutedEventArgs e)
        {
            ContactReport contactReport = new ContactReport();
            contactReport.ShowDialog();
        }

        private void PendingPaymentReport_Click(object sender, RoutedEventArgs e)
        {
            PendingPaymentReport pendingPaymentReport = new PendingPaymentReport();
            pendingPaymentReport.ShowDialog();
        }

        private void Contact_Click(object sender, RoutedEventArgs e)
        {
            ContactMaster contactMaster = new ContactMaster();
            contactMaster.ShowDialog();
        }

        private void dpDashboardFromDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            validateCalendarMinMaxDate();
            SetDate();
        }

        private void dpDashboardToDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            validateCalendarMinMaxDate();
            SetDate();
        }

        private void PendingPayment_Click(object sender, RoutedEventArgs e)
        {
            PendingPayment pendingPayment = new PendingPayment();
            pendingPayment.Show();
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            InitializeSettings();
        }
        #endregion
    }
}
