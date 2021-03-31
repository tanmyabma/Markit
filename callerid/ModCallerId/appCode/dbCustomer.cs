using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;

namespace ModCallerId.appCode
{
    class dbCustomer
    {

        SqlConnection con = ModConnection.SubConDbTanmyaMarketString();


        // Customer Insert
        public int funCustomerInsert(string TxtCustomerName, string TxtCustomerPhone, string TxtCustomerAddress)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(Procedures.eProcedures.SPCustomerInsert.ToString(), con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TxtCustomerName", SqlDbType.NVarChar, 50).Value = TxtCustomerName;
            cmd.Parameters.Add("@TxtCustomerPhone", SqlDbType.NVarChar, 50).Value = TxtCustomerPhone;
            cmd.Parameters.Add("@TxtCustomerAddress", SqlDbType.NVarChar, 50).Value = TxtCustomerAddress;
            int vTxtCustomerCode = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return vTxtCustomerCode;
        }


        // Customer By Phone
        public DataTable funCustomerInfoByPhone(string TxtCustomerPhone)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(Procedures.eProcedures.SPCustomerInfoByPhone.ToString(), con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TxtCustomerPhone", SqlDbType.NVarChar, 50).Value = TxtCustomerPhone;
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            con.Close();
            return dt;
        }
    }
}
