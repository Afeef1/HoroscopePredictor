namespace HoroscopePredictorAPI.Models
{
    public class UserCacheData
    {
       public string Id { get;set; } = Guid.NewGuid().ToString();
       public string Zodiac {  get; set; }
       public string UserId {  get; set; }
       public DateTime TimeStamp { get; set; }
    }
}
