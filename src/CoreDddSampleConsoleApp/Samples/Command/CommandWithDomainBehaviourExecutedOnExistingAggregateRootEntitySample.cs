﻿using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CoreDdd.Commands;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Register.Castle;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Register.Castle;

namespace CoreDddSampleConsoleApp.Samples.Command
{
    public class CommandWithDomainBehaviourExecutedOnExistingAggregateRootEntitySample
    {
        public async Task UpdateShipData()
        {
            var ioCContainer = new WindsorContainer();
            _RegisterComponents(ioCContainer);

            var unitOfWork = ioCContainer.Resolve<NhibernateUnitOfWork>();

            try
            {
                unitOfWork.BeginTransaction();

                try
                {
                    var shipController = ioCContainer.Resolve<ShipController>();
                    var generatedShipId = await shipController.CreateNewShipAsync(shipName: "lady", tonnage: 10m);
                    unitOfWork.Flush();


                    await shipController.UpdateShipData(generatedShipId, shipName: "star", tonnage: 20m);


                    Console.WriteLine("Update ship data command was executed.");

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
                Component.For<INhibernateConfigurator>() // register nhibernate configurator
                    .ImplementedBy<CoreDddSampleNhibernateConfigurator>()
                    .LifeStyle.Singleton,
                Component.For<ShipController>() // register ship controller to get command executor injected into the constructor
            );
        }
    }
}