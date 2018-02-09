using System;
using System.Collections.Generic;
using System.Linq;
using GLAA.Domain.Core.Models;
using GLAA.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GLAA.Domain
{
    public static class GLAAContextExtensions
    {
        public static void Seed(this GLAAContext context, List<LicenceStatus> defaultStatuses)
        {
            context.Database.Migrate();

            var rnd = new Random();

            var _companyPart1 = new[] { "Labour", "People", "Agricultural", "Shellfish", "Synergy" };
            var _companyPart2 = new[] { "Solutions", "Solutions Ltd.", "Sourcing", "Sourcing Ltd.", "Services", "Services Ltd.", "Informantics Ltd", "Synergystics" };
            var _firstNames = new[] { "Aaron", "Abdul", "Abe", "Abel", "Abraham", "Adam", "Adan", "Adrian", "Abby", "Abigail", "Adele", "Christina", "Doug", "Chantelle", "Adam", "Luke", "Conrad", "Moray" };
            var _lastNames = new[] { "Abbott", "Acosta", "Adams", "Adkins", "Aguilar", "Aguilara", "McDonald", "MacDonald", "Danson", "Spokes", "Grinnell", "Jackson" };
            
            var countries = new[]
            {
                "Afghanistan",
                "Albania",
                "Algeria",
                "Andorra",
                "Angola",
                "Antigua & Deps",
                "Argentina",
                "Armenia",
                "Australia",
                "Austria",
                "Azerbaijan",
                "Bahamas",
                "Bahrain",
                "Bangladesh",
                "Barbados",
                "Belarus",
                "Belgium",
                "Belize",
                "Benin",
                "Bhutan",
                "Bolivia",
                "Bosnia Herzegovina",
                "Botswana",
                "Brazil",
                "Brunei",
                "Bulgaria",
                "Burkina",
                "Burundi",
                "Cambodia",
                "Cameroon",
                "Canada",
                "Cape Verde",
                "Central African Republic",
                "Chad",
                "Chile",
                "China",
                "Colombia",
                "Comoros",
                "Costa Rica",
                "Croatia",
                "Cuba",
                "Cyprus",
                "Czech Republic",
                "Democratic Republic of the Congo",
                "Denmark",
                "Djibouti",
                "Dominica",
                "Dominican Republic",
                "East Timor",
                "Ecuador",
                "Egypt",
                "El Salvador",
                "Equatorial Guinea",
                "Eritrea",
                "Estonia",
                "Ethiopia",
                "Fiji",
                "Finland",
                "France",
                "Gabon",
                "Gambia",
                "Georgia",
                "Germany",
                "Ghana",
                "Greece",
                "Grenada",
                "Guatemala",
                "Guinea",
                "Guinea-Bissau",
                "Guyana",
                "Haiti",
                "Honduras",
                "Hungary",
                "Iceland",
                "India",
                "Indonesia",
                "Iran",
                "Iraq",
                "Israel",
                "Italy",
                "Ivory Coast",
                "Jamaica",
                "Japan",
                "Jordan",
                "Kazakhstan",
                "Kenya",
                "Kiribati",
                "Korea North",
                "Korea South",
                "Kosovo",
                "Kuwait",
                "Kyrgyzstan",
                "Laos",
                "Latvia",
                "Lebanon",
                "Lesotho",
                "Liberia",
                "Libya",
                "Liechtenstein",
                "Lithuania",
                "Luxembourg",
                "Macedonia",
                "Madagascar",
                "Malawi",
                "Malaysia",
                "Maldives",
                "Mali",
                "Malta",
                "Marshall Islands",
                "Mauritania",
                "Mauritius",
                "Mexico",
                "Micronesia",
                "Moldova",
                "Monaco",
                "Mongolia",
                "Montenegro",
                "Morocco",
                "Mozambique",
                "Myanmar",
                "Namibia",
                "Nauru",
                "Nepal",
                "Netherlands",
                "New Zealand",
                "Nicaragua",
                "Niger",
                "Nigeria",
                "Norway",
                "Oman",
                "Pakistan",
                "Palau",
                "Panama",
                "Papua New Guinea",
                "Paraguay",
                "Peru",
                "Philippines",
                "Poland",
                "Portugal",
                "Qatar",
                "Republic of Ireland",
                "Republic of the Congo",
                "Romania",
                "Russian Federation",
                "Rwanda",
                "St Kitts & Nevis",
                "St Lucia",
                "Saint Vincent & the Grenadines",
                "Samoa",
                "San Marino",
                "Sao Tome & Principe",
                "Saudi Arabia",
                "Senegal",
                "Serbia",
                "Seychelles",
                "Sierra Leone",
                "Singapore",
                "Slovakia",
                "Slovenia",
                "Solomon Islands",
                "Somalia",
                "South Africa",
                "South Sudan",
                "Spain",
                "Sri Lanka",
                "Sudan",
                "Suriname",
                "Swaziland",
                "Sweden",
                "Switzerland",
                "Syria",
                "Taiwan",
                "Tajikistan",
                "Tanzania",
                "Thailand",
                "Togo",
                "Tonga",
                "Trinidad & Tobago",
                "Tunisia",
                "Turkey",
                "Turkmenistan",
                "Tuvalu",
                "Uganda",
                "Ukraine",
                "United Arab Emirates",
                "United Kingdom",
                "United States",
                "Uruguay",
                "Uzbekistan",
                "Vanuatu",
                "Vatican City",
                "Venezuela",
                "Vietnam",
                "Yemen",
                "Zambia",
                "Zimbabwe"
            };

            var counties = new[]
            {
                "Bedfordshire",
                "Buckinghamshire",
                "Cambridgeshire",
                "Cheshire",
                "Cleveland",
                "Cornwall",
                "Cumbria",
                "Derbyshire",
                "Devon",
                "Dorset",
                "Durham",
                "East Sussex",
                "Essex",
                "Gloucestershire",
                "Greater London",
                "Greater Manchester",
                "Hampshire",
                "Hertfordshire",
                "Kent",
                "Lancashire",
                "Leicestershire",
                "Lincolnshire",
                "Merseyside",
                "Norfolk",
                "North Yorkshire",
                "Northamptonshire",
                "Northumberland",
                "Nottinghamshire",
                "Oxfordshire",
                "Shropshire",
                "Somerset",
                "South Yorkshire",
                "Staffordshire",
                "Suffolk",
                "Surrey",
                "Tyne and Wear",
                "Warwickshire",
                "West Berkshire",
                "West Midlands",
                "West Sussex",
                "West Yorkshire",
                "Wiltshire",
                "Worcestershire",
                "Flintshire",
                "Glamorgan",
                "Merionethshire",
                "Monmouthshire",
                "Montgomeryshire",
                "Pembrokeshire",
                "Radnorshire",
                "Anglesey",
                "Breconshire",
                "Caernarvonshire",
                "Cardiganshire",
                "Carmarthenshire",
                "Denbighshire",
                "Aberdeen City",
                "Aberdeenshire",
                "Angus",
                "Argyll and Bute",
                "City of Edinburgh",
                "Clackmannanshire",
                "Dumfries and Galloway",
                "Dundee City",
                "East Ayrshire",
                "East Dunbartonshire",
                "East Lothian",
                "East Renfrewshire",
                "Eilean Siar",
                "Falkirk",
                "Fife",
                "Glasgow City",
                "Highland",
                "Inverclyde",
                "Midlothian",
                "Moray",
                "North Ayrshire",
                "North Lanarkshire",
                "Orkney Islands",
                "Perth and Kinross",
                "Renfrewshire",
                "Scottish Borders",
                "Shetland Islands",
                "South Ayrshire",
                "South Lanarkshire",
                "Stirling",
                "West Dunbartonshire",
                "West Lothian",
                "Antrim",
                "Armagh",
                "Down",
                "Fermanagh",
                "Derry and Londonderry",
                "Tyrone"
            };

            if (!context.Countries.Any())
            {
                context.Countries.AddRange(countries.Select(c => new Country {Name = c}));
                context.SaveChanges();
            }

            if (!context.Counties.Any())
            {
                context.Counties.AddRange(counties.Select(c => new County {Name = c}));
                context.SaveChanges();
            }

            IEnumerable<LicensingStandard> defaultLicensingStandards = new[]
            {
                new LicensingStandard {Name = "1.1 Fit and Proper", IsCritical = true},
                new LicensingStandard {Name = "1.2 Principal Authority Competency Test", IsCritical = true},
                new LicensingStandard {Name = "1.3 Correcting Additional Licence Conditions", IsCritical = true},
                new LicensingStandard {Name = "1.4 Changes in Details"},
                new LicensingStandard {Name = "2.1 PAYE, NI and VAT", IsCritical = true},
                new LicensingStandard {Name = "2.2 Paying Wages", IsCritical = true},
                new LicensingStandard {Name = "2.3 Benefits"},
                new LicensingStandard {Name = "2.4 Payslips"},
                new LicensingStandard {Name = "3.1 Physical and Mental Mistreatment", IsCritical = true},
                new LicensingStandard {Name = "3.2 Restricting Workers Movement", IsCritical = true},
                new LicensingStandard {Name = "3.3 Withholding Wages", IsCritical = true},
                new LicensingStandard {Name = "4.1 Quality of Accomodation", IsCritical = true},
                new LicensingStandard {Name = "4.2 Licensing of Accomodation"},
                new LicensingStandard {Name = "4.3 Situations Where Workers are Provided with Travel or Required to Live Away From Home"},
                new LicensingStandard {Name = "5.1 Rest Periods, Breaks and Annual Leave"},
                new LicensingStandard {Name = "5.2 Working Hours"},
                new LicensingStandard {Name = "5.3 Trade Union"},
                new LicensingStandard {Name = "5.4 IndustrialDisputes"},
                new LicensingStandard {Name = "5.5 Confidentiality"},
                new LicensingStandard {Name = "5.6 Disciplinary and Grievance Procedures"},
                new LicensingStandard {Name = "5.7 Discrimination"},
                new LicensingStandard {Name = "6.1 Assigning Responsibility and Assessing Risk"},
                new LicensingStandard {Name = "6.2 Instruction and Training"},
                new LicensingStandard {Name = "6.3 Safety at Work"},
                new LicensingStandard {Name = "6.4 Transport", IsCritical = true},
                new LicensingStandard {Name = "6.5 Using Workers to Gather Shellfish – Planning and Supervision", IsCritical = true},
                new LicensingStandard {Name = "6.6 Using Workers to Gather Shellfish – Getting to the Work Area", IsCritical = true},
                new LicensingStandard {Name = "6.7 Using Workers to Gather Shellfish – Lifejackets and Life Rafts", IsCritical = true},
                new LicensingStandard {Name = "6.8 Using Workers to Gather Shellfish – Use of Boats", IsCritical = true},
                new LicensingStandard {Name = "6.9 Using Workers to Gather Shellfish – Shellfish Gathering Permits and Licences", IsCritical = true},
                new LicensingStandard {Name = "7.1 Fees and Providing Additional Services", IsCritical = true},
                new LicensingStandard {Name = "7.2 Right to Work"},
                new LicensingStandard {Name = "7.3 Workers: Contractual Arrangements and Records"},
                new LicensingStandard {Name = "7.4 Labour User: Agreements and Records"},
                new LicensingStandard {Name = "7.5 Restriction on Charges to Labour Users"},
                new LicensingStandard {Name = "8.1 Sub-Contracting and Using Other Labour Providers", IsCritical = true},
                new LicensingStandard {Name = "8.2 Records of Dealing With Other Licence Holders"}
            };

            if (!context.LicensingStandards.Any())
            {
                context.LicensingStandards.AddRange(defaultLicensingStandards);
                context.SaveChanges();
            }

            if (!context.LicenceStatuses.Any())
            {
                context.LicenceStatuses.AddRange(defaultStatuses);
                context.LinkNextStatuses(defaultStatuses);
                context.SaveChanges();
            }

            if (!context.Licences.Any())
            {
                var licences = new List<Licence>();

                for (var i = 0; i < 50; i++)
                {
                    var newStatus = defaultStatuses[rnd.Next(defaultStatuses.Count)];
                    var newStatusEntry = context.LicenceStatuses.Find(newStatus.Id);
                    licences.Add(new Licence
                    {
                        ApplicationId = $"DRAFT-{1234 + i}",
                        BusinessName = $"Demo Organisation {i + 1}",
                        TradingName =
                            $"{_companyPart1[rnd.Next(_companyPart1.Length)]} {_companyPart2[rnd.Next(_companyPart2.Length)]}",
                        LicenceStatusHistory = new List<LicenceStatusChange>
                        {
                            new LicenceStatusChange
                            {
                                DateCreated = new DateTime(2017, 6 + rnd.Next(3), 1 + rnd.Next(29)),
                                Status = newStatusEntry
                            }
                        },
                        PrincipalAuthorities = new List<PrincipalAuthority>
                        {
                            new PrincipalAuthority
                            {
                                FullName =
                                    $"{_firstNames[rnd.Next(_firstNames.Length)]} {_lastNames[rnd.Next(_lastNames.Length)]}",
                                IsCurrent = true
                            }
                        },
                        //User = adminUser
                    });
                }

                context.Licences.AddRange(licences);
            }

            if (!context.Industries.Any())
            {
                var defaultIndustries = new List<Industry>
                {
                    new Industry
                    {
                        Name = "Horticulture"
                    },
                    new Industry
                    {
                        Name = "Argiculture"
                    },
                    new Industry
                    {
                        Name = "Food Packaging"
                    },
                    new Industry
                    {
                        Name = "Shellfish gathering"
                    },
                    new Industry
                    {
                        Name = "Other"
                    }
                };

                context.Industries.AddRange(defaultIndustries);
            }

            if (!context.WorkerCountries.Any())
            {
                var defaultCountries = new List<WorkerCountry>
                {
                    new WorkerCountry
                    {
                        Name = "England"
                    },
                    new WorkerCountry
                    {
                        Name = "Scotland"
                    },
                    new WorkerCountry
                    {
                        Name = "Wales"
                    },
                    new WorkerCountry
                    {
                        Name = "Northern Ireland"
                    }
                };

                context.WorkerCountries.AddRange(defaultCountries);
            }

            if (!context.Multiples.Any())
            {
                var defaultMultiples = new List<Multiple>
                {
                    new Multiple
                    {
                        Name = "Multiple"
                    },
                    new Multiple
                    {
                        Name = "Franchise"
                    },
                    new Multiple
                    {
                        Name = "Other"
                    }
                };

                context.Multiples.AddRange(defaultMultiples);
            }

            if (!context.Sectors.Any())
            {
                var defaultSectors = new List<Sector>
                {
                    new Sector
                    {
                        Name = "Agriculture"
                    },
                    new Sector
                    {
                        Name = "Horticulture"
                    },
                    new Sector
                    {
                        Name = "Food Packaging and Processing"
                    },
                    new Sector
                    {
                        Name = "Shellfish Gathering"
                    }
                };

                context.Sectors.AddRange(defaultSectors);
            }

            context.SaveChanges();

            if (!context.Licences.Any(x => x.ApplicationId == "FULL-1234"))
            {
                var fullLicence = new Licence
                {
                    ApplicationId = "FULL-1234",
                    Address = new Address
                    {
                        AddressLine1 = "123 Fake Street",
                        AddressLine2 = "Fake Grove",
                        Town = "Faketon",
                        County = context.Counties.Single(c => c.Name.Equals("Nottinghamshire")),
                        Postcode = "FA2 4KE",
                        Country = context.Countries.Single(c => c.Name.Equals("United Kingdom")),
                        NonUK = false
                    },
                    BusinessEmailAddress = "joe@example.com",
                    BusinessEmailAddressConfirmation = "joe@example.com",
                    BusinessPhoneNumber = "07777777777",
                    BusinessMobileNumber = "07777777777",
                    BusinessWebsite = "http://www.example.com",
                    CommunicationPreference = CommunicationPreference.Email,
                    CompaniesHouseNumber = "12341234",
                    CompanyRegistrationDate = DateTime.Now,
                    HasMultiples = true,
                    HasPAYEERNNumber = true,
                    HasTradingName = true,
                    HasPreviousTradingName = true,
                    PreviousTradingNames = new[]
                    {
                        new PreviousTradingName
                        {
                            BusinessName = "Old business name",
                            Town = "Slough",
                            Country = "UK"
                        }
                    },
                    HasWrittenAgreementsInPlace = true,
                    HasVATNumber = true,
                    IsPSCControlled = true,
                    LegalStatus = LegalStatusEnum.LimitedCompany,
                    NumberOfMultiples = 3,
                    OperatingCountries =
                        new List<LicenceWorkerCountry>
                        {
                            new LicenceWorkerCountry {WorkerCountry = context.WorkerCountries.Find(1)}
                        },
                    OperatingIndustries =
                        new List<LicenceIndustry>
                        {
                            new LicenceIndustry
                            {
                                Industry = context.Industries.Find(1)
                            }
                        },
                    BusinessName = "Fully Populated Company",
                    OtherMultiple = "Some other Multiple",
                    OtherSector = "Some other Sector",
                    PAYEERNNumber = "123/A12345",
                    PAYEERNRegistrationDate = DateTime.Now,
                    PSCDetails = "Here are some details about the PSC. And some more. And some more.",
                    SelectedMultiples = new List<LicenceMultiple>
                    {
                        new LicenceMultiple {Multiple = context.Multiples.Find(1)},
                        new LicenceMultiple {Multiple = context.Multiples.Find(3)}
                    },
                    SelectedSectors = new List<LicenceSector>
                    {
                        new LicenceSector {Sector = context.Sectors.Find(1)},
                        new LicenceSector {Sector = context.Sectors.Find(3)}
                    },
                    SuppliesWorkersOutsideLicensableAreas = true,
                    TaxReferenceNumber = "1334404714",
                    TradingName = "FullPopCo",
                    TurnoverBand = TurnoverBand.FiveToTenMillion,
                    VATNumber = "GB999 9999 73",
                    VATRegistrationDate = DateTime.Now,
                    TransportsWorkersToWorkplace = true,
                    TransportWorkersChoose = false,
                    TransportDeductedFromPay = true,
                    NumberOfVehicles = 1,
                    AccommodatesWorkers = true,
                    AccommodationWorkersChoose = true,
                    AccommodationDeductedFromPay = false,
                    NumberOfProperties = 2,
                    HasBeenBanned = true,
                    DateOfBan = DateTime.Now,
                    BanDescription = "Reasons for the ban",
                    WorkerSupplyMethod = WorkerSupplyMethod.Other,
                    WorkerSupplyOther = "How I'm supplying workers",
                    WorkerContract = WorkerContract.ContractOfEmployment,
                    UsesSubcontractors = true,
                    SubcontractorNames = "The names of subcontractors",
                    HasAlternativeBusinessRepresentatives = true,
                    NumberOfDirectorsOrPartners = 2,
                    HasNamedIndividuals = true,
                    NamedIndividualType = NamedIndividualType.PersonalDetails,
                    SuppliesWorkers = true,
                    AgreedToStatementOne = true,
                    AgreedToStatementTwo = true,
                    AgreedToStatementThree = true,
                    AgreedToStatementFour = true,
                    AgreedToStatementFive = true,
                    AgreedToStatementSix = true,
                    SignatoryName = "The signatory name",
                    SignatureDate = new DateTime(2017, 1, 1),

                    AlternativeBusinessRepresentatives = new List<AlternativeBusinessRepresentative>
                    {
                        new AlternativeBusinessRepresentative
                        {
                            Address = new Address
                            {
                                AddressLine1 = "123 Fake Street",
                                AddressLine2 = "Fake Grove",
                                Town = "Faketon",
                                County = context.Counties.Single(c => c.Name.Equals("Nottinghamshire")),
                                Postcode = "FA2 4KE",
                                Country = context.Countries.Single(c => c.Name.Equals("United Kingdom")),
                                NonUK = false
                            },
                            AlternativeName = "Alan Smithee",
                            BusinessExtension = "999",
                            BusinessPhoneNumber = "07777777777",
                            CountryOfBirth = "Peru",
                            CountyOfBirth = "Wiltshire",
                            DateOfBirth = DateTime.Now,
                            FullName = "Dave Bloggs",
                            HasAlternativeName = true,
                            JobTitle = "CEO",
                            NationalInsuranceNumber = "JT123456A",
                            PersonalEmailAddress = "joe@example.com",
                            PersonalMobileNumber = "07777777777",
                            TownOfBirth = "Nottingham",
                            Nationality = "British",
                            HasPassport = true,
                            RequiresVisa = true,
                            VisaDescription = "description",
                            IsUndischargedBankrupt = true,
                            BankruptcyDate = DateTime.Now,
                            BankruptcyNumber = "1234567",
                            IsDisqualifiedDirector = true,
                            DisqualificationDetails = "Some details",
                            HasRestraintOrders = true,
                            RestraintOrders = new[]
                            {
                                new RestraintOrder
                                {
                                    Date = DateTime.Now,
                                    Description = "Restraint description"
                                }
                            },
                            HasUnspentConvictions = true,
                            UnspentConvictions = new[]
                            {
                                new Conviction
                                {
                                    Date = DateTime.Now,
                                    Description = "Conviction description"
                                }
                            },
                            HasOffencesAwaitingTrial = true,
                            OffencesAwaitingTrial = new[]
                            {
                                new OffenceAwaitingTrial
                                {
                                    Date = DateTime.Now,
                                    Description = "Offence description"
                                }
                            },
                            HasPreviouslyHeldLicence = true,
                            PreviousLicenceDescription = "I had a previous licence."
                        }
                    },
                    DirectorOrPartners = new List<DirectorOrPartner>
                    {
                        new DirectorOrPartner
                        {
                            Address = new Address
                            {
                                AddressLine1 = "123 Fake Street",
                                AddressLine2 = "Fake Grove",
                                Town = "Faketon",
                                County = context.Counties.Single(c => c.Name.Equals("Nottinghamshire")),
                                Postcode = "FA2 4KE",
                                Country = context.Countries.Single(c => c.Name.Equals("United Kingdom")),
                                NonUK = false
                            },
                            AlternativeName = "Alan Smithee",
                            BusinessExtension = "999",
                            BusinessPhoneNumber = "07777777777",
                            CountryOfBirth = "Peru",
                            CountyOfBirth = "Wiltshire",
                            DateOfBirth = DateTime.Now,
                            FullName = "Fred Bloggs",
                            HasAlternativeName = true,
                            JobTitle = "CEO",
                            NationalInsuranceNumber = "JT123456A",
                            PersonalEmailAddress = "joe@example.com",
                            PersonalMobileNumber = "07777777777",
                            TownOfBirth = "Nottingham",
                            IsPreviousPrincipalAuthority = false,
                            Nationality = "British",
                            HasPassport = true,
                            RequiresVisa = true,
                            VisaDescription = "description",
                            IsUndischargedBankrupt = true,
                            BankruptcyDate = DateTime.Now,
                            BankruptcyNumber = "1234567",
                            IsDisqualifiedDirector = true,
                            DisqualificationDetails = "Some details",
                            HasRestraintOrders = true,
                            RestraintOrders = new[]
                            {
                                new RestraintOrder
                                {
                                    Date = DateTime.Now,
                                    Description = "Restraint description"
                                }
                            },
                            HasUnspentConvictions = true,
                            UnspentConvictions = new[]
                            {
                                new Conviction
                                {
                                    Date = DateTime.Now,
                                    Description = "Conviction description"
                                }
                            },
                            HasOffencesAwaitingTrial = true,
                            OffencesAwaitingTrial = new[]
                            {
                                new OffenceAwaitingTrial
                                {
                                    Date = DateTime.Now,
                                    Description = "Offence description"
                                }
                            },
                            HasPreviouslyHeldLicence = true,
                            PreviousLicenceDescription = "I had a previous licence.",
                        }
                    },
                    NamedIndividuals = new List<NamedIndividual>
                    {
                        new NamedIndividual
                        {
                            BusinessExtension = "999",
                            BusinessPhoneNumber = "07777777777",
                            DateOfBirth = DateTime.Now,
                            FullName = "Joe Bloggs",
                            IsUndischargedBankrupt = true,
                            BankruptcyDate = DateTime.Now,
                            BankruptcyNumber = "1234567",
                            IsDisqualifiedDirector = true,
                            DisqualificationDetails = "Some details",
                            HasRestraintOrders = true,
                            RequiresVisa = false,
                            RestraintOrders = new[]
                            {
                                new RestraintOrder
                                {
                                    Date = DateTime.Now,
                                    Description = "Restraint description"
                                }
                            },
                            HasUnspentConvictions = true,
                            UnspentConvictions = new[]
                            {
                                new Conviction
                                {
                                    Date = DateTime.Now,
                                    Description = "Conviction description"
                                }
                            },
                            HasOffencesAwaitingTrial = true,
                            OffencesAwaitingTrial = new[]
                            {
                                new OffenceAwaitingTrial
                                {
                                    Date = DateTime.Now,
                                    Description = "Offence description"
                                }
                            },
                            HasPreviouslyHeldLicence = true,
                            PreviousLicenceDescription = "I had a previous licence."
                        }
                    },
                    LicenceStatusHistory = new List<LicenceStatusChange>
                    {
                        new LicenceStatusChange
                        {
                            DateCreated = DateTime.Now,
                            Status = context.LicenceStatuses.Find(110)
                        }
                    },
                };

                context.Licences.Add(fullLicence);

                context.SaveChanges();

                var id = fullLicence.Id;

                var licence = context.Licences.Find(id);

                var pa = new PrincipalAuthority
                {
                    Address = new Address
                    {
                        AddressLine1 = "123 Fake Street",
                        AddressLine2 = "Fake Grove",
                        Town = "Faketon",
                        County = context.Counties.Single(c => c.Name.Equals("Nottinghamshire")),
                        Postcode = "FA2 4KE",
                        Country = context.Countries.Single(c => c.Name.Equals("United Kingdom")),
                        NonUK = false
                    },
                    AlternativeName = "Alan Smithee",
                    BusinessExtension = "999",
                    BusinessPhoneNumber = "07777777777",
                    CountryOfBirth = "Peru",
                    CountyOfBirth = "Wiltshire",
                    DateOfBirth = DateTime.Now,
                    FullName = "Joe Bloggs",
                    HasAlternativeName = true,
                    JobTitle = "CEO",
                    NationalInsuranceNumber = "JT123456A",
                    PersonalEmailAddress = "joe@example.com",
                    PersonalMobileNumber = "07777777777",
                    TownOfBirth = "Nottingham",
                    IsCurrent = true,
                    IsDirector = true,
                    PreviousExperience =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    WillProvideConfirmation = true,
                    Nationality = "British",
                    HasPassport = true,
                    PermissionToWorkStatus = PermissionToWorkEnum.HasVisa,
                    VisaNumber = "12341234",
                    ImmigrationStatus = "Some status",
                    LeaveToRemainTo = new DateTime(2019, 1, 1),
                    LengthOfUKWorkMonths = 6,
                    LengthOfUKWorkYears = 2,
                    IsUndischargedBankrupt = true,
                    BankruptcyDate = DateTime.Now,
                    BankruptcyNumber = "1234567",
                    IsDisqualifiedDirector = true,
                    DisqualificationDetails = "Some details",
                    HasRestraintOrders = true,
                    RestraintOrders = new[]
                    {
                        new RestraintOrder
                        {
                            Date = DateTime.Now,
                            Description = "Restraint description"
                        }
                    },
                    HasUnspentConvictions = true,
                    UnspentConvictions = new[]
                    {
                        new Conviction
                        {
                            Date = DateTime.Now,
                            Description = "Conviction description"
                        }
                    },
                    HasOffencesAwaitingTrial = true,
                    OffencesAwaitingTrial = new[]
                    {
                        new OffenceAwaitingTrial
                        {
                            Date = DateTime.Now,
                            Description = "Offence description"
                        }
                    },
                    HasPreviouslyHeldLicence = true,
                    PreviousLicenceDescription = "I had a previous licence.",
                    DirectorOrPartner = new DirectorOrPartner
                    {
                        Address = new Address
                        {
                            AddressLine1 = "123 Fake Street",
                            AddressLine2 = "Fake Grove",
                            Town = "Faketon",
                            County = context.Counties.Single(c => c.Name.Equals("Nottinghamshire")),
                            Postcode = "FA2 4KE",
                            Country = context.Countries.Single(c => c.Name.Equals("United Kingdom")),
                            NonUK = false
                        },
                        AlternativeName = "Alan Smithee",
                        BusinessExtension = "999",
                        BusinessPhoneNumber = "07777777777",
                        CountryOfBirth = "Peru",
                        CountyOfBirth = "Wiltshire",
                        DateOfBirth = DateTime.Now,
                        FullName = "Joe Bloggs",
                        HasAlternativeName = true,
                        JobTitle = "CEO",
                        NationalInsuranceNumber = "JT123456A",
                        PersonalEmailAddress = "joe@example.com",
                        PersonalMobileNumber = "07777777777",
                        TownOfBirth = "Nottingham",
                        IsPreviousPrincipalAuthority = true,
                        Licence = licence,
                        Nationality = "British",
                        HasPassport = true,
                        RequiresVisa = true,
                        VisaDescription = "description",
                        IsUndischargedBankrupt = true,
                        BankruptcyDate = DateTime.Now,
                        BankruptcyNumber = "1234567",
                        IsDisqualifiedDirector = true,
                        DisqualificationDetails = "Some details",
                        HasRestraintOrders = true,
                        RestraintOrders = new[]
                        {
                            new RestraintOrder
                            {
                                Date = DateTime.Now,
                                Description = "Restraint description"
                            }
                        },
                        HasUnspentConvictions = true,
                        UnspentConvictions = new[]
                        {
                            new Conviction
                            {
                                Date = DateTime.Now,
                                Description = "Conviction description"
                            }
                        },
                        HasOffencesAwaitingTrial = true,
                        OffencesAwaitingTrial = new[]
                        {
                            new OffenceAwaitingTrial
                            {
                                Date = DateTime.Now,
                                Description = "Offence description"
                            }
                        },
                        HasPreviouslyHeldLicence = true,
                        PreviousLicenceDescription = "I had a previous licence.",
                    },
                    Licence = licence
                };

                context.PrincipalAuthorities.Add(pa);

                context.SaveChanges();
            }
        }

        private static void LinkNextStatuses(this GLAAContext context, IEnumerable<LicenceStatus> _defaultStatuses)
        {
            foreach (var statusToAdd in _defaultStatuses)
            {
                var status = context.LicenceStatuses.Find(statusToAdd.Id);
                if (status?.NextStatusIds != null)
                {
                    foreach (var nextStatusId in statusToAdd.NextStatusIds)
                    {
                        if (status.NextStatuses == null)
                        {
                            status.NextStatuses = new List<LicenceStatusNextStatus>();
                        }
                        status.NextStatuses.Add(new LicenceStatusNextStatus
                        {
                            NextStatus = context.LicenceStatuses.Find(nextStatusId),
                            LicenceStatus = statusToAdd
                        });
                    }
                }
            }
        }
    }
}
