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
        private ShipCargoPolicyItemArgs[] _shipCargoPolicyItemArgses;
        private TruckCargoPolicyItemArgs[] _truckCargoPolicyItemArgses;

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

        public PolicyBuilder WithShipCargoPolicyItems(params ShipCargoPolicyItemArgs[] shipCargoPolicyItemArgses)
        {
            _shipCargoPolicyItemArgses = shipCargoPolicyItemArgses;
            return this;
        }

        public PolicyBuilder WithTruckCargoPolicyItems(params TruckCargoPolicyItemArgs[] truckCargoPolicyItemArgses)
        {
            _truckCargoPolicyItemArgses = truckCargoPolicyItemArgses;
            return this;
        }

        public Policy Build()
        {
            var policy = new Policy(_policyHolder, _startDate, _endDate, _terms);
            if (_shipCargoPolicyItemArgses != null)
            {
                foreach (var shipCargoPolicyItemArgs in _shipCargoPolicyItemArgses)
                {
                    policy.AddShipCargoPolicyItem(shipCargoPolicyItemArgs);
                }
            }
            if (_truckCargoPolicyItemArgses != null)
            {
                foreach (var truckCargoPolicyItemArgs in _truckCargoPolicyItemArgses)
                {
                    policy.AddTruckCargoPolicyItem(truckCargoPolicyItemArgs);
                }
            }
            return policy;
        }
    }
}