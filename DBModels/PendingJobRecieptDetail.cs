using System;


public class PendingJobRecieptDetail
{
    public int SerialNumber { get; set; }
    public DateTime ReferenceDate { get; set; }
    public Int16 ProcessCode { get; set; }
    public string ProcessName { get; set; }
    public string BillingType { get; set; }
    public Int16 ItemSerialNumber { get; set; }
    public Int16 PacketNumber { get; set; }
    public double ItemPieces { get; set; }
    public double ItemCarats { get; set; }
    public double ItemLines { get; set; }
    public double Bal_ItemPieces { get; set; }
    public double Bal_ItemCarats { get; set; }
}