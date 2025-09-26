using UrlShortener.Api;

namespace UrlShortener.Tests;

public class ShorteningServiceTests
{
    private readonly ShorteningService _service = new();

    // This is our first test case.
    [Fact]
    public async Task GenerateUniqueShortCode_ShouldReturn_CorrectLengthString()
    {
        // Arrange: Define what we expect.
        int expectedLength = 7;

        // Act: Run the method we are testing.
        var shortCode = await _service.GenerateUniqueShortCode();

        // Assert: Verify the result is correct.
        Assert.NotNull(shortCode);
        Assert.Equal(expectedLength, shortCode.Length);
    }

    // This is our second test case.
    [Fact]
    public async Task GenerateUniqueShortCode_ShouldReturn_StringWithValidChars()
    {
        // Arrange
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        // Act
        var shortCode = await _service.GenerateUniqueShortCode();

        // Assert: Check that every character in the result is a valid one.
        Assert.All(shortCode, c => Assert.Contains(c, validChars));
    }
}