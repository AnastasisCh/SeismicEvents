using SeismicEventsFireEvents.Models;

namespace SeismicEventsFireEvents.DTOs
{
    public class InMemoryFlushQueue
    {
        public Queue<SeismicProperties> PropertiesQueue { get; set; }
        public SeismicCompressedProperties MetadataColumnProperties { get; set; }
    }
}
