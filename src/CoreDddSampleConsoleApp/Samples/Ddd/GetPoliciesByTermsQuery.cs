using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class GetPoliciesByTermsQuery : IQuery
    {
        public string Terms { get; set; }

    }
}