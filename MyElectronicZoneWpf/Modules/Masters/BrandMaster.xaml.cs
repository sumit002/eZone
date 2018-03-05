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
using MyElectronicZoneWpf.DataLayer;
using System.Data;
using MyElectronicZoneWpf.Utils;

namespace MyElectronicZoneWpf.Modules.Masters
{
    /// <summary>
    /// Interaction logic for BrandMaster.xaml
    /// </summary>
    public partial class BrandMaster : MahApps.Metro.Controls.MetroWindow
    {
        ILogger logger = new Logger(typeof(BrandMaster));
        //private DataTable dtBrands = new DataTable();
        public BrandMaster()
        {
            InitializeComponent();
            this.txtBrandName.Focus();
            //loadBrands();

            // on esc close
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void loadBrands()
        {
            DataTable dtBrands = new DataTable();
            DataAccess da = new DataAccess();

            dtBrands = da.GetAllBrands();
            datagridBrands.ItemsSource = dtBrands.DefaultView;
        }

        private bool validateForm()
        {
            DataAccess da = new DataAccess();
            if (string.IsNullOrEmpty(txtBrandName.Text.Trim()))
            {
                txtBrandName.Focus();
                return false;
            }
            else if (da.ifExistsValue("tblBrandMaster", "Name", txtBrandName.Text.Trim()))
            {
                MessageBoxResult result = MessageBox.Show("Name already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else
                return true;
        }

        # region events
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
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
                    folderFields.Add("Name", txtBrandName.Text);
                    folderFields.Add("Description", txtBrandDesc.Text);
                    folderFields.Add("CreatedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    folderFields.Add("ModifiedDate", null);

                    DataAccess dataAccess = new DataAccess();
                    int status = dataAccess.InsertOrUpdateBrandMaster(folderFields, "tblBrandMaster");
                    //check if it is insert/updated
                    if (status == 1)
                    {
                        MessageBoxResult result = MessageBox.Show("Brand Added Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ResetForm();
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Error While Adding Brand!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                //bind brand list on demand
                if (tabControl1.SelectedIndex != 0)
                    loadBrands();
            }
        }
        # endregion

        /// <summary>
        /// Reset Form Fields
        /// </summary>
        private void ResetForm()
        {
            this.txtBrandName.Text = "";
            this.txtBrandDesc.Text = "";
        }

    }

}
