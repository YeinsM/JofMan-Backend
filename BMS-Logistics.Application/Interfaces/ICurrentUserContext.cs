namespace BMS_Logistics.Application.Interfaces
{
    public interface ICurrentUserContext
    {
        int UserId { get; }
        string UserName { get; }
    }
}
