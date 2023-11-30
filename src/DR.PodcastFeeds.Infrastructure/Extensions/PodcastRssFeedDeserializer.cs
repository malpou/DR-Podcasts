using System.Xml.Serialization;

namespace DR.PodcastFeeds.Infrastructure.Extensions;

public static class PodcastRssFeedDeserializer
{
    public static Channel DeserializeRssFeed(this Stream xml)
    {
        var serializer = new XmlSerializer(typeof(Rss));
        return ((Rss) serializer.Deserialize(xml)!).Channel;
    }
}

[XmlRoot(ElementName = "rss")]
public class Rss
{
    [XmlNamespaceDeclarations] public XmlSerializerNamespaces? Namespaces { get; set; }

    [XmlElement(ElementName = "channel")] public Channel Channel { get; set; } = null!;
}

public class Channel
{
    [XmlElement(ElementName = "title")] public string Title { get; set; } = null!;

    [XmlElement(ElementName = "description")]
    public string Description { get; set; } = null!;

    [XmlElement(ElementName = "link")] public string Link { get; set; } = null!;

    [XmlElement(ElementName = "image")] public Image Image { get; set; } = null!;

    [XmlElement(ElementName = "category")] public string Category { get; set; } = null!;

    [XmlElement(ElementName = "item")] public List<Item> Items { get; set; } = null!;
}

public class Image
{
    [XmlElement(ElementName = "url")] public string Url { get; set; } = null!;
}

public class Item
{
    [XmlElement(ElementName = "guid")] public string Id { get; set; } = null!;

    [XmlElement(ElementName = "title")] public string Title { get; set; } = null!;

    [XmlElement(ElementName = "description")]
    public string Description { get; set; } = null!;

    [XmlElement(ElementName = "duration", Namespace = "itunes")]
    public string Duration { get; set; } = null!;

    [XmlElement(ElementName = "pubDate")] public string PubDate { get; set; } = null!;
}