using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Queries;
using CoreDddSampleConsoleApp.Dtos;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class PolicyService
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public PolicyService(
            ICommandExecutor commandExecutor,
            IQueryExecutor queryExecutor
            )
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        public async Task<int> CreateNewPolicyAsync(
            int policyHolderId,
            DateTime startDate,
            DateTime endDate, 
            string terms
            )
        {
            var createNewPolicyCommand = new CreateNewPolicyCommand
            {
                PolicyHolderId = policyHolderId,
                StartDate = startDate,
                EndDate = endDate,
                Terms = terms
            };
            var newPolicyId = 0;
            _commandExecutor.CommandExecuted += args => newPolicyId = (int) args.Args;

            await _commandExecutor.ExecuteAsync(createNewPolicyCommand);

            return newPolicyId;
        }

        public async Task AddShipToPolicyAsync(
            int policyId,
            int shipId,
            decimal insuredTonnage,
            decimal ratePerTonnage
        )
        {
            await _commandExecutor.ExecuteAsync(new AddShipToPolicyCommand
            {
                PolicyId = policyId,
                ShipId = shipId,
                InsuredTonnage = insuredTonnage,
                RatePerTonnage = ratePerTonnage
            });
        }

        public async Task<(IEnumerable<PolicyDto> policiesByTerm, IEnumerable<ShipCargoPolicyItemDto> shipCargoPolicyItemsByShipName)> 
            GetResultFromMultipleQueries(
                string policyTerms,
                string shipName
            )
        {
            var getPoliciesByTermsQuery = new GetPoliciesByTermsQuery { Terms = policyTerms };
            var policyDtos = _queryExecutor.Execute<GetPoliciesByTermsQuery, PolicyDto>(getPoliciesByTermsQuery);

            // At this point, GetPoliciesByTermsQuery have not been sent to the DB. Only when the result (policyDtos) is enumerated.

            var getPolicyItemsByShipNameQuery = new GetShipCargoPolicyItemsByShipNameQuery { ShipName = shipName };
            var shipCargoPolicyItemDtos = await _queryExecutor.ExecuteAsync<GetShipCargoPolicyItemsByShipNameQuery, ShipCargoPolicyItemDto>(getPolicyItemsByShipNameQuery);

            // GetShipCargoPolicyItemsByShipNameQuery was awaited, and that sent both queries to the DB in one go, saving one DB round trip.

            return (policyDtos, shipCargoPolicyItemDtos);
        }      
    }
}