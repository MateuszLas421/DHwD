namespace DHwD.Models.REST
{
    public class Place
    {
        public int Id { get; set; }
        public Games Games { get; set; }
        public ActivePlace ActivePlace { get; set; }
        public string Name {get; set;}
        public string Description { get; set; }
        public Location Location { get; set; }
        public int LocationRef { get; set; }
    }
}
