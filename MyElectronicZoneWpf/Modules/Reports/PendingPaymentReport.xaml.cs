using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using MyElectronicZoneWpf.DataLayer;
using MyElectronicZoneWpf.Utils;

namespace MyElectronicZoneWpf.Modules.Reports
{
    /// <summary>
    /// Interaction logic for PendingPaymentReport.xaml
    /// </summary>
    public partial class PendingPaymentReport : MahApps.Metro.Controls.MetroWindow
    {
        ILogger logger = new Logger(typeof(PendingPaymentReport));
        public PendingPaymentReport()
        {
            InitializeComponent();
            loadSalesPerson();

            // on esc close
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dtPendingPayment = new DataTable();
                DataAccess da = new DataAccess();
                dtPendingPayment = da.SearchPendingPayment((int?)this.txtPriceFrom.Value, (int?)this.txtPriceTo.Value,
                    string.IsNullOrEmpty(fromDate.Text) ? "" : (DateTime.Parse(fromDate.Text).ToString("yyyy-MM-dd HH:mm:ss")),
                    string.IsNullOrEmpty(toDate.Text) ? "" : (DateTime.Parse(toDate.Text).ToString("yyyy-MM-dd HH:mm:ss")),
                    (this.cbSalesPerson.SelectedValue == null ? string.Empty : this.cbSalesPerson.SelectedValue.ToString()),
                    (this.chkbPaid.IsChecked == null ? 0 : this.chkbPaid.IsChecked == true ? 1 : 0));
                if (dtPendingPayment.Rows.Count > 0)
                {
                    btnExport.Visibility = System.Windows.Visibility.Visible;
                    dataGridPendingPayment.ItemsSource = dtPendingPayment.DefaultView;
                }
                else
                {
                    btnExport.Visibility = System.Windows.Visibility.Hidden;
                    dataGridPendingPayment.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetForm();
                dataGridPendingPayment.ItemsSource = null;
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
            }
        }

        private void ResetForm()
        {
            // textbox
            this.txtPriceFrom.Value = null;
            this.txtPriceTo.Value = null;
            //date picker
            this.fromDate.Text = "";
            this.toDate.Text = "";
            this.chkbPaid.IsChecked = false;
            this.cbSalesPerson.SelectedIndex = -1;
            // hide export button
            btnExport.Visibility = System.Windows.Visibility.Hidden;
        }
        private void loadSalesPerson()
        {
            DataTable dtPerson = new DataTable();
            DataAccess da = new DataAccess();
            dtPerson = da.GetAllSalesPerson();
            // bind to combobox
            cbSalesPerson.ItemsSource = dtPerson.DefaultView;
            cbSalesPerson.SelectedItem = null;
            //cbSalesPerson.SelectedIndex = -1;
        }

        private void fromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            validateCalendarMinMaxDate();
        }

        private void toDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            validateCalendarMinMaxDate();
        }

        private void validateCalendarMinMaxDate()
        {
            try
            {
                this.fromDate.DisplayDateEnd = string.IsNullOrEmpty(toDate.Text) ? (DateTime?)null : DateTime.Parse(toDate.Text);
                this.toDate.DisplayDateStart = string.IsNullOrEmpty(fromDate.Text) ? (DateTime?)null : DateTime.Parse(fromDate.Text);
            }
            catch (Exception ex)
            {
                //ex.Message;
                logger.LogException(ex);
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            goExcelOut goExcelOut = new Utils.goExcelOut();
            bool result = goExcelOut.generateExcel(dataGridPendingPayment, "PendingPaymentReport");
            if (result)
                MessageBox.Show("File exported successfully.");
        }
    }
}
