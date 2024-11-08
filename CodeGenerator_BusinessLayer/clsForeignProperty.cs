using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator_BusinessLayer
{
    public class clsForeignProperty : clsProperty 
    {
        public string ParentTable { set; get; }

        public void FillFogienProperty(string name,   string sqlDataType , bool IsNullabel, string ParentTable)
        {
            base.FillProperty(name, sqlDataType, IsNullabel);

            this.ParentTable = ParentTable;
        }

    }



}
