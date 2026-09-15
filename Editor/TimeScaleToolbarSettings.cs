using System.Globalization;
using UnityEngine;

namespace Kamer.TimeScaleToolbar.Editor
{
    internal static class TimeScaleToolbarSettings
    {
        public const string OverlayId = "Kamer/Time Scale Toolbar";
        public const string ElementId = OverlayId + "/Slider";
        public const string DisplayName = "Time Scale";
        public const string ElementName = "Time Scale Slider";
        public const string LabelText = "Time";
        public const string ResetButtonText = "1x";
        public const string Tooltip = "Adjust Time.timeScale while the Editor is in Play Mode.";
        public const string ResetTooltip = "Reset Time.timeScale to 1x.";
        public const string ToolbarSettingsClass = "unity-tool-settings";
        public const float MinimumTimeScale = 0f;
        public const float MaximumTimeScale = 3f;
        public const float DefaultTimeScale = 1f;
        public const float ValueComparisonTolerance = 0.001f;
        public const float SliderWidth = 120f;
        public const float ValueLabelWidth = 36f;

        private const string ValueFormat = "0.00x";

        public static float Clamp(float value)
        {
            return Mathf.Clamp(value, MinimumTimeScale, MaximumTimeScale);
        }

        public static bool IsSynchronized(float displayedValue, float runtimeValue)
        {
            return Mathf.Abs(displayedValue - runtimeValue) <= ValueComparisonTolerance;
        }

        public static string Format(float value)
        {
            return Clamp(value).ToString(ValueFormat, CultureInfo.InvariantCulture);
        }
    }
}
