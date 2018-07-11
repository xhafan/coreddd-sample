using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.MultipleQueries
{
    public class GetPoliciesByTermsQuery : IQuery
    {
        public string Terms { get; set; }

    }
}