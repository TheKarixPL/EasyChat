using System.Security.Cryptography;
using System.Text;

namespace EasyChat.Models;

public class Password
{
    private readonly byte[] _hashedPassword;
    private readonly byte[] _salt;

    public Password(string rawPassword)
    {
        if (string.IsNullOrWhiteSpace(rawPassword))
            throw new ArgumentException("password is empty");
        _salt = RandomNumberGenerator.GetBytes(16);
        _hashedPassword = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(rawPassword), _salt, 100000).GetBytes(20);
    }

    private Password(byte[] hashedPassword, byte[] salt)
    {
        _hashedPassword = hashedPassword;
        _salt = salt;
    }

    public static Password Parse(string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentException("hashed password is empty");
        var hashedPasswordParts = hashedPassword.Split(":");
        if (hashedPasswordParts.Length != 2)
            throw new FormatException("hashed password is in wrong format");
        return new Password(Convert.FromBase64String(hashedPasswordParts[0]),
            Convert.FromBase64String(hashedPasswordParts[1]));
    }

    public override string ToString()
        => Convert.ToBase64String(_hashedPassword) + ":" + Convert.ToBase64String(_salt);

    public override bool Equals(object obj)
    {
        if (obj is not Password)
            return false;
        var pwd = (Password)obj;
        return _hashedPassword.SequenceEqual(pwd._hashedPassword) && _salt.SequenceEqual(pwd._salt);
    }

    public bool EqualsRawPassword(string rawPassword)
        => Equals(new Password(rawPassword));

    public bool EqualsHashedPassword(string hashedPassword)
        => Equals(Parse(hashedPassword));

    public static bool operator==(Password obj1, object obj2)
    {
        if (ReferenceEquals(obj1, null))
            return ReferenceEquals(obj2, null);
        return obj1.Equals(obj2);
    }

    public static bool operator !=(Password obj1, object obj2)
    {
        return !(obj1 == obj2);
    }

    public override int GetHashCode()
        => unchecked(_hashedPassword.GetHashCode() + _salt.GetHashCode());
}