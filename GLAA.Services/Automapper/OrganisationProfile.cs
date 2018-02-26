using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.Services.Automapper
{
    public class OrganisationProfile : Profile
    {
        public OrganisationProfile()
        {
            CreateMap<Licence, OrganisationViewModel>()
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.IsShellfish, opt => opt.MapFrom(y => y.IsShellfish))
                .ForMember(x => x.TransportingWorkersViewModel, opt => opt.ResolveUsing(TransportingWorkersResolver))
                .ForMember(x => x.AccommodatingWorkersViewModel, opt => opt.ResolveUsing(AccomodatingWorkersResolver))
                .ForMember(x => x.SourcingWorkersViewModel, opt => opt.ResolveUsing(SourcingWorkersResolver))
                .ForMember(x => x.WorkerSupplyMethodViewModel, opt => opt.ResolveUsing(SupplyWorkersResolver))
                .ForMember(x => x.WorkerContractViewModel, opt => opt.ResolveUsing(WorkerContractResolver))
                .ForMember(x => x.BannedFromTradingViewModel, opt => opt.ResolveUsing(BannedFromTradingResolver))
                .ForMember(x => x.SubcontractorViewModel, opt => opt.ResolveUsing(SubcontracterResolver))
                .ForMember(x => x.OutsideSectorsViewModel, opt => opt.ResolveUsing(SectorResolver))
                .ForMember(x => x.WrittenAgreementViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.PscControlledViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.MultipleBranchViewModel, opt => opt.ResolveUsing(MultipleResolver))
                .ForMember(x => x.ShellfishWorkerNumberViewModel, opt => opt.ResolveUsing(ShellfishNumberResolver))
                .ForMember(x => x.ShellfishWorkerNationalityViewModel, opt => opt.ResolveUsing(ShellfishNationalityResolver))
                .ForMember(x => x.PreviouslyWorkedInShellfishViewModel, opt => opt.ResolveUsing(PreviouslyWorkedInShellfishResolver))
                .ForMember(x => x.IsSubmitted, opt => opt.ResolveUsing(ProfileHelpers.GetIsSubmitted));

            CreateMap<CheckboxListItem, ICheckboxListable>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Name))
                .ReverseMap();

            CreateMap<Licence, WrittenAgreementViewModel>()
                .ForMember(x => x.HasWrittenAgreementsInPlace, opt => opt.MapFrom(y => y.HasWrittenAgreementsInPlace))
                .ForMember(x => x.YesNo, opt => opt.UseValue(ProfileHelpers.YesNoList));

            CreateMap<Licence, PSCControlledViewModel>()
                .ForMember(x => x.IsPSCControlled, opt => opt.MapFrom(y => y.IsPSCControlled))
                .ForMember(x => x.PSCDetails, opt => opt.MapFrom(y => y.PSCDetails))
                .ForMember(x => x.YesNo, opt => opt.UseValue(ProfileHelpers.YesNoList));

            CreateMap<OrganisationViewModel, Licence>()
                .ForMember(x => x.SuppliesWorkersOutsideLicensableAreas, opt => opt.MapFrom(y => y.OutsideSectorsViewModel.SuppliesWorkersOutsideLicensableAreas))
                .ForMember(x => x.OtherSector, opt => opt.MapFrom(y => y.OutsideSectorsViewModel.OtherSector))
                .ForMember(x => x.HasWrittenAgreementsInPlace, opt => opt.MapFrom(y => y.WrittenAgreementViewModel.HasWrittenAgreementsInPlace))
                .ForMember(x => x.IsPSCControlled, opt => opt.MapFrom(y => y.PscControlledViewModel.IsPSCControlled))
                .ForMember(x => x.PSCDetails, opt => opt.MapFrom(y => y.PscControlledViewModel.PSCDetails))
                .ForMember(x => x.HasMultiples, opt => opt.MapFrom(y => y.MultipleBranchViewModel.HasMultiples))
                .ForMember(x => x.OtherMultiple, opt => opt.MapFrom(y => y.MultipleBranchViewModel.OtherMultiple))
                .ForMember(x => x.NumberOfMultiples, opt => opt.MapFrom(y => y.MultipleBranchViewModel.NumberOfMultiples))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<ShellfishWorkerNumberViewModel, Licence>()
                .ForMember(x => x.NumberOfShellfishWorkers, opt => opt.MapFrom(y => y.NumberOfWorkers))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<ShellfishWorkerNationalityViewModel, Licence>()
                .ForMember(x => x.NationalityOfShellfishWorkers, opt => opt.MapFrom(y => y.NationalityOfWorkers))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PreviouslyWorkedInShellfishViewModel, Licence>()
                .ForMember(x => x.PreviouslyWorkedInShellfish, opt => opt.MapFrom(y => y.PreviouslyWorkedInShellfish))
                .ForMember(x => x.GatheringLocation, opt => opt.MapFrom(y => y.GatheringLocation))
                .ForMember(x => x.GatheringDate, opt => opt.MapFrom(y => y.GatheringDate.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<OutsideSectorsViewModel, Licence>()
                .ForMember(x => x.OtherSector, opt => opt.MapFrom(y => y.OtherSector))
                .ForMember(x => x.SuppliesWorkersOutsideLicensableAreas, opt => opt.MapFrom(y => y.SuppliesWorkersOutsideLicensableAreas))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<WrittenAgreementViewModel, Licence>()
                .ForMember(x => x.HasWrittenAgreementsInPlace, opt => opt.MapFrom(y => y.HasWrittenAgreementsInPlace))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PSCControlledViewModel, Licence>()
                .ForMember(x => x.IsPSCControlled, opt => opt.MapFrom(y => y.IsPSCControlled))
                .ForMember(x => x.PSCDetails, opt => opt.MapFrom(y => y.PSCDetails))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<MultipleBranchViewModel, Licence>()
                .ForMember(x => x.HasMultiples, opt => opt.MapFrom(y => y.HasMultiples))
                .ForMember(x => x.OtherMultiple, opt => opt.MapFrom(y => y.OtherMultiple))
                .ForMember(x => x.NumberOfMultiples, opt => opt.MapFrom(y => y.NumberOfMultiples))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<TransportingWorkersViewModel, Licence>()
                .ForMember(x => x.TransportsWorkersToWorkplace, opt => opt.MapFrom(y => y.TransportsWorkersToWorkplace))
                .ForMember(x => x.NumberOfVehicles, opt => opt.MapFrom(y => y.NumberOfVehicles))
                .ForMember(x => x.TransportDeductedFromPay, opt => opt.MapFrom(y => y.TransportDeductedFromPay))
                .ForMember(x => x.TransportWorkersChoose, opt => opt.MapFrom(y => y.TransportWorkersChoose))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AccommodatingWorkersViewModel, Licence>()
                .ForMember(x => x.AccommodatesWorkers, opt => opt.MapFrom(y => y.AccommodatesWorkers))
                .ForMember(x => x.NumberOfProperties, opt => opt.MapFrom(y => y.NumberOfProperties))
                .ForMember(x => x.AccommodationDeductedFromPay, opt => opt.MapFrom(y => y.AccommodationDeductedFromPay))
                .ForMember(x => x.AccommodationWorkersChoose, opt => opt.MapFrom(y => y.AccommodationWorkersChoose))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<SourcingWorkersViewModel, Licence>()
                .ForMember(x => x.WorkerSource, opt => opt.MapFrom(y => y.WorkerSource))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<WorkerSupplyMethodViewModel, Licence>()
                .ForMember(x => x.WorkerSupplyMethod, opt => opt.MapFrom(y => y.WorkerSupplyMethod))
                .ForMember(x => x.WorkerSupplyOther, opt => opt.MapFrom(y => y.WorkerSupplyOther))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<WorkerContractViewModel, Licence>()
                .ForMember(x => x.WorkerContract, opt => opt.MapFrom(y => y.SelectedContract))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BannedFromTradingViewModel, Licence>()
                .ForMember(x => x.HasBeenBanned, opt => opt.MapFrom(y => y.HasBeenBanned))
                .ForMember(x => x.DateOfBan, opt => opt.MapFrom(y => y.DateOfBan.Date))
                .ForMember(x => x.BanDescription, opt => opt.MapFrom(y => y.BanDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<SubcontractorViewModel, Licence>()
                .ForMember(x => x.UsesSubcontractors, opt => opt.MapFrom(y => y.UsesSubcontractors))
                .ForMember(x => x.SubcontractorNames, opt => opt.MapFrom(y => y.SubcontractorNames))
                .ForAllOtherMembers(opt => opt.Ignore());
        }

        private ShellfishWorkerNumberViewModel ShellfishNumberResolver(Licence licence)
        {
            return new ShellfishWorkerNumberViewModel
            {
                NumberOfWorkers = licence.NumberOfShellfishWorkers,
                IsShellfish = licence.IsShellfish
            };
        }

        private ShellfishWorkerNationalityViewModel ShellfishNationalityResolver(Licence licence)
        {
            return new ShellfishWorkerNationalityViewModel
            {
                NationalityOfWorkers = licence.NationalityOfShellfishWorkers,
                IsShellfish = licence.IsShellfish
            };
        }

        private PreviouslyWorkedInShellfishViewModel PreviouslyWorkedInShellfishResolver(Licence licence)
        {
            return new PreviouslyWorkedInShellfishViewModel
            {
                PreviouslyWorkedInShellfish = licence.PreviouslyWorkedInShellfish,
                GatheringLocation = licence.GatheringLocation,
                GatheringDate = new DateViewModel
                {
                    Date = licence.GatheringDate
                },
                IsShellfish = licence.IsShellfish
            };
        }

        private SourcingWorkersViewModel SourcingWorkersResolver(Licence licence)
        {
            return new SourcingWorkersViewModel
            {
                WorkerSource = licence.WorkerSource
            };
        }

        private WorkerSupplyMethodViewModel SupplyWorkersResolver(Licence licence)
        {
            return new WorkerSupplyMethodViewModel
            {
                WorkerSupplyMethod = licence.WorkerSupplyMethod,
                WorkerSupplyOther = licence.WorkerSupplyOther,
                AvailableSources = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Employee",
                        Value = "1"
                    },
                    new SelectListItem
                    {
                        Text = "Self Employed",
                        Value = "2"
                    },
                    new SelectListItem
                    {
                        Text = "Other",
                        Value = "3"
                    }
                }
            };

        }

        private WorkerContractViewModel WorkerContractResolver(Licence licence)
        {
            return new WorkerContractViewModel
            {
                SelectedContract = licence.WorkerContract,
                AvailableContracts = new List<AvailableContract>
                {
                    new AvailableContract
                    {
                        Id = 1,
                        Name = "Contract of employment",
                        Checked = false,
                        EnumMappedTo =  WorkerContract.ContractOfEmployment
                    },
                    new AvailableContract
                    {
                        Id = 2,
                        Name = "Contract for Services",
                        Checked = false,
                        EnumMappedTo = WorkerContract.ContractForServices
                    },
                    new AvailableContract
                    {
                        Id = 3,
                        Name = "None",
                        Checked = false,
                        EnumMappedTo = WorkerContract.None
                    }
                }
            };
        }

        private BannedFromTradingViewModel BannedFromTradingResolver(Licence licence)
        {
            return new BannedFromTradingViewModel
            {
                HasBeenBanned = licence.HasBeenBanned,
                DateOfBan = new DateViewModel
                {
                    Date = licence.DateOfBan
                },
                BanDescription = licence.BanDescription
            };
        }

        private SubcontractorViewModel SubcontracterResolver(Licence licence)
        {
            return new SubcontractorViewModel
            {
                UsesSubcontractors = licence.UsesSubcontractors,
                SubcontractorNames = licence.SubcontractorNames
            };
        }

        private TransportingWorkersViewModel TransportingWorkersResolver(Licence licence)
        {
            return new TransportingWorkersViewModel
            {
                TransportsWorkersToWorkplace = licence.TransportsWorkersToWorkplace,
                NumberOfVehicles = licence.NumberOfVehicles,
                TransportDeductedFromPay = licence.TransportDeductedFromPay,
                TransportWorkersChoose = licence.TransportWorkersChoose
            };
        }

        private AccommodatingWorkersViewModel AccomodatingWorkersResolver(Licence licence)
        {
            return new AccommodatingWorkersViewModel
            {
                AccommodatesWorkers = licence.AccommodatesWorkers,
                NumberOfProperties = licence.NumberOfVehicles,
                AccommodationDeductedFromPay = licence.AccommodationDeductedFromPay,
                AccommodationWorkersChoose = licence.AccommodationWorkersChoose
            };
        }

        private OutsideSectorsViewModel SectorResolver(Licence licence)
        {
            var vm = new OutsideSectorsViewModel
            {
                OtherSector = licence.OtherSector,
                SuppliesWorkersOutsideLicensableAreas = licence.SuppliesWorkersOutsideLicensableAreas,
                YesNo = ProfileHelpers.YesNoList
            };

            if (licence.SelectedSectors != null)
            {
                foreach (var item in licence.SelectedSectors)
                {
                    vm.SelectedSectors.Single(x => x.Id == item.Id).Checked = true;
                }
            }

            return vm;
        }

        private MultipleBranchViewModel MultipleResolver(Licence licence)
        {
            var vm = new MultipleBranchViewModel
            {
                OtherMultiple = licence.OtherMultiple,
                HasMultiples = licence.HasMultiples,
                NumberOfMultiples = licence.NumberOfMultiples,
                YesNo = ProfileHelpers.YesNoList
            };

            if (licence.SelectedMultiples != null)
            {
                foreach (var item in licence.SelectedMultiples)
                {
                    vm.SelectedMultiples.Single(x => x.Id == item.Id).Checked = true;
                }
            }

            return vm;
        }
    }
}
