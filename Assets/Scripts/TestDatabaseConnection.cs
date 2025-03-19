using UnityEngine;
using Npgsql;
using System;

public class TestDatabaseConnection : MonoBehaviour
{
    void Start()
    {
        string connectionString = "Host=aws-0-eu-central-1.pooler.supabase.com;Port=6543;Username=postgres.bebzpyzauuqwsrapxpbd;Password=86fWrUnq2006;Database=postgres;";
        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                Debug.Log("Pøipojení k databázi úspìšné!");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Chyba pøi pøipojení k databázi: " + ex.Message);
        }
    }
}
