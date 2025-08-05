namespace eCommercePanel.BLL.Results;

public interface IResult
{
    bool Success { get; }
    string Message { get; }
}
