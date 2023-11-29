using System.Xml.Serialization;

namespace DR.PodcastFeeds.Infrastructure.Extensions;

public static class PodcastRssDeserializer
{
    public static Channel DeserializeRssFeed(this Stream xml)
    {
        var serializer = new XmlSerializer(typeof(Channel));
        return (Channel) serializer.Deserialize(xml)!;
    }
}

[XmlRoot (ElementName = "channel")]
public class Channel
{
    [XmlElement(ElementName = "title")]
    public string Title { get; set; } = null!;
    
    [XmlElement(ElementName = "description")]
    public string Description { get; set; } = null!;
    
    [XmlElement(ElementName = "link")]
    public string Link { get; set; } = null!;
    
    [XmlElement(ElementName = "image")]
    public Image Image { get; set; } = null!;
    
    [XmlElement(ElementName = "category")]
    public string Category { get; set; } = null!;
    
    [XmlElement(ElementName = "item")]
    public List<Item> Items { get; set; } = null!;
}

public class Image
{
    [XmlElement(ElementName = "url")]
    public string Url { get; set; } = null!;
}

public class Item
{
    [XmlElement(ElementName = "id")]
    public string Id { get; set; } = null!;
    
    [XmlElement(ElementName = "title")]
    public string Title { get; set; } = null!;
    
    [XmlElement(ElementName = "description")]
    public string Description { get; set; } = null!;
    
    [XmlElement(ElementName = "duration")]
    public string Duration { get; set; } = null!;
    
    [XmlElement(ElementName = "pubDate")]
    public string PubDate { get; set; } = null!;
}