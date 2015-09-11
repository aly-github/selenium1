
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.OleDb;


/// <summary>
/// Summary description for RH_DB
/// </summary>
public class DB_Connect
{
    public static SqlConnection GetSQLConnection()
    {

        return new SqlConnection("Data Source=KOMPUTA;Initial Catalog=AutoTesting;User ID=alyd;Password=Password1");
    }
   
   

    public static SqlConnection GetUserSQLConnection()
    {
        return new SqlConnection("Data Source=KOMPUTA;Initial Catalog=AutoTesting;User ID=MIL_SEG;Password=Password1");
    }

    public static void ExecuteProcedure(string sqlcmd)
    {
        SqlCommand SqlCommand = new SqlCommand(sqlcmd);
        SqlCommand.CommandType = CommandType.Text;
        ExecuteProcedure(SqlCommand);
    }
    public static Boolean doesItemExist(string tablename, string columnName, string itemtocheck)
    {
        Boolean rtn = true;
        StringBuilder qry = new StringBuilder("Select [%columnName%] from [%tablename%]  where  [%columnName%]=@itemtocheck");
        qry.Replace("[%columnName%]", columnName);
        qry.Replace("[%tablename%]", tablename);
        SqlCommand cmd = new SqlCommand(qry.ToString());
        cmd.Parameters.Add("@itemtocheck", SqlDbType.VarChar).Value = itemtocheck;
        DataTable dt = null;
        dt = DB_Connect.ExecuteReaderTable(cmd);
        if (dt != null && dt.Rows.Count == 0)
        {
            rtn = false;
        }

        return rtn;
    }

    public static void ExecuteProcedure(SqlCommand SqlCommand)
    {
        SqlConnection SQLCon = GetSQLConnection();
        SQLCon.Open();
        try
        {
            SqlCommand.Connection = SQLCon;
            SqlCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SQLCon.Close();
        }
    }

    public static SqlDataReader ExecuteReader(string sqlcmd)
    {
        SqlCommand SqlCommand = new SqlCommand(sqlcmd);
        SqlCommand.CommandType = CommandType.Text;
        return ExecuteReader(SqlCommand);
    }

    public static SqlDataReader ExecuteReader(SqlCommand SqlCommand)
    {
        SqlConnection SQLCon = GetSQLConnection(); ;
        SqlDataReader sdr = null;

        try
        {
            SqlCommand.Connection = SQLCon;
            SQLCon.Open();
            sdr = SqlCommand.ExecuteReader();
        }
        catch (Exception ex)
        {
            string v = ex.Message;
        }
        finally
        {
            //if (SQLCon != null) SQLCon.Close();
        }
        return sdr;
    }

    public static SqlDataReader ExecuteReader(SqlCommand SqlCommand, SqlConnection SqlCon)
    {
        SqlDataReader sdr = null;
        try
        {
            SqlCommand.Connection = SqlCon;
            sdr = SqlCommand.ExecuteReader();
        }
        catch (Exception ex)
        {
            string v = ex.Message;
        }
        return sdr;
    }

    public static DataTable ExecuteReaderTable(SqlCommand SqlCommand)
    {
        DataTable table = new DataTable();
        SqlConnection SQLCon = GetSQLConnection();
        SqlCommand.Connection = SQLCon;

        try
        {
            SQLCon.Open();
            SqlDataAdapter Adapter = new SqlDataAdapter(SqlCommand);
            Adapter.Fill(table);
        }
        catch (Exception ex)
        {
            string test = ex.Message;
        }
        finally
        {
            if (SQLCon != null) SQLCon.Close();
        }
        return table;
    }

    public static DataTable ExecuteReaderTable(string sqlcmd)
    {
        SqlCommand SqlCommand = new SqlCommand(sqlcmd);
        SqlCommand.CommandType = CommandType.Text;

        return ExecuteReaderTable(SqlCommand);
    }

    public static DataTable ExecuteReaderTableUsers(SqlCommand SqlCommand)
    {
        DataTable table = new DataTable();
        SqlConnection SQLCon = GetUserSQLConnection();
        SqlCommand.Connection = SQLCon;

        try
        {
            SQLCon.Open();
            SqlDataAdapter Adapter = new SqlDataAdapter(SqlCommand);
            Adapter.Fill(table);
        }
        catch (Exception ex)
        {
            string test = ex.Message;

        }
        finally
        {
            if (SQLCon != null) SQLCon.Close();

        }
        return table;
    }

    public static DataTable ExecuteReaderTableUsers(string sqlcmd)
    {
        SqlCommand SqlCommand = new SqlCommand(sqlcmd);
        SqlCommand.CommandType = CommandType.Text;

        return ExecuteReaderTableUsers(SqlCommand);
    }

    public static void ExecuteUserProcedure(SqlCommand SqlCommand)
    {
        SqlConnection SQLCon = GetUserSQLConnection();
        SQLCon.Open();
        try
        {
            SqlCommand.Connection = SQLCon;
            SqlCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SQLCon.Close();
        }
    }

    public static void ExecuteBulkCopy(DataTable Data, string Destination)
    {
        SqlConnection SqlCon = GetSQLConnection();
        SqlBulkCopy SqlBulkCopy = new SqlBulkCopy(SqlCon);
        try
        {
            SqlCon.Open();
            SqlBulkCopy.DestinationTableName = Destination;
            SqlBulkCopy.WriteToServer(Data);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (SqlBulkCopy != null) SqlBulkCopy.Close();
            if (SqlCon != null) SqlCon.Close();
        }
    }
    public static void BulkCopy(DataTable Data)
    {
        SqlConnection SQLCon = GetSQLConnection();
        SqlBulkCopy SqlBulkCopy = new SqlBulkCopy(SQLCon);
        SqlBulkCopy.DestinationTableName = "tbl_users";
        try
        {
            SQLCon.Open();
            SqlBulkCopy.WriteToServer(Data);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (SQLCon != null) SQLCon.Close();
        }

    }

    public static void BulkCopy(DataTable Data, string tbl)
    {
        SqlConnection SQLCon = GetSQLConnection();
        SqlBulkCopy SqlBulkCopy = new SqlBulkCopy(SQLCon);
        SqlBulkCopy.DestinationTableName = tbl;
        try
        {
            SQLCon.Open();
            SqlBulkCopy.WriteToServer(Data);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (SQLCon != null) SQLCon.Close();
        }

    }

   


   

}

