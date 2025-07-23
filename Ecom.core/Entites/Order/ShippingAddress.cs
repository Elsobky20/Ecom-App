namespace Ecom.core.Entites.Order
{
    public class ShippingAddress :BaseEntity<int>
    {
        public ShippingAddress()
        {
            
        }
        public ShippingAddress(string firstName , string lastName ,string city , string zipCode , string State, string street  )
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.City = city;
            this.ZipCode = zipCode;
            this.State = State;
            this.Street = street;
        }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string State { get; set; }
            public string Street { get; set; }

    }
}