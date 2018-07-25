using CoreDdd.Commands;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class CreateNewPolicyHolderCommand : ICommand
    {
        public string Name { get; set; }
    }
}