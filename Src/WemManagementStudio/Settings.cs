using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WemManagementStudio
{
    [Serializable]
    public class Settings
    {
        public Settings()
        {
            Path = string.Empty;

            Machines = new List<Machine>();
        }
        public string Path { get; set; }

        public List<Machine> Machines { get; set; }
    }

    [Serializable]
    public class Machine
    {
        public string Name { get; set; }

        public string Host { get; set; }

        public string User { get; set; }

        public string Pass { get; set; }

        public MachineType MachineType { get; set; }

        public bool Equals(Machine obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Name == obj.Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Machine);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
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
