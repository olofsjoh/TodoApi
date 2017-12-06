using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TodoApp
{
    public class Todo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
