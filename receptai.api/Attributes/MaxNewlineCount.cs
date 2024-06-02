using System.ComponentModel.DataAnnotations;

namespace receptai.api;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class MaxNewlineCount : ValidationAttribute
{
    private readonly int _maxNewlineCount;
    public MaxNewlineCount(int length)
    {
        _maxNewlineCount = length;
    }

    public override bool IsValid(object? value)
    {

        if (value == null) return false;
        string? str = value.ToString();
        if (str == null) {
            return false;
        }

        return str.Count(i => i.Equals('\n')) < _maxNewlineCount;
    }
}
