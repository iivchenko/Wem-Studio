namespace WemManagementStudio
{
    public class Machine
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Host { get; set; }

        public string User { get; set; }

        public string Pass { get; set; }

        public string Path { get; set; }

        public MachineType MachineType { get; set; }

        public bool Equals(Machine obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Id == obj.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Machine);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Machine left, Machine right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            
            if (((object)left == null) || ((object)right== null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Machine left, Machine right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
