using Freezone.Core.Persistence.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Fuel:Entity
{
    public string Name { get; set; }

    public ICollection<Model> Models { get; set; }

    public Fuel()
    {
        Models = new HashSet<Model>();
    }

    public Fuel(int id, string name):this()
    {
        Id = id;
        Name = name;
    }
}
