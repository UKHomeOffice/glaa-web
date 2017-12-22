using System;
using System.ComponentModel.DataAnnotations;
using GLAA.Domain.Models;

namespace GLAA.ViewModels.LicenceApplication
{
    public class DateViewModel
    {
        [Range(1, 31)]
        public int? Day { get; set; }

        [Range(1, 12)]
        public int? Month { get; set; }
        
        [Range(1000, 3000)]
        public int? Year { get; set; }

        public DateTime? Date
        {
            get
            {
                if (Year.HasValue && Month.HasValue && Day.HasValue)
                {
                    return new DateTime(Year.Value, Month.Value, Day.Value);
                }
                return null;
            }

            set
            {
                if (value.HasValue)
                {
                    this.Day = value.Value.Day;
                    this.Month = value.Value.Month;
                    this.Year = value.Value.Year;
                }
            }

        }

        public override string ToString()
        {
            return Date?.ToString("dd/MM/yyyy") ?? string.Empty;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="dateFormat">The custom date and time format string to use when converting the Date.</param>
        /// <returns></returns>
        public string ToString(string dateFormat)
        {
            return Date?.ToString(dateFormat) ?? string.Empty;
        }
    }
}