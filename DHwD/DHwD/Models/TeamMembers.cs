namespace DHwD.Models
{
    public class TeamMembers
    {
        public int Id { get; set; }
        public User User { get; set; }// FK
        public Team Team { get; set; }// FK
    }
}
