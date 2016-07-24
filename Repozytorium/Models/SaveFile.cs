using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;

public class SaveFile
{
    public string fileName { set; get; }
    public string fileExtension { set; get; }
    public byte[] data { set; get; }

    public string SaveFileToDB()
    {
        using (SqlConnection conn = new SqlConnection("Data Source=RAMILU-PC\\SQLEXPRESS;" +
             "Initial Catalog=ExampleDB;Integrated Security=True;Pooling=False"))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SaveFilesProc";
            cmd.Connection = conn;

            cmd.Parameters.AddWithValue("@Data", data);
            cmd.Parameters.AddWithValue("@FileName", fileName);
            cmd.Parameters.AddWithValue("@FileExtension", fileExtension);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return "File stored Successfully!!!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
                conn.Dispose();
            }
        }
    }

    private static DataTable ProcessCSV(string fileName)
    {
        //Set up our variables
        string Feedback = string.Empty;
        string line = string.Empty;
        string[] strArray;
        DataTable dt = new DataTable();
        DataRow row;
        // work out where we should split on comma, but not in a sentence
        Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        //Set the filename in to our stream
        StreamReader sr = new StreamReader(fileName);

        //Read the first line and split the string at , with our regular expression in to an array
        line = sr.ReadLine();
        strArray = r.Split(line);

        //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
        Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));

        //Read each line in the CVS file until it’s empty
        while ((line = sr.ReadLine()) != null)
        {
            row = dt.NewRow();

            //add our current value to our data row
            row.ItemArray = r.Split(line);
            dt.Rows.Add(row);
        }

        //Tidy Streameader up
        sr.Dispose();

        //return a the new DataTable
        return dt;

    }
}