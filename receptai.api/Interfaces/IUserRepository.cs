namespace receptai.api.Interfaces;

public interface IUserRepository
{
    Task<int> RecalculateKarmaScoreAsync(int userId);
}
