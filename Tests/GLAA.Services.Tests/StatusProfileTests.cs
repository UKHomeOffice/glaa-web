using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Services.Automapper;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests
{
    [TestClass]
    public class StatusProfileTests
    {
        private IMapper mapper;

        [TestInitialize]
        public void Setup()
        {
            var config = new AutoMapperConfig().Configure();

            this.mapper = config.CreateMapper();
        }

        [TestMethod]
        public void it_should_map_the_status_change_entity_to_the_status_view_model()
        {
            var input = new LicenceStatusChange
            {
                Id = 1,
                DateCreated = DateTime.MinValue,
                Reason = new StatusReason
                {
                    Description = "SelectedReason"
                },
                Status = new LicenceStatus
                {
                    Id = 100,
                    ExternalDescription = "External",
                    ActiveCheckDescription = "Active",
                    InternalDescription = "Internal",
                    InternalStatus = "Status",
                    ShowInPublicRegister = true,
                    RequireNonCompliantStandards = true,
                    NextStatuses = new List<LicenceStatusNextStatus>
                    {
                        new LicenceStatusNextStatus {
                            NextStatus = new LicenceStatus
                            {
                                Id = 110,
                                InternalDescription = "Internal2",
                                InternalStatus = "Status2",
                                RequireNonCompliantStandards = true,
                                StatusReasons = new List<StatusReason>
                                {
                                    new StatusReason
                                    {
                                        Description = "Description",
                                        Id = 1
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var result = this.mapper.Map<LicenceStatusViewModel>(input);

            Assert.AreEqual(100, result.Id);
            Assert.AreEqual("External", result.ExternalDescription);
            Assert.AreEqual("Active", result.ActiveCheckDescription);
            Assert.AreEqual("Internal", result.InternalDescription);
            Assert.AreEqual("Status", result.InternalStatus);
            Assert.AreEqual(true, result.ShowInPublicRegister);
            Assert.AreEqual(true, result.RequireNonCompliantStandards);
            Assert.AreEqual("SelectedReason", result.SelectedReason);
            Assert.AreEqual(DateTime.MinValue, result.DateCreated);
            Assert.AreEqual(110, result.NextStatuses.Single().Id);
            Assert.AreEqual("Internal2", result.NextStatuses.Single().InternalDescription);
            Assert.AreEqual("Status2", result.NextStatuses.Single().InternalStatus);
            Assert.AreEqual(true, result.NextStatuses.Single().RequireNonCompliantStandards);
            Assert.AreEqual("Description", result.NextStatuses.Single().Reasons.Single().Description);
            Assert.AreEqual(1, result.NextStatuses.Single().Reasons.Single().Id);
        }

        [TestMethod]
        public void it_should_map_the_status_entity_to_the_status_view_model()
        {
            var input = new LicenceStatus
            {
                Id = 100,
                ExternalDescription = "External",
                ActiveCheckDescription = "Active",
                InternalDescription = "Internal",
                InternalStatus = "Status",
                ShowInPublicRegister = true,
                RequireNonCompliantStandards = true,
                NextStatuses = new List<LicenceStatusNextStatus>
                {
                    new LicenceStatusNextStatus
                    {
                        NextStatus = new LicenceStatus
                        {
                            Id = 110,
                            InternalDescription = "Internal2",
                            InternalStatus = "Status2",
                            RequireNonCompliantStandards = true,
                            StatusReasons = new List<StatusReason>
                            {
                                new StatusReason
                                {
                                    Description = "Description",
                                    Id = 1
                                }
                            }
                        }
                    }
                }
            };

            var result = this.mapper.Map<LicenceStatusViewModel>(input);

            Assert.AreEqual(100, result.Id);
            Assert.AreEqual("External", result.ExternalDescription);
            Assert.AreEqual("Active", result.ActiveCheckDescription);
            Assert.AreEqual("Internal", result.InternalDescription);
            Assert.AreEqual("Status", result.InternalStatus);
            Assert.AreEqual(true, result.ShowInPublicRegister);
            Assert.AreEqual(true, result.RequireNonCompliantStandards);
            Assert.IsNull(result.DateCreated);
            Assert.IsNull(result.NextStatuses);
            Assert.IsNull(result.NonCompliantStandards);
            Assert.IsNull(result.SelectedReason);
        }
    }
}
