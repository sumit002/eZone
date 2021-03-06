﻿using System;
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
using MyElectronicZoneWpf.Model;
using System.Reflection;
using System.ComponentModel;

namespace MyElectronicZoneWpf.Modules.Masters
{
    /// <summary>
    /// Interaction logic for SupportMaster.xaml
    /// </summary>
    public partial class SupportMaster : MahApps.Metro.Controls.MetroWindow
    {
        ILogger logger = new Logger(typeof(SupportMaster));
        public ICollectionView SupportPayment { get; private set; }
        public SupportMaster()
        {
            InitializeComponent();
            dpSupportDate.DisplayDateEnd = DateTime.Today;

            // on esc close
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetForm();
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
            }
        }

        /// <summary>
        /// Reset Support Form
        /// </summary>
        private void ResetForm()
        {
            this.dpSupportDate.Text = "";
            this.txtAmountEarned.Value = null;
            this.txtSupportDesc.Text = "";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validateForm())
                {
                    //create record
                    Dictionary<string, string> folderFields = new Dictionary<string, string>();
                    folderFields.Add("Id", null);
                    folderFields.Add("Description", txtSupportDesc.Text);
                    folderFields.Add("Amount", txtAmountEarned.Value.ToString());
                    folderFields.Add("SupportDate", (DateTime.Parse(dpSupportDate.Text).ToString("yyyy-MM-dd HH:mm:ss")));
                    //folderFields.Add("Remarks", null);

                    DataAccess dataAccess = new DataAccess();
                    int rslt = dataAccess.InsertOrUpdateSupportPaymentMaster(folderFields, "tblSupportPaymentMaster");
                    if (rslt > 0)
                    {
                        PaymentTransaction paymentTransaction = new PaymentTransaction();
                        bool paymentStatus = paymentTransaction.AddPaymentTransaction(1, double.Parse(txtAmountEarned.Value.ToString()), PaymentTransaction.PaymentStatus.SUPPORT_PAYMENT, rslt);
                        if (paymentStatus)
                        {
                            MessageBoxResult result = MessageBox.Show("Payment Added Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            ResetForm();
                        }
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Error While Adding Payment!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Invalid Data ! Please check the fields entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
            }
        }

        private bool validateForm()
        {
            if (string.IsNullOrEmpty(dpSupportDate.Text.Trim()))
                return false;
            else if (string.IsNullOrEmpty(txtAmountEarned.Value.ToString()))
                return false;
            else if (string.IsNullOrEmpty(txtSupportDesc.Text.Trim()))
                return false;
            else
                return true;
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                //bind support list on demand
                if (tabControl1.SelectedIndex != 0)
                    loadSupportPayment();
            }
        }

        
        private void loadSupportPayment()
        {
            DataAccess da = new DataAccess();
            DataTable dtSupportPayment = da.GetAllSupportPayment(String.Empty);

            //List<SupportPayment> supportList = dtSupportPayment.DataTableToList<SupportPayment>();
            //List<SupportPayment> list = dtSupportPayment.ToList<SupportPayment>();// use this
            //SupportPayment = CollectionViewSource.GetDefaultView(supportList);
            
            datagridSupportPayment.ItemsSource = dtSupportPayment.DefaultView;
            //datagridStocks.UpdateLayout();
        }

        
    }
}
