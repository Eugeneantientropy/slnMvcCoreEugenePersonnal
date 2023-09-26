using System;
using System.Linq;
using System.Text;

public class RandomPasswordGenerator
{
    private static readonly Random random = new Random();
    private static readonly string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789~!@#$";

    public static string GeneratePassword(int length)
    {
        if (length < 8)
        {
            throw new ArgumentException("密码长度必须至少为8个字符");
        }

        StringBuilder password = new StringBuilder(length);

        // 添加至少一个大写字母、一个小写字母、一个数字和一个特殊字符
        password.Append(GetRandomCharacter("ABCDEFGHIJKLMNOPQRSTUVWXYZ")); // 大写字母
        password.Append(GetRandomCharacter("abcdefghijklmnopqrstuvwxyz")); // 小写字母
        password.Append(GetRandomCharacter("0123456789")); // 数字
        password.Append(GetRandomCharacter("~!@#$")); // 特殊字符

        // 剩余的字符随机生成
        for (int i = 4; i < length; i++)
        {
            password.Append(GetRandomCharacter(characters));
        }

        // 将密码中的字符随机排序
        string shuffledPassword = new string(password.ToString().ToCharArray().OrderBy(x => random.Next()).ToArray());

        return shuffledPassword;
    }

    private static char GetRandomCharacter(string characterSet)
    {
        int index = random.Next(characterSet.Length);
        return characterSet[index];
    }
}
