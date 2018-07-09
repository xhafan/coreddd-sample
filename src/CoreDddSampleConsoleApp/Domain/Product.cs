using CoreDdd.Domain;

namespace CoreDddSampleConsoleApp.Domain
{
    public class Product : Entity, IAggregateRoot
    {
        protected Product() {}

        public Product(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual decimal Price { get; protected set; }
    }
}