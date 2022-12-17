using System.Security.Cryptography;
using NSubstitute.ReceivedExtensions;

namespace TestProject.Infrastructures;

public static class NSubstituteExtensions
{
    private static Random Random = new Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}