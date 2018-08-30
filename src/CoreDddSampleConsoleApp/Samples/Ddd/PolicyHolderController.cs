using System.Threading.Tasks;
using CoreDdd.Commands;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class PolicyHolderController
    {
        private readonly ICommandExecutor _commandExecutor;

        public PolicyHolderController(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        public async Task<int> CreateNewPolicyHolderAsync(string name)
        {
            var createNewPolicyHolderCommand = new CreateNewPolicyHolderCommand
            {
                Name = name
            };
            var newPolicyHolderId = 0;
            _commandExecutor.CommandExecuted += args => newPolicyHolderId = (int) args.Args;

            await _commandExecutor.ExecuteAsync(createNewPolicyHolderCommand);

            return newPolicyHolderId;
        }
    }
}