
namespace EnumDescription
{
    public class Invoice
    {
        public string Name { get; set; }
        public Status Status { get; set; }

        public override string ToString()
        {
            return $"Invoice {Name}: {Status.GetDescription()}";
        }
    }
}
