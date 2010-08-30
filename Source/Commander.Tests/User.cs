namespace Commander.Tests
{
    public class User
    {
        public User(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}