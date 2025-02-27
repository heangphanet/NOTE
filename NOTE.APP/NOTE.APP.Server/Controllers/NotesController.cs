using Microsoft.AspNetCore.Mvc;
using NOTEAPI.Model;
using NOTE.APP.Server.Repositories;

[Route("api/notes")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly NoteRepository _repository;

    public NotesController(NoteRepository repository)
    {
        _repository = repository;
    }

    // Create a new note for the user
    [HttpPost("{userId}")]
    public async Task<IActionResult> CreateNote(int userId, [FromBody] Note noted)
    {
        if (noted == null)
        {
            return BadRequest("Note data is required.");
        }

        // Set the UserId
        noted.UserId = userId;
        noted.CreatedAt = DateTime.Now;
        noted.UpdatedAt = null;

        var createdNote = await _repository.CreateNoteAsync(noted);

        if (createdNote == null)
        {
            return NotFound("Unable to create note.");
        }

        return Ok(createdNote);
    }

    // Get all notes for a user
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetNotes(int userId)
    {
        var notes = await _repository.GetNotesByUserIdAsync(userId);

        if (notes == null || !notes.Any())
        {
            return NotFound("No notes found for this user.");
        }

        return Ok(notes);
    }

    [HttpGet("{userId}/{id}")]
    public async Task<IActionResult> GetNoteByIdAsync(int userId, int id)
    {
        var note = await _repository.GetNoteByIdAsync(userId, id);

        if (note == null)
        {
            return NotFound("Note not found.");
        }

        return Ok(note);
    }

    [HttpPut("{userId}/{id}")]
    public async Task<IActionResult> UpdateNote(int userId, int id, [FromBody] Note note)
    {
        if (note.UserId != userId || note.Id != id)
        {
            return BadRequest("Invalid note or user.");
        }

        var existingNote = await _repository.GetNoteByIdAsync(userId, id);
        if (existingNote == null)
        {
            return NotFound("Note not found.");
        }

        note.UpdatedAt = DateTime.Now.ToString("MMM dd yyyy hh:mm tt");
        var updatedNote = await _repository.UpdateNoteAsync(note);

        if (updatedNote == null)
        {
            return NotFound("Failed to update the note.");
        }

        return Ok(updatedNote);
    }

    // Delete a note for a user
    [HttpDelete("{userId}/{id}")]
    public async Task<IActionResult> DeleteNote(int userId, int id)
    {
        var result = await _repository.DeleteNoteAsync(userId, id);
        if (result == null)
        {
            return NotFound("Note not found.");
        }

        return Ok("Note Deleted");
    }
}
