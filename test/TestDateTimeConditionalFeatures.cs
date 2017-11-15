using System;
using System.IO;
using System.Reflection;
using MicroKnights.ConditionalFeature.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MicroKnights.ConditionalFeature.Test
{
    public class TestDateTimeConditionalFeatures
    {
        #region  MyCustom... test classes
        private class MyCustomOverrideDateTimeConditionalMinFeature : DateTimeConditionalFeature
        {
            protected override DateTime ResolveFeatureValue()
            {
                return DateTime.MinValue;
            }
        }
        private class MyCustomOverrideDateTimeConditionalMaxFeature : DateTimeConditionalFeature
        {
            protected override DateTime ResolveFeatureValue()
            {
                return DateTime.MaxValue;
            }
        }
        private class MyCustomDelegateDateTimeConditionalFeature : DateTimeConditionalFeature
        {
            public MyCustomDelegateDateTimeConditionalFeature(Func<DateTime> funcResolveValue, DateTime defaultValue = default(DateTime)) : base(funcResolveValue, defaultValue)
            {
            }

            public MyCustomDelegateDateTimeConditionalFeature(Lazy<DateTime> lazyResolveValue, DateTime defaultValue = default(DateTime)) : base(lazyResolveValue, defaultValue)
            {
            }
        }

        private class MyCustomConstructorDateTimeConditional19910123Feature : DateTimeConditionalFeature
        {
            public MyCustomConstructorDateTimeConditional19910123Feature()
                : base(new DateTime(1991,01,23))
            { }
        }

        private class MyCustomConstructorDateTimeConditional20011020Feature : DateTimeConditionalFeature
        {
            public MyCustomConstructorDateTimeConditional20011020Feature()
                : base(new DateTime(2001,10,20))
            { }
        }


        private class MyConfigurationRootMinDateTimeFeature : DateTimeConditionalConfigurationFeature
        {
            public MyConfigurationRootMinDateTimeFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationRootMaxDateTimeFeature : DateTimeConditionalConfigurationFeature
        {
            public MyConfigurationRootMaxDateTimeFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationRootFormatExceptionDateTimeFeature : DateTimeConditionalConfigurationFeature
        {
            public MyConfigurationRootFormatExceptionDateTimeFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }

        private class MyConfigurationSectionMinDateTimeFeature : DateTimeConditionalConfigurationFeature
        {
            public MyConfigurationSectionMinDateTimeFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationSectionMaxDateTimeFeature : DateTimeConditionalConfigurationFeature
        {
            public MyConfigurationSectionMaxDateTimeFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationSectionFormatExceptionDateTimeFeature : DateTimeConditionalConfigurationFeature
        {
            public MyConfigurationSectionFormatExceptionDateTimeFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }

        private class MyConfigurationMissingDefaultTo19911122DateTimeFeature : DateTimeConditionalConfigurationFeature
        {
            public MyConfigurationMissingDefaultTo19911122DateTimeFeature(IConfiguration configuration, DateTime defaultValue)
                : base(configuration, defaultValue)
            { }
        }
        #endregion

        [Fact]
        public void TestCustomOverrideFeatures()
        {
            var customMin = new MyCustomOverrideDateTimeConditionalMinFeature();
            Assert.True(customMin.IsBefore(DateTime.MinValue.AddMilliseconds(1)), $"{customMin.GetType().Name} failed");

            var customMax = new MyCustomOverrideDateTimeConditionalMaxFeature();
            Assert.True(customMax.IsAfter(DateTime.MaxValue.Subtract(TimeSpan.FromMilliseconds(1))), $"{customMax.GetType().Name} failed");
        }

        [Fact]
        public void TestCustomConstructorFeatures()
        {
            var custom19910123 = new MyCustomConstructorDateTimeConditional19910123Feature();
            Assert.True(custom19910123.IsBetween(new DateTime(1991,01,22), new DateTime(1991, 01, 24)), $"{custom19910123.GetType().Name} failed");

            var custom20011020 = new MyCustomConstructorDateTimeConditional20011020Feature();
            Assert.True(custom20011020.IsOutside(new DateTime(1991, 01, 22), new DateTime(1991, 01, 24)), $"{custom20011020.GetType().Name} failed");
            Assert.True(custom20011020.IsOutside(new DateTime(2002, 01, 22), new DateTime(2002, 01, 24)), $"{custom20011020.GetType().Name} failed");
        }

        [Fact]
        public void TestCustomDelegateFeatures()
        {
            var customDelegate20010203040506 = new MyCustomDelegateDateTimeConditionalFeature(() => new DateTime(2001,2,3,4,5,6));
            Assert.True(customDelegate20010203040506.FeatureValue == new DateTime(2001, 2, 3, 4, 5, 6), $"{customDelegate20010203040506.GetType().Name} delegate failed");

            var customDelegate20090807060504 = new MyCustomDelegateDateTimeConditionalFeature(new Lazy<DateTime>(()=>new DateTime(2009,8,7,6,5,4)));
            Assert.True(customDelegate20090807060504.FeatureValue == new DateTime(2009, 8, 7, 6, 5, 4), $"{customDelegate20090807060504.GetType().Name} delegate failed");
        }

        [Fact]
        public void TestAppSettingsFeatures()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(GetType().GetTypeInfo().Assembly.Location))
                .AddJsonFile("appsettings.json");
            var config = builder.Build();


            var rootMin = new MyConfigurationRootMinDateTimeFeature(config);
            Assert.True(rootMin.FeatureValue == DateTime.MinValue, $"{rootMin.GetType().Name} failed");

            var rootMax = new MyConfigurationRootMaxDateTimeFeature(config);
            Assert.True(rootMax.FeatureValue == DateTime.MaxValue, $"{rootMax.GetType().Name} failed");

            var rootFormatException = new MyConfigurationRootFormatExceptionDateTimeFeature(config);
            Assert.Throws<FormatException>(() => rootFormatException.IsAfter(DateTime.MaxValue));


            var sectionMin = new MyConfigurationSectionMinDateTimeFeature(config);
            Assert.True(sectionMin.FeatureValue == DateTime.MinValue, $"{sectionMin.GetType().Name} failed");

            var sectionMax = new MyConfigurationSectionMaxDateTimeFeature(config);
            Assert.True(sectionMax.FeatureValue == DateTime.MaxValue, $"{sectionMax.GetType().Name} failed");

            var sectionFormatException = new MyConfigurationSectionFormatExceptionDateTimeFeature(config);
            Assert.Throws<FormatException>(() => sectionFormatException.IsBefore(DateTime.MinValue));

            var missingDefault191919 = new MyConfigurationMissingDefaultTo19911122DateTimeFeature(config, new DateTime(1919,1,9));
            Assert.True(missingDefault191919.FeatureValue == new DateTime(1919, 1, 9), $"{missingDefault191919.GetType().Name} failed");
        }
    }
}