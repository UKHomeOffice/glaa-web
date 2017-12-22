using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using GLAA.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.LicenceApplication
{
    public class LicenceStatusHistoryViewModel
    {
        public ICollection<LicenceStatusViewModel> LicenceStatusHistory { get; set; }
    }

    public class LicenceStatusViewModel
    {
        public int Id { get; set; }
        public string ExternalDescription { get; set; }
        public string InternalStatus { get; set; }
        public string InternalDescription { get; set; }
        public string ActiveCheckDescription { get; set; }
        public bool ShowInPublicRegister { get; set; }
        public bool RequireNonCompliantStandards { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CssClassStem { get; set; }
        public StatusAdminCategory AdminCategory { get; set; }
        // The reason for this status to be set
        public string SelectedReason { get; set; }
        public IEnumerable<NextStatusViewModel> NextStatuses { get; set; }
        public IEnumerable<ReasonViewModel> StatusReasons { get; set; }

        /// <summary>
        /// A list of areas of non-compliance
        /// Expected for statuses 410, 710 and 720
        /// </summary>
        public IEnumerable<LicensingStandardViewModel> NonCompliantStandards { get; set; }

        public string ExternalClassName => $"external-status {CssClassStem}";
        public string InternalClassName => $"external-status {CssClassStem}";
    }

    public class LicensingStandardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCritical { get; set; }
    }

    public class NextStatusViewModel
    {
        public int Id { get; set; }
        public string InternalStatus { get; set; }
        public string InternalDescription { get; set; }
        public bool RequireNonCompliantStandards { get; set; }
        // Possible reasons for this status
        public IEnumerable<ReasonViewModel> Reasons { get; set; }
        public SelectListItem DropDownItem => new SelectListItem
        {
            Value = Id.ToString(),
            Text = InternalStatus,
            Selected = false
        };

        public IEnumerable<SelectListItem> ReasonDropDown => Reasons.Select(r => r.SelectListItem);
    }

    public class ReasonViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public SelectListItem SelectListItem =>
            new SelectListItem {Selected = false, Value = Id.ToString(), Text = Description};
    }

    public class ALCStatusViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateApplied { get; set; }
    }

    public static class EnumHelpers
    {
        private static Random rnd = new Random();

        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(rnd.Next(v.Length));
        }

        public static T RandomEnumValue<T>(int startRange, int endRange)
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(rnd.Next(startRange, endRange));
        }
    }

    public static class AttributeExtensions
    {
        public static string GetDescription<T>(this T value)
        {
            return value.GetType().GetField(value.ToString()).GetAttributeValue((DescriptionAttribute da) => da.Description);
        }

        /// <summary>
        /// Adapted from https://stackoverflow.com/questions/2656189/how-do-i-read-an-attribute-on-a-class-at-runtime
        /// </summary>
        public static TValue GetAttributeValue<TAttribute, TValue>(this ICustomAttributeProvider field, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var att = field.GetCustomAttributes(typeof(TAttribute), true).SingleOrDefault() as TAttribute;

            if (att != null)
            {
                var result = valueSelector(att);
                return result;
            }

            return default(TValue);
        }

        public static bool HasAttribute<TAttribute>(this ICustomAttributeProvider field) where TAttribute : Attribute
        {
            return field.GetCustomAttributes(typeof(TAttribute), true).Any();
        }
    }
}