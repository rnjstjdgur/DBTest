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

        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            try
            {
                // 데이터베이스 열기
                conn.Open();
                Debug.Log("MySQL 연결 성공!");

                string query = "SELECT * FROM gameuser";
                using(MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using(MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string firstCol = reader.GetValue(0).ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"MySQL 연결 또는 조회 실패: {ex.Message}");
            }
        }
    }
}
