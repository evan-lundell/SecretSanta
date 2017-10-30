using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Models
{
    public class PersonExceptionGroup
    {
        public int PersonExceptionGroupId { get; set; }
        public ExceptionGroup ExceptionGroup { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
