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
using System.IO;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using MyElectronicZoneWpf.Modules.Print;

namespace MyElectronicZoneWpf.Modules.Screen
{
    /// <summary>
    /// Interaction logic for SalesInvoice.xaml
    /// </summary>
    public partial class SalesInvoice : MahApps.Metro.Controls.MetroWindow
    {
        public SalesInvoice()
        {
            InitializeComponent();
        }
        public FixedDocumentSequence Document { get; set; }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("printPreview.xps"))
            {
                File.Delete("printPreview.xps");
            }
            var xpsDocument = new XpsDocument("printPreview.xps", FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
            writer.Write(((IDocumentPaginatorSource)FD).DocumentPaginator);
            Document = xpsDocument.GetFixedDocumentSequence();
            xpsDocument.Close();
            var windows = new PrintWindow(Document);
            windows.ShowDialog();




            //PrintDialog printDialog = new PrintDialog();
            //if (printDialog.ShowDialog() == true)
            //{
            //    printDialog.PrintVisual(this, this.Title);
            //}
        }
    }
}
