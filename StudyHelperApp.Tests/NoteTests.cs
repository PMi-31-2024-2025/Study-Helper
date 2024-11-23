using StudyHelperApp.Models;
using System;
using Xunit;

public class NoteTests
{
    [Fact]
    public void Note_ShouldInitializeWithCorrectValues()
    {
        // Arrange: Create a new note with sample data
        int noteId = 1;
        int subjectId = 101;
        int userId = 1001;
        string text = "This is a test note.";
        DateTime createdAt = DateTime.Now;
        DateTime updatedAt = DateTime.Now.AddMinutes(5);

        var note = new Note
        {
            NoteId = noteId,
            SubjectId = subjectId,
            UserId = userId,
            Text = text,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        // Act: Verify that all properties are set correctly

        // Assert: Check the Note's properties
        Assert.Equal(noteId, note.NoteId);
        Assert.Equal(subjectId, note.SubjectId);
        Assert.Equal(userId, note.UserId);
        Assert.Equal(text, note.Text);
        Assert.Equal(createdAt, note.CreatedAt);
        Assert.Equal(updatedAt, note.UpdatedAt);
    }
}
