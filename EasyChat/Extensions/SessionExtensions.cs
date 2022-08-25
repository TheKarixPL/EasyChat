namespace EasyChat.Extensions;

public static class SessionExtensions
{
    public static long? GetInt64(this ISession session, string key)
    {
        var data = session.Get(key);
        if (data == null || data.Length < 8)
            return null;
        var convertedData = new long[8];
        for (int i = 0; i < 8; i++)
            convertedData[i] = data[i];
        return convertedData[0] << 56 | convertedData[1] << 48 | convertedData[2] << 40 | convertedData[3] << 32
               | convertedData[4] << 24 | convertedData[5] << 16 | convertedData[6] << 8 | convertedData[7];
    }

    public static void SetInt64(this ISession session, string key, long value)
    {
        var bytes = new byte[]
        {
            (byte)(value >> 56),
            (byte)(0xFF & (value >> 48)),
            (byte)(0xFF & (value >> 40)),
            (byte)(0xFF & (value >> 32)),
            (byte)(0xFF & (value >> 24)),
            (byte)(0xFF & (value >> 16)),
            (byte)(0xFF & (value >> 8)),
            (byte)(0xFF & value)
        };
        session.Set(key, bytes);
    }
}