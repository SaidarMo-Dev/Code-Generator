using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator_BusinessLayer
{
    public class DataTypesMapping
    {

        public static string GetCSharpDataType(string sqlDataType)
        {
            switch (sqlDataType)
            {
                case "int":
                    return "int";

                case "tinyint":
                    return "byte";

                case "smallint":
                    return "int";

                case "Decimal":
                    return "double";

                case "smallmoney":
                    return "double";

                case "money":
                    return "double";

                case "varchar":
                    return "string";

                case "nvarchar":
                    return "string";

                case "datetime":
                    return "DateTime";

                case "smallDateTime":
                    return "DateTime";

                case "date":
                    return "DateTime";

                case "bit":
                    return "bool";

                default:
                    return "int";

            }
        }
     

    }
}
