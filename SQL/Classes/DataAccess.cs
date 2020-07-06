using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; //used for database connections 

namespace SQL.Classes {
    class DataAccess {
        public List<properties> GetPeople(string term) {
            List<properties> lst_return      = new List<properties>();

            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString      = helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY
            string queryStatement = "SELECT * FROM Records WHERE first_name LIKE '%" + term + 
                "%' OR last_name LIKE '%" + term + "%' ;";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            SqlDataReader data_reader =  sql_command.ExecuteReader();

            //READ DATA RETURNED
            while (data_reader.Read()) {
                //ARRAY FOR DATA IN CURRENT ROW
                object[] records = new object[8];

                //NEW PERSON INSTANCE
                properties new_person = new properties();
                
                //POPULATE DATA FROM DATA READER INTO ARRAY
                data_reader.GetValues(records);
                //SET PROPERTIES OF PERSON INSTANCE
                new_person.id           = Convert.ToInt32(records[0].ToString());
                new_person.first_name    = records[1].ToString();
                new_person.last_name   = records[2].ToString();
                new_person.cell         = records[3].ToString();
                new_person.work         = records[4].ToString();
                new_person.mail         = records[5].ToString();
                new_person.notes        = records[6].ToString();
                new_person.active       = SetActive(records[7].ToString());
                

                if(new_person.active == true) {
                    lst_return.Add(new_person);
                }
                //ADD TO LIST
            }//end while

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

            //RETURN LIST OF PERSONS
            return lst_return;
        }//end method

        public List<string> GetEmails(string term) {
            List<string> lst_return      = new List<string>();
            #region Open Connection
            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString      = helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();
            #endregion
            //BUILD QUERY
            string queryStatement = "SELECT * FROM Email WHERE Contact_ID LIKE '%" + term + "%';";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            SqlDataReader data_reader =  sql_command.ExecuteReader();

            //READ DATA RETURNED
            while (data_reader.Read()) {
                //ARRAY FOR DATA IN CURRENT ROW
                object[] records = new object[3];

                //NEW PERSON INSTANCE
                string new_person;
                
                //POPULATE DATA FROM DATA READER INTO ARRAY
                data_reader.GetValues(records);
                //SET PROPERTIES OF PERSON INSTANCE
                new_person = records[2].ToString();

                lst_return.Add(new_person);

                //ADD TO LIST
            }//end while

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

            //RETURN LIST OF PERSONS
            return lst_return;
        }//end method

        public List<properties> GetFalsePeople(string term) {
            List<properties> lst_return      = new List<properties>();

            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString      = helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY
            string queryStatement = "SELECT * FROM Records WHERE first_name LIKE '%" + term + "%' OR last_name LIKE '%" + term + "%' ;";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            SqlDataReader data_reader =  sql_command.ExecuteReader();

            //READ DATA RETURNED
            while (data_reader.Read()) {
                //ARRAY FOR DATA IN CURRENT ROW
                object[] records = new object[8];

                //NEW PERSON INSTANCE
                properties new_person = new properties();

                //POPULATE DATA FROM DATA READER INTO ARRAY
                data_reader.GetValues(records);

                //SET PROPERTIES OF PERSON INSTANCE
                new_person.id           = Convert.ToInt32(records[0].ToString());
                new_person.last_name    = records[1].ToString();
                new_person.first_name   = records[2].ToString();
                new_person.cell         = records[3].ToString();
                new_person.work         = records[4].ToString();
                new_person.mail         = records[5].ToString();
                new_person.notes        = records[6].ToString();
                new_person.active       = SetActive(records[7].ToString());
                
                if(new_person.active == false) {
                    lst_return.Add(new_person);
                }
                //ADD TO LIST
            }//end while

            //DESTROY COMMAND INSTANCE
            sql_command.Dispose();

            //CLOSE CONNECTION WHEN DONE (IMPORTANT)
            sql_connection.Close();

            //RETURN LIST OF PERSONS
            return lst_return;
        }//end method

