namespace BMS_Logistics.Application.DTOs
{
    public class UserDto
    {
        //public int Id { get; set; }
        //public string? Name { get; set; }
        //public string? Surname { get; set; }
        //public string? Username { get; set; }
        //public string? Email { get; set; }
        //public int CompanyId { get; set; }
        //public int BranchId { get; set; }
        //public string? CompanyName { get; set; }
        //public string? BranchName { get; set; }
        //public int StatusId { get; set; }
        //public int Status { get; set; }
        //public string? RoleId { get; set; }
        //public string? UpdatedBy { get; set; }
        //public DateTime? ExpirationDate { get; set; }
        //public int BlockLogin { get; set; }
        //public int Attempts { get; set; }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? RoleId { get; set; }
        public string? Status { get; set; }
        public int StatusId { get; set; }
        public string? UserType { get; set; }

        public List<string> Roles { get; set; } = new();
    }

    public class UserCreateDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? RoleId { get; set; }
    }
}
