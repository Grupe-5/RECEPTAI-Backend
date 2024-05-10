﻿using receptai.api.Dtos.Comment;
using receptai.data;

namespace receptai.api.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto
        {
            CommentId = commentModel.CommentId,
            RecipeId = commentModel.RecipeId,
            UserId = commentModel.UserId,
            CommentText = commentModel.CommentText,
            CommentDate = commentModel.CommentDate
        };
    }

    public static Comment ToCommentFromCreateDto(
        this CreateCommentRequestDto commentModel)
    {
        return new Comment
        {
            RecipeId = commentModel.RecipeId,
            UserId = commentModel.UserId,
            CommentText = commentModel.CommentText,
            CommentDate = commentModel.CommentDate
        };
    }
}