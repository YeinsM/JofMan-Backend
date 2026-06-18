namespace BMS_Logistics.Domain.Entities
{
    public class User : SharedProperty
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string? RoleId { get; set; }
        public int StatusId { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool BlockLogin { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int Attempts { get; set; }

        public Status? Status { get; set; }

        //private User() { }

        public User(int companyId, int branchId, string? roleId, int statusId, string? name, string? surName, string? userName, string? password, string? email, bool blockLogin)
        {
            CompanyId = companyId;
            BranchId = branchId;
            RoleId = roleId;
            StatusId = statusId;
            Name = name;
            SurName = surName;
            UserName = userName;
            Password = password;
            Email = email;
            ExpirationDate = DateTime.Now.AddDays(90);
            BlockLogin = blockLogin;
        }

        public void ChangePassword(string newPasswordHash)
        {
            Password = newPasswordHash;
            ExpirationDate = DateTime.UtcNow.AddDays(90);
        }

        public void UpdateInfo(string firstName, string lastName, string username, string email)
        {
            Name = firstName;
            SurName = lastName;
            UserName = username;
            Email = email;
        }

        public void SetRoles(string rol)
        {
            RoleId = rol;
        }

        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }

        public void IncrementAttempts(int blockedStatusId)
        {
            Attempts++;

            if (Attempts >= 3)
            {
                BlockUser(blockedStatusId);
            }
        }

        public void ResetAttempts()
        {
            Attempts = 0;
        }

        public void BlockUser(int statusId)
        {
            StatusId = statusId;
        }

        public bool IsPasswordExpired()
        {
            return DateTime.UtcNow > ExpirationDate;
        }
    }
}
