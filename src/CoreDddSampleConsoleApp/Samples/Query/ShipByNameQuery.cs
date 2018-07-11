using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class ShipByNameQuery : IQuery
    {
        public string ShipName { get; set; }
    }
}