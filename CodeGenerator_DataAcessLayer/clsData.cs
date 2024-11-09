using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data ;






// Developed BY 
// Mohammed Saidar


namespace CodeGenerator_DataAcessLayer
{
    public class clsData
    {



        public static List<string> GetTables()
        {
            List<string> TableNames = new List<string>();

            try
            {
               

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    
                    string query = @"


                        SELECT DISTINCT    

	                        c.TABLE_NAME 

                        FROM  
	                        INFORMATION_SCHEMA.COLUMNS  c
	                        join

	                        INFORMATION_SCHEMA.TABLES  t

	                        ON c.TABLE_NAME = t.TABLE_NAME 

	                        WHERE t.TABLE_TYPE = 'BASE TABLE' --include only basetable 


	                        AND t.TABLE_CATALOG  = @Database -- filtering by databse name 
	                        AND t.TABLE_NAME NOT IN ('sysdiagrams') -- and exclude sysdiagrams tables ";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        connection.Open();

                        command.Parameters.AddWithValue("@Database", clsDataAccessSettings.DataBase);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                TableNames.Add((string)reader["TABLE_NAME"]);

                            }


                        }
                    }

                }
            }
            catch
            {
             
            }


            return TableNames;

        }

        public static string GetPrimaryKey( string TableName)
        {

            string PrimaryKey = "";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                string query = @"

                       
                                
				   
				   
			        SELECT c.COLUMN_NAME

                            FROM 

	                            INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
				                            join
	                            INFORMATION_SCHEMA.COLUMNS c

				                            ON
					                            kcu .TABLE_NAME = c .TABLE_NAME 

				                            AND 

					                            kcu.COLUMN_NAME = c.COLUMN_NAME 

			                            WHERE kcu .TABLE_NAME = @TableName and kcu.CONSTRAINT_NAME like 'PK%' and kcu.TABLE_CATALOG = @DataBase
		
                    ;";


                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    connection.Open();


                    command.Parameters.AddWithValue("@TableName", TableName);
                    command.Parameters.AddWithValue("@DataBase", clsDataAccessSettings .DataBase);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PrimaryKey = (string)reader["COLUMN_NAME"];


                        }
                    }

                 }

            }

            return PrimaryKey;
        }

        public static DataTable GetTableInfo(string tableName)
        {
            DataTable dtTableInfo = new DataTable();

            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    string query = @"




                        SELECT DISTINCT    

	                        c.table_name,
	                        c.COLUMN_NAME ,
	                        c.DATA_TYPE,
	                        c.IS_NULLABLE 
                        FROM  
	                        INFORMATION_SCHEMA.COLUMNS  c
	                                join

	                        INFORMATION_SCHEMA.TABLES  t

                                     ON
	                         c.TABLE_NAME = t.TABLE_NAME 


                            WHERE
	                             t.TABLE_CATALOG  = @Database -- filtering by databse name 
	                            AND t.TABLE_NAME = @TableName ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        connection.Open();


                        command.Parameters.AddWithValue("@Database",clsDataAccessSettings.DataBase);
                        command.Parameters.AddWithValue("@TableName", tableName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            dtTableInfo.Load(reader);

                        }
                    }

                }
            }
            catch
            {

            }


            return dtTableInfo;

        

        }

        public static DataTable GetForeigKeys(string TableName)
        {
            DataTable dtForeigns = new DataTable();


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    string query = @"




                        select 

	
	                        tr.name as PARENT_TABLE_NAME,
	                        c.name FOREIGN_NAME,
	                        case c.is_nullable when 0 then 'NO'ELse 'YES' end as  Is_Nullabel,
	                        ty.name as DATA_TYPE

                        from 

	                        sys.foreign_keys fk

	                        join sys.tables t on fk.parent_object_id = t.object_id


	                        join sys.tables tr on fk.referenced_object_id = tr.object_id

	
	                        inner join sys.foreign_key_columns fkc ON 
	                        fk.object_id = fkc.constraint_object_id 
	
	                        inner join sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id 

	                        inner join sys.types ty on c.user_type_id  = ty.user_type_id

	                        where t.name = @TableName


 ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        connection.Open();

                        command.Parameters.AddWithValue("@TableName", TableName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            dtForeigns.Load(reader);

                        }
                    }

                }
            }
            catch
            {

            }


            return dtForeigns;

        }

        public static bool IsCredentialsCorrect()
        {
            bool Correct = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.connectionString))
                {
                    conn.Open();

                    conn.Close();

                    Correct = true;

                }

            }
            catch
            {
                Correct = false;

            }

            return Correct;

        }

    }



}
