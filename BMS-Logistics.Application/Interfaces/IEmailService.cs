using BMS_Logistics.Application.DTOs;

namespace BMS_Logistics.Application.Interfaces
{
    public interface IEmailService
    {
        bool Send(string EmailTo, string? CopyTo, string Subject, string BodyMessage);
        string BodyMessage<T>(UserDto user, T requestData, string currentController);
        string CreateForgetPasswordEmail(UserDto user, string code);
    }
}
