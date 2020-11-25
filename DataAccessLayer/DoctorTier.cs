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
    public class DoctorTier : BaseTier
    {
        public DoctorTier() : base()
        {

        }
        public List<Doctor> getAllDoctors()
        {
            List<Doctor> doctorList = null;
            Doctor       doctor     = null;

            query = "SELECT * FROM Doctors;";

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
        public Doctor retrieveDoctor(int id)
        {
            Doctor doctor = null;

            query = "SELECT * FROM Doctors WHERE DoctorID = @DoctorID;";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@DoctorID", System.Data.SqlDbType.Int).Value = id;
                try
                {
                    conn.Open();
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
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

            return doctor;
        }
        public List<SelectListItem> listAllDoctors()
        {
            List<SelectListItem> doctorList = null;
            SelectListItem       item       = null;

            query = "Select ('Dr. ' + FirstName + ' ' + LastName) AS FullName, " +
                    "(OfficeNum + ', ' + Address + ', ' + City + ', ' + UPPER(State) + ', ' + CAST(ZipCode AS NVARCHAR(50))) AS DoctorOfficeAddress, " + 
                    "DoctorID, Pending " + 
                    "FROM Doctors;";

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
                            doctorList = new List<SelectListItem>();
                            while (reader.Read())
                            {
                                if (reader["Pending"].ToString().Equals("False"))
                                {
                                    item       = new SelectListItem();
                                    var name   = reader["FullName"].ToString();
                                    var office = reader["DoctorOfficeAddress"].ToString();
                                    item.Value = reader["DoctorID"].ToString();
                                    item.Text  = name + " " + office;
                                    doctorList.Add(item);
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
            return doctorList;
        }
        public int getDoctorByID(String id)
        {
            Doctor  doctor          = null;
            Boolean identityIDCheck = false;
            int myid = 0;

            query = "SELECT * FROM Doctors WHERE IdentityID = @IdentityID;";

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
                                doctor = new Doctor();
                                doctor.userID = (string)reader["IdentityID"];
                                identityIDCheck = (doctor.userID).Equals(id);
                                if (identityIDCheck)
                                {
                                    myid = (int)reader["DoctorID"];
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

        public bool insertDoctor(Doctor doctor)
        {
            int rows = 0;

            //DoctorID is an auto number
            query = "INSERT INTO Doctors" +
                "(IdentityID, FirstName, LastName, Address, OfficeNum, City, State, ZipCode, Pending, Email)" +
                "VALUES(@IdentityID, @FName, @LName, @Address, @OfficeNum, @City, @State, @ZipCode, @Pending, @Email)";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@IdentityID", System.Data.SqlDbType.NVarChar, 128).Value = doctor.userID;
                cmd.Parameters.Add("@FName",      System.Data.SqlDbType.NVarChar, 50).Value  = doctor.FirstName;
                cmd.Parameters.Add("@LName",      System.Data.SqlDbType.NVarChar, 50).Value  = doctor.LastName;
                cmd.Parameters.Add("@Address",    System.Data.SqlDbType.NVarChar, 50).Value  = doctor.Address;
                cmd.Parameters.Add("@OfficeNum",  System.Data.SqlDbType.NVarChar, 50).Value  = doctor.OfficeNum;
                cmd.Parameters.Add("@City",       System.Data.SqlDbType.NVarChar, 50).Value  = doctor.City;
                cmd.Parameters.Add("@State",      System.Data.SqlDbType.NVarChar, 50).Value  = doctor.State;
                cmd.Parameters.Add("@ZipCode",    System.Data.SqlDbType.Int,      50).Value  = doctor.ZipCode;
                cmd.Parameters.Add("@Pending",    System.Data.SqlDbType.Bit         ).Value  = doctor.Pending;
                cmd.Parameters.Add("@Email",      System.Data.SqlDbType.NVarChar, 50).Value  = doctor.Email;
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
        public (Boolean pendingCheck, Boolean emailCheck) isPendingDoctor(String email)
        {
            Doctor  doctor       = null;
            Boolean emailCheck   = false;
            Boolean pendingCheck = false;

            query = "SELECT * FROM Doctors";

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
                                doctor       = new Doctor();
                                doctor.Email = (string)reader["Email"];
                                emailCheck   = (doctor.Email).Equals(email);
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
        public List<PatientFromDatabase> listPendingPatients(int id)
        {
            List<PatientFromDatabase> patientList = null;
            PatientFromDatabase       patient     = null;

            query = "SELECT (Patients.FirstName + ' ' + Patients.LastName) AS FullName, " +
                    "(Patients.Address + ', ' + Patients.City + ', ' + UPPER(Patients.State) + ', ' + CAST(Patients.ZipCode AS NVARCHAR(50))) AS PatientAddress, Patients.PatientID, Patients.Email, Patients.CreatedAt " +
                    "FROM Doctors Inner Join PatientAssignment on Doctors.DoctorID = PatientAssignment.DoctorID " +
                    "INNER JOIN Patients on PatientAssignment.PatientID = Patients.PatientID WHERE Doctors.DoctorID = @PatientID AND Patients.Pending = 1; ";

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
                            patientList = new List<PatientFromDatabase>();
                            while (reader.Read())
                            {
                                patient                = new PatientFromDatabase();
                                patient.PatientID      = (int)reader["PatientID"];
                                patient.FullName       = (string)reader["FullName"];
                                patient.PatientAddress = (string)reader["PatientAddress"];
                                patient.Email          = (string)reader["Email"];
                                patient.CreatedAt      = (DateTime)reader["CreatedAt"];
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
        public List<PatientFromDatabase> listAllPatients(int id)
        {
            List<PatientFromDatabase> patientList = null;
            PatientFromDatabase       patient     = null;

            query = "SELECT (Patients.FirstName + ' ' + Patients.LastName) AS FullName, " +
                    "(Patients.Address + ', ' + Patients.City + ', ' + UPPER(Patients.State) + ', ' + CAST(Patients.ZipCode AS NVARCHAR(50))) AS PatientAddress, Patients.PatientID, Patients.Email, Patients.CreatedAt " +
                    "FROM Doctors Inner Join PatientAssignment on Doctors.DoctorID = PatientAssignment.DoctorID " +
                    "INNER JOIN Patients on PatientAssignment.PatientID = Patients.PatientID WHERE Doctors.DoctorID = @PatientID; ";

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
                            patientList = new List<PatientFromDatabase>();
                            while (reader.Read())
                            {
                                patient                = new PatientFromDatabase();
                                patient.PatientID      = (int)reader["PatientID"];
                                patient.FullName       = (string)reader["FullName"];
                                patient.PatientAddress = (string)reader["PatientAddress"];
                                patient.Email = (string)reader["Email"];
                                patient.CreatedAt      = (DateTime)reader["CreatedAt"];
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
        public bool updateDoctorInfo(Doctor doctor)
        {
            int rows = 0;

            //DoctorID is an auto number
            query = "UPDATE Doctors SET " +
                "FirstName = @FirstName, LastName = @LastName, Address = @Address, OfficeNum = @OfficeNum, City = @City, State = @State, ZipCode = @ZipCode " +
                "WHERE DoctorID = @DoctorID";

            using (conn = new SqlConnection(connectionString))
            using (cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@DoctorID",   System.Data.SqlDbType.Int         ).Value  = doctor.DoctorID;
                cmd.Parameters.Add("@FirstName",  System.Data.SqlDbType.NVarChar, 50).Value  = doctor.FirstName;
                cmd.Parameters.Add("@LastName",   System.Data.SqlDbType.NVarChar, 50).Value  = doctor.LastName;
                cmd.Parameters.Add("@Address",    System.Data.SqlDbType.NVarChar, 50).Value  = doctor.Address;
                cmd.Parameters.Add("@OfficeNum",  System.Data.SqlDbType.NVarChar, 50).Value  = doctor.OfficeNum;
                cmd.Parameters.Add("@City",       System.Data.SqlDbType.NVarChar, 50).Value  = doctor.City;
                cmd.Parameters.Add("@State",      System.Data.SqlDbType.NVarChar, 50).Value  = doctor.State;
                cmd.Parameters.Add("@ZipCode",    System.Data.SqlDbType.Int,      50).Value  = doctor.ZipCode;
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
        public bool deleteDoctor(int id)
        {
            return success;
        }
    }
}