using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModCallerId
{
    class ModConnection
    {

        //Connection

        public static SqlConnection ConDbTanmyaMarket = new SqlConnection();
        static string VarDbServerIP;
        static string VarDbServerName;
        static string VarDbName;
        static string VarDBInstanceName;
        static string VarDbUserID = "tanmya";
        static string VarDbUserPW = "mrmr16112007";


        //BackUp Path
        public string VarDbBackupPath ;

        public static SqlConnection SubConDbTanmyaMarketString()
        {
            SqlConnection con = new SqlConnection();
            try
            {

                if (ConDbTanmyaMarket.State == ConnectionState.Closed)
                {

                    VarDbServerName = File.ReadLines("TxtConnectionInfo.txt").Skip(0).Take(1).First();
                    VarDbServerIP = File.ReadLines("TxtConnectionInfo.txt").Skip(1).Take(1).First();
                    VarDbName = File.ReadLines("TxtConnectionInfo.txt").Skip(2).Take(1).First();
                    VarDBInstanceName = File.ReadLines("TxtConnectionInfo.txt").Skip(3).Take(1).First();

                    //MessageBox.Show(VarDbServerName + Environment.NewLine + VarDbServerIP + Environment.NewLine + VarDbName + Environment.NewLine + VarDBInstanceName);
                    //ConDbTanmyaMarket.ConnectionString = "Data Source=" + VarDbServerName + ";" + "Initial Catalog=" + VarDbName + ";" + ";" + "User ID=" + VarDbUserID + ";" + ";" + ";" + "Password=" + VarDbUserPW + ";Connection Timeout=15";

                    con = new SqlConnection("Data Source=" + VarDbServerName + ";" + "Initial Catalog=" + VarDbName + ";" + ";" + "User ID=" + VarDbUserID + ";" + ";" + ";" + "Password=" + VarDbUserPW + ";Connection Timeout=15");

           

                    return con;
                }

                return con;
            }
            catch (Exception ex)
            {
        
            }
            return con;
        }

    }
}
