namespace NOTEAPI.Model;
public class Note
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public int UserId { get; set; }
}