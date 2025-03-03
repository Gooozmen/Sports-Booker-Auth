using Application.Builders;
using Shared.Enums;

namespace Tests.Application.Builders;

public class HttpResponseBuilderTests
{
    private readonly HttpResponseBuilder _builder;

    public HttpResponseBuilderTests()
    {
        _builder = new HttpResponseBuilder();
    }

    /// <summary>
    ///     The success flag from response base should be applied by the status code value received from function
    /// </summary>
    [Theory]
    [InlineData((int)HttpStatusCodes.BadRequest, false)]
    [InlineData((int)HttpStatusCodes.OK, true)]
    public void SuccessFlagAppliedByStatusCode(int statusCode, bool expectedSuccess)
    {
        //arrange its defined by inline data
        var data = default(object);
        //act
        var response = _builder.CreateResponse(statusCode, data, string.Empty);
        //assert
        Assert.True(response.IsSuccess == expectedSuccess);
    }

    /// <summary>
    ///     Test meant to verify the funcionality on applying default response messages
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="expectedMessage"></param>
    [Theory]
    [InlineData((int)HttpStatusCodes.BadRequest, "Bad Request")]
    [InlineData((int)HttpStatusCodes.OK, "OK")]
    public void ResponseMessageShouldBeAppliedByStatusCodeIfMessageIsNull(int statusCode, string expectedMessage)
    {
        //arrange its defined by inline data
        var data = default(object);
        //act
        var response = _builder.CreateResponse(statusCode, data, string.Empty);
        //assert
        Assert.Equal(response.Message, expectedMessage);
    }


    [Fact]
    public void ResponseMessageShouldBeAppliedByStatusCodeIfMessageIsNotNull()
    {
        //arrange its defined by inline data
        var data = default(object);
        //act
        var response = _builder.CreateResponse((int)HttpStatusCodes.OK, data, "You Found An Easter Egg");
        //assert
        Assert.True(response.Message == "You Found An Easter Egg");
    }
}