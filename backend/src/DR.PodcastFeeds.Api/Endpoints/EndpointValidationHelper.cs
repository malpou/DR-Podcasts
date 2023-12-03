namespace DR.PodcastFeeds.Api.Endpoints;

public static class EndpointValidationHelper
{
    public static IResult? ValidateCommonParameters(int? page, int? size, DateOnly? from, DateOnly? to, int? last)
    {
        if ((page.HasValue && !size.HasValue) || (!page.HasValue && size.HasValue))
            return Results.BadRequest("Both 'page' and 'size' parameters must be provided together.");

        if ((from.HasValue && !to.HasValue) || (!from.HasValue && to.HasValue))
            return Results.BadRequest("Both 'from' and 'to' parameters must be provided together.");

        if (size is < 1 or > 100)
            return Results.BadRequest("'size' must be between 1 and 100.");

        if (last is < 1 or > 100)
            return Results.BadRequest("'last' must be between 1 and 100.");

        if (page is < 1)
            return Results.BadRequest("'page' must be greater than 0.");

        if (!from.HasValue || !to.HasValue) return null; // Indicates no validation errors
        
        var fromDate = from.Value;
        var toDate = to.Value;
        return fromDate > toDate 
            ? Results.BadRequest("'from' must be before 'to'.") 
            : null; // Indicates no validation errors
    }
}
