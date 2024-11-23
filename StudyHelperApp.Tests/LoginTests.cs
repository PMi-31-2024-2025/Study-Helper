using StudyHelperApp.Models;
using System;
using Xunit;

public class LoginTests
{
    [Fact]
    public void User_ShouldInitializeWithCorrectValues()
    {
        // Arrange: Set up the expected values for the user object
        int userId = 1;
        string username = "john_doe";
        string passwordHash = "hashed_password";
        string email = "john.doe@example.com";
        string role = "student";
        DateTime createdAt = DateTime.Now;

        // Act: Create a new user object with the specified properties
        var user = new User
        {
            UserId = userId,
            Username = username,
            PasswordHash = passwordHash,
            Email = email,
            Role = role,
            CreatedAt = createdAt
        };

        // Assert: Ensure that the properties were correctly assigned
        Assert.Equal(userId, user.UserId);
        Assert.Equal(username, user.Username);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Equal(email, user.Email);
        Assert.Equal(role, user.Role);
        Assert.Equal(createdAt, user.CreatedAt);
    }
}
