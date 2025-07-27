using System.Xml.Serialization;

namespace RateOffersAPI2.Models
{
    [XmlRoot("Result")]
    public class XmlExchangeResponse
    {
        [XmlElement("amount")]
        public decimal Amount { get; set; }
        [XmlElement("errors")]
        public Error Errors { get; set; } = new Error();
    }

    public class Error
    {
        public List<string> Errors { get; set; } = [];
    }
}
