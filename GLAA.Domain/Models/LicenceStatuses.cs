using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GLAA.Domain.Models
{
    public class LicenceStatus : IId
    {
        [NotMapped]
        private IEnumerable<int> _nextStatusIds;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string ExternalDescription { get; set; }
        public string InternalStatus { get; set; }
        public string InternalDescription { get; set; }
        public string ActiveCheckDescription { get; set; }
        public bool ShowInPublicRegister { get; set; }
        public bool RequireNonCompliantStandards { get; set; }
        public string CssClassStem { get; set; }
        public StatusAdminCategory AdminCategory { get; set; }
        public virtual ICollection<LicenceStatusNextStatus> NextStatuses { get; set; }
        public virtual ICollection<StatusReason> StatusReasons { get; set; }
        public virtual ICollection<LicenceStatusChange> LicenceStatusChanges { get; set; }
        public bool LicenceIssued { get; set; }
        public bool LicenceSubmitted { get; set; }
        // TODO: Remove when no longer needed for seeding
        [NotMapped]
        public IEnumerable<int> NextStatusIds
        {
            get
            {
                if (_nextStatusIds == null || !_nextStatusIds.Any())
                {
                    return NextStatuses?.Select(s => s.Id);
                }
                return _nextStatusIds;
            }
            set
            {
                _nextStatusIds = value;
            }
        }

        public LicenceStatus()
        {
            ShowInPublicRegister = false;
            RequireNonCompliantStandards = false;
        }

        [NotMapped]
        public bool IsApplication => Id < 400 || (Id > 500 && Id < 560);
        [NotMapped]
        public bool IsLicence => !IsApplication;
    }

    public class LicenceStatusNextStatus
    {
        public int Id { get; set; }
        public virtual LicenceStatus LicenceStatus { get; set; }

        public int NextStatusId { get; set; }
        public virtual LicenceStatus NextStatus { get; set; }
    }

    public class StatusReason : IId
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<LicenceStatusChange> LicenceStatusChanges { get; set; }
    }

    public class LicensingStandard : IId
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCritical { get; set; }
        public virtual ICollection<LicenceStatusChangeLicensingStandard> LicenceStatusChanges { get; set; }
    }

    public class LicenceStatusChangeLicensingStandard : IId
    {
        public int Id
        {
            get { return LicenceStatusChangeId; }
            set { LicenceStatusChangeId = value; }
        }

        public int LicensingStandardId { get; set; }
        public LicensingStandard LicensingStandard { get; set; }
        public int LicenceStatusChangeId { get; set; }
        public LicenceStatusChange LicenceStatusChange { get; set; }
    }

    public class LicenceStatusChange : IId
    {
        [Key]
        public int Id { get; set; }
        public Licence Licence { get; set; }
        public LicenceStatus Status { get; set; }
        public StatusReason Reason { get; set; }
        public DateTime? DateCreated { get; set; }
        public virtual ICollection<LicenceStatusChangeLicensingStandard> NonCompliantStandards { get; set; }
    }
}
