using CoreDdd.Domain;

namespace CoreDddSampleConsoleApp.Domain
{
    public class PolicyHolder : Entity, IAggregateRoot
    {
        protected PolicyHolder() {}

        public PolicyHolder(string name)
        {
            Name = name;
        }

        public virtual string Name { get; protected set; }
    }
}