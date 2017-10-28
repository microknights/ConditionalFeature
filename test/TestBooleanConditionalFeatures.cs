using System;
using System.IO;
using System.Reflection;
using MicroKnights.ConditionalFeature.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MicroKnights.ConditionalFeature.Test
{
    public class TestBooleanConditionalFeatures
    {
        #region  MyCustom... test classes
        private class MyCustomOverrideBooleanConditionalTrueFeature : BooleanConditionalFeature
        {
            public MyCustomOverrideBooleanConditionalTrueFeature() 
            {}

            public MyCustomOverrideBooleanConditionalTrueFeature(Func<bool> funcResolveValue, bool defaultValue = default(bool)) : base(funcResolveValue, defaultValue)
            {}

            protected override bool ResolveFeatureValue()
            {
                return true;
            }
        }
        private class MyCustomOverrideBooleanConditionalFalseFeature : BooleanConditionalFeature
        {
            public MyCustomOverrideBooleanConditionalFalseFeature()
            { }

            public MyCustomOverrideBooleanConditionalFalseFeature(Func<bool> funcResolveValue, bool defaultValue = default(bool)) : base(funcResolveValue, defaultValue)
            {
            }

            protected override bool ResolveFeatureValue()
            {
                return false;
            }
        }

        private class MyCustomConstructorBooleanConditionalTrueFeature : BooleanConditionalFeature
        {
            public MyCustomConstructorBooleanConditionalTrueFeature()
                : base(true)
            { }
        }

        private class MyCustomConstructorBooleanConditionalFalseFeature : BooleanConditionalFeature
        {
            public MyCustomConstructorBooleanConditionalFalseFeature()
                : base(false)
            { }
        }

        private class MyConfigurationRootTrueBooleanFeature : BooleanConditionalConfigurationFeature
        {
            public MyConfigurationRootTrueBooleanFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationRootFalseBooleanFeature : BooleanConditionalConfigurationFeature
        {
            public MyConfigurationRootFalseBooleanFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationRootFormatExceptionBooleanFeature : BooleanConditionalConfigurationFeature
        {
            public MyConfigurationRootFormatExceptionBooleanFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }

        private class MyConfigurationSectionTrueBooleanFeature : BooleanConditionalConfigurationFeature
        {
            public MyConfigurationSectionTrueBooleanFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationSectionFalseBooleanFeature : BooleanConditionalConfigurationFeature
        {
            public MyConfigurationSectionFalseBooleanFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationSectionFormatExceptionBooleanFeature : BooleanConditionalConfigurationFeature
        {
            public MyConfigurationSectionFormatExceptionBooleanFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }

        private class MyConfigurationMissingDefaultToTrueBooleanFeature : BooleanConditionalConfigurationFeature
        {
            public MyConfigurationMissingDefaultToTrueBooleanFeature(IConfiguration configuration, bool defaultValue)
                : base(configuration, defaultValue)
            { }
        }
        #endregion

        [Fact]
        public void TestCustomOverrideFeatures()
        {
            var customTrue = new MyCustomOverrideBooleanConditionalTrueFeature();
            Assert.True(customTrue.IsEnabled, $"{customTrue.GetType().Name} failed");

            var customFalse = new MyCustomOverrideBooleanConditionalFalseFeature();
            Assert.True(customFalse.IsDisabled, $"{customFalse.GetType().Name} failed");
        }

        [Fact]
        public void TestCustomConstructorFeatures()
        {
            var customTrue = new MyCustomConstructorBooleanConditionalTrueFeature();
            Assert.True(customTrue.IsEnabled, $"{customTrue.GetType().Name} failed");

            var customFalse = new MyCustomConstructorBooleanConditionalFalseFeature();
            Assert.True(customFalse.IsDisabled, $"{customFalse.GetType().Name} failed");
        }

        [Fact]
        public void TestCustomDelegateFeatures()
        {
            var customDelegateTrue = new MyCustomOverrideBooleanConditionalFalseFeature(() => true);
            Assert.True(customDelegateTrue.IsEnabled, $"{customDelegateTrue.GetType().Name} delegate failed");

            var customDelegateFalse = new MyCustomOverrideBooleanConditionalTrueFeature(() => false);
            Assert.True(customDelegateFalse.IsDisabled, $"{customDelegateFalse.GetType().Name} delegate failed");
        }

        [Fact]
        public void TestAppSettingsFeatures()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(GetType().Assembly.Location))
                .AddJsonFile("appsettings.json");
            var config = builder.Build();


            var rootTrue = new MyConfigurationRootTrueBooleanFeature(config);
            Assert.True(rootTrue.IsEnabled, $"{rootTrue.GetType().Name} failed");

            var rootFalse = new MyConfigurationRootFalseBooleanFeature(config);
            Assert.True(rootFalse.IsDisabled, $"{rootFalse.GetType().Name} failed");

            var rootFormatException = new MyConfigurationRootFormatExceptionBooleanFeature(config);
            Assert.Throws<FormatException>(() => rootFormatException.IsEnabled);


            var sectionTrue = new MyConfigurationSectionTrueBooleanFeature(config);
            Assert.True(sectionTrue.IsEnabled, $"{sectionTrue.GetType().Name} failed");

            var sectionFalse = new MyConfigurationSectionFalseBooleanFeature(config);
            Assert.True(sectionFalse.IsDisabled, $"{sectionFalse.GetType().Name} failed");

            var sectionFormatException = new MyConfigurationSectionFormatExceptionBooleanFeature(config);
            Assert.Throws<FormatException>(() => sectionFormatException.IsEnabled);

            var missingDefaultTrue = new MyConfigurationMissingDefaultToTrueBooleanFeature(config, true);
            Assert.True(missingDefaultTrue.IsEnabled, $"{missingDefaultTrue.GetType().Name} failed");
        }
    }
}