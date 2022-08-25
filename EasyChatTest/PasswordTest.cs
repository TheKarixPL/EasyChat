using EasyChat.Models;

namespace EasyChatTest;

public class PasswordTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void PasswordHash()
    {
        string GeneratePassword()
        {
            var random = new Random();
            return new string(Enumerable.Range(0, random.Next(32)).Select(it => (char)random.Next(32, char.MaxValue)).ToArray());
        }

        var random = new Random();
        var correctPassword = GeneratePassword();
        var incorrectPassword = Enumerable.Range(0, 100).Select(it => GeneratePassword()).Where(p => p != correctPassword).ToArray();
        var passwordObject = new Password(correctPassword);
        var hashedPassword = passwordObject.ToString();
        Assert.That(hashedPassword, Is.Not.Empty);
        Assert.That(passwordObject.EqualsRawPassword(correctPassword), Is.True);
        Assert.That(incorrectPassword.Any(p => passwordObject.EqualsRawPassword(p)), Is.False);
        Assert.That(Password.Parse(hashedPassword).EqualsRawPassword(correctPassword), Is.True);
        Assert.That(passwordObject.EqualsHashedPassword(hashedPassword), Is.True);
    }
}