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
    public class PatientTier : BaseTier
    {
        public PatientTier() : base()
        {

        }

        public List<Patient> getAllPatients()
        {
            List<Patient> patientList = null;
            Patient patient = null;

            query = "SELECT * FROM patients;";

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
                            patientList = new List<Patient>();
                            while (reader.Read())
                            {
                                patient = new Patient();
                                patient.PatientID = (int)reader["PatientID"];
                                patient.FirstName = (string)reader["FirstName"];
                                if(reader["MiddleName"] != DBNull.Value)
                                {
                                    patient.MiddleName = (string)reader["MiddleName"];
                                }
                                else
                                {
                                    patient.MiddleName = "N\\A";
                                }
                                patient.LastName = (string)reader["LastName"];
                                patient.Address  = (string)reader["Address"];
                                if(reader["Address2"] != DBNull.Value)
                                {
                                    patient.Address2 = (string)reader["Address2"];
                                }
                                else
                                {
                                    patient.Address2 = "N\\A";
                                }
                                if(reader["AptNum"] != DBNull.Value)
                                {
                                    patient.AptNum = (string)reader["AptNum"];
                                }
                                else
                                {
                                    patient.AptNum = "N\\A";
                                }
                                patient.City    = (string)reader["City"];
                                patient.State   = (string)reader["State"];
                                patient.ZipCode = (int)reader["ZipCode"];

                                patientList.Add(patient);
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

            return patientList;
        }
        public Patient getPatientByID(int id)
        {
            Patient patient = null;
            return patient;
        }
        //public bool insertPatient(Patient patient)
        //{
        //    int rows = 0;

        //    //patient_id is an auto number
        //    query = "INSERT INTO patients" +
        //        "(FirstName, MiddleName, LastName, Address, Address2, City, State, ZipCode)" +
        //        "VALUES(@FName, @MName, @LName, @Address, @Address2, @City, @State, @ZipCode)";

        //    using (conn = new SqlConnection(connectionString))
        //    using (cmd  = new SqlCommand(query, conn))
        //    {
        //        cmd.Parameters.Add("@FName", System.Data.SqlDbType.NVarChar, 50).Value = patient.FirstName;
        //        if(patient.MiddleName != null)
        //        {
        //            cmd.Parameters.Add("@MName", System.Data.SqlDbType.NVarChar, 50).Value = patient.MiddleName;
        //        }
        //        else
        //        {
        //            cmd.Parameters.Add("@MName", System.Data.SqlDbType.NVarChar, 50).Value = DBNull.Value;
        //        }
        //        cmd.Parameters.Add("@LName", System.Data.SqlDbType.NVarChar, 50).Value   = patient.LastName;
        //        cmd.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar, 50).Value = patient.Address;
        //        if (patient.Address2 != null)
        //        {
        //            cmd.Parameters.Add("@Address2", System.Data.SqlDbType.NVarChar, 50).Value = patient.Address2;
        //        }
        //        else
        //        {
        //            cmd.Parameters.Add("@Address2", System.Data.SqlDbType.NVarChar, 50).Value  = DBNull.Value;
        //        }
        //        cmd.Parameters.Add("@City", System.Data.SqlDbType.NVarChar, 50).Value  = patient.City;
        //        cmd.Parameters.Add("@State", System.Data.SqlDbType.NVarChar, 50).Value = patient.State;
        //        cmd.Parameters.Add("@ZipCode", System.Data.SqlDbType.Int, 50).Value    = patient.ZipCode;

        //        try
        //        {
        //            conn.Open();
        //            rows = cmd.ExecuteNonQuery();

        //            if(rows > 0)
        //            {
        //                success = true;
        //            }
        //            else
        //            {
        //                success = false;
        //            }
        //        }catch(SqlException ex)
        //        {
        //            throw new Exception(ex.Message);
        //        }
        //        finally
        //        {
        //            conn.Close();
        //        }
        //        return success;
        //    }

        //}
        ///trying to figure out what to do here
        ///had an error with converting reg to patient... i think i want patient to inherit from registration
        public bool insertPatient(Registration regInfo)
        {
            int rows = 0;

            //patient_id is an auto number
            query = "INSERT INTO patients" +
                "(FirstName, MiddleName, LastName, Address, Address2, City, State, ZipCode)" +
                "VALUES(@FName, @MName, @LName, @Address, @Address2, @City, @State, @ZipCode)";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@FName", System.Data.SqlDbType.NVarChar, 50).Value = regInfo.FirstName;
                if(regInfo.MiddleName != null)
                {
                    cmd.Parameters.Add("@MName", System.Data.SqlDbType.NVarChar, 50).Value = regInfo.MiddleName;
                }
                else
                {
                    cmd.Parameters.Add("@MName", System.Data.SqlDbType.NVarChar, 50).Value = DBNull.Value;
                }
                cmd.Parameters.Add("@LName", System.Data.SqlDbType.NVarChar, 50).Value   = regInfo.LastName;
                cmd.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar, 50).Value = regInfo.Address;
                if (regInfo.Address2 != null)
                {
                    cmd.Parameters.Add("@Address2", System.Data.SqlDbType.NVarChar, 50).Value = regInfo.Address2;
                }
                else
                {
                    cmd.Parameters.Add("@Address2", System.Data.SqlDbType.NVarChar, 50).Value  = DBNull.Value;
                }
                cmd.Parameters.Add("@City", System.Data.SqlDbType.NVarChar, 50).Value  = regInfo.City;
                cmd.Parameters.Add("@State", System.Data.SqlDbType.NVarChar, 50).Value = regInfo.State;
                cmd.Parameters.Add("@ZipCode", System.Data.SqlDbType.Int, 50).Value    = regInfo.ZipCode;

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
        public bool updatePatient(Patient patient)
        {
            return success;
        }
        public bool deletePatient(int id)
        {
            return success;
        }

    }
}