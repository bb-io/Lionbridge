using Apps.Lionbridge.Models.Requests.Job;

namespace Apps.Lionbridge.Extensions
{
    internal static class JobsFilterBuilder
    {
        public static string? Build(SearchJobsRequest input)
        {
            var filters = new List<string>();

            if (!string.IsNullOrWhiteSpace(input.StatusCode))
                filters.Add($"statusCode eq '{input.StatusCode}'");

            if (input.ExcludeCompleted)
                filters.Add("statusCode ne 'COMPLETED'");

            if (!string.IsNullOrWhiteSpace(input.ProviderId))
                filters.Add($"providerId eq '{input.ProviderId}'");

            if (input.DueBefore.HasValue)
                filters.Add($"dueDate lt '{input.DueBefore.Value:yyyy-MM-ddTHH:mm:ss}'");

            if (input.SubmittedAfter.HasValue)
                filters.Add($"submittedDate gt '{input.SubmittedAfter.Value:yyyy-MM-dd}'");

            if (input.SubmittedBefore.HasValue)
                filters.Add($"submittedDate lt '{input.SubmittedBefore.Value:yyyy-MM-dd}'");

            if (input.CreatedAfter.HasValue)
            {
                filters.Add(
                    $"createdDate gt '{input.CreatedAfter.Value:yyyy-MM-ddTHH:mm:ss}'");
            }

            if (input.CreatedBefore.HasValue)
            {
                filters.Add(
                    $"createdDate lt '{input.CreatedBefore.Value:yyyy-MM-ddTHH:mm:ss}'");
            }

            return filters.Any()
                ? string.Join(" and ", filters)
                : null;
        }
    }
}
