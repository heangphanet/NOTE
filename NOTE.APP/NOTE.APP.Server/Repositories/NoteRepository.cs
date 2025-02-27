using Dapper;
using Microsoft.Data.SqlClient;
using NOTE.APP.Server.Repositories;
using NOTEAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NoteRepository : INoteRepository
{
    private readonly string _connectionString;

    // Constructor now takes a connection string from DI
    public NoteRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Note>> GetNotesByUserIdAsync(int userId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Note>(
            "SELECT * FROM Notes WHERE UserId = @UserId",
            new { UserId = userId }
        );
    }

    public async Task<Note> GetNoteByIdAsync(int userId, int noteId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<Note>(
            "SELECT * FROM Notes WHERE Id = @NoteId AND UserId = @UserId",
            new { NoteId = noteId, UserId = userId }
        );
    }

    public async Task<int> CreateNoteAsync(Note note)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "INSERT INTO Notes (Title, Content, UserId, CreatedAt, UpdatedAt) OUTPUT INSERTED.Id VALUES (@Title, @Content, @UserId, @CreatedAt, @UpdatedAt)";
        return await connection.ExecuteScalarAsync<int>(sql, note);
    }

    public async Task<bool> UpdateNoteAsync(Note note)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "UPDATE Notes SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt WHERE Id = @Id AND UserId = @UserId";
        var affectedRows = await connection.ExecuteAsync(sql, note);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteNoteAsync(int userId, int noteId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "DELETE FROM Notes WHERE Id = @NoteId AND UserId = @UserId";
        var affectedRows = await connection.ExecuteAsync(sql, new { NoteId = noteId, UserId = userId });
        return affectedRows > 0;
    }
}
