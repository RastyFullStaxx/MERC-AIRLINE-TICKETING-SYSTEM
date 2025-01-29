namespace MERC
{
    public partial class LandingPage : BaseForm
    {
        // Properties to store the logged-in account's details
        public int AccountID { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string FullName { get; private set; }

        // Constructor with account details
        public LandingPage(int accountId, string username, string email, string phoneNumber, string fullName)
        {
            InitializeComponent();

            // Assign the passed account details to the properties
            this.AccountID = accountId;
            this.Username = username;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.FullName = fullName;

            // Example: Update a label to show a welcome message
            //lblWelcome.Text = $"Welcome, {this.FullName}!";
        }

        // Optional: Parameterless constructor for design-time support
        public LandingPage()
        {
            InitializeComponent();
        }
    }
}
