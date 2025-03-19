using System;
using System.Threading.Tasks;
using UnityEngine;
using Npgsql;

public class DatabaseManager : MonoBehaviour
{
    private static DatabaseManager _instance;
    public static DatabaseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("❌ DatabaseManager není ve scéně!");
            }
            return _instance;
        }
    }

    private NpgsqlConnection persistentConnection;
    private readonly string connectionString = "Host=aws-0-eu-central-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.bebzpyzauuqwsrapxpbd;Password=86fWrUnq2006;SslMode=Require;Trust Server Certificate=true;";

    private async void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            await ConnectToDatabaseAsync();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async Task ConnectToDatabaseAsync()
    {
        try
        {
            if (persistentConnection != null && persistentConnection.State == System.Data.ConnectionState.Open)
            {
                Debug.Log("✅ Databáze je již připojena.");
                return;
            }

            persistentConnection = new NpgsqlConnection(connectionString);
            await persistentConnection.OpenAsync();
            Debug.Log("✅ Připojeno k databázi Supabase!");
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Chyba při připojení k databázi: " + ex.Message);
        }
    }

    public bool IsConnected()
    {
        return persistentConnection != null && persistentConnection.State == System.Data.ConnectionState.Open;
    }

    public async Task<NpgsqlConnection> GetConnectionAsync()
    {
        if (!IsConnected())
        {
            Debug.LogWarning("⚠ Databázové připojení ztraceno. Pokus o nové připojení...");
            await ConnectToDatabaseAsync();
        }

        try
        {
            var newConn = new NpgsqlConnection(connectionString);
            await newConn.OpenAsync();
            return newConn;
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Chyba při vytváření nového připojení: " + ex.Message);
            return null;
        }
    }

    private void OnApplicationQuit()
    {
        if (persistentConnection != null && persistentConnection.State == System.Data.ConnectionState.Open)
        {
            persistentConnection.Close();
            Debug.Log("📴 Databázové spojení uzavřeno při ukončení aplikace.");
        }
    }
}
