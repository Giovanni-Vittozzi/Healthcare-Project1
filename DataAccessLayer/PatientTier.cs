using System;
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
            Patient       patient     = null;

            query = "SELECT * FROM Patients;";

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
                            patientList = new List<Patient>();
                            while (reader.Read())
                            {
                                patient           = new Patient();
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
                                if (reader["Pending"] != DBNull.Value)
                                {
                                    patient.Pending = (bool)(reader["Pending"]); //Convert.ToBoolean
                                }
                                else
                                {
                                    patient.Pending = false;
                                }
                                patient.Email = (string)reader["Email"];

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
        public bool insertPatient(Patient patient)
        {
            int rows = 0;

            //patient_id is an auto number
            query = "INSERT INTO Patients" +
                "(IdentityID, FirstName, MiddleName, LastName, Address, Address2, City, State, ZipCode, Pending, Email)" +
                "VALUES(@IdentityID, @FName, @MName, @LName, @Address, @Address2, @City, @State, @ZipCode, @Pending, @Email)";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@IdentityID", System.Data.SqlDbType.NVarChar, 128).Value = patient.userID;
                cmd.Parameters.Add("@FName", System.Data.SqlDbType.NVarChar, 50).Value       = patient.FirstName;
                if(patient.MiddleName != null)
                {
                    cmd.Parameters.Add("@MName", System.Data.SqlDbType.NVarChar, 50).Value = patient.MiddleName;
                }
                else
                {
                    cmd.Parameters.Add("@MName", System.Data.SqlDbType.NVarChar, 50).Value = DBNull.Value;
                }
                cmd.Parameters.Add("@LName", System.Data.SqlDbType.NVarChar, 50).Value   = patient.LastName;
                cmd.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar, 50).Value = patient.Address;
                if (patient.Address2 != null)
                {
                    cmd.Parameters.Add("@Address2", System.Data.SqlDbType.NVarChar, 50).Value = patient.Address2;
                }
                else
                {
                    cmd.Parameters.Add("@Address2", System.Data.SqlDbType.NVarChar, 50).Value  = DBNull.Value;
                }
                cmd.Parameters.Add("@City", System.Data.SqlDbType.NVarChar, 50).Value  = patient.City;
                cmd.Parameters.Add("@State", System.Data.SqlDbType.NVarChar, 50).Value = patient.State;
                cmd.Parameters.Add("@ZipCode", System.Data.SqlDbType.Int, 50).Value    = patient.ZipCode;
                cmd.Parameters.Add("@Pending", System.Data.SqlDbType.Bit).Value        = patient.Pending;
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 50).Value = patient.Email;
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
        //get pending Patients and returns list [where pending = true or 1]
        public int getPatientByID(String id)
        {
            Patient patient         = null;
            Boolean identityIDCheck = false;
            int     myid            = 0;

            query = "SELECT * FROM Patients WHERE IdentityID = '" + id + "';";

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
                            while (reader.Read())
                            {
                                patient = new Patient();
                                patient.userID  = (string)reader["IdentityID"];
                                identityIDCheck = (patient.userID).Equals(id);
                                if (identityIDCheck)
                                {
                                    myid = (int)reader["PatientID"];
                                }
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
            return myid;
        }

        public bool updatePatient(Patient patient)
        {
            return success;
        }
        public bool deletePatient(int id)
        {
            return success;
        }
        public (Boolean pendingCheck, Boolean emailCheck) isPendingPatient(String email)
        {
            Patient patient      = null;
            Boolean emailCheck   = false;
            Boolean pendingCheck = false;

            query = "SELECT * FROM Patients WHERE Pending = 'True';";

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
                            while (reader.Read())
                            {
                                patient           = new Patient();
                                patient.Email     = (string)reader["Email"];
                                emailCheck        = (patient.Email).Equals(email);
                                if (emailCheck)
                                {
                                    if (reader["Pending"] != DBNull.Value)
                                    {
                                        pendingCheck    = true;
                                    }
                                    else
                                    {
                                        pendingCheck = false;
                                    }
                                }
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
            return (pendingCheck, emailCheck);
        }
        public bool insertBloodSugar(int patientID, BloodSugar bloodSugar)
        {
            int rows = 0;

            //patient_id is an auto number
            query = "INSERT INTO PatientMedicalData" +
                "(TypeID, PatientID, DateEntered, Value1, TimeOfDay)" +
                "VALUES(@TypeID, @PatientID, @DateEntered, @Value1, @TimeOfDay)";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value           = patientID; 
                cmd.Parameters.Add("@BloodSugarValue", System.Data.SqlDbType.Int).Value     = bloodSugar.BloodSugarValue;
                //cmd.Parameters.Add("@BloodSugarTime", System.Data.SqlDbType.Time).Value     = bloodSugar.Hour.TimeOfDay;
                //cmd.Parameters.Add("@BloodSugarDate", System.Data.SqlDbType.DateTime).Value = bloodSugar.ReleaseDate;
                try
                {
                    conn.Open();
                    rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
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
                return success;
            }

        }
        public bool pairDoctorPatient(int patientID, int DoctorID)
        {
            int rows = 0;

            //patient_id is an auto number
            query = "INSERT INTO PatientAssignment" +
                "(PatientID, DoctorID) " +
                "VALUES(@PatientID, @DoctorID);";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = patientID; 
                cmd.Parameters.Add("@DoctorID", System.Data.SqlDbType.Int).Value  = DoctorID;
                try
                {
                    conn.Open();
                    rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
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
                return success;
            }

        }
    }
}
