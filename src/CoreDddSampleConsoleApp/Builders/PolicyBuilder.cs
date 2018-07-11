using System;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Builders
{
    public class PolicyBuilder
    {
        private PolicyHolder _policyHolder;
        private DateTime _startDate = new DateTime(2018, 7, 9, 0, 0, 0);
        private DateTime _endDate = new DateTime(2019, 7, 8, 23, 59, 59);
        private string _terms = "terms";
        private ShipPolicyItemArgs[] _shipPolicyItemArgses;
        private TruckPolicyItemArgs[] _truckPolicyItemArgses;

        public PolicyBuilder WithPolicyHolder(PolicyHolder policyHolder)
        {
            _policyHolder = policyHolder;
            return this;
        }

        public PolicyBuilder WithStartDate(DateTime startDate)
        {
            _startDate = startDate;
            return this;
        }

        public PolicyBuilder WithEndDate(DateTime endDate)
        {
            _endDate = endDate;
            return this;
        }

        public PolicyBuilder WithTerms(string terms)
        {
            _terms = terms;
            return this;
        }

        public PolicyBuilder WithShipPolicyItems(params ShipPolicyItemArgs[] shipPolicyItemArgses)
        {
            _shipPolicyItemArgses = shipPolicyItemArgses;
            return this;
        }

        public PolicyBuilder WithTruckPolicyItems(params TruckPolicyItemArgs[] truckPolicyItemArgses)
        {
            _truckPolicyItemArgses = truckPolicyItemArgses;
            return this;
        }

        public Policy Build()
        {
            var policy = new Policy(_policyHolder, _startDate, _endDate, _terms);
            if (_shipPolicyItemArgses != null)
            {
                foreach (var shipPolicyItemArgs in _shipPolicyItemArgses)
                {
                    policy.AddShipPolicyItem(shipPolicyItemArgs);
                }
            }
            if (_truckPolicyItemArgses != null)
            {
                foreach (var truckPolicyItemArgs in _truckPolicyItemArgses)
                {
                    policy.AddTruckPolicyItem(truckPolicyItemArgs);
                }
            }
            return policy;
        }
    }
}