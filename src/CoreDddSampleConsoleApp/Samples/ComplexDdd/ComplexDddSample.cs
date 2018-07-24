using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CoreDdd.Commands;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Register.Castle;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;
using CoreDdd.Register.Castle;
using CoreDddSampleConsoleApp.Builders;
using CoreDddSampleConsoleApp.Domain;
using CoreDddSampleConsoleApp.Samples.Command;

namespace CoreDddSampleConsoleApp.Samples.ComplexDdd
{
    public class ComplexDddSample
    {
        public async Task BuildAndPersistPolicyEntitiesAndExecuteDomainBehaviourOnThemAndExecuteQueriesOverThem(
            CoreDddSampleNhibernateConfigurator nhibernateConfigurator
            )
        {
            var ioCContainer = new WindsorContainer();
            _RegisterComponents(ioCContainer);

            var unitOfWork = ioCContainer.Resolve<NhibernateUnitOfWork>();

            try
            {
                unitOfWork.BeginTransaction();

                try
                {
                    var shipService = ioCContainer.Resolve<ShipService>();
                    var policyHolderService = ioCContainer.Resolve<PolicyHolderService>();
                    var policyService = ioCContainer.Resolve<PolicyService>();

                    var shipOneId = await shipService.CreateNewShipAsync("lady star", 10m);
                    var shipTwoId = await shipService.CreateNewShipAsync("golden sea", 20m);
                    var newPolicyHolderId = await policyHolderService.CreateNewPolicyHolderAsync("Policy holder name");

                    var today = DateTime.Today;
                    var todayPlusOneYear = today.AddYears(1);

                    var policyOneId = await policyService.CreateNewPolicyAsync(
                        policyHolderId: newPolicyHolderId,
                        startDate: today,
                        endDate: todayPlusOneYear,
                        terms: "policy one terms"
                    );
                    await policyService.AddShipToPolicyAsync(policyOneId, shipOneId, insuredTonnage: 8m, ratePerTonnage: 5m);
                    await policyService.AddShipToPolicyAsync(policyOneId, shipTwoId, insuredTonnage: 18m, ratePerTonnage: 6m);

                    var policyTwoId = await policyService.CreateNewPolicyAsync(
                        policyHolderId: newPolicyHolderId,
                        startDate: today,
                        endDate: todayPlusOneYear,
                        terms: "policy two terms"
                    );

                    unitOfWork.Flush();

                    var (policyDtos, shipPolicyItemDtos) = await policyService.GetResultFromMultipleQueries(policyTerms: "one", shipName: "golden");

                    Console.WriteLine($"Policies by terms query was executed. Number of policy dtos queried: {policyDtos.Count()}");
                    Console.WriteLine($"Ship policy items by ship name query was executed. Number of ship policy item dtos queried: {shipPolicyItemDtos.Count()}");

                    await unitOfWork.CommitAsync();
                }
                catch
                {
                    await unitOfWork.RollbackAsync();
                    throw;
                }
            }
            finally
            {
                ioCContainer.Release(unitOfWork);
            }

            ioCContainer.Dispose();
        }

        private async Task _BuildAndSaveTwoPolicies(NhibernateUnitOfWork unitOfWork)
        {
            var policyHolderRepository = new NhibernateRepository<PolicyHolder>(unitOfWork);
            var policyShipRepository = new NhibernateRepository<Ship>(unitOfWork);
            var policyRepository = new NhibernateRepository<Policy>(unitOfWork);


            var policyHolder = new PolicyHolderBuilder().Build();
            await policyHolderRepository.SaveAsync(policyHolder);


            var ship = new ShipBuilder().WithName("some ship name").Build();
            await policyShipRepository.SaveAsync(ship);


            var policyOne = new PolicyBuilder()
                .WithPolicyHolder(policyHolder)
                .WithTerms("policy one terms")
                .Build();
            await policyRepository.SaveAsync(policyOne);


            var policyTwoWithShip = new PolicyBuilder()
                .WithPolicyHolder(policyHolder)
                .WithShipPolicyItems(new ShipPolicyItemArgs
                {
                    Ship = ship,
                    InsuredTonnage = 7m,
                    RatePerTonnage = 5m
                })
                .Build();
            await policyRepository.SaveAsync(policyTwoWithShip);


            unitOfWork.Flush();
        }

        private void _RegisterComponents(WindsorContainer ioCContainer)
        {
            NhibernateInstaller.SetUnitOfWorkLifeStyle(x => x.PerThread);

            ioCContainer.Install(
                FromAssembly.Containing<QueryAndCommandExecutorInstaller>(),
                FromAssembly.Containing<NhibernateInstaller>()
            );

            ioCContainer.Register(
                Classes
                    .FromAssemblyContaining<CreateNewShipCommand>() // register all command handlers in this assembly
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient()),            
                Classes
                    .FromAssemblyContaining<GetPoliciesByTermsQuery>() // register all query handlers in this assembly
                    .BasedOn(typeof(IQueryHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient()),
                Component.For<INhibernateConfigurator>() // register nhibernate configurator
                    .ImplementedBy<CoreDddSampleNhibernateConfigurator>()
                    .LifeStyle.Singleton,
                Component.For<ShipService>(), // register ship service to get query executor and command executor injected into the constructor
                Component.For<PolicyHolderService>(), // register policy holder service to get query executor and command executor injected into the constructor
                Component.For<PolicyService>() // register policy service to get query executor and command executor injected into the constructor
            );
        }
    }
}