using System.Globalization;
using System.Windows.Controls;

namespace GeneralPacketSender
{
    public class RangeValidator : ValidationRule
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public RangeValidator()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double valueEntered = 0;

            try
            {
                if (((string)value).Length > 0)
                    valueEntered = double.Parse((string)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            if ((valueEntered < Min) || (valueEntered > Max))
            {
                return new ValidationResult(false, $"Out of Range: {Min}-{Max}.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
