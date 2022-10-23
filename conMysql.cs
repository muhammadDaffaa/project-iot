using System;
using MySql.Data.MySqlClient;

class conMysql
{
    private MySqlConnection connection;
    
    // constructor
    public conMysql(){
        Initialize();
    }

    private void Initialize(){
        
        string connection_string = $"server=aessa.space;user=user_daffa;database=iot_kit;port=3306;password=admin06";

        connection = new MySqlConnection(connection_string);   
    }

 //open connection to database
    private bool OpenConnection()
    {
        try
        {
            connection.Open();
            return true;
        }
    catch (MySqlException ex)
        {
     
        Console.WriteLine(ex);
        return false; 
        } 
    }

    //Close connection
    private bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
    catch (MySqlException ex)
        {
            Console.WriteLine(ex);
            return false;
        }

    }

    //Insert statement
    public void SaveTemperature(string temperature, string humidity,string vOut,string vIn)
    {
            
            string query = $"INSERT INTO sensor (temperature,humidity,vOut,vIn) VALUES ({temperature},{humidity},{vOut},{vIn});";

    //open connection
    if (this.OpenConnection() == true)
        {
            //create command and assign the query and connection from the constructor
             MySqlCommand cmd = new MySqlCommand(query, connection);
        
            //Execute command
            cmd.ExecuteNonQuery();

            //close connection
            this.CloseConnection();
        }
    }
    // //Update statement
    // public void Update()
    // {
    // }

    // //Delete statement
    // public void Delete()
    // {
    // }

    // //Select statement
    // public List <string> [] Select()
    // {
    // }

    // //Count statement
    // public int Count()
    // {
    // }

    // //Backup
    // public void Backup()
    // {
    // }

    //Restore
    // public void Restore()
    // {
    // }

}