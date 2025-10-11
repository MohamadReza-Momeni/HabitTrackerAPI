using System.ComponentModel.DataAnnotations;
using HabitTrackerAPI.Models.Enums;

namespace HabitTrackerAPI.Validation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class HabitCounterValidationAttribute : ValidationAttribute
    {
        public string TrackingModeProperty { get; }
        public string PositiveCounterProperty { get; }
        public string NegativeCounterProperty { get; }

        public HabitCounterValidationAttribute(
            string trackingModeProperty = "TrackingMode",
            string positiveCounterProperty = "PositiveCounter",
            string negativeCounterProperty = "NegativeCounter")
        {
            TrackingModeProperty = trackingModeProperty;
            PositiveCounterProperty = positiveCounterProperty;
            NegativeCounterProperty = negativeCounterProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            var targetType = value.GetType();
            var trackingModeInfo = targetType.GetProperty(TrackingModeProperty);
            if (trackingModeInfo == null)
                return ValidationResult.Success;

            var trackingModeValue = trackingModeInfo.GetValue(value);
            if (trackingModeValue is not HabitTrackingMode trackingMode)
                return ValidationResult.Success;

            var positiveInfo = targetType.GetProperty(PositiveCounterProperty);
            var negativeInfo = targetType.GetProperty(NegativeCounterProperty);

            var tracksPositive = trackingMode is HabitTrackingMode.PositiveOnly or HabitTrackingMode.Both;
            var tracksNegative = trackingMode is HabitTrackingMode.NegativeOnly or HabitTrackingMode.Both;

            if (!tracksPositive && !tracksNegative)
            {
                return new ValidationResult(
                    "Tracking mode must include at least one counter.",
                    new[] { trackingModeInfo.Name });
            }

            var positiveValue = positiveInfo?.GetValue(value);
            var negativeValue = negativeInfo?.GetValue(value);

            if (tracksPositive && positiveValue is null)
            {
                return new ValidationResult(
                    "PositiveCounter is required when tracking positive outcomes.",
                    new[] { positiveInfo?.Name ?? PositiveCounterProperty });
            }

            if (!tracksPositive && positiveValue is not null)
            {
                return new ValidationResult(
                    "PositiveCounter must be omitted when the habit does not track positive outcomes.",
                    new[] { positiveInfo?.Name ?? PositiveCounterProperty });
            }

            if (tracksNegative && negativeValue is null)
            {
                return new ValidationResult(
                    "NegativeCounter is required when tracking negative outcomes.",
                    new[] { negativeInfo?.Name ?? NegativeCounterProperty });
            }

            if (!tracksNegative && negativeValue is not null)
            {
                return new ValidationResult(
                    "NegativeCounter must be omitted when the habit does not track negative outcomes.",
                    new[] { negativeInfo?.Name ?? NegativeCounterProperty });
            }

            return ValidationResult.Success;
        }
    }
}
