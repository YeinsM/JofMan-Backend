namespace BMS_Logistics.Domain.Enums
{
    public readonly struct Statuses
    {
        private readonly string _value;

        // We define “values” as static properties
        public static Statuses IN_PROGRESS => new("IN_PROGRESS");
        public static Statuses ACTIVE => new("ACTIVE");
        public static Statuses INACTIVE => new("INACTIVE");
        public static Statuses BLOCKED => new("BLOCKED");
        public static Statuses DELETED => new("DELETED");
        public static Statuses COMPLETED => new("COMPLETED");

        // Private constructor
        private Statuses(string value) => _value = value;

        // Implicit conversion to a string
        public static implicit operator string(Statuses status) => status._value;

        public override string ToString() => _value;
    }
}
