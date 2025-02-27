using NOTEAPI.Model;
namespace NOTE.APP.Server.Repositories;
public interface INoteRepository
{
    Task<IEnumerable<Note>> GetNotesByUserIdAsync(int userId);
    Task<Note> GetNoteByIdAsync(int noteId,int userid);
    Task<int> CreateNoteAsync(Note note);
    Task<bool> UpdateNoteAsync(Note note);
    Task<bool> DeleteNoteAsync(int Userid, int noteId);
}