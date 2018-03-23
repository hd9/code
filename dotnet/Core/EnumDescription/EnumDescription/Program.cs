using System;
using System.Collections.Generic;

namespace EnumDescription
{
    class Program
    {
        static void Main(string[] args)
        {
            var recs = new List<Invoice>
            {
                new Invoice { Name = "#5489-A", Status = Status.Approved },
                new Invoice { Name = "#5489-B", Status = Status.Draft },
                new Invoice { Name = "#5489-C", Status = Status.Pending },
                new Invoice { Name = "#5489-C", Status = Status.Submitted },
                new Invoice { Name = "#5489-D", Status = Status.Approved }
            };

            recs.ForEach(r => Console.WriteLine(r));
        }
    }
}
