
// Developed BY 
// Mohammed Saidar


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using CodeGenerator_DataAcessLayer;

namespace CodeGenerator_BusinessLayer
{
    public class clsGenerator
    {


        public string DataAccessProjectPath { set; get; }
        public string BusinessProjectPath { set; get; }


        string DataAccessFileName;
        string BusinessFileName;
        

        private clsTable _table;
        public  clsTable  table
        {
            get { return _table; }

            set
            {
                _table = value;

                DataAccessFileName = "cls" + _table.TableName + "DataAccess.cs";
                BusinessFileName = "cls" + _table.TableName + ".cs";

            }

        }

        public clsGenerator ()
        {
            this.table = new clsTable();
             
        }

        public static void SetCredentials(string DataBase, string UserID , string Password )
        {
            clsDataAccessSettings.SetCredetials(DataBase, UserID, Password);

        }


        public static bool IsCredetialsCorrect()
        {
            return clsData.IsCredentialsCorrect();

        }

        public void Start()
        {
            _GenerateSettingsClass();

        }

        public void Generate()
        {
            _GenerateDataAccessLayerFile();

            _GenerateBusinessLayerFile();
           
        }


        // --------------------------- Business layer -----------------------------

        private void _GenerateBusinessLayerFile()
        {

            if (string.IsNullOrEmpty(BusinessProjectPath))
                return;


            string content = _GenerateBusinessHeader();

            content += _GenerateFisrtConstractor();
            content += _GenerateSecondConstractor();
            content += _GenerateReadByID();
            content += _GenerateAddNew();
            content += _GenerateUpdate();
            content += _GenerateSaveMethod();
            content += _GenerateDelete();
            content += _GenerateGetAll();
            content += _GenerateIsExiste();


            content += "\n   }\n}";

           
            string path = BusinessProjectPath+ "\\" + BusinessFileName;


         


            File.WriteAllText(path, content);



        }

        public string _GenerateBusinessHeader()
        {
            string content = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using Project_DataAcessLayer;


namespace Project_BusinessLayer
{

    public class cls"+ _table .TableName + @"
    {

        enum enMode { AddNew, Update};

        enMode _Mode;

        " + _GenerateClassProperties() + @"

        "+ _GenerateClassForeignProperties() + @"
 

";

            return content;
        }


        // first constractor 

        private string _GenerateFisrtConstractor()
        {
            string signature  = "public cls" + _table.TableName + "()";

            string content = signature + @"
{
    this._Mode = enMode.AddNew;

 " + _InitializeProperties() + @" 

 " + _InitializeForeign() +  @"
}

";
            return content;

 

        }
        
        // Second constractor 
        private string _GenerateSecondConstractor()
        {
            string signature = "private cls" + _table.TableName + "( " + _FillConstractorParameters() + ")";

            string content = signature + @"
{
    this._Mode = enMode.Update;

 " + _InitializeProperties(true) + @" 
 
 " + _InitializeForeign (true)  + @"


}

";
            return content;



        }

        // generate get info method or read method ;

        public string _GenerateReadByID()
        {
            string methodSignature = "public static cls" + _table.TableName + " Get" + _table.TableName + "InfoByID(" + _PrimaryKeyMethodsPrameter() + ")";

           
      


            string methodCode = methodSignature + @"
{
    " + _DeclareVariablesForReadMethod() + @" 
    
    if (cls" + _table .TableName + "DataAccess.Get" + _table .TableName +
             "InfoByID("+ _table .PrimaryProperty .name + "," + __FillReadMethodParameters () + @"))
    {
        return new cls" +_table .TableName + "(" + _GetParametrs () + @");

    }

    return null;

}

    ";

            return methodCode;


        }

        // generate addNew
        private string _GenerateAddNew()
        {
            string MethodSignature = @"private bool _AddNew" + _table.TableName + "()";


            string methodCode = MethodSignature + @"
{ 
    this." + _table.PrimaryProperty.name + @" = cls" + _table.TableName
                + "DataAccess.AddNew" + _table.TableName + "(" + _FillMethodParameters() + @");

    return (this." + _table.PrimaryProperty.name + @" != -1); 
            
}";


            return methodCode; 



        }
        
        // generate Update
        private string _GenerateUpdate()
        {
            string MethodSignature = @"private bool _Update" + _table.TableName + "()";

            string methodCode = MethodSignature + @"
{
    return cls" + _table.TableName + "DataAccess.AddNew" +
                _table.TableName + "(" + _FillMethodParameters(true) + @");
}

";

            return methodCode;

        }
        
        // save 
        private string _GenerateSaveMethod()
        {
             string methodCode = @" public bool Save()
{
    switch (_Mode)
    {
        case enMode.AddNew :
            
            if ( _AddNew"+ _table .TableName + @"())
            {

                _Mode = enMode.Update;
                return true;
            }
            else 
                return false;

        case enMode.Update :
            
            return _Update" + _table .TableName + @"(); 
            
    }

} ";

             return methodCode;


    

        }
        
        // is existe By Primary key
        private string _GenerateIsExiste()
        {
            string MethodSignature = @"public static bool Is" + _table.TableName + "ExisteByID" + "( " + _PrimaryKeyMethodsPrameter() + ")";

            string methodCode = MethodSignature + @"
{
    return cls" + _table.TableName + "DataAccess.Is" +
                _table.TableName + "ExisteByID(" + _table .PrimaryProperty .name + @");

}
";

            return methodCode;
        }
       
        // GetAll Records method 
        private string _GenerateGetAll()
        {
            string methodCode = @"public static bool GetAll" + _table.TableName + @"()
{

    return cls" + _table.TableName + "DataAccess.GetAll" + _table.TableName + @"();
}

";

            return methodCode;

        }



        // generate Delete
        private string _GenerateDelete()
        {
            string MethodSignature = @"public static bool Delete" + _table.TableName + "( " + _PrimaryKeyMethodsPrameter() + ")";

            string methodCode = MethodSignature + @"
{
    return cls" + _table.TableName + "DataAccess.Delete" +
                _table.TableName + "(" + _table.PrimaryProperty.name + @");

}
";

            return methodCode;
        }
       
        

        private string _InitializeProperties(bool value = false)
        {
            StringBuilder sbContent = new StringBuilder();


            // if true we prepare for Second constractor 

            if (value)
            {
                // Primary key Property

                sbContent.Append("this.");
                sbContent.Append(_table.PrimaryProperty.name);
                sbContent.Append(" = ");
                sbContent.Append(_table.PrimaryProperty.name + ";\n");


                // Normal properties

                foreach (clsProperty prp in _table.NormalProperties)
                {
                    sbContent.Append("this.");
                    sbContent.Append(prp.name);
                    sbContent.Append(" = ");
                    sbContent.Append(prp.name + ";\n");

                }


            
            }

            //  else we prepare for First constractor 

            else
            {
                    // Primary propety 

                sbContent.Append("this.");
                sbContent.Append(_table.PrimaryProperty.name);
                sbContent.Append(" = ");
                sbContent.Append(_table.PrimaryProperty.DefaultValue + ";\n");


                // Normal properties
 
                foreach (clsProperty prp in _table.NormalProperties)
                {
                    sbContent.Append("this.");
                    sbContent.Append(prp.name);
                    sbContent.Append(" = ");
            
                    if (prp.CSharpDataType == "string")
                    {
                        sbContent.Append(@"""" + prp.DefaultValue + @"""");

                    }
                    else
                        sbContent.Append(prp.DefaultValue);

                    sbContent.Append(";\n");
                }
              
            }

            return sbContent.ToString();


        }
      
        private string _InitializeForeign(bool value = false )
        {
            StringBuilder sbContent = new StringBuilder();

            if (value)
            {
                foreach (clsForeignProperty Fprp in _table.ForeignProperties)
                {
                    sbContent.Append("this.");
                    sbContent.Append(Fprp .ParentTable + "Info");
                    sbContent.Append(" = ");
                    sbContent.Append("cls" + Fprp.ParentTable);
                    sbContent.Append("Get" + Fprp.ParentTable + "InfoByID(");
                    sbContent.Append(Fprp.name + ");\n");

                }
            }
            else
            {
                foreach (clsForeignProperty Fprp in _table.ForeignProperties)
                {
                    sbContent.Append("this.");
                    sbContent.Append(Fprp.ParentTable + "Info");
                    sbContent.Append(" = ");
                    sbContent.Append("null ;\n");
                }
            }
          

            return sbContent.ToString();

        }
        

        private string _GetParametrs ()
        {

            string Parameters = _table .PrimaryProperty .name + ",";
            
            foreach (var prp in _table .NormalProperties)
            {
                Parameters += prp.name + ",";

            }

            return (Parameters.TrimEnd (','));



        }

        private string _FillMethodParameters(bool value = false)
        {
            StringBuilder sbContent = new StringBuilder();
            
            // if true this means that i want Update Method Parameters  we'll include Primmary key

            if (value)
            {
                sbContent.Append("this.");
                sbContent.Append(_table.PrimaryProperty.name + ",");

            }
            
            foreach (var prp in _table .NormalProperties)
            {
                sbContent.Append("this.");
                sbContent.Append(prp.name);
                sbContent.Append(",");


            }


            return sbContent.ToString().TrimEnd(',', ' ');


            

        }
        private string _PrimaryKeyMethodsPrameter()
        {
            return  ( _table.PrimaryProperty.CSharpDataType + " " + _table.PrimaryProperty.name) + " " ;


        }
        
        private string _DeclareVariablesForReadMethod()
        {
            StringBuilder sbContent = new StringBuilder();


            foreach (clsProperty prp in _table .NormalProperties)
            {
                sbContent.Append(prp.CSharpDataType);
                sbContent.Append(" ");
                sbContent.Append(prp.name);
                sbContent.Append(" = ");
                if (prp.CSharpDataType == "string")
                {
                    sbContent.Append(@"""" + prp.DefaultValue + @"""");

                }
                else
                  sbContent.Append(prp.DefaultValue);
               
                sbContent.Append(";\n");

            }


            return sbContent.ToString();
        }

        private string __FillReadMethodParameters()
        {
            StringBuilder sbContent = new StringBuilder();

            foreach (var prp in _table.NormalProperties)
            {
                sbContent.Append("ref ");
                sbContent.Append(prp.name);
                sbContent.Append(",");


            }

            return sbContent .ToString ().TrimEnd (',');

        }

        private string _FillConstractorParameters()
        {
            // fill primary key to paramerters 

            string content = _table.PrimaryProperty.CSharpDataType + " "+ _table.PrimaryProperty.name + "," ;
  
            // normal properies 

            foreach (var prp in _table .NormalProperties)
            {

                content += prp.CSharpDataType + " " + prp.name + ",";


            }

            return content.TrimEnd(',');

        
        }

        private string _GenerateClassProperties ()
        {
            StringBuilder sbContent = new StringBuilder();


            // Primary key Property

            sbContent.Append("public ");
            sbContent.Append(_table.PrimaryProperty.CSharpDataType + " ");
            sbContent.Append(_table.PrimaryProperty.name);
            sbContent.Append(" { set; get; }\n");


            // Normal properties

            foreach (clsProperty prp in _table.NormalProperties)
            {
                sbContent.Append("public ");
                sbContent.Append(prp.CSharpDataType + " ");
                sbContent.Append(prp.name);
                sbContent.Append(" { set; get; }\n");


            }



            return sbContent.ToString();

        }

        private string _GenerateClassForeignProperties()
        {
            StringBuilder sbContent = new StringBuilder();


           
            foreach (clsForeignProperty Fprp in _table.ForeignProperties)
            {
                sbContent.Append("public ");
                sbContent.Append("cls" + Fprp .ParentTable + " ");
                sbContent.Append(Fprp.ParentTable + "Info");
                sbContent.Append(" { set; get; }\n");


            }



            return sbContent.ToString();

        }


        // ---------------------------Data Access layer -----------------------------


        private void _GenerateDataAccessLayerFile()
        {

            if (string.IsNullOrEmpty(DataAccessProjectPath))
                return;


            string content = GenerateDataAccesssHeader();

            content += GenerateDataAccessReadByID();
            content += GenerateDataAccessAddNew();
            content += GenerateDataAccessUpdate();
            content += GenerateDataAccessGetAll();
            content += GenerateDataAccessDelete();
            content += GenerateDataAccessIsExiste();

            content += "\n   }\n}";


            string path = DataAccessProjectPath + "\\" + DataAccessFileName;



        

            File.WriteAllText(path, content);
            
             
         
            
        }
   
    
        public string GenerateDataAccesssHeader()
        {
            string content = @" 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Project_DataAccessLayer
{

    public class cls" + _table.TableName + @"DataAccess
    {

";

            return content;

        }


        // create clsDataAccessSettings class 

        private void _GenerateSettingsClass()
        {
            string content = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project_DataAccessLayer
{


    public class clsDataAccessSettings
    {

        public static string connectionString = """ + clsDataAccessSettings.connectionString + @""";



    }   

}";



            if (string.IsNullOrEmpty(DataAccessProjectPath))
                return;

            string path = Path.Combine(DataAccessProjectPath, "clsDataAccessSettings.cs");


            File.WriteAllText(path, content);

            


        }



        // generate Data Access Read Method 
 
        public string GenerateDataAccessReadByID()
        {
           

           // string TableSignature = tableName.Remove(tableName.Length - 1);

            

            string MethodSignature = "public static bool Get" + _table .TableName + "InfoByID(" + _table.PrimaryProperty.CSharpDataType + " " + _table .PrimaryProperty .name + ",";

            string whereClause = _table.PrimaryProperty.name + " = " + "@" + _table.PrimaryProperty.name;

           

            // dict to store property and CSharp data type 
            Dictionary<string, string> ColumnsInfo = new Dictionary<string, string>();


            foreach (clsProperty property in _table.NormalProperties)
            {
                
                MethodSignature += "ref " + property.CSharpDataType + " " + property.name+ ",";

                ColumnsInfo[property.name] = property .CSharpDataType;

                   

            }



            MethodSignature = MethodSignature.TrimEnd(',', ' ') + ")";


            StringBuilder sbPerformeFillInfo = new StringBuilder();


            // lines That we will use them to take data from reader 

            foreach (var column in ColumnsInfo)
            {
                sbPerformeFillInfo.Append(column.Key);
                sbPerformeFillInfo.Append(" = (");
                sbPerformeFillInfo.Append( column.Value + ")");
                sbPerformeFillInfo.Append("reader[" + @"""");
                sbPerformeFillInfo.Append(column.Key);
                sbPerformeFillInfo.Append(@""""+ "];\n");


            }
         


            string MethodCode =
MethodSignature + @"
    {
        bool IsFound  = false;

        try 
        {

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            { 
                connection.Open();

                string query = @"" Select * From " + _table .TableName + " Where " + whereClause + @""";
        

                using (SqlCommand command  = new SqlCommand(query, connection))
                {
                    " + GenerateParametersAssignementsForPrimaryKey(_table .PrimaryProperty .name) + @"

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                       
                          if (reader.Read())
                          {
                              IsFound = true;

                              " + sbPerformeFillInfo .ToString() + @"
                          }
                    }
                
                    
            
                }
            }
        }
        catch
        {
            IsFound = false;
        
        }

        return IsFound ;
          
    }";


            return MethodCode;


        }

        // generate data access Add New Method 
        public string GenerateDataAccessAddNew()
        {

            string MethodSignature = "public static int AddNew" + _table.TableName + "(";

            string Parameters = "";
            string Values = "";



            foreach (clsProperty property in _table.NormalProperties)
            {

                MethodSignature += property.CSharpDataType + " " + property.name + ",";

              //  ColumnsInfo[property.name] = property.CSharpDataType;
                Parameters += "@" + property .name + ", ";
                Values += property .name + ", ";




            }


          
           


            MethodSignature = MethodSignature.TrimEnd(',', ' ') + ")";

            Parameters = Parameters.TrimEnd(',', ' ');

            Values = Values.TrimEnd(',', ' ');



            string MethodCode =
    MethodSignature + @"
    {
        int InsertedID  = -1;

        try 
        {

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            { 
                connection.Open();

                string query = @""INSERT INTO " + _table.TableName + "(" + Values + ")" + @"
                                   
                                       VALUES (" + Parameters + @") 
                
                                SELECT SCOPE_IDENTITY();" + @"""; 
        

                using (SqlCommand command  = new SqlCommand(query, connection))
                {
                    " + GenerateParametersAssignements(_table .NormalProperties) + @"

                    object Result = command.ExecuteScalar();
                
                    int ID = 0;

                    if (Result != null && int.TryParse(Result.ToString(), out ID))
                    {
                        InsertedID = ID;

                    }
            
                }
            }
        }
        catch
        {
    
        
        }

        return InsertedID ;
          
    }";


            return MethodCode;


        }

        // generate Data Acceess Update Method 
        public string GenerateDataAccessUpdate()
        {


            string MethodSignature = "public static bool Update" + _table .TableName  + "(" +  _table .PrimaryProperty.CSharpDataType + " " + _table.PrimaryProperty .name + ",";


            List<string> SetClause = new List<string>();



            foreach (clsProperty prp in _table.NormalProperties)
            {
              
                MethodSignature += prp.CSharpDataType + " " +  prp.name+ ", ";


                SetClause.Add(prp.name + " = @" + prp.name);

              
            }



            MethodSignature = MethodSignature.TrimEnd(',', ' ') + ")";




            string MethodCode =
MethodSignature + @"
    {
        int RowsAffected  = -1;

        try 
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            { 
                connection.Open();

                string query = @""Update " + _table.TableName + " SET " + string.Join(",", SetClause) + @"

                                    WHERE " + _table.PrimaryProperty.name + "= @" + _table.PrimaryProperty.name  + @""";
        

                using (SqlCommand command  = new SqlCommand(query, connection))
                {

                    " + GenerateParametersAssignementsForPrimaryKey(_table .PrimaryProperty .name) 
                      + GenerateParametersAssignements(_table .NormalProperties) + @"

                     RowsAffected = command.ExecuteNonQuery();
                
                    
            
                }
            }
        }
        catch
        {
    
        
        }

        return (RowsAffected != -1 ) ;
          
    }";


            return MethodCode;


        }

        // generate Data Acceess Delete Method
        public  string GenerateDataAccessDelete()
        {

            
            string MethodSignature = "public static bool Delete" + _table.TableName + "(" + _table .PrimaryProperty.CSharpDataType + " " + _table .PrimaryProperty .name + ")";

            string whereClause = _table.PrimaryProperty.name + " = " + "@" + _table.PrimaryProperty.name;

            string MethodCode =
MethodSignature + @"
    {
        int RowsAffected  = -1;

        try 
        {

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            { 
                connection.Open();

                string query = @"" Delete From " + _table.TableName + " Where " + whereClause + @""";
        

                using (SqlCommand command  = new SqlCommand(query, connection))
                {
                    " + GenerateParametersAssignementsForPrimaryKey(_table.PrimaryProperty.name) +@"

                     RowsAffected = command.ExecuteNonQuery();
                
                    
            
                }
            }
        }
        catch
        {
    
        
        }

        return (RowsAffected != -1 ) ;
          
    }";


            return MethodCode;


        }

        // generate Data Acceess GetAll  Method
        public  string GenerateDataAccessGetAll()
        {

            string MethodSignature = "public static DataTable GetList" + _table.TableName + "()";




            string MethodCode =
MethodSignature + @"
    {
        DataTable dtList = new DataTable();

        try 
        {

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            { 
                connection.Open();

                string query = @"" Select * From " + _table.TableName + @""";
        

                using (SqlCommand command  = new SqlCommand(query, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                       
                          if (reader.HasRows)
                          {
                              
                                dtList.Load(reader);
                              
                          }
                    }
                
                    
            
                }
            }
        }
        catch
        {
            
        
        }

        return dtList ;
          
    }";


            return MethodCode;


        }

        // generate Data Acceess IsExiste  Method
        public string GenerateDataAccessIsExiste()
        {


            string MethodSignature = "public static bool Is" + _table.TableName + "ExisteByID(" + _table.PrimaryProperty.CSharpDataType + " " + _table.PrimaryProperty.name + ")";

            string whereClause = _table.PrimaryProperty.name + " = " + "@" + _table.PrimaryProperty.name;






            string MethodCode =
MethodSignature + @"
    {
        bool IsFound  = false;

        try 
        {

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            { 
                connection.Open();

                string query = @"" Select Found = 1 From " + _table.TableName + " Where " + whereClause + @""";
        

                using (SqlCommand command  = new SqlCommand(query, connection))
                {
                    " + GenerateParametersAssignementsForPrimaryKey(_table.PrimaryProperty.name) + @"

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                       
                          if (reader.Read())
                          {
                              IsFound = true;

                              
                          }
                    }
                
                    
            
                }
            }
        }
        catch
        {
            IsFound = false;
        
        }

        return IsFound ;
          
    }";


            return MethodCode;


        }

        public static string GenerateParametersAssignementsForPrimaryKey(string ColumnName)
        {


            return "command.Parameters.AddWithValue(" + @"""@" + ColumnName + @""", " + ColumnName + ");\n\t\t\t";




        }
        public static string GenerateParametersAssignements( List <clsProperty> Properties)
        {
            string Assignements = "";

            foreach (clsProperty prp in Properties)
            {
                if (prp.IsNullabel)
                {

                    Assignements += "\nif ( string.IsNullOrEmpty(" + prp.name + @"))
                                     { 
                                        command.Parameters.AddWithValue(" + @"""@" + prp.name + @""", System.DBNull.Value );
                                     }
                                     else 
                                     { 
                                        command.Parameters.AddWithValue(" + @"""@" + prp.name + @""", " + prp.name + @");
                                      
                                     }";
                    
                }
                else
                {
                    Assignements += "command.Parameters.AddWithValue(" + @"""@" + prp.name + @""", " + prp.name + ");\n";

                }

         

            }

            return Assignements;

        }





    }




}
