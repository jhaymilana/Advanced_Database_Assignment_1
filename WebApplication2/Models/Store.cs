namespace WebApplication2.Models
{
    public class Store
    {
        // Properties
        public Guid Id { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public CanadianProvince Province
        {
            get { return Province; }
            set
            {
                if (Enum.IsDefined(typeof(CanadianProvince), value))
                {
                    Province = value;
                }
                else
                {
                    throw new ArgumentException("Invalid Canadian province.");
                }
            }
        }
        public List<Laptop> Stock { get; private set; }

        // Enum
        public enum CanadianProvince
        {
            Alberta,
            BritishColumbia,
            Manitoba,
            NewBrunswick,
            NewfoundlandAndLabrador,
            NovaScotia,
            Ontario,
            PrinceEdwardIsland,
            Quebec,
            Saskatchewan,
        }

        // Constructor
        public Store(string streetName, string streetNumber, CanadianProvince province)
        {
            Id = Guid.NewGuid();
            StreetName = streetName;
            StreetNumber = streetNumber;
            Province = province;
            Stock = new List<Laptop>();
        }
    }
}
