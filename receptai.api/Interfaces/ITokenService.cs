using receptai.data;

namespace receptai.api;

public interface ITokenService
{
    string CreateToken(User user);
}
