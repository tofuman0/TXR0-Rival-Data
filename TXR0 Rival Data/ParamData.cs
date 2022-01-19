using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TXR0_Rival_Data
{
    class ParamData
    {
        public DataSet dsDataStructure;
        public ParamData()
        {
            dsDataStructure = new DataSet("Rival Data");
        }
        public ParamData(String name)
        {
            dsDataStructure = new DataSet(name);
        }
        public DataSet CreateDataStructure(String name, List<ParamDataManager.FileStructure> structures)
        {
            DataTable dt = new DataTable(name);
            if(dsDataStructure.Tables.Contains(name)) // Check if table already exists in tables and if so remove it to be replaced
                dsDataStructure.Tables.Remove(name);
            foreach (var structure in structures)
            {
                if (structure.varType != ParamDataManager.VarType.DataLength && dt.Columns.Contains(structure.name) == false)
                {
                    if(structure.dataType == typeof(Int32[]))
                        dt.Columns.Add(structure.name, typeof(String));
                    else
                        dt.Columns.Add(structure.name, structure.dataType);
                }
            }
            dsDataStructure.Tables.Add(dt);
            return dsDataStructure;
        }
    }
}
