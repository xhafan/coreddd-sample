using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.MultipleQueries
{
    public class GetProductByDescriptionQuery : IQuery
    {
        public string Description { get; set; }

    }
}