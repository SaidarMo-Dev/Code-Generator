using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator_DataAcessLayer
{
    public class clsDataAccessSettings
    {


        public static string DataBase = "dvld" , UserID = "" , Password = "";


        public static string connectionString;

        public static void SetCredetials(string dataBase, string userID, string password)
        {
            DataBase = dataBase ;
            UserID = userID;
            Password = password;

            connectionString = @"Server =.;DataBase =" + DataBase + ";User ID =" + UserID + ";Password =" + Password;


        }


    }
}
