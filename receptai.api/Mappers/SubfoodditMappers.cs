using receptai.api.Dtos.Subfooddit;
using receptai.data;

namespace receptai.api.Mappers;

public static class SubfoodditMappers
{
    public static SubfoodditDto ToSubfoodditDto(this Subfooddit subfoodditModel)
    {
        return new SubfoodditDto
        {
            SubfoodditId = subfoodditModel.SubfoodditId,
            Title = subfoodditModel.Title,
            Description = subfoodditModel.Description,
            CreationDate = subfoodditModel.CreationDate,
        };
    }

    public static Subfooddit ToSubfoodditFromCreateDto(
        this CreateSubfoodditRequestDto subfoodditModel)
    {
        return new Subfooddit
        {
            Title = subfoodditModel.Title,
            Description = subfoodditModel.Description,
            CreationDate = DateTime.UtcNow,
        };
    }
}