using System;

public class JobDespatchDetail
{
    public string ProcessName { get; set; }
    public Int16 ProcessCode { get; set; }
    public int SerialNumber { get; set; }
    public Int16 ItemSerialNumber { get; set; }

    public int JRSerialNumber { get; set; }
    public Int16 JRItemSerialNumber { get; set; }
    public double ItemPieces { get; set; }
    public double ItemCarats { get; set; }
    public double ItemLines { get; set; }
    public double WeightLoss { get; set; }
    public string PacketStatus { get; set; }
    public string Remarks { get; set; }
    public double BillingQuantity { get; set; }

    public double NoChargeQuantity { get; set; }
    public double BillingRate { get; set; }
    public string AccountCode { get; set; }
    public DateTime? ReferenceDate { get; set; }
}