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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.SQLite;
using MyElectronicZoneWpf.DataLayer;
using System.Data;
using MahApps.Metro.Controls;
using MyElectronicZoneWpf.Utils;
using System.IO;

namespace MyElectronicZoneWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class loginWindow : MetroWindow
    {
        //private readonly Context _ezContext;
        ILogger logger = new Logger(typeof(loginWindow));
        public loginWindow()
        {
            InitializeComponent();
            LoadUIControls();
            OnLoginLoad();
        }

        private void LoadUIControls()
        {
            //string asd = _ezContext.GetCultureInfo.m_resourceManger.GetString("LoginPasswordLabel");
            lblUsername.Content = "Username";// _ezContext.GetCultureInfo.m_resourceManger.GetString("LoginPasswordLabel");
        }

        private bool validateForm()
        {
            if (string.IsNullOrEmpty(txtUsername.Text.Trim())){
                txtUsername.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPassword.Password.Trim())){
                txtPassword.Focus();
                return false;
            }
            else
                return true;
        }

        private void OnLoginLoad()
        {
            this.txtUsername.Focus();
        }
        /// <summary>
        /// on login button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            DataAccess dataAccess = new DataAccess();
            try {
                if (validateForm()){
                    dt = dataAccess.validateUserLogin(this.txtUsername.Text.Trim(), this.txtPassword.Password.Trim());
                    if (dt.Rows.Count == 1)
                    {
                        this.Hide();
                        UserDashboard form = new UserDashboard();
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password!");
                        logger.LogError("Invalid Username or Password");
                    }
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Invalid Data ! Please check the fields entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                logger.LogException(ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
            }
            
        }
    }
}
