using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.ComplexDdd
{
    public class GetPoliciesByTermsQuery : IQuery
    {
        public string Terms { get; set; }

    }
}