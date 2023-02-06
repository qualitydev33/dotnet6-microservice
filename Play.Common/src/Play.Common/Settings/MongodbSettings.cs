namespace Play.Common.Settings
{
  public class MongodbSettings {
    public string host {get; set;}
    public int port {get; set;}
    public string connectionStr => $"mongodb://{host}:{port}";
  }
}