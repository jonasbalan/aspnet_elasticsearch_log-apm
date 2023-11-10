namespace SamplesApi3.Model
{
    public class DoctorAddress
    {
        public Guid Id { get; set; }
        public Doctor Doctor { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIPCode { get; set; }

        public string Complement { get; set; }

        public DoctorAddress()
        {

        }


        // Constructor to initialize the address
        public DoctorAddress(string street, string number, string neighborhood, string city, string state, string zipCode, string complement, string country,  string name)
        {
            Country = country;
            Name = name;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZIPCode = zipCode;
            Complement = complement;
        }

        // Method to display the formatted address
        public void DisplayAddress()
        {
            Console.WriteLine($"Street: {Street}, Number: {Number}, Neighborhood: {Neighborhood}, City: {City}, State: {State}, ZIP Code: {ZIPCode}");
        }


    }
}
