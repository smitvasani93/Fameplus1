using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Transactiondetails.DBModels
{
    public class ProcessMaster
    {
        //public ProcessMaster()
        //{
        //    ProcessDet = new List<ProcessDetail>();
        //}
        public Int16 ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string BillingType { get; set; }
        public double SGSTTaxRate { get; set; }
        public double CGSTTaxRate { get; set; }
        public double IGSTTaxRate { get; set; }
        public string ProcessDet { get; set; }
        //public List<ProcessDetail> ProcessDetails { get; set; }

    }


    [XmlRoot(ElementName = "ProcessDet", Namespace = "")]
    public class ProcessDetail
    {
        [XmlElement("ProcessCode")]

        public Int16 ProcessCode { get; set; }
        [XmlElement("SerialNumber")]

        public Int16 SerialNumber { get; set; }
        [XmlElement("RangeFrom")]

        public decimal RangeFrom { get; set; }

        [XmlElement("RangeTo")]

        public decimal RangeTo { get; set; }
        [XmlElement("BillingRate")]
        public decimal BillingRate { get; set; }
    }
}