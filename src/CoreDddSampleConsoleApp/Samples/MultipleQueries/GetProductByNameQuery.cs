using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.MultipleQueries
{
    public class GetProductByNameQuery : IQuery
    {
        public string Name { get; set; }

    }
}