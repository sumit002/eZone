// -----------------------------------------------------------------------
// <copyright file="Common.cs" company="ElectronicZone">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MyElectronicZoneWpf.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    /// <summary>
    /// 
    /// </summary>
    public class Common
    {
        #region properties
        static readonly int topRow = 5;
        #endregion properties
        /// <summary>
        /// Sort Table By Column
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sortByColumn"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public static DataTable sortTable(DataTable dt, string sortByColumn, bool? isDesc = false) {
            DataView dv = dt.DefaultView;
            dv.Sort = string.Format("{0} {1}", sortByColumn, isDesc == true ? "DESC" : "ASC");
            dt = dv.ToTable();
            return dt;
        }
        /// <summary>
        /// Select Top 5 rows from DataTable By Linq
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static DataTable GetTopRow(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return dataTable;
            return dataTable.Rows.Cast<System.Data.DataRow>().Take(topRow).CopyToDataTable();
        }

        public static decimal getSum(DataTable dataTable, string columnName)
        {
            if (dataTable.Rows.Count == 0)
                return 0;
            var d = dataTable.Compute("Sum(" + columnName + ")", "");
            //return (decimal) d.ToString("C2");
            return decimal.Parse(d.ToString());
        }
    }
}
