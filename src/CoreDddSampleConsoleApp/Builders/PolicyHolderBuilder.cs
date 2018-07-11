using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Builders
{
    public class PolicyHolderBuilder
    {
        private string _name = "policy holder name";

        public PolicyHolderBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PolicyHolder Build()
        {
            return new PolicyHolder(_name);
        }
    }
}