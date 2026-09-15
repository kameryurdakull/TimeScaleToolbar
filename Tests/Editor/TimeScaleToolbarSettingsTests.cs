using System.Globalization;
using Kamer.TimeScaleToolbar.Editor;
using NUnit.Framework;

namespace Kamer.TimeScaleToolbar.Tests
{
    public sealed class TimeScaleToolbarSettingsTests
    {
        [TestCase(-1f, 0f)]
        [TestCase(0f, 0f)]
        [TestCase(1.5f, 1.5f)]
        [TestCase(3f, 3f)]
        [TestCase(5f, 3f)]
        public void Clamp_ConstrainsValuesToSupportedRange(float value, float expected)
        {
            Assert.That(TimeScaleToolbarSettings.Clamp(value), Is.EqualTo(expected));
        }

        [Test]
        public void IsSynchronized_UsesConfiguredTolerance()
        {
            Assert.That(TimeScaleToolbarSettings.IsSynchronized(1f, 1.0005f), Is.True);
            Assert.That(TimeScaleToolbarSettings.IsSynchronized(1f, 1.01f), Is.False);
        }

        [Test]
        public void Format_IsCultureInvariant()
        {
            var previousCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");
                Assert.That(TimeScaleToolbarSettings.Format(1.25f), Is.EqualTo("1.25x"));
            }
            finally
            {
                CultureInfo.CurrentCulture = previousCulture;
            }
        }
    }
}
