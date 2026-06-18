namespace BMS_Logistics.Application.Interfaces
{
    public interface IUserInfo
    {
        int Id { get; set; }
        string Name { get; set; }
        string Lastname => string.Empty;
        string Username => string.Empty;
        string Email { get; set; }
        int RoleId => 0;
        bool PasswordForgotten => false;
        string IdentityNumber => string.Empty;
    }
}
