
using System;
using System.Collections.Generic;
using WBEADMS.Models;

namespace WBEADMS.DocIt.Controllers
{
    public class CalendarDate
    {
        public CalendarDate(DateTime newdate) :
            this(newdate, new List<ChainOfCustody>())
        { }

        public CalendarDate(DateTime newdate, List<ChainOfCustody> cocs)
        {
            Date = newdate;
            ChainOfCustodys = cocs;
        }

        public DateTime Date { get; private set; }

        public int Day { get { return Date.Day; } }

        public List<ChainOfCustody> ChainOfCustodys { get; private set; }
    }
}