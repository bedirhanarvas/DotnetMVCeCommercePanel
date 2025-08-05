namespace eCommercePanel.BLL.Results;

public interface IDataResult<T> :IResult
{
        T Data { get; }
    
}
