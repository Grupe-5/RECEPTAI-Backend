namespace receptai.api;

public class UserInfoDto
{
    public int Id { get; set;}
    public string Username { get; set; }
    public DateTime JoinDate { get; set; }
    public int? ImageId { get; set; }
    public int KarmaScore { get; set; }
}
