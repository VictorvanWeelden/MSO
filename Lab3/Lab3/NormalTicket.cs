using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class NormalTicket : Ticket
    {
        //dsaasdads
        public bool Day;
        public bool Single = false;
        public bool International = false;
        public NormalTicket(string from, string to, int classes, float price, bool today, bool single, bool international) : base(from, to, classes, price)
        {
            Day = today;
            Single = single;
            International = international;
        }
    }
}