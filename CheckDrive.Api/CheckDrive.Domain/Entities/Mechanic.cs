namespace CheckDrive.Domain.Entities;

public class Mechanic : Employee
{
    public virtual ICollection<MechanicHandover> Handovers { get; set; }
    public virtual ICollection<MechanicAcceptance> Acceptances { get; set; }

    public Mechanic()
    {
        Handovers = new HashSet<MechanicHandover>();
        Acceptances = new HashSet<MechanicAcceptance>();
    }
}
