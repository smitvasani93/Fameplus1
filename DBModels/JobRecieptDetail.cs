﻿using System;

namespace Transactiondetails.DBModels
{
    public class JobRecieptDetail
    {
        public int ProcessCode { get; set; }
        public Int16 ItemSerialNumber { get; set; }
        public Int16 PacketNumber { get; set; }
        public double ItemPieces { get; set; }
        public double ItemCarats { get; set; }
        public double ItemLines { get; set; }
        public string AccountCode { get; set; }
        public DateTime ReferenceDate { get; set; }
    }
}
