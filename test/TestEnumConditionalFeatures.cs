using System;
using System.IO;
using MicroKnights.ConditionalFeature.Configuation;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MicroKnights.ConditionalFeature.Test
{
    public class TestEnumConditionalFeatures
    {
        private enum MyColorEnum
        {
            Red,
            Green,
            Blue,
            Transparent
        }

        private class MyCustomEnumConditionalRedFeature : EnumConditionalFeature<MyColorEnum>
        {
            public MyCustomEnumConditionalRedFeature(MyColorEnum defaultValue = default(MyColorEnum)) : base(defaultValue)
            {
            }

            public MyCustomEnumConditionalRedFeature(Func<MyColorEnum> funcResolveValue, MyColorEnum defaultValue = default(MyColorEnum)) : base(funcResolveValue, defaultValue)
            {
            }

            protected override MyColorEnum ResolveFeatureValue()
            {
                return MyColorEnum.Red;
            }
        }

        private class MyCustomEnumConditionalGreenFeature : EnumConditionalFeature<MyColorEnum>
        {
            public MyCustomEnumConditionalGreenFeature(MyColorEnum defaultValue = default(MyColorEnum)) : base(defaultValue)
            {
            }

            public MyCustomEnumConditionalGreenFeature(Func<MyColorEnum> funcResolveValue, MyColorEnum defaultValue = default(MyColorEnum)) : base(funcResolveValue, defaultValue)
            {
            }

            protected override MyColorEnum ResolveFeatureValue()
            {
                return MyColorEnum.Green;
            }
        }


        private class MyConfigurationRootRedEnumFeature : EnumConditionalConfigurationFeature<MyColorEnum>
        {
            public MyConfigurationRootRedEnumFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationRootGreenEnumFeature : EnumConditionalConfigurationFeature<MyColorEnum>
        {
            public MyConfigurationRootGreenEnumFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationRootFormatExceptionEnumFeature :  EnumConditionalConfigurationFeature<MyColorEnum>
        {
            public MyConfigurationRootFormatExceptionEnumFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }

        private class MyConfigurationSectionRedEnumFeature : EnumConditionalConfigurationFeature<MyColorEnum>
        {
            public MyConfigurationSectionRedEnumFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationSectionGreenEnumFeature : EnumConditionalConfigurationFeature<MyColorEnum>
        {
            public MyConfigurationSectionGreenEnumFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }
        private class MyConfigurationSectionFormatExceptionEnumFeature : EnumConditionalConfigurationFeature<MyColorEnum>
        {
            public MyConfigurationSectionFormatExceptionEnumFeature(IConfiguration configuration)
                : base(configuration)
            { }
        }

        private class MyConfigurationMissingDefaultToTransparentEnumFeature : EnumConditionalConfigurationFeature<MyColorEnum>
        {
            public MyConfigurationMissingDefaultToTransparentEnumFeature(IConfiguration configuration, MyColorEnum defaultValue)
                : base(configuration, defaultValue)
            { }
        }


        [Fact]
        public void TestCustomOverrideFeatures()
        {
            var customGreen = new MyCustomEnumConditionalGreenFeature();
            Assert.True(customGreen.Is(MyColorEnum.Green), $"{customGreen.GetType().Name} failed");

            var customRed = new MyCustomEnumConditionalRedFeature();
            Assert.True(customRed.Is(MyColorEnum.Red), $"{customRed.GetType().Name} failed");
        }

        [Fact]
        public void TestCustomDelegateFeatures()
        {
            var customDelegateGreen = new MyCustomEnumConditionalRedFeature(() => MyColorEnum.Green);
            Assert.True(customDelegateGreen.Is(MyColorEnum.Green), $"{customDelegateGreen.GetType().Name} delegate failed");

            var customDelegateRed = new MyCustomEnumConditionalGreenFeature(() => MyColorEnum.Red);
            Assert.True(customDelegateRed.Is(MyColorEnum.Red), $"{customDelegateRed.GetType().Name} delegate failed");
        }

        [Fact]
        public void TestAppSettingsFeatures()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(GetType().Assembly.Location))
                .AddJsonFile("appsettings.json");
            var config = builder.Build();


            var rootGreen = new MyConfigurationRootGreenEnumFeature(config);
            Assert.True(rootGreen.Is(MyColorEnum.Green), $"{rootGreen.GetType().Name} failed");
            Assert.True(rootGreen.Not(MyColorEnum.Blue), $"{rootGreen.GetType().Name} failed");

            var rootRed = new MyConfigurationRootRedEnumFeature(config);
            Assert.True(rootRed.Is(MyColorEnum.Red), $"{rootRed.GetType().Name} failed");
            Assert.False(rootRed.Is(MyColorEnum.Blue), $"{rootRed.GetType().Name} failed");
            Assert.True(rootRed.Any(MyColorEnum.Blue, MyColorEnum.Red), $"{rootRed.GetType().Name} failed");
            Assert.True(rootRed.NotAny(MyColorEnum.Blue, MyColorEnum.Green), $"{rootRed.GetType().Name} failed");

            var rootFormatException = new MyConfigurationRootFormatExceptionEnumFeature(config);
            Assert.Throws<FormatException>(()=>rootFormatException.Is(MyColorEnum.Transparent));


            var sectionGreen = new MyConfigurationSectionGreenEnumFeature(config);
            Assert.True(sectionGreen.Is(MyColorEnum.Green), $"{sectionGreen.GetType().Name} failed");

            var sectionRed = new MyConfigurationSectionRedEnumFeature(config);
            Assert.True(sectionRed.Is(MyColorEnum.Red), $"{sectionRed.GetType().Name} failed");

            var sectionFormatException = new MyConfigurationSectionFormatExceptionEnumFeature(config);
            Assert.Throws<FormatException>(() => sectionFormatException.Is(MyColorEnum.Transparent));

            var missingDefaultTransparent = new MyConfigurationMissingDefaultToTransparentEnumFeature(config, MyColorEnum.Transparent);
            Assert.True(missingDefaultTransparent.Is(MyColorEnum.Transparent), $"{missingDefaultTransparent.GetType().Name} failed");
        }
    }
}