using Shared.Wrapper.Enums;

namespace Shared.Wrapper.Interfaces;

public interface IResult
{
    List<string> Messages { get; set; }

    bool Succeeded { get; set; }
    
    ResultMessageType ResultMessageType { get; set; }
}

public interface IResult<out T> : IResult
{
    T Data { get; }
}