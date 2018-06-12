using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Configuration;

namespace Gazpar_History
{
    class SQL
    {
        public SQL()
        {
            connect();
        }

        private static string DBinfo = "server=PLKWIM0ITRON01;UID=svc_itron;password=AdG9t&bjfN;Trusted_Connection=false;database=WasMfg;connection timeout=5";// ConfigurationManager.AppSettings["DBinfo"];
        SqlConnection conn = new SqlConnection(DBinfo);

        public string GetHistory(string serialNumber)
        {
            string query = History(serialNumber);
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            string last_sn = "";
            while (reader.Read())
            {
                last_sn = reader["SerialNumber"].ToString();
            }
            reader.Close();
            return last_sn;
        }

        public DataTable GetHistoryTable(string serialNumber)
        {
            string query = HistoryWithDetail(serialNumber);
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            var answer = new DataTable();
            adapter.Fill(answer);
            return answer;
        }

        private void connect()
        {
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error", e.ToString());
            }
        }

        private string History(string SerialNumber)
        {
            return  "SELECT     TestEvent.SerialNumber " +
                    "FROM TestEvent INNER JOIN " + 
                    "StationConfiguration ON TestEvent.StationConfigKey = StationConfiguration.StationConfigKey " +
                    "WHERE(Testevent.Mfgserialnumber = '" + SerialNumber + "')";
        }

        private string HistoryWithDetail(string SerialNumber)
        {
            string answer = "SELECT     TestEvent.TestEventKey, TestEvent.SerialNumber, TestEvent.MfgSerialNumber, TestEvent.PartNumber, TestEvent.ProcessFlowKey, TestEvent.BatchKey, TestEvent.DataCategoryID, " +
                    "StationConfiguration.StationKey, TestEvent.Passed, TestEvent.TestDate, TestEvent.Comment " +
                    "FROM         TestEvent Inner Join " +
                    "StationConfiguration ON TestEvent.StationConfigKey = StationConfiguration.StationConfigKey " +
                    "WHERE(";
            return ((SerialNumber[0] == 'J') || (SerialNumber[0] == 'j')) ? answer + "TestEvent.Mfgserialnumber = '" + SerialNumber + "')" : answer + "Testevent.SerialNumber = '" + SerialNumber + "')";
        }
    }
}
