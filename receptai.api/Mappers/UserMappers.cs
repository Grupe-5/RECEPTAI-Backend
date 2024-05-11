using receptai.data;

namespace receptai.api;

public static class UserMappers
{
    public static UserInfoDto ToUserInfoDto(
        this User userModel)
    {
        return new UserInfoDto
        {
            Id = userModel.Id,
            Username = userModel.UserName!,
            JoinDate = userModel.JoinDate,
            ImageId = userModel.ImgId,
            /* This prob needs to be dynamically accessed */
            KarmaScore = userModel.KarmaScore,
        };
    }
}
