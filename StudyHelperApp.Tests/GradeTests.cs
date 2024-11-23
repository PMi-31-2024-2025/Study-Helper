using Xunit;
using StudyHelperApp.Models;

public class GradeTests
{
    [Fact]
    public void GradeValue_ShouldBeValid()
    {
        // Arrange
        var grade = new Grade { Value = 95 };

        // Act
        var result = grade.Value;

        // Assert
        Assert.Equal(95, result);
    }
}