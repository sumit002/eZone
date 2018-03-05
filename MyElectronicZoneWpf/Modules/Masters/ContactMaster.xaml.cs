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

namespace MyElectronicZoneWpf.Modules.Masters
{
    /// <summary>
    /// Interaction logic for ContactMaster.xaml
    /// </summary>
    public partial class ContactMaster : MahApps.Metro.Controls.MetroWindow
    {
        ILogger logger = new Logger(typeof(ContactMaster));
        public ContactMaster()
        {
            InitializeComponent();
            this.cbSalutation.Focus();
            loadSalutation();

            // on esc close
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                //bind brand list on demand
                if (tabControl1.SelectedIndex != 0)
                    loadContacts();
            }
        }

        private void loadContacts()
        {
            DataTable dtContacts = new DataTable();
            DataAccess da = new DataAccess();

            dtContacts = da.GetAllSalesPerson();
            datagridContacts.ItemsSource = dtContacts.DefaultView;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
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
                    folderFields.Add("Title", (cbSalutation.SelectedValue == null ? string.Empty : cbSalutation.SelectedValue.ToString()));
                    folderFields.Add("Name", txtName.Text);
                    folderFields.Add("Contact", txtContact.Text);
                    folderFields.Add("AlternateContact", txtAltContact.Text);
                    folderFields.Add("Email", txtEmail.Text);
                    folderFields.Add("Address", txtAddress.Text);

                    DataAccess dataAccess = new DataAccess();
                    int status = dataAccess.InsertOrUpdateSalePerson(folderFields, "tblSalePerson");
                    //check if it is insert/updated
                    if (status > 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Contact Added Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ResetForm();
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Error While Adding Contact!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
        
        private void loadSalutation()
        {
            List<string> salutationLst = new List<string>();
            salutationLst.Add("Mr");
            salutationLst.Add("Ms");
            salutationLst.Add("Miss");
            // bind to combobox
            cbSalutation.ItemsSource = salutationLst;
        }

        private bool validateForm()
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                txtName.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtContact.Text.Trim()))
            {
                txtContact.Focus();
                return false;
            }
            else
                return true;
        }

        private void ResetForm()
        {
            this.txtName.Text = "";
            this.txtContact.Text = "";
            this.txtAltContact.Text = "";
            this.txtEmail.Text = "";
            this.txtAddress.Text = "";
            this.cbSalutation.SelectedIndex = -1;
        }
    }
}
