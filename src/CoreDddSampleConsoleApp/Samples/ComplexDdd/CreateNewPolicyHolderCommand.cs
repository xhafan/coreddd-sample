using CoreDdd.Commands;

namespace CoreDddSampleConsoleApp.Samples.ComplexDdd
{
    public class CreateNewPolicyHolderCommand : ICommand
    {
        public string Name { get; set; }
    }
}