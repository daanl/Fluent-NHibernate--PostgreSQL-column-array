using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Project
    {
        public virtual Guid Id { get; set; }
        public virtual List<string> Tags { get; set; }
    }
}