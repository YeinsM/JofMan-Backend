namespace BMS_Logistics.Domain.Entities
{
    public class Status : SharedProperty
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Table { get; set; }
        public string? Description { get; set; }

        private Status() { }

        public Status(string code, string table, string description)
        {
            Code = code;
            Table = table;
            Description = description;
        }

        public void Update(string code, string table, string description)
        {
            Code = code;
            Table = table;
            Description = description;
        }
    }
}
