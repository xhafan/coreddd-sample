using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleWebAppCommon;
using CoreDddSampleWebAppCommon.Domain;
using NUnit.Framework;
using Shouldly;

namespace CoreDddSample.PersistenceTests
{
    [TestFixture]
    public class when_persisting_ship
    {
        private NhibernateUnitOfWork _unitOfWork;
        private Ship _newShip;
        private Ship _persistedShip;

        [SetUp]
        public void Context()
        {
            _unitOfWork = new NhibernateUnitOfWork(new CoreDddSampleNhibernateConfigurator());
            _unitOfWork.BeginTransaction();

            _newShip = new Ship("ship name", tonnage: 23.4m);

            _unitOfWork.Save(_newShip); // save entity into DB -> send INSERT SQL statement into DB

            _unitOfWork.Clear(); // clear NHibernate session so the next SQL SELECT would not load cached entity version, but would query the database

            _persistedShip = _unitOfWork.Get<Ship>(_newShip.Id);
        }

        [Test]
        public void ship_can_be_retrieved()
        {
            _persistedShip.ShouldNotBeNull();
        }

        [Test]
        public void persisted_ship_id_matches_the_saved_ship_id()
        {
            _persistedShip.ShouldBe(_newShip);
        }

        [Test]
        public void ship_data_are_persisted_correctly()
        {
            _persistedShip.Name.ShouldBe("ship name");
            _persistedShip.Tonnage.ShouldBe(23.4m);
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork.Rollback();
        }
    }
}
