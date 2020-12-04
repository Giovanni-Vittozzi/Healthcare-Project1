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
                "(IdentityID, FirstName, MiddleName, LastName, Address, Address2, AptNum, City, State, ZipCode, Pending, Email)" +
                "VALUES(@IdentityID, @FName, @MName, @LName, @Address, @Address2, @AptNum, @City, @State, @ZipCode, @Pending, @Email)";

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
                if (patient.AptNum != null)
                {
                    cmd.Parameters.Add("@AptNum", System.Data.SqlDbType.NVarChar, 50).Value = patient.AptNum;
                }
                else
                {
                    cmd.Parameters.Add("@AptNum", System.Data.SqlDbType.NVarChar, 50).Value  = DBNull.Value;
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
        public Patient retrievePatient(int id)
        {
            Patient patient = null;

            query = "SELECT * FROM Patients WHERE PatientID = @PatientID;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = id;
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
                                patient.PatientID = (int)reader["PatientID"];
                                patient.FirstName = (string)reader["FirstName"];
                                if (reader["MiddleName"] != DBNull.Value)
                                {
                                    patient.MiddleName = (string)reader["MiddleName"];
                                }
                                else
                                {
                                    patient.MiddleName = null;
                                }
                                patient.LastName  = (string)reader["LastName"];
                                patient.Address   = (string)reader["Address"];
                                if (reader["Address2"] != DBNull.Value)
                                {
                                    patient.Address2 = (string)reader["Address2"];
                                }
                                else
                                {
                                    patient.Address2 = null;
                                }
                                if (reader["AptNum"] != DBNull.Value)
                                {
                                    patient.AptNum = (string)reader["AptNum"];
                                }
                                else
                                {
                                    patient.AptNum = null;
                                }
                                patient.City      = (string)reader["City"];
                                patient.State     = (string)reader["State"];
                                patient.ZipCode   = (int)reader["ZipCode"];
                                patient.Pending   = (Boolean)reader["Pending"];
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
            return patient;
        }

        //get pending Patients and returns list [where pending = true or 1]
        public int getPatientByID(String id)
        {
            Patient patient         = null;
            Boolean identityIDCheck = false;
            int     myid            = 0;

            query = "SELECT * FROM Patients WHERE IdentityID = @IdentityID;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@IdentityID", System.Data.SqlDbType.NVarChar, 128).Value = id;
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

        //get pending Patients and returns list [where pending = true or 1]
        public String getPatientEmail(int id)
        {
            String  email           = null;

            query = "SELECT * FROM Patients WHERE PatientID = @PatientID;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = id;
                try
                {
                    conn.Open();
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                email = (String)reader["Email"];
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
            return email;
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
            //where patientid is equal to this and pending is true
            query = "SELECT * FROM Patients WHERE Email = @Email;";

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
                                patient           = new Patient();
                                patient.Email     = (string)reader["Email"];
                                emailCheck        = (patient.Email).Equals(email);
                                if (emailCheck)
                                {
                                    if ((bool)reader["Pending"])
                                    {
                                        pendingCheck = true;
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
        public bool updatePatientInfo(Patient patient)
        {
            int rows = 0;

            //DoctorID is an auto number
            query = "UPDATE Patients SET " +
                "FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Address = @Address, Address2 = @Address2, AptNum = @AptNum, City = @City, State = @State, ZipCode = @ZipCode " +
                "WHERE PatientID = @PatientID";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@PatientID",  System.Data.SqlDbType.Int         ).Value = patient.PatientID;
                cmd.Parameters.Add("@FirstName",  System.Data.SqlDbType.NVarChar, 50).Value = patient.FirstName;
                if (patient.MiddleName != null)
                {
                    cmd.Parameters.Add("@MiddleName", System.Data.SqlDbType.NVarChar, 50).Value = patient.MiddleName;
                }
                else
                {
                    cmd.Parameters.Add("@MiddleName", System.Data.SqlDbType.NVarChar, 50).Value = DBNull.Value;
                }
                cmd.Parameters.Add("@LastName",   System.Data.SqlDbType.NVarChar, 50).Value = patient.LastName;
                cmd.Parameters.Add("@Address",    System.Data.SqlDbType.NVarChar, 50).Value = patient.Address;
                if (patient.Address2 != null)
                {
                    cmd.Parameters.Add("@Address2", System.Data.SqlDbType.NVarChar, 50).Value = patient.Address2;
                }
                else
                {
                    cmd.Parameters.Add("@Address2", System.Data.SqlDbType.NVarChar, 50).Value = DBNull.Value;
                }
                if (patient.AptNum != null)
                {
                    cmd.Parameters.Add("@AptNum", System.Data.SqlDbType.NVarChar, 50).Value = patient.AptNum;
                }
                else
                {
                    cmd.Parameters.Add("@AptNum", System.Data.SqlDbType.NVarChar, 50).Value = DBNull.Value;
                }
                cmd.Parameters.Add("@City",      System.Data.SqlDbType.NVarChar, 50).Value = patient.City;
                cmd.Parameters.Add("@State",     System.Data.SqlDbType.NVarChar, 50).Value = patient.State;
                cmd.Parameters.Add("@ZipCode",   System.Data.SqlDbType.Int,      50).Value = patient.ZipCode;
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
        public bool insertMedicalData(MedicalData medicalData)
        {
            int rows = 0;

            //patient_id is an auto number
            query = "INSERT INTO PatientMedicalData" +
                "(TypeID, PatientID, DateEntered, Value1, Value2, TimeOfDay) " +
                "VALUES(@TypeID, @PatientID, @DateEntered, @Value1, @Value2, @TimeOfDay)";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@TypeID",    System.Data.SqlDbType.Int).Value = medicalData.TypeID; 
                cmd.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = medicalData.PatientID;
                cmd.Parameters.Add("@Value1",    System.Data.SqlDbType.Int).Value = medicalData.Value1;
                cmd.Parameters.AddWithValue("@DateEntered", DateTime.Now);
                if (medicalData.Value2 != 0)
                {
                    cmd.Parameters.Add("@Value2", System.Data.SqlDbType.Int).Value = medicalData.Value2;
                }
                else
                {
                    cmd.Parameters.Add("@Value2", System.Data.SqlDbType.Int).Value = DBNull.Value;
                }
                cmd.Parameters.Add("@TimeOfDay", System.Data.SqlDbType.NVarChar, 50).Value = medicalData.TimeOfDay;
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
        public List<MedicalData> listMedicalData(int PatientID)
        {
            List<MedicalData> medicalDataList = null;
            MedicalData       medicalData     = null;
            int               rows            = 0;

            //patient_id is an auto number
            query = "Select * FROM PatientMedicalData WHERE PatientID = @PatientID;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = PatientID;
                try
                {
                    conn.Open();
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            medicalDataList = new List<MedicalData>();
                            while (reader.Read())
                            {
                                medicalData             = new MedicalData();
                                medicalData.TypeID      = (int)reader["TypeID"];
                                medicalData.PatientID   = (int)reader["PatientID"];
                                medicalData.Now         = (DateTime)reader["DateEntered"];
                                medicalData.Value1      = (int)reader["Value1"];
                                medicalData.TimeOfDay   = (string)reader["TimeOfDay"];
                            if (reader["Value2"] != DBNull.Value)
                            {
                                medicalData.Value2 = (int)reader["Value2"];
                            }
                            else
                            {
                                medicalData.Value2 = null;
                            }
                            medicalDataList.Add(medicalData);
                            }
                        }
                    }
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
                return medicalDataList;
            }
        }
        public List<MedicalData> listMedicalDataByTypeID(int PatientID, int TypeID)
        {
            List<MedicalData> medicalDataList = null;
            MedicalData       medicalData     = null;
            int               rows            = 0;

            //patient_id is an auto number
            query = "Select * FROM PatientMedicalData WHERE PatientID = @PatientID AND TypeID = @TypeID;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = PatientID;
                cmd.Parameters.Add("@TypeID", System.Data.SqlDbType.Int).Value    = TypeID;
                try
                {
                    conn.Open();
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            medicalDataList = new List<MedicalData>();
                            while (reader.Read())
                            {
                                medicalData             = new MedicalData();
                                medicalData.TypeID      = (int)reader["TypeID"];
                                medicalData.PatientID   = (int)reader["PatientID"];
                                medicalData.Now         = (DateTime)reader["DateEntered"];
                                medicalData.Value1      = (int)reader["Value1"];
                                medicalData.TimeOfDay   = (string)reader["TimeOfDay"];
                            if (reader["Value2"] != DBNull.Value)
                            {
                                medicalData.Value2 = (int)reader["Value2"];
                            }
                            else
                            {
                                medicalData.Value2 = null;
                            }
                            medicalDataList.Add(medicalData);
                            }
                        }
                    }

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
                return medicalDataList;
            }
        }
        public List<MedicalData> listMedicalDataByTypeIDByDate(int PatientID, int TypeID, int? Month, int Year)
        {
            List<MedicalData> medicalDataList = null;
            MedicalData       medicalData     = null;
            int               rows            = 0;

            //patient_id is an auto number
            query = "Select * FROM PatientMedicalData WHERE PatientID = @PatientID AND TypeID = @TypeID AND MONTH(DateEntered)=@Month AND YEAR(DateEntered)=@Year;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = PatientID;
                cmd.Parameters.Add("@TypeID",    System.Data.SqlDbType.Int).Value = TypeID;
                cmd.Parameters.Add("@Month",     System.Data.SqlDbType.Int).Value = Month;
                cmd.Parameters.Add("@Year",      System.Data.SqlDbType.Int).Value = Year;
                try
                {
                    conn.Open();
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            medicalDataList = new List<MedicalData>();
                            while (reader.Read())
                            {
                                medicalData             = new MedicalData();
                                medicalData.TypeID      = (int)reader["TypeID"];
                                medicalData.PatientID   = (int)reader["PatientID"];
                                medicalData.Now         = (DateTime)reader["DateEntered"];
                                medicalData.Value1      = (int)reader["Value1"];
                                medicalData.TimeOfDay   = (string)reader["TimeOfDay"];
                            if (reader["Value2"] != DBNull.Value)
                            {
                                medicalData.Value2 = (int)reader["Value2"];
                            }
                            else
                            {
                                medicalData.Value2 = null;
                            }
                            medicalDataList.Add(medicalData);
                            }
                        }
                    }

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
                return medicalDataList;
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
        public bool approvePatient(int patientID)
        {
            int rows = 0;

            //patient_id is an auto number
            query = "UPDATE Patients SET Pending = 0 WHERE PatientID = @PatientID;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Value = patientID; 
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