        public bool SetActive(string data) {
            if(data == "true") {
                return true;
            }
            else {
                return false;
            }
        }

        public void AddData(object[] term) {
            #region Open Connection
            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString      = helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();
            #endregion

            //BUILD QUERY
            string queryStatement = $"INSERT INTO Records (first_name, last_name, cell, work, mail, notes, Active) VALUES " +
                $"('{term[0]}','{term[1]}','{term[2]}','{term[3]}','{term[5]}','{term[6]}','true');";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            sql_command.ExecuteNonQuery();

            sql_connection.Dispose();
            sql_connection.Close();

        }//end method

        public void DeleteData(string id) {
            #region Open Connection
            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString      = helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();
            #endregion

            //BUILD QUERY
            string queryStatement = $"DELETE FROM Records WHERE (id = {id});";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            sql_command.ExecuteNonQuery();

            sql_connection.Dispose();
            sql_connection.Close();
        }

        public void DeletePhoto(string id) {
            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString      = helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            //BUILD QUERY
            string queryStatement = $"DELETE FROM Photos WHERE (Contact_ID = {id});";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            sql_command.ExecuteNonQuery();

            sql_connection.Dispose();
            sql_connection.Close();
        }

        public void Deactivate(string id) {
            #region Open Connection
            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString      = helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();
            #endregion

            //BUILD QUERY
            string queryStatement = $"UPDATE Records SET Active = 'false' WHERE (id ={id} );";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            sql_command.ExecuteNonQuery();

            sql_connection.Dispose();
            sql_connection.Close();
        }

        public void Reactivate(string id) {
            #region Open Connection
            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString      = helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();
            #endregion

            //BUILD QUERY
            string queryStatement = $"UPDATE Records SET Active = 'true' WHERE (id ={id} );";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);

            //EXECUTE QUERY
            sql_command.ExecuteNonQuery();

            sql_connection.Dispose();
            sql_connection.Close();
        }

        public void UpdateData(string[] data, string id) {

            #region Open Connection
            //GET CONNECTION STRING FROM HELPER CLASS
            string connectionString      = helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();
            #endregion

            //BUILD QUERY STATEMENTS
            string queryStatement;
            
            queryStatement = $"UPDATE Records SET first_name = '{data[0]}', last_name = '{data[1]}', cell = '{data[2]}', work = '{data[3]}'," +
            $" mail = '{data[5]}', notes = '{data[6]}' WHERE (id = {id});";
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);
            sql_command.ExecuteNonQuery();
            

            sql_connection.Dispose();
            sql_connection.Close();

        }

        public void UpdateEmail(string email, string id) {
            string connectionString      = helper.ConnectionValue("PeopleDB");

            //CONNECT TO DB USING CONNECTION STRING
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();

            string queryStatement = $"INSERT INTO Email (Contact_ID, Emails) VALUES ({id},'{email}');";

            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);
            sql_command.ExecuteNonQuery();
            //close up shop
            sql_connection.Dispose();
            sql_connection.Close();
        }

        public void AddPicture(byte[] img, int contactId) {

            string connectionString      = helper.ConnectionValue("PeopleDB");
            SqlConnection sql_connection = new SqlConnection(connectionString);

            //OPEN CONNECTION
            sql_connection.Open();
            
            string queryStatement = $"INSERT INTO Photos (Contact_ID, photo) VALUES ({contactId}, @img);";
            //prep command
            SqlCommand sql_command = new SqlCommand(queryStatement, sql_connection);
                    
            //set params as var binary
            SqlParameter param = sql_command.Parameters.Add("@img", System.Data.SqlDbType.VarBinary);
                    
            //set the value to byte array
            param.Value = img;
            //execute 
            sql_command.ExecuteNonQuery();
            //close up shop
            sql_connection.Dispose();
            sql_connection.Close();
        }
    
    }//end class
}
