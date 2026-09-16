using System.Data;
using System.Globalization;

namespace AbsSilkSaris.Data;

public static class Ado
{
    public static IDbDataParameter Param(IDbCommand cmd, string name, object? value)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = name.StartsWith('@') ? name : "@" + name;
        p.Value = value ?? DBNull.Value;
        cmd.Parameters.Add(p);
        return p;
    }

    public static async Task<IDbConnection> OpenAsync(IDbConnectionFactory factory)
    {
        var conn = factory.Create();
        if (conn is System.Data.Common.DbConnection db)
        {
            await db.OpenAsync();
        }
        else
        {
            conn.Open();
        }
        return conn;
    }

    public static async Task<int> ExecuteAsync(IDbConnection conn, string sql, params (string Name, object? Value)[] args)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        foreach (var (name, value) in args) Param(cmd, name, value);
        if (cmd is System.Data.Common.DbCommand dbc) return await dbc.ExecuteNonQueryAsync();
        return cmd.ExecuteNonQuery();
    }

    public static async Task<object?> ScalarAsync(IDbConnection conn, string sql, params (string Name, object? Value)[] args)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        foreach (var (name, value) in args) Param(cmd, name, value);
        if (cmd is System.Data.Common.DbCommand dbc) return await dbc.ExecuteScalarAsync();
        return cmd.ExecuteScalar();
    }

    public static async Task<List<T>> QueryAsync<T>(IDbConnection conn, string sql, Func<IDataRecord, T> map, params (string Name, object? Value)[] args)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        foreach (var (name, value) in args) Param(cmd, name, value);
        var list = new List<T>();
        if (cmd is System.Data.Common.DbCommand dbc)
        {
            await using var reader = await dbc.ExecuteReaderAsync();
            while (await reader.ReadAsync()) list.Add(map(reader));
        }
        else
        {
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(map(reader));
        }
        return list;
    }

    public static async Task<T?> QuerySingleAsync<T>(IDbConnection conn, string sql, Func<IDataRecord, T> map, params (string Name, object? Value)[] args)
        where T : class
    {
        var rows = await QueryAsync(conn, sql, map, args);
        return rows.FirstOrDefault();
    }

    public static string Str(IDataRecord r, string col)
    {
        var i = r.GetOrdinal(col);
        return r.IsDBNull(i) ? string.Empty : Convert.ToString(r.GetValue(i), CultureInfo.InvariantCulture) ?? string.Empty;
    }

    public static string? StrN(IDataRecord r, string col)
    {
        var i = r.GetOrdinal(col);
        return r.IsDBNull(i) ? null : Convert.ToString(r.GetValue(i), CultureInfo.InvariantCulture);
    }

    public static int Int(IDataRecord r, string col)
    {
        var i = r.GetOrdinal(col);
        return r.IsDBNull(i) ? 0 : Convert.ToInt32(r.GetValue(i), CultureInfo.InvariantCulture);
    }

    public static int? IntN(IDataRecord r, string col)
    {
        var i = r.GetOrdinal(col);
        return r.IsDBNull(i) ? null : Convert.ToInt32(r.GetValue(i), CultureInfo.InvariantCulture);
    }

    public static decimal Dec(IDataRecord r, string col)
    {
        var i = r.GetOrdinal(col);
        return r.IsDBNull(i) ? 0 : Convert.ToDecimal(r.GetValue(i), CultureInfo.InvariantCulture);
    }

    public static decimal? DecN(IDataRecord r, string col)
    {
        var i = r.GetOrdinal(col);
        return r.IsDBNull(i) ? null : Convert.ToDecimal(r.GetValue(i), CultureInfo.InvariantCulture);
    }

    public static double Dbl(IDataRecord r, string col)
    {
        var i = r.GetOrdinal(col);
        return r.IsDBNull(i) ? 0 : Convert.ToDouble(r.GetValue(i), CultureInfo.InvariantCulture);
    }

    public static bool Flag(IDataRecord r, string col)
    {
        var i = r.GetOrdinal(col);
        if (r.IsDBNull(i)) return false;
        var v = r.GetValue(i);
        if (v is bool b) return b;
        if (v is int or long or short or byte) return Convert.ToInt32(v, CultureInfo.InvariantCulture) != 0;
        return Convert.ToBoolean(v, CultureInfo.InvariantCulture);
    }

    public static DateTime Dt(IDataRecord r, string col)
    {
        var i = r.GetOrdinal(col);
        if (r.IsDBNull(i)) return DateTime.MinValue;
        var v = r.GetValue(i);
        return v is DateTime d ? d : DateTime.Parse(Convert.ToString(v)!, CultureInfo.InvariantCulture);
    }
}
