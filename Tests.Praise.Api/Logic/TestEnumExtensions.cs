using Praise.Api.Logic;

namespace Tests.Praise.Api;

[TestClass]
public class TestEnumExtensions
{
    [TestMethod]
    public void GIVENDescriptionDefined_WHENDescriptionCalled_THENDescriptionReturned()
    {
        var testEnum = TestEnum.FirstValue;

        Assert.AreEqual("First Value Description", testEnum.GetDescription());
    }

    [TestMethod]
    public void GIVENDescription2Defined_WHENDescription2Called_THENDescription2Returned()
    {
        var testEnum = TestEnum.SecondValue;

        Assert.AreEqual("Second Value Description", testEnum.GetDescription());
    }

    [TestMethod]
    public void GIVENNoDescription_WHENDescriptionCalled_THENEnumReturned()
    {
        var testEnum = TestEnum.ThirdValueWithoutDescription;

        Assert.AreEqual("ThirdValueWithoutDescription", testEnum.GetDescription());
    }


    public enum TestEnum
    {
        [System.ComponentModel.Description("First Value Description")]
        FirstValue,
        [System.ComponentModel.Description("Second Value Description")]
        SecondValue,
        ThirdValueWithoutDescription
    }
}
