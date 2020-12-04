using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient; //connect to microsoft SQL server (provides acess to connection object, command object and reader)
using System.Data.SqlTypes; //in case we need data strings, but we most likely won't in a web app
using System.Configuration; //gives access to web config files (pull out connection string)
using HealthcareCompanion.DataAccessLayer;
using HealthcareCompanion.Models;
using System.Web.Mvc;

namespace HealthcareCompanion.DataAccessLayer
{
    public class AdminTier : BaseTier
    {
        public AdminTier() : base()
        {

        }
        public List<Doctor> getAllAdmins()
        {
            List<Doctor> doctorList = null;
            Doctor       doctor     = null;

            query = "SELECT * FROM Admins;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            doctorList = new List<Doctor>();
                            while (reader.Read())
                            {
                                doctor = new Doctor();
                                doctor.DoctorID  = (int)reader["DoctorID"];
                                doctor.FirstName = (string)reader["FirstName"];
                                doctor.LastName  = (string)reader["LastName"];
                                doctor.Address   = (string)reader["Address"];
                                doctor.OfficeNum = (string)reader["OfficeNum"];
                                doctor.City      = (string)reader["City"];
                                doctor.State     = (string)reader["State"];
                                doctor.ZipCode   = (int)reader["ZipCode"];
                                doctor.Pending   = (Boolean)reader["Pending"];
                                doctorList.Add(doctor);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return doctorList;
        }
         public List<DoctorFromDatabase> listPendingDoctors()
         {
            List<DoctorFromDatabase> doctorList = null;
            DoctorFromDatabase       doctor     = null;

            query = "SELECT (Doctors.FirstName + ' ' + Doctors.LastName) AS FullName, " +
                    "(Doctors.Address + ', ' + Doctors.City + ', ' + UPPER(Doctors.State) + ', ' + CAST(Doctors.ZipCode AS NVARCHAR(50))) AS DoctorAddress, Doctors.DoctorID, Doctors.Email, Doctors.CreatedAt FROM Doctors WHERE Pending = 1;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            doctorList = new List<DoctorFromDatabase>();
                            while (reader.Read())
                            {
                                doctor               = new DoctorFromDatabase();
                                doctor.DoctorID      = (int)reader["DoctorID"];
                                doctor.FullName      = (string)reader["FullName"];
                                doctor.DoctorAddress = (string)reader["DoctorAddress"];
                                doctor.Email         = (string)reader["Email"];
                                doctor.CreatedAt     = (DateTime)reader["CreatedAt"];
                                doctorList.Add(doctor);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return doctorList;
        }
        public bool isAdmin(String email)
        {
            Boolean adminCheck = false;

            query = "SELECT * FROM Admins WHERE Email = @Email;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 50).Value = email;
                try
                {
                    conn.Open();
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                adminCheck = true;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return adminCheck;
        }
    }
}