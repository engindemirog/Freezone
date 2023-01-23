using Freezone.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Transmission : Entity
{
    public string Name { get; set; }

    public ICollection<Model> Models { get; set; }

    public Transmission()
    {
        Models = new HashSet<Model>();
    }

    public Transmission(int id, string name) : this()
    {
        Id = id;
        Name = name;
    }
}