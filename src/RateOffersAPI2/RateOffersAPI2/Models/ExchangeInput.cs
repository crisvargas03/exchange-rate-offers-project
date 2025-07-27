using System.Xml.Serialization;

namespace RateOffersAPI2.Models
{
    using System.Xml.Serialization;

    [XmlRoot("ExchangeInput", Namespace = "")]
    public class ExchangeInput
    {
        [XmlElement("from")]
        public string From { get; set; } = string.Empty;

        [XmlElement("to")]
        public string To { get; set; } = string.Empty;

        [XmlElement("amount")]
        public decimal Amount { get; set; }
    }

}
