using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class CreateNewPolicyCommandHandler : BaseCommandHandler<CreateNewPolicyCommand>
    {
        private readonly IRepository<PolicyHolder> _policyHolderRepository;
        private readonly IRepository<Policy> _policyRepository;

        public CreateNewPolicyCommandHandler(
            IRepository<PolicyHolder> policyHolderRepository,
            IRepository<Policy> policyRepository
            )
        {
            _policyRepository = policyRepository;
            _policyHolderRepository = policyHolderRepository;
        }

        public override async Task ExecuteAsync(CreateNewPolicyCommand command)
        {
            var policyHolder = await _policyHolderRepository.GetAsync(command.PolicyHolderId);
            var newPolicy = new Policy(policyHolder, command.StartDate, command.EndDate, command.Terms);
            await _policyRepository.SaveAsync(newPolicy);

            RaiseCommandExecutedEvent(new CommandExecutedArgs { Args = newPolicy.Id });
        }
    }
}