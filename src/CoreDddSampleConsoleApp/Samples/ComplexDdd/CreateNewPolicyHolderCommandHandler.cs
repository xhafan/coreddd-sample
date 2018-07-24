using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Samples.ComplexDdd
{
    public class CreateNewPolicyHolderCommandHandler : BaseCommandHandler<CreateNewPolicyHolderCommand>
    {
        private readonly IRepository<PolicyHolder> _policyHolderRepository;

        public CreateNewPolicyHolderCommandHandler(IRepository<PolicyHolder> policyHolderRepository)
        {
            _policyHolderRepository = policyHolderRepository;
        }

        public override async Task ExecuteAsync(CreateNewPolicyHolderCommand command)
        {
            var newPolicyHolder = new PolicyHolder(command.Name);
            await _policyHolderRepository.SaveAsync(newPolicyHolder);

            RaiseCommandExecutedEvent(new CommandExecutedArgs { Args = newPolicyHolder.Id });
        }
    }
}