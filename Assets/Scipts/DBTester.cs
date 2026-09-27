using UnityEngine;
using System;
using MySqlConnector;
using System.ComponentModel.Design.Serialization;

public class DBTester : MonoBehaviour
{
    void Start()
    {
        string server = "127.0.0.1";    // 로컬이라서 그냥 사용 [외부IP로 사용하는 경우는 gitignore해서 올려야한다]
        string port = "3307";
        string database = "sample_game";
        string user = "root";
        string password = "1234";

        string connString = $"Server={server};Port={port};Database={database};Uid={user};Pwd={password};SslMode=None;";
    }
}
