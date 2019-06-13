using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public DateTime TaskDate { get; set; }
        public int RepeatMode { get; set; }
        public int JobType { get; set; }

        public Job()
        {
            Id = Guid.NewGuid();
        }
    }
}