namespace GameStore.Dtos
{
    public record GameSummaryDto(
            int Id,
            string Name,
            string Genre,
            decimal Price,
            DateOnly ReleaseDate
        );
}
