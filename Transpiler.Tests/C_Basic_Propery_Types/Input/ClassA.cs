using System;
using System.Collections.Generic;

namespace CS2TS.Tests.C
{
    public class ClassA
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Check { get; set; }
        public DateTime Date { get; set; }
        public DateTimeOffset DateOffset { get; set; }
        public List<int> NumberList { get; set; }
    }
}