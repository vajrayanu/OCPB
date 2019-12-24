using OCPB.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace OCPB.Controllers
{
    public class IntegrationController : ApiController
    {
        // GET api/integration
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/integration/5
        public string Get(int id)
        {
            SqlParameter[] param = new SqlParameter[0];
            string Stored = null;
            switch(id)
            {
                case 2: Stored = "Bi_rpt_Complains_By_Area"; break;
                case 3: Stored = "Bi_rpt_Complain_Dept"; break;
                case 4: Stored = "Bi_rpt_Complains_By_Call_Center"; break;
                case 5: Stored = "Bi_rpt_Complains_By_Division"; break;
                case 7: Stored = "Bi_rpt_in_process"; break;
                case 8: Stored = "Bi_Rpt_Complains_by_CompanyName"; break; 
                    
            }

            return DataTableToJsonObj(DirectSqlDao.GetAllStored_Datatable(Stored, param));
        }

        // POST api/integration
        public string Post([FromBody]string value)
        {

            SqlParameter[] param = new SqlParameter[0];

            return DataTableToJsonObj( DirectSqlDao.GetAllStored_Datatable("Bi_rpt_Complain_Dept", param));

        }

        // PUT api/integration/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/integration/5
        public void Delete(int id)
        {
        }



        public static string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder jsonString = new StringBuilder();

            if (ds.Tables[0].Rows.Count > 0)
            {
                jsonString.Append("[");
                for (int rows = 0; rows < ds.Tables[0].Rows.Count; rows++)
                {
                    jsonString.Append("{");
                    for (int cols = 0; cols < ds.Tables[0].Columns.Count; cols++)
                    {
                        jsonString.Append(@"""" + ds.Tables[0].Columns[cols].ColumnName + @""":");

                        /* 
                        //IF NOT LAST PROPERTY

                        if (cols < ds.Tables[0].Columns.Count - 1)
                        {
                            GenerateJsonProperty(ds, rows, cols, jsonString);
                        }

                        //IF LAST PROPERTY

                        else if (cols == ds.Tables[0].Columns.Count - 1)
                        {
                            GenerateJsonProperty(ds, rows, cols, jsonString, true);
                        }
                        */

                        var b = (cols < ds.Tables[0].Columns.Count - 1)
                            ? GenerateJsonProperty(ds, rows, cols, jsonString)
                            : (cols != ds.Tables[0].Columns.Count - 1)
                              || GenerateJsonProperty(ds, rows, cols, jsonString, true);
                    }
                    jsonString.Append(rows == ds.Tables[0].Rows.Count - 1 ? "}" : "},");
                }
                jsonString.Append("]");
                return jsonString.ToString();
            }
            return null;
        }

        private static bool GenerateJsonProperty(DataSet ds, int rows, int cols, StringBuilder jsonString, bool isLast = false)
        {

            // IF LAST PROPERTY THEN REMOVE 'COMMA'  IF NOT LAST PROPERTY THEN ADD 'COMMA'
            string addComma = isLast ? "" : ",";

            if (ds.Tables[0].Rows[rows][cols] == DBNull.Value)
            {
                jsonString.Append(" null " + addComma);
            }
            else if (ds.Tables[0].Columns[cols].DataType == typeof(DateTime))
            {
                jsonString.Append(@"""" + (((DateTime)ds.Tables[0].Rows[rows][cols]).ToString("yyyy-MM-dd HH':'mm':'ss")) + @"""" + addComma);
            }
            else if (ds.Tables[0].Columns[cols].DataType == typeof(string))
            {
                jsonString.Append(@"""" + (ds.Tables[0].Rows[rows][cols]) + @"""" + addComma);
            }
            else if (ds.Tables[0].Columns[cols].DataType == typeof(bool))
            {
                jsonString.Append(Convert.ToBoolean(ds.Tables[0].Rows[rows][cols]) ? "true" : "fasle");
            }
            else
            {
                jsonString.Append(ds.Tables[0].Rows[rows][cols] + addComma);
            }

            return true;
        }
    }
}
