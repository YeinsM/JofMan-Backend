namespace BMS_Logistics.Domain.Enums
{
    public readonly struct Tables
    {
        private readonly string _value;

        // We define “values” as static properties
        public static Tables USERS => new Tables("USERS");
        public static Tables ROLES => new Tables("ROLES");
        public static Tables PERMISSIONS => new Tables("PERMISSIONS");
        public static Tables ROLEPERMISSIONS => new Tables("ROLESPERMISSIONS");
        public static Tables MENUS => new Tables("MENUS");
        public static Tables SUBMENUS => new Tables("SUBMENUS");
        public static Tables STATUS => new Tables("STATUS");
        public static Tables DEPARTMENTS => new Tables("DEPARTMENTS");

        // Private constructor
        private Tables(string value) => _value = value;

        // Implicit conversion to a string
        public static implicit operator string(Tables table) => table._value;

        public override string ToString() => _value;
    }
}
