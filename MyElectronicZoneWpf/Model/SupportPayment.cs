using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyElectronicZoneWpf.Model
{
    public class SupportPayment
    {
        #region Variables
        int _id;
        string _supportDate = string.Empty;
        double _amount;
        string _description = string.Empty;
        string _remarks = string.Empty;
        #endregion

        #region Properties  
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string SupportDate
        {
            get{ return _supportDate; }
            set { _supportDate = value; }
        }

        public double AmountEarned
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }
        #endregion 
    }
}
