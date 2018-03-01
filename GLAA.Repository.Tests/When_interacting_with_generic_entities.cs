using System;
using System.Linq;
using GLAA.Common;
using GLAA.Domain;
using GLAA.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace GLAA.Repository.Tests
{
    [TestClass]
    public class When_interacting_with_generic_entities
    {
        public TestContext TestContext { get; set; }

        private IDateTimeProvider dtp;
        private DbContextOptions<GLAAContext> options;

        [TestInitialize]
        public void Setup()
        {
            dtp = Substitute.For<IDateTimeProvider>();
            options = new DbContextOptionsBuilder<GLAAContext>().UseInMemoryDatabase(TestContext.TestName).Options;
        }

        [TestMethod]
        public void it_should_retrieve_a_non_deleted_entity_by_id()
        {
            const int id = 1;
            const string expectedAddress = "Line 1";

            using (var context = new GLAAContext(options))
            {
                context.Addresses.Add(new Address {Id = id, AddressLine1 = expectedAddress});
                context.SaveChanges();
            }

            using (var context = new GLAAContext(options))
            {
                var repo = new EntityFrameworkRepositoryBase(context, dtp);
                var result = repo.GetById<Address>(id);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(expectedAddress, result.AddressLine1);
            }
        }

        [TestMethod]
        public void it_should_not_retrieve_a_deleted_entity_by_id()
        {
            const int id = 1;
            const string expectedAddress = "Line 1";

            using (var context = new GLAAContext(options))
            {
                context.Addresses.Add(new Address {Id = id, AddressLine1 = expectedAddress, Deleted = true});
                context.SaveChanges();
            }

            using (var context = new GLAAContext(options))
            {
                var repo = new EntityFrameworkRepositoryBase(context, dtp);
                var result = repo.GetById<Address>(id);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void it_should_retrieve_a_deleted_entity_by_id_if_specified()
        {
            const int id = 1;
            const string expectedAddress = "Line 1";

            using (var context = new GLAAContext(options))
            {
                context.Addresses.Add(new Address {Id = id, AddressLine1 = expectedAddress, Deleted = true});
                context.SaveChanges();
            }

            using (var context = new GLAAContext(options))
            {
                var repo = new EntityFrameworkRepositoryBase(context, dtp);
                var result = repo.GetDeletedById<Address>(id);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(expectedAddress, result.AddressLine1);
                Assert.IsTrue(result.Deleted);
            }
        }

        [TestMethod]
        public void it_should_retrieve_a_non_deleted_entity_by_predicate()
        {
            const int id = 1;
            const string expectedAddress = "Line 1";

            using (var context = new GLAAContext(options))
            {
                context.Addresses.Add(new Address {Id = id, AddressLine1 = expectedAddress});
                context.SaveChanges();
            }

            using (var context = new GLAAContext(options))
            {
                var repo = new EntityFrameworkRepositoryBase(context, dtp);
                var result = repo.Find<Address>(a => a.AddressLine1.Equals(expectedAddress, StringComparison.InvariantCultureIgnoreCase));
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(expectedAddress, result.AddressLine1);
            }
        }

        [TestMethod]
        public void it_should_get_all_non_deleted_entities()
        {
            const int id = 1;
            const string expectedAddress = "Line 1";

            using (var context = new GLAAContext(options))
            {
                context.Addresses.Add(new Address { Id = id, AddressLine1 = expectedAddress });
                context.Addresses.Add(new Address { Id = 2, AddressLine1 = expectedAddress, Deleted = true});
                context.SaveChanges();
            }

            using (var context = new GLAAContext(options))
            {
                var repo = new EntityFrameworkRepositoryBase(context, dtp);
                var result = repo.GetAll<Address>().ToArray();
                Assert.AreEqual(1, result.Length);
                Assert.AreEqual(id, result.Single().Id);
                Assert.AreEqual(expectedAddress, result.Single().AddressLine1);
            }
        }

        [TestMethod]
        public void it_should_get_all_entities_including_deleted_if_specified()
        {
            const int id1 = 1;
            const int id2 = 2;
            const string expectedAddress = "Line 1";
            const string expectedDeletedAddress = "Deleted Line 1";

            using (var context = new GLAAContext(options))
            {
                context.Addresses.Add(new Address { Id = id1, AddressLine1 = expectedAddress });
                context.Addresses.Add(new Address { Id = id2, AddressLine1 = expectedDeletedAddress, Deleted = true});
                context.SaveChanges();
            }

            using (var context = new GLAAContext(options))
            {
                var repo = new EntityFrameworkRepositoryBase(context, dtp);
                var result = repo.GetAll<Address>(true).ToArray();
                Assert.AreEqual(2, result.Length);
                var result1 = result.Single(x => x.Id == id1);
                var result2 = result.Single(x => x.Id == id2);
                Assert.AreEqual(expectedAddress, result1.AddressLine1);
                Assert.AreEqual(expectedDeletedAddress, result2.AddressLine1);
                Assert.IsTrue(result2.Deleted);
            }
        }

        [TestMethod]
        public void it_should_mark_a_deletable_entry_as_deleted()
        {
            const int id = 1;
            const string expectedAddress = "Line 1";
            var now = new DateTime(2018, 1, 1);

            dtp.Now().Returns(now);

            using (var context = new GLAAContext(options))
            {
                context.Addresses.Add(new Address { Id = id, AddressLine1 = expectedAddress });
                context.SaveChanges();
            }

            using (var context = new GLAAContext(options))
            {
                var repo = new EntityFrameworkRepositoryBase(context, dtp);
                repo.Delete<Address>(id);
                var result = repo.GetDeletedById<Address>(id);
                Assert.IsTrue(result.Deleted);
                Assert.AreEqual(now, result.DateDeleted);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(expectedAddress, result.AddressLine1);
            }
        }

        [TestMethod]
        public void it_should_mark_items_marked_for_cascading_as_deleted()
        {
            const int id = 1;
            const string expectedRestraintOrder = "ro";
            const string expectedConviction = "conviction";
            const string expectedOffence = "offence";
            var now = new DateTime(2018, 1, 1);

            dtp.Now().Returns(now);

            using (var context = new GLAAContext(options))
            {
                context.NamedIndividuals.Add(new NamedIndividual
                {
                    Id = id,
                    RestraintOrders = new[]
                    {
                        new RestraintOrder
                        {
                            Id = id,
                            Description = expectedRestraintOrder
                        }
                    },
                    UnspentConvictions = new[]
                    {
                        new Conviction
                        {
                            Id = id,
                            Description = expectedConviction
                        }
                    },
                    OffencesAwaitingTrial = new[]
                    {
                        new OffenceAwaitingTrial
                        {
                            Id = id,
                            Description = expectedOffence
                        }
                    }
                });
                context.SaveChanges();
            }

            using (var context = new GLAAContext(options))
            {
                var repo = new EntityFrameworkRepositoryBase(context, dtp);
                repo.Delete<NamedIndividual>(id);

                var ni = repo.GetDeletedById<NamedIndividual>(id);
                Assert.IsTrue(ni.Deleted);
                Assert.AreEqual(now, ni.DateDeleted);

                var ro = repo.GetDeletedById<RestraintOrder>(id);
                Assert.IsTrue(ro.Deleted);
                Assert.AreEqual(now, ro.DateDeleted);
                Assert.AreEqual(expectedRestraintOrder, ro.Description);

                var c = repo.GetDeletedById<Conviction>(id);
                Assert.IsTrue(c.Deleted);
                Assert.AreEqual(now, c.DateDeleted);
                Assert.AreEqual(expectedConviction, c.Description);

                var oat = repo.GetDeletedById<OffenceAwaitingTrial>(id);
                Assert.IsTrue(oat.Deleted);
                Assert.AreEqual(now, oat.DateDeleted);
                Assert.AreEqual(expectedOffence, oat.Description);
            }
        }

        [TestMethod]
        public void it_should_create_a_new_instance_of_the_entity_and_attach_it_to_be_created()
        {
            using (var context = new GLAAContext(options))
            {
                var repo = new EntityFrameworkRepositoryBase(context, dtp);
                var result = repo.Create<Address>();
                var state = context.Entry(result).State;
                Assert.AreEqual(EntityState.Added, state);
            }
        }

        [TestMethod]
        public void it_should_save_any_changes_to_the_state_of_the_context()
        {
            using (var context = new GLAAContext(options))
            {
                var repo = new EntityFrameworkRepositoryBase(context, dtp);
                var result = repo.Create<Address>();
                var state = context.Entry(result).State;
                Assert.AreEqual(EntityState.Added, state);

                repo.Upsert(result);
                state = context.Entry(result).State;
                Assert.AreEqual(EntityState.Unchanged, state);
            }
        }
    }
}
