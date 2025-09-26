using System.Security.Cryptography;

namespace UrlShortener.Api;

public class ShorteningService
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private readonly int _shortCodeLength = 7;

    // This method generates a secure, random 7-character code.
    public Task<string> GenerateUniqueShortCode()
    {
        var codeChars = new char[_shortCodeLength];
        var randomBytes = RandomNumberGenerator.GetBytes(_shortCodeLength);
        
        for (int i = 0; i < _shortCodeLength; i++)
        {
            int index = randomBytes[i] % Alphabet.Length;
            codeChars[i] = Alphabet[index];
        }

        return Task.FromResult(new string(codeChars));
    }
}