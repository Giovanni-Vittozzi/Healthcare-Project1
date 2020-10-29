﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient; //connect to microsoft SQL server (provides acess to connection object, command object and reader)
using System.Data.SqlTypes; //in case we need data strings, but we most likely won't in a web app
using System.Configuration; //gives access to web config files (pull out connection string)
using HealthcareCompanion.DataAccessLayer;
using HealthcareCompanion.Models;

namespace HealthcareCompanion.DataAccessLayer
{
    public class DoctorTier : BaseTier
    {
        public DoctorTier() : base()
        {

        }

        public List<Doctor> getAllDoctors()
        {
            List<Doctor> doctorList = null;
            Doctor doctor = null;

            query = "SELECT * FROM doctors;";

            using (conn = new SqlConnection(connectionString))
            using (cmd = new SqlCommand(query, conn))
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
        public Doctor getDoctorByID(int id)
        {
            Doctor doctor = null;
            return doctor;
        }
        public bool insertDoctor(Doctor doctor)
        {
            int rows = 0;

            //DoctorID is an auto number
            query = "INSERT INTO doctors" +
                "(FirstName, LastName, Address, OfficeNum, City, State, ZipCode)" +
                "VALUES(@FName, @LName, @Address, @OfficeNum, @City, @State, @ZipCode)";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@FName", System.Data.SqlDbType.NVarChar, 50).Value     = doctor.FirstName;
                cmd.Parameters.Add("@LName", System.Data.SqlDbType.NVarChar, 50).Value     = doctor.LastName;
                cmd.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar, 50).Value   = doctor.Address;
                cmd.Parameters.Add("@OfficeNum", System.Data.SqlDbType.NVarChar, 50).Value = doctor.OfficeNum;
                cmd.Parameters.Add("@City", System.Data.SqlDbType.NVarChar, 50).Value      = doctor.City;
                cmd.Parameters.Add("@State", System.Data.SqlDbType.NVarChar, 50).Value     = doctor.State;
                cmd.Parameters.Add("@ZipCode", System.Data.SqlDbType.Int, 50).Value        = doctor.ZipCode;

                try
                {
                    conn.Open();
                    rows = cmd.ExecuteNonQuery();

                    if(rows > 0)
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }catch(SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
                return success;
            }
        }
        public bool updateDoctor(Doctor doctor)
        {
            return success;
        }
        public bool deleteDoctor(int id)
        {
            return success;
        }
    }
}