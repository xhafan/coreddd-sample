using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CoreDdd.Commands;
using CoreDdd.Domain.Events;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Register.Castle;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;
using CoreDdd.Register.Castle;
using CoreDddSampleConsoleApp.Domain;
using CoreDddSampleConsoleApp.Samples.Command;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class DddSample
    {
        public async Task BuildAndPersistPolicyEntitiesAndExecuteDomainBehaviourOnThemAndExecuteQueriesOverThem(
            CoreDddSampleNhibernateConfigurator nhibernateConfigurator,
            bool isDelayedDomainEventHandlingEnabled
            )
        {
            var ioCContainer = new WindsorContainer();
            _RegisterComponents(ioCContainer);

            _InitializeDomainEvents(ioCContainer, isDelayedDomainEventHandlingEnabled);

            _RegisterDomainEventHandlers(ioCContainer);

            var unitOfWork = ioCContainer.Resolve<NhibernateUnitOfWork>();

            try
            {
                unitOfWork.BeginTransaction();

                try
                {
                    var shipController = ioCContainer.Resolve<ShipController>();
                    var policyHolderController = ioCContainer.Resolve<PolicyHolderController>();
                    var policyController = ioCContainer.Resolve<PolicyController>();

                    await _CreateEntitiesUsingCommands(shipController, policyHolderController, policyController, unitOfWork);
                    await _QueryOverCreatedEntities(policyController);

                    await unitOfWork.CommitAsync();
                }
                catch
                {
                    await unitOfWork.RollbackAsync();
                    throw;
                }

                if (isDelayedDomainEventHandlingEnabled)
                {
                    DomainEvents.RaiseDelayedEvents();
                }
            }
            finally
            {
                ioCContainer.Release(unitOfWork);
            }

            ioCContainer.Dispose();
        }

        private void _InitializeDomainEvents(WindsorContainer ioCContainer, bool isDelayedDomainEventHandlingEnabled)
        {
            var domainEventHandlerFactory = ioCContainer.Resolve<IDomainEventHandlerFactory>();
            DomainEvents.Initialize(domainEventHandlerFactory, isDelayedDomainEventHandlingEnabled: isDelayedDomainEventHandlingEnabled);

            if (isDelayedDomainEventHandlingEnabled)
            {
                DomainEvents.ResetDelayedEventsStorage();
            }
        }

        private void _RegisterDomainEventHandlers(WindsorContainer ioCContainer)
        {
            ioCContainer.Register(
                Classes
                    .FromAssemblyContaining<ShipCargoPolicyItemAddedDomainEvent>()
                    .BasedOn(typeof(IDomainEventHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient()));
        }

        private async Task _CreateEntitiesUsingCommands(
            ShipController shipController, 
            PolicyHolderController policyHolderController,
            PolicyController policyController, 
            NhibernateUnitOfWork nhibernateUnitOfWork
            )
        {
            var shipOneId = await shipController.CreateNewShipAsync("lady star", 10m);
            var shipTwoId = await shipController.CreateNewShipAsync("golden sea", 20m);
            var newPolicyHolderId = await policyHolderController.CreateNewPolicyHolderAsync("Policy holder name");

            var today = DateTime.Today;
            var todayPlusOneYear = today.AddYears(1);

            var policyOneId = await policyController.CreateNewPolicyAsync(
                policyHolderId: newPolicyHolderId,
                startDate: today,
                endDate: todayPlusOneYear,
                terms: "policy one terms"
            );
            await policyController.AddShipToPolicyAsync(policyOneId, shipOneId, insuredTonnage: 8m, ratePerTonnage: 5m);
            await policyController.AddShipToPolicyAsync(policyOneId, shipTwoId, insuredTonnage: 18m, ratePerTonnage: 6m);

            var policyTwoId = await policyController.CreateNewPolicyAsync(
                policyHolderId: newPolicyHolderId,
                startDate: today,
                endDate: todayPlusOneYear,
                terms: "policy two terms"
            );

            nhibernateUnitOfWork.Flush();
        }

        private async Task _QueryOverCreatedEntities(PolicyController policyController)
        {
            var (policyDtos, shipCargoPolicyItemDtos) =
                await policyController.GetResultFromMultipleQueries(policyTerms: "one", shipName: "golden");

            Console.WriteLine($"Policies by terms query was executed. Number of policy dtos queried: {policyDtos.Count()}");
            Console.WriteLine(
                $"Ship cargo policy items by ship name query was executed. Number of ship cargo policy item dtos queried: " +
                $"{shipCargoPolicyItemDtos.Count()}");
        }

        private void _RegisterComponents(WindsorContainer ioCContainer)
        {
            CoreDddNhibernateInstaller.SetUnitOfWorkLifeStyle(x => x.PerThread);

            ioCContainer.Install(
                FromAssembly.Containing<CoreDddInstaller>(),
                FromAssembly.Containing<CoreDddNhibernateInstaller>()
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
                Component.For<ShipController>(), // register ship controller to get query executor and command executor injected into the constructor
                Component.For<PolicyHolderController>(), // register policy holder controller to get query executor and command executor injected into the constructor
                Component.For<PolicyController>() // register policy controller to get query executor and command executor injected into the constructor
            );
        }
    }
}