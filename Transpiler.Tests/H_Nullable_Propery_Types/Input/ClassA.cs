using System;
using System.Collections.Generic;

namespace CS2TS.Tests.H
{
    public class ClassA
    {
        public int? NullableId { get; set; }
        public bool? NullableCheck { get; set; }
        public DateTime? NullableDate { get; set; }
        public DateTimeOffset? NullableDateOffset { get; set; }
        public Guid? NullableGuid { get; set; }
    }
}