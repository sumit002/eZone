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
using MyElectronicZoneWpf.Modules.Screen;
using System.IO;
using MyElectronicZoneWpf.Utils;

namespace MyElectronicZoneWpf.Modules.Masters
{
    /// <summary>
    /// Interaction logic for SaleMaster.xaml
    /// </summary>
    public partial class SaleMaster : MahApps.Metro.Controls.MetroWindow
    {
        ILogger logger = new Logger(typeof(SaleMaster));
        public SaleMaster()
        {
            InitializeComponent();
            //this.txtPriceTo.Minimum = (double) (this.txtPriceFrom.Value == null ? 1 : this.txtPriceFrom.Value);

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
            try{
                SearchSale();
            }
            catch (Exception ex){
                logger.LogException(ex);
            }
        }

        private void SearchSale()
        {
            DataTable dtProduct = new DataTable();
            DataAccess da = new DataAccess();
            dtProduct = da.SearchStocks((this.cbProduct.SelectedValue == null ? "" : this.cbProduct.SelectedValue.ToString()), (this.cbBrandCompany.SelectedValue == null ? "" : this.cbBrandCompany.SelectedValue.ToString()), this.txtProdCode.Text, this.txtStockCode.Text, (int?)this.txtPriceFrom.Value, (int?)this.txtPriceTo.Value, string.Empty, string.Empty, true);
            dataGridPurchase.ItemsSource = dtProduct.DefaultView;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try{
                ResetForm();
                dataGridPurchase.ItemsSource = null;
            }
            catch (Exception ex){
                logger.LogException(ex);
            }
        }

        //private void btnSaveSale_Click(object sender, RoutedEventArgs e)
        //{
        //    #region Add Sale Person
        //    int salePersonId = AddSalePerson();
        //    #endregion
        //    //create record
        //    Dictionary<string, string> folderFields = new Dictionary<string, string>();
        //    //folderFields.Add("Id", null);
        //    folderFields.Add("StockId", cbProduct.SelectedValue.ToString());///ItemId which has to be sale
        //    folderFields.Add("SalePersonId", "1");// get the saved Id from SalePerson
        //    folderFields.Add("Quantity", "1");
        //    folderFields.Add("Price", "100");
        //    folderFields.Add("Total", "200");
        //    folderFields.Add("AmountPaid", "100");
        //    folderFields.Add("Pending", "100");
        //    folderFields.Add("SaleDate", (DateTime.Parse("1/16/2016").ToString("yyyy-MM-dd HH:mm:ss")));
        //    folderFields.Add("CreatedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //    folderFields.Add("ModifiedDate", null);

        //    DataAccess dataAccess = new DataAccess();
        //    int status = dataAccess.InsertOrUpdateStockMaster(folderFields, "tblStockMaster");
        //    if (status == 1)
        //    {
        //        MessageBoxResult result = MessageBox.Show("Stock Added Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //        ResetForm();
        //    }
        //    else
        //    {
        //        MessageBoxResult result = MessageBox.Show("Error While Adding Stock!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private int AddSalePerson()
        //{
        //    int personId = 0;
        //    Dictionary<string, string> folderFields = new Dictionary<string, string>();
        //    //folderFields.Add("Id", null);
        //    folderFields.Add("StockId", cbProduct.SelectedValue.ToString());///ItemId which has to be sale
        //    folderFields.Add("SalePersonId", "1");// get the saved Id from SalePerson
        //    folderFields.Add("Quantity", "1");
        //    folderFields.Add("Price", "100");
        //    folderFields.Add("Total", "200");
        //    folderFields.Add("AmountPaid", "100");
        //    folderFields.Add("Pending", "100");
        //    folderFields.Add("SaleDate", (DateTime.Parse("1/16/2016").ToString("yyyy-MM-dd HH:mm:ss")));
        //    folderFields.Add("CreatedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //    folderFields.Add("ModifiedDate", null);

        //    DataAccess dataAccess = new DataAccess();
        //    int status = dataAccess.InsertOrUpdateStockMaster(folderFields, "tblStockMaster");
        //    if (status == 1)
        //    {
        //        personId = 1;
        //    }
        //    return personId;
        //}

        /// <summary>
        /// Load All Active Products
        /// </summary>
        private void loadProduct()
        {
            DataTable dtProduct = new DataTable();
            DataAccess da = new DataAccess();
            dtProduct = da.GetAllProducts();
            // bind to combobox
            cbProduct.ItemsSource = dtProduct.DefaultView;
        }
        /// <summary>
        /// Load All Active Brands
        /// </summary>
        private void loadBrands()
        {
            DataTable dtBrand = new DataTable();
            DataAccess da = new DataAccess();
            dtBrand = da.GetAllBrands();
            // bind to combobox
            cbBrandCompany.ItemsSource = dtBrand.DefaultView;
        }

        private void loadSales()
        {
            DataTable dtSales = new DataTable();
            DataAccess da = new DataAccess();

            dtSales = da.GetAllSales(string.Empty);
            datagridSales.ItemsSource = dtSales.DefaultView;
            //datagridStocks.UpdateLayout();
        }
        /// <summary>
        /// Reset Form Fields
        /// </summary>
        private void ResetForm()
        {
            // combobox
            this.cbProduct.SelectedIndex = -1;
            this.cbBrandCompany.SelectedIndex = -1;
            // textbox
            this.txtProdCode.Text = "";
            this.txtStockCode.Text = "";
            this.txtPriceFrom.Value = null;
            this.txtPriceTo.Value = null;
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                //bind Sales list on demand
                if (tabControl1.SelectedIndex == 0)
                {
                    loadProduct();
                    loadBrands();
                }
                else
                {
                    loadSales();
                }
            }
        }

        private void dataGridPurchase_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //Get the newly selected cells
            IList<DataGridCellInfo> selectedcells = e.AddedCells;

            DataRowView drv = (DataRowView)dataGridPurchase.SelectedItem;
            if (drv != null) {
                var selectedRow = drv.Row.ItemArray;

                //open modal for sale Item
                AddSale saleScreen = new AddSale(selectedRow);
                saleScreen.ShowDialog();
                //refresh sale data
                SearchSale();
            }
            
        }
    }
}
