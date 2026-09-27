using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public static class Names
{
    public static string Server = "127.0.0.1";
    public static string Port = "3307";
    public static string Database = "sample_game";
    public static string User = "root";
    public static string Password = "1234";

    // 테이블
    public static string GameUserTable = "gameuser";

    public static string ConnectionString => $"Server={Server};Port={Port};Database={Database};Uid={User};Pwd={Password};SslMode=None;";

}

public class DBTester : MonoBehaviour
{
    private Dictionary<int, string> User = new Dictionary<int, string>();

    private void Start()
    {
        
    }

    private void SelectAllUsers(MySqlConnection conn)
    {
        // 3. SELECT 쿼리 작성 (gameuser 테이블의 모든 데이터 조회)
        string query = $"SELECT * FROM {Names.GameUserTable}";

        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            Debug.Log("--- 유저 데이터 조회 결과 ---");
            while (reader.Read()) // 한 행(Row)씩 읽기
            {
                // 컬럼 이름을 모를 경우 인덱스(0, 1...)로 접근
                string firstCol = reader.GetValue(0).ToString();
                string secondCol = reader.GetValue(1).ToString();

                // 만약 컬럼명을 정확히 안다면 아래처럼 사용할 수 있습니다.
                // int id = reader.GetInt32("userId");
                // string name = reader.GetString("userName");

                Debug.Log($"Row 데이터 -> 1번째: {firstCol}, 2번째: {secondCol}");
            }
        }
    }

    private void InsertUser(MySqlConnection conn, int UniqueId, string userName)
    {
        var uniqueId = UniqueId;

        if (CheckUserIdExist(uniqueId) == true) return;

        string query = $"INSERT INTO {Names.GameUserTable} (userId, userName) VALUES (@userId, @userName);";

        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@userId", uniqueId);
            cmd.Parameters.AddWithValue("@userName", userName);

            int rowsAffected = cmd.ExecuteNonQuery();
            Debug.Log($"유저 추가 완료. 영향받은 행의 수: {rowsAffected}");
        }

        User.Add(uniqueId, userName);
    }

    private void UpdateUserExp(MySqlConnection conn, string userId, int newExp)
    {
        //  string updateQuery = $"UPDATE {tableName} SET userTotalExp = {sampleTotalExp} WHERE userId = '{userId}';";
        string query = $"UPDATE {Names.GameUserTable} SET userTotalExp = @exp WHERE userId = @userId;";

        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@exp", newExp);
            cmd.Parameters.AddWithValue("@userId", userId);

            int rowsAffected = cmd.ExecuteNonQuery();
            Debug.Log($"유저 경험치 업데이트 완료. 영향받은 행의 수: {rowsAffected}");
        }
    }

    private void DeleteUser(MySqlConnection conn, int userId)
    {
        string query = $"DELETE FROM {Names.GameUserTable} WHERE userId = @userId;";

        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@userId", userId);

            // DELETE 역시 결과 행을 읽어올 필요가 없으므로 ExecuteNonQuery를 사용합니다.
            int rowsAffected = cmd.ExecuteNonQuery();
            Debug.Log($"유저 삭제 완료. 영향받은 행의 수: {rowsAffected}");
        }

        User.Remove(userId);
    }

    private bool CheckUserIdExist(int userId)
    {
        if (User.ContainsKey(userId))
        {
            Debug.Log("이미 존재하는 ID 입니다");
            return true;
        }

        return false;
    }
}
