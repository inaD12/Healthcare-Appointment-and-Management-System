using Ratings.Application.Features.Ratings.Models;
using Ratings.Domain.Entities;

namespace Ratings.Application.Features.Ratings.Mappers;

public static class CommandMapper
{
    public static RatingCommandViewModel ToCommandViewModel(
        this Rating rating)
        => new(
            rating.Id);
}
  