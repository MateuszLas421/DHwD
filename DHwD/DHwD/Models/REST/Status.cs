namespace DHwD.Models.REST
{
    public class Status
    {
        public int ID { get; set; }
        public Team Team { get; set; }
        //[Required]
        //bool Status;   // TODO!!!
        public ActivePlace ActivePlace { get; set; }
    }
}
