using System.ComponentModel.DataAnnotations;

namespace GLAA.ViewModels.LicenceApplication
{
    public class TimeSpanViewModel
    {
        private int? months;
        private int? years;

        [Range(0, int.MaxValue)]
        public int? Months
        {
            get { return months % 12; }
            set { months = value; }
        }

        [Range(0, int.MaxValue)]
        public int? Years
        {
            get
            {
                if (!years.HasValue && (!months.HasValue || months.Value < 12))
                {
                    return null;
                }

                //var years = model.LengthOfUKWork.Years ?? 0;
                var monthYears = months / 12 ?? 0;

                return (years ?? 0) + monthYears;
            }
            set { years = value; }
        }

        public bool IsValid()
        {
            return Months.HasValue || Years.HasValue;
        }

        public override string ToString()
        {
            if (!Months.HasValue && !Years.HasValue)
            {
                return string.Empty;
            }

            var stringYears = Years.HasValue ? $"{Years.Value} years" : string.Empty;
            var stringMonths = Months.HasValue ? $"{Months.Value} months" : string.Empty;

            return $"{stringYears} {stringMonths}";
        }
    }
}
