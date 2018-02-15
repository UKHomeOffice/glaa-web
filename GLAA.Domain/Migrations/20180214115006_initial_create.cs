using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GLAA.Domain.Migrations
{
    public partial class initial_create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AddressLine3 = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    NonUK = table.Column<bool>(nullable: false),
                    Postcode = table.Column<string>(nullable: true),
                    Town = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Industry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LicenceStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ActiveCheckDescription = table.Column<string>(nullable: true),
                    AdminCategory = table.Column<int>(nullable: false),
                    CssClassStem = table.Column<string>(nullable: true),
                    ExternalDescription = table.Column<string>(nullable: true),
                    InternalDescription = table.Column<string>(nullable: true),
                    InternalStatus = table.Column<string>(nullable: true),
                    LicenceIssued = table.Column<bool>(nullable: false),
                    LicenceSubmitted = table.Column<bool>(nullable: false),
                    RequireNonCompliantStandards = table.Column<bool>(nullable: false),
                    ShowInPublicRegister = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LicensingStandard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsCritical = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicensingStandard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Multiple",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Multiple", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sector",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    AddressId = table.Column<int>(nullable: true),
                    CommunicationPreference = table.Column<int>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LicenceStatusNextStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NextStatusId = table.Column<int>(nullable: false),
                    NextStatusId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceStatusNextStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenceStatusNextStatus_LicenceStatus_NextStatusId",
                        column: x => x.NextStatusId,
                        principalTable: "LicenceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenceStatusNextStatus_LicenceStatus_NextStatusId1",
                        column: x => x.NextStatusId1,
                        principalTable: "LicenceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatusReason",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    LicenceStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusReason_LicenceStatus_LicenceStatusId",
                        column: x => x.LicenceStatusId,
                        principalTable: "LicenceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Licence",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccommodatesWorkers = table.Column<bool>(nullable: true),
                    AccommodationDeductedFromPay = table.Column<bool>(nullable: true),
                    AccommodationWorkersChoose = table.Column<bool>(nullable: true),
                    AddressId = table.Column<int>(nullable: true),
                    AgreedToTermsAndConditions = table.Column<bool>(nullable: true),
                    ApplicationId = table.Column<string>(nullable: true),
                    BanDescription = table.Column<string>(nullable: true),
                    BusinessEmailAddress = table.Column<string>(nullable: true),
                    BusinessEmailAddressConfirmation = table.Column<string>(nullable: true),
                    BusinessMobileNumber = table.Column<string>(nullable: true),
                    BusinessName = table.Column<string>(nullable: true),
                    BusinessPhoneNumber = table.Column<string>(nullable: true),
                    BusinessWebsite = table.Column<string>(nullable: true),
                    CommunicationPreference = table.Column<int>(nullable: true),
                    CompaniesHouseNumber = table.Column<string>(nullable: true),
                    CompanyRegistrationDate = table.Column<DateTime>(nullable: true),
                    ContinueApplication = table.Column<bool>(nullable: true),
                    DateOfBan = table.Column<DateTime>(nullable: true),
                    EmailAlreadyRegistered = table.Column<bool>(nullable: false),
                    GatheringDate = table.Column<DateTime>(nullable: true),
                    GatheringLocation = table.Column<string>(nullable: true),
                    HasAlternativeBusinessRepresentatives = table.Column<bool>(nullable: true),
                    HasBeenBanned = table.Column<bool>(nullable: true),
                    HasMultiples = table.Column<bool>(nullable: true),
                    HasNamedIndividuals = table.Column<bool>(nullable: true),
                    HasPAYENumber = table.Column<bool>(nullable: true),
                    HasPreviousTradingName = table.Column<bool>(nullable: true),
                    HasTaxReferenceNumber = table.Column<bool>(nullable: true),
                    HasTradingName = table.Column<bool>(nullable: true),
                    HasVATNumber = table.Column<bool>(nullable: true),
                    HasWrittenAgreementsInPlace = table.Column<bool>(nullable: true),
                    IsPSCControlled = table.Column<bool>(nullable: true),
                    IsShellfish = table.Column<bool>(nullable: false),
                    LegalStatus = table.Column<int>(nullable: true),
                    NamedIndividualType = table.Column<int>(nullable: false),
                    NationalityOfShellfishWorkers = table.Column<string>(nullable: true),
                    NumberOfDirectorsOrPartners = table.Column<int>(nullable: true),
                    NumberOfMultiples = table.Column<int>(nullable: false),
                    NumberOfProperties = table.Column<int>(nullable: true),
                    NumberOfShellfishWorkers = table.Column<int>(nullable: true),
                    NumberOfVehicles = table.Column<int>(nullable: true),
                    OtherLegalStatus = table.Column<string>(nullable: true),
                    OtherMultiple = table.Column<string>(nullable: true),
                    OtherOperatingIndustry = table.Column<string>(nullable: true),
                    OtherSector = table.Column<string>(nullable: true),
                    PSCDetails = table.Column<string>(nullable: true),
                    PreviouslyWorkedInShellfish = table.Column<bool>(nullable: true),
                    SignatoryName = table.Column<string>(nullable: true),
                    SignatureDate = table.Column<DateTime>(nullable: true),
                    SubcontractorNames = table.Column<string>(nullable: true),
                    SuppliesWorkers = table.Column<bool>(nullable: true),
                    SuppliesWorkersOutsideLicensableAreas = table.Column<bool>(nullable: true),
                    TaxReferenceNumber = table.Column<string>(nullable: true),
                    TradingName = table.Column<string>(nullable: true),
                    TransportDeductedFromPay = table.Column<bool>(nullable: true),
                    TransportWorkersChoose = table.Column<bool>(nullable: true),
                    TransportsWorkersToWorkplace = table.Column<bool>(nullable: true),
                    TurnoverBand = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UsesSubcontractors = table.Column<bool>(nullable: true),
                    VATNumber = table.Column<string>(nullable: true),
                    VATRegistrationDate = table.Column<DateTime>(nullable: true),
                    WorkerContract = table.Column<int>(nullable: false),
                    WorkerSource = table.Column<int>(nullable: false),
                    WorkerSupplyMethod = table.Column<int>(nullable: false),
                    WorkerSupplyOther = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licence_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Licence_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlternativeBusinessRepresentative",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressId = table.Column<int>(nullable: true),
                    AlternativeName = table.Column<string>(nullable: true),
                    BankruptcyDate = table.Column<DateTime>(nullable: true),
                    BankruptcyNumber = table.Column<string>(nullable: true),
                    BusinessExtension = table.Column<string>(nullable: true),
                    BusinessPhoneNumber = table.Column<string>(nullable: true),
                    CountryOfBirth = table.Column<string>(nullable: true),
                    CountyOfBirth = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    DisqualificationDetails = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    HasAlternativeName = table.Column<bool>(nullable: true),
                    HasOffencesAwaitingTrial = table.Column<bool>(nullable: true),
                    HasPassport = table.Column<bool>(nullable: true),
                    HasPreviouslyHeldLicence = table.Column<bool>(nullable: true),
                    HasRestraintOrders = table.Column<bool>(nullable: true),
                    HasUnspentConvictions = table.Column<bool>(nullable: true),
                    IsDisqualifiedDirector = table.Column<bool>(nullable: true),
                    IsUndischargedBankrupt = table.Column<bool>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    LicenceId = table.Column<int>(nullable: false),
                    NationalInsuranceNumber = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    PersonalEmailAddress = table.Column<string>(nullable: true),
                    PersonalMobileNumber = table.Column<string>(nullable: true),
                    PreviousLicenceDescription = table.Column<string>(nullable: true),
                    RequiresVisa = table.Column<bool>(nullable: true),
                    TownOfBirth = table.Column<string>(nullable: true),
                    VisaDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternativeBusinessRepresentative", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlternativeBusinessRepresentative_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlternativeBusinessRepresentative_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenceCountry",
                columns: table => new
                {
                    LicenceId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceCountry", x => new { x.LicenceId, x.CountryId });
                    table.ForeignKey(
                        name: "FK_LicenceCountry_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenceCountry_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenceIndustry",
                columns: table => new
                {
                    LicenceId = table.Column<int>(nullable: false),
                    IndustryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceIndustry", x => new { x.LicenceId, x.IndustryId });
                    table.ForeignKey(
                        name: "FK_LicenceIndustry_Industry_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenceIndustry_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenceMultiple",
                columns: table => new
                {
                    LicenceId = table.Column<int>(nullable: false),
                    MultipleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceMultiple", x => new { x.LicenceId, x.MultipleId });
                    table.ForeignKey(
                        name: "FK_LicenceMultiple_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenceMultiple_Multiple_MultipleId",
                        column: x => x.MultipleId,
                        principalTable: "Multiple",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenceSector",
                columns: table => new
                {
                    LicenceId = table.Column<int>(nullable: false),
                    SectorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceSector", x => new { x.LicenceId, x.SectorId });
                    table.ForeignKey(
                        name: "FK_LicenceSector_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenceSector_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenceStatusChange",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    LicenceId = table.Column<int>(nullable: true),
                    ReasonId = table.Column<int>(nullable: true),
                    StatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceStatusChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenceStatusChange_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenceStatusChange_StatusReason_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "StatusReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenceStatusChange_LicenceStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "LicenceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NamedIndividual",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BankruptcyDate = table.Column<DateTime>(nullable: true),
                    BankruptcyNumber = table.Column<string>(nullable: true),
                    BusinessExtension = table.Column<string>(nullable: true),
                    BusinessPhoneNumber = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    DisqualificationDetails = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    HasOffencesAwaitingTrial = table.Column<bool>(nullable: true),
                    HasPreviouslyHeldLicence = table.Column<bool>(nullable: true),
                    HasRestraintOrders = table.Column<bool>(nullable: true),
                    HasUnspentConvictions = table.Column<bool>(nullable: true),
                    IsDisqualifiedDirector = table.Column<bool>(nullable: true),
                    IsUndischargedBankrupt = table.Column<bool>(nullable: true),
                    LicenceId = table.Column<int>(nullable: false),
                    PreviousLicenceDescription = table.Column<string>(nullable: true),
                    RequiresVisa = table.Column<bool>(nullable: true),
                    VisaDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamedIndividual", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NamedIndividual_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NamedJobTitle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JobTitle = table.Column<string>(nullable: true),
                    JobTitleNumber = table.Column<int>(nullable: true),
                    LicenceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamedJobTitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NamedJobTitle_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PAYENumber",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LicenceId = table.Column<int>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYENumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PAYENumber_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreviousTradingName",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BusinessName = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    LicenceId = table.Column<int>(nullable: true),
                    Town = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousTradingName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreviousTradingName_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LicenceStatusChangeLicensingStandard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LicenceStatusChangeId = table.Column<int>(nullable: false),
                    LicensingStandardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceStatusChangeLicensingStandard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenceStatusChangeLicensingStandard_LicenceStatusChange_LicenceStatusChangeId",
                        column: x => x.LicenceStatusChangeId,
                        principalTable: "LicenceStatusChange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenceStatusChangeLicensingStandard_LicensingStandard_LicensingStandardId",
                        column: x => x.LicensingStandardId,
                        principalTable: "LicensingStandard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectorOrPartner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressId = table.Column<int>(nullable: true),
                    AlternativeName = table.Column<string>(nullable: true),
                    BankruptcyDate = table.Column<DateTime>(nullable: true),
                    BankruptcyNumber = table.Column<string>(nullable: true),
                    BusinessExtension = table.Column<string>(nullable: true),
                    BusinessPhoneNumber = table.Column<string>(nullable: true),
                    CountryOfBirth = table.Column<string>(nullable: true),
                    CountyOfBirth = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    DisqualificationDetails = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    HasAlternativeName = table.Column<bool>(nullable: true),
                    HasOffencesAwaitingTrial = table.Column<bool>(nullable: true),
                    HasPassport = table.Column<bool>(nullable: true),
                    HasPreviouslyHeldLicence = table.Column<bool>(nullable: true),
                    HasRestraintOrders = table.Column<bool>(nullable: true),
                    HasUnspentConvictions = table.Column<bool>(nullable: true),
                    IsDisqualifiedDirector = table.Column<bool>(nullable: true),
                    IsPreviousPrincipalAuthority = table.Column<bool>(nullable: true),
                    IsUndischargedBankrupt = table.Column<bool>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    LicenceId = table.Column<int>(nullable: false),
                    NationalInsuranceNumber = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    PersonalEmailAddress = table.Column<string>(nullable: true),
                    PersonalMobileNumber = table.Column<string>(nullable: true),
                    PreviousLicenceDescription = table.Column<string>(nullable: true),
                    PrincipalAuthorityId = table.Column<int>(nullable: true),
                    RequiresVisa = table.Column<bool>(nullable: true),
                    TownOfBirth = table.Column<string>(nullable: true),
                    VisaDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorOrPartner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectorOrPartner_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirectorOrPartner_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrincipalAuthority",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressId = table.Column<int>(nullable: true),
                    AlternativeName = table.Column<string>(nullable: true),
                    BankruptcyDate = table.Column<DateTime>(nullable: true),
                    BankruptcyNumber = table.Column<string>(nullable: true),
                    BusinessExtension = table.Column<string>(nullable: true),
                    BusinessPhoneNumber = table.Column<string>(nullable: true),
                    CountryOfBirth = table.Column<string>(nullable: true),
                    CountyOfBirth = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    DirectorOrPartnerId = table.Column<int>(nullable: true),
                    DisqualificationDetails = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    HasAlternativeName = table.Column<bool>(nullable: true),
                    HasOffencesAwaitingTrial = table.Column<bool>(nullable: true),
                    HasPassport = table.Column<bool>(nullable: true),
                    HasPreviouslyHeldLicence = table.Column<bool>(nullable: true),
                    HasRestraintOrders = table.Column<bool>(nullable: true),
                    HasUnspentConvictions = table.Column<bool>(nullable: true),
                    ImmigrationStatus = table.Column<string>(nullable: true),
                    IsCurrent = table.Column<bool>(nullable: false),
                    IsDirector = table.Column<bool>(nullable: true),
                    IsDisqualifiedDirector = table.Column<bool>(nullable: true),
                    IsUndischargedBankrupt = table.Column<bool>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    LeaveToRemainTo = table.Column<DateTime>(nullable: true),
                    LengthOfUKWorkMonths = table.Column<int>(nullable: true),
                    LengthOfUKWorkYears = table.Column<int>(nullable: true),
                    LicenceId = table.Column<int>(nullable: false),
                    NationalInsuranceNumber = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    PermissionToWorkStatus = table.Column<int>(nullable: true),
                    PersonalEmailAddress = table.Column<string>(nullable: true),
                    PersonalMobileNumber = table.Column<string>(nullable: true),
                    PreviousExperience = table.Column<string>(nullable: true),
                    PreviousLicenceDescription = table.Column<string>(nullable: true),
                    RequiresVisa = table.Column<bool>(nullable: true),
                    TownOfBirth = table.Column<string>(nullable: true),
                    VisaDescription = table.Column<string>(nullable: true),
                    VisaNumber = table.Column<string>(nullable: true),
                    WillProvideConfirmation = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrincipalAuthority", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrincipalAuthority_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrincipalAuthority_DirectorOrPartner_DirectorOrPartnerId",
                        column: x => x.DirectorOrPartnerId,
                        principalTable: "DirectorOrPartner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrincipalAuthority_Licence_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conviction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlternativeBusinessRepresentativeId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DirectorOrPartnerId = table.Column<int>(nullable: true),
                    NamedIndividualId = table.Column<int>(nullable: true),
                    PrincipalAuthorityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conviction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conviction_AlternativeBusinessRepresentative_AlternativeBusinessRepresentativeId",
                        column: x => x.AlternativeBusinessRepresentativeId,
                        principalTable: "AlternativeBusinessRepresentative",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conviction_DirectorOrPartner_DirectorOrPartnerId",
                        column: x => x.DirectorOrPartnerId,
                        principalTable: "DirectorOrPartner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conviction_NamedIndividual_NamedIndividualId",
                        column: x => x.NamedIndividualId,
                        principalTable: "NamedIndividual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conviction_PrincipalAuthority_PrincipalAuthorityId",
                        column: x => x.PrincipalAuthorityId,
                        principalTable: "PrincipalAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OffenceAwaitingTrial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlternativeBusinessRepresentativeId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DirectorOrPartnerId = table.Column<int>(nullable: true),
                    NamedIndividualId = table.Column<int>(nullable: true),
                    PrincipalAuthorityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffenceAwaitingTrial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffenceAwaitingTrial_AlternativeBusinessRepresentative_AlternativeBusinessRepresentativeId",
                        column: x => x.AlternativeBusinessRepresentativeId,
                        principalTable: "AlternativeBusinessRepresentative",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OffenceAwaitingTrial_DirectorOrPartner_DirectorOrPartnerId",
                        column: x => x.DirectorOrPartnerId,
                        principalTable: "DirectorOrPartner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OffenceAwaitingTrial_NamedIndividual_NamedIndividualId",
                        column: x => x.NamedIndividualId,
                        principalTable: "NamedIndividual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OffenceAwaitingTrial_PrincipalAuthority_PrincipalAuthorityId",
                        column: x => x.PrincipalAuthorityId,
                        principalTable: "PrincipalAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RestraintOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlternativeBusinessRepresentativeId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DirectorOrPartnerId = table.Column<int>(nullable: true),
                    NamedIndividualId = table.Column<int>(nullable: true),
                    PrincipalAuthorityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestraintOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestraintOrder_AlternativeBusinessRepresentative_AlternativeBusinessRepresentativeId",
                        column: x => x.AlternativeBusinessRepresentativeId,
                        principalTable: "AlternativeBusinessRepresentative",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RestraintOrder_DirectorOrPartner_DirectorOrPartnerId",
                        column: x => x.DirectorOrPartnerId,
                        principalTable: "DirectorOrPartner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RestraintOrder_NamedIndividual_NamedIndividualId",
                        column: x => x.NamedIndividualId,
                        principalTable: "NamedIndividual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RestraintOrder_PrincipalAuthority_PrincipalAuthorityId",
                        column: x => x.PrincipalAuthorityId,
                        principalTable: "PrincipalAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeBusinessRepresentative_AddressId",
                table: "AlternativeBusinessRepresentative",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeBusinessRepresentative_LicenceId",
                table: "AlternativeBusinessRepresentative",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Conviction_AlternativeBusinessRepresentativeId",
                table: "Conviction",
                column: "AlternativeBusinessRepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Conviction_DirectorOrPartnerId",
                table: "Conviction",
                column: "DirectorOrPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Conviction_NamedIndividualId",
                table: "Conviction",
                column: "NamedIndividualId");

            migrationBuilder.CreateIndex(
                name: "IX_Conviction_PrincipalAuthorityId",
                table: "Conviction",
                column: "PrincipalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorOrPartner_AddressId",
                table: "DirectorOrPartner",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorOrPartner_LicenceId",
                table: "DirectorOrPartner",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorOrPartner_PrincipalAuthorityId",
                table: "DirectorOrPartner",
                column: "PrincipalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Licence_AddressId",
                table: "Licence",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Licence_UserId",
                table: "Licence",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceCountry_CountryId",
                table: "LicenceCountry",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceIndustry_IndustryId",
                table: "LicenceIndustry",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceMultiple_MultipleId",
                table: "LicenceMultiple",
                column: "MultipleId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceSector_SectorId",
                table: "LicenceSector",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceStatusChange_LicenceId",
                table: "LicenceStatusChange",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceStatusChange_ReasonId",
                table: "LicenceStatusChange",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceStatusChange_StatusId",
                table: "LicenceStatusChange",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceStatusChangeLicensingStandard_LicenceStatusChangeId",
                table: "LicenceStatusChangeLicensingStandard",
                column: "LicenceStatusChangeId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceStatusChangeLicensingStandard_LicensingStandardId",
                table: "LicenceStatusChangeLicensingStandard",
                column: "LicensingStandardId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceStatusNextStatus_NextStatusId",
                table: "LicenceStatusNextStatus",
                column: "NextStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceStatusNextStatus_NextStatusId1",
                table: "LicenceStatusNextStatus",
                column: "NextStatusId1");

            migrationBuilder.CreateIndex(
                name: "IX_NamedIndividual_LicenceId",
                table: "NamedIndividual",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_NamedJobTitle_LicenceId",
                table: "NamedJobTitle",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_OffenceAwaitingTrial_AlternativeBusinessRepresentativeId",
                table: "OffenceAwaitingTrial",
                column: "AlternativeBusinessRepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_OffenceAwaitingTrial_DirectorOrPartnerId",
                table: "OffenceAwaitingTrial",
                column: "DirectorOrPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OffenceAwaitingTrial_NamedIndividualId",
                table: "OffenceAwaitingTrial",
                column: "NamedIndividualId");

            migrationBuilder.CreateIndex(
                name: "IX_OffenceAwaitingTrial_PrincipalAuthorityId",
                table: "OffenceAwaitingTrial",
                column: "PrincipalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_PAYENumber_LicenceId",
                table: "PAYENumber",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousTradingName_LicenceId",
                table: "PreviousTradingName",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PrincipalAuthority_AddressId",
                table: "PrincipalAuthority",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PrincipalAuthority_DirectorOrPartnerId",
                table: "PrincipalAuthority",
                column: "DirectorOrPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PrincipalAuthority_LicenceId",
                table: "PrincipalAuthority",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_RestraintOrder_AlternativeBusinessRepresentativeId",
                table: "RestraintOrder",
                column: "AlternativeBusinessRepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_RestraintOrder_DirectorOrPartnerId",
                table: "RestraintOrder",
                column: "DirectorOrPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RestraintOrder_NamedIndividualId",
                table: "RestraintOrder",
                column: "NamedIndividualId");

            migrationBuilder.CreateIndex(
                name: "IX_RestraintOrder_PrincipalAuthorityId",
                table: "RestraintOrder",
                column: "PrincipalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusReason_LicenceStatusId",
                table: "StatusReason",
                column: "LicenceStatusId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AddressId",
                schema: "dbo",
                table: "AspNetUsers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_DirectorOrPartner_PrincipalAuthority_PrincipalAuthorityId",
                table: "DirectorOrPartner",
                column: "PrincipalAuthorityId",
                principalTable: "PrincipalAuthority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DirectorOrPartner_Address_AddressId",
                table: "DirectorOrPartner");

            migrationBuilder.DropForeignKey(
                name: "FK_Licence_Address_AddressId",
                table: "Licence");

            migrationBuilder.DropForeignKey(
                name: "FK_PrincipalAuthority_Address_AddressId",
                table: "PrincipalAuthority");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Address_AddressId",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_DirectorOrPartner_Licence_LicenceId",
                table: "DirectorOrPartner");

            migrationBuilder.DropForeignKey(
                name: "FK_PrincipalAuthority_Licence_LicenceId",
                table: "PrincipalAuthority");

            migrationBuilder.DropForeignKey(
                name: "FK_PrincipalAuthority_DirectorOrPartner_DirectorOrPartnerId",
                table: "PrincipalAuthority");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Conviction");

            migrationBuilder.DropTable(
                name: "LicenceCountry");

            migrationBuilder.DropTable(
                name: "LicenceIndustry");

            migrationBuilder.DropTable(
                name: "LicenceMultiple");

            migrationBuilder.DropTable(
                name: "LicenceSector");

            migrationBuilder.DropTable(
                name: "LicenceStatusChangeLicensingStandard");

            migrationBuilder.DropTable(
                name: "LicenceStatusNextStatus");

            migrationBuilder.DropTable(
                name: "NamedJobTitle");

            migrationBuilder.DropTable(
                name: "OffenceAwaitingTrial");

            migrationBuilder.DropTable(
                name: "PAYENumber");

            migrationBuilder.DropTable(
                name: "PreviousTradingName");

            migrationBuilder.DropTable(
                name: "RestraintOrder");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Industry");

            migrationBuilder.DropTable(
                name: "Multiple");

            migrationBuilder.DropTable(
                name: "Sector");

            migrationBuilder.DropTable(
                name: "LicenceStatusChange");

            migrationBuilder.DropTable(
                name: "LicensingStandard");

            migrationBuilder.DropTable(
                name: "AlternativeBusinessRepresentative");

            migrationBuilder.DropTable(
                name: "NamedIndividual");

            migrationBuilder.DropTable(
                name: "StatusReason");

            migrationBuilder.DropTable(
                name: "LicenceStatus");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Licence");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DirectorOrPartner");

            migrationBuilder.DropTable(
                name: "PrincipalAuthority");
        }
    }
}
