using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using crud_demo1.Models;
using System.Web.Http.Cors;


namespace crud_demo1.Controllers
{
    [EnableCorsAttribute("*", "*", "*")]
    public class EmployeeController : ApiController
    {
        private SqlConnection con;
        private ADO_Configrations ado_configrations = null;
        public EmployeeController()
        {
            ado_configrations = new ADO_Configrations();
        }
        public class ADO_Configrations
        {
            private SqlConnection conn;
            private FindUserId findUserId = new FindUserId();
            public SqlConnection connection()
            {
                string constr = ConfigurationManager.ConnectionStrings["DBSC"].ToString();
                conn = new SqlConnection(constr);
                return conn;
            }

            public SqlConnection connection(string connectionType)
            {
                if (connectionType == "DBSC")
                {
                    string constr = ConfigurationManager.ConnectionStrings["DBSC"].ToString();
                    conn = new SqlConnection(constr);
                }
                return conn;
            }
        }
        public async Task<List<emp>> Get()
         {
            try
            {
                con = ado_configrations.connection();
                List<emp> EmpList = new List<emp>();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Connection = con;
                cmd1.CommandText = "spAddEmployee";
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataTable dt1 = new DataTable();
                da1.SelectCommand = cmd1;
                await con.OpenAsync();
                da1.Fill(dt1);
                con.Close();

                //Bind EmpModel generic list using LINQ 
                EmpList = (from DataRow drr in dt1.Rows
                           select new emp()
                           {
                               id = (int)drr["id"],
                               firstname = (string)drr["firstname"],
                               lastname = (string)drr["lastname"],
                               emailid = (string)drr["emailid"],
                               salary = (long)drr["salary"],
                               mobilenumber = (long)drr["mobilenumber"]

                           }).ToList();

                return EmpList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public async Task<List<emp>> Get(int id)
        {
            try
            {
                con = ado_configrations.connection();
                List<emp> EmpList = new List<emp>();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Connection = con;
                cmd1.CommandText = "spgetemployee";
                cmd1.Parameters.AddWithValue("@empid", id);
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataTable dt1 = new DataTable();
                da1.SelectCommand = cmd1;
                await con.OpenAsync();
                da1.Fill(dt1);
                con.Close();

                //Bind EmpModel generic list using LINQ 
                EmpList = (from DataRow drr in dt1.Rows
                           select new emp()
                           {
                               firstname = (string)drr["firstname"],
                               lastname = (string)drr["lastname"],
                               emailid = (string)drr["emailid"],
                               salary = (long)drr["salary"],
                               mobilenumber = (long)drr["mobilenumber"]

                           }).ToList();

                return EmpList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
       
        [HttpPut]
        //public bool update([FromBody] int id, string name , string lastname , string emailid , long salary , long mobilenumber)
        public bool update(emp objEmp)
        {

            
            try
            {
                con = ado_configrations.connection();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "spgeupdateemployees";
                //cmd.Parameters.AddWithValue("@empid", objEmp.id);
                cmd.Parameters.AddWithValue("@empname", objEmp.firstname);
                cmd.Parameters.AddWithValue("@emplastname", objEmp.lastname);
                cmd.Parameters.AddWithValue("@emailid", objEmp.emailid);
                cmd.Parameters.AddWithValue("@salary", objEmp.salary);
                cmd.Parameters.AddWithValue("@mobilenumber", objEmp.mobilenumber);//@mobilenumber
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                    //return Request.CreateResponse(true);
                    return true;
                else
                    //return Request.CreateResponse(false);
                    return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            //return true;

        }
       

        public bool add(emp2 objEmp)
        {


            try
            {
                con = ado_configrations.connection();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "spaddemployees";
                //cmd.Parameters.AddWithValue("@empid", objEmp.id);
                cmd.Parameters.AddWithValue("@empname", objEmp.firstname);
                cmd.Parameters.AddWithValue("@emplastname", objEmp.lastname);
                cmd.Parameters.AddWithValue("@emailid", objEmp.emailid);
                cmd.Parameters.AddWithValue("@mobilenumber", objEmp.mobilenumber);
                cmd.Parameters.AddWithValue("@salary", objEmp.salary);
                //cmd.Parameters.AddWithValue("@updated_by", 1);
                //cmd.Parameters.AddWithValue("@updated", System.DateTime.Now);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                    //return Request.CreateResponse(true);
                    return true;
                else
                    //return Request.CreateResponse(false);
                    return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //return true;

        }
     
        public bool Delete(int id)
        {
              con = ado_configrations.connection();
              SqlCommand cmd = new SqlCommand();
              cmd.CommandType = CommandType.StoredProcedure;
              cmd.Connection = con;
              cmd.CommandText = "spdeleteemployees";
              cmd.Parameters.AddWithValue("@empid", id);
              con.Open();
              int i = cmd.ExecuteNonQuery();
              con.Close();

              if (i >= 1)
                  return true;
              else
                  return false;
 
        }
    }

    /* public void Get()
     {
         string ConnectionString = ConfigurationManager.ConnectionStrings["DBSC"].ConnectionString;
         using (SqlConnection con = new SqlConnection(ConnectionString))
         {
             //Create the SqlCommand object
             SqlCommand cmd = new SqlCommand("spAddEmployee", con);
             cmd.CommandType = System.Data.CommandType.StoredProcedure;
             con.Open();
             cmd.ExecuteReader();
         }
     }*/
}

