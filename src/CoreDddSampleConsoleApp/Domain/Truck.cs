using CoreDdd.Domain;

namespace CoreDddSampleConsoleApp.Domain
{
    public class Truck : Entity, IAggregateRoot
    {
        protected Truck() { }

        public Truck(string registrationPlate, string vin)
        {
            RegistrationPlate = registrationPlate;
            Vin = vin;
        }

        public virtual string RegistrationPlate { get; protected set; }
        public virtual string Vin { get; protected set; }
    }
}