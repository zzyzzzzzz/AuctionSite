using MongoDB.Entities;
namespace SearchService.Models;

public class Item : Entity
{
//复制auctiondto中的所有属性
//但是不需要Id because we're going to derive this item class.
//s going to derive from a mongo db entity class called entity
//because we've derived from entity when we do initialize our database this is considered an entity class now inside mongo db.
//我们不需要id 因为来自entity
   // public string Id { get; set; }
    public int ReservePrice { get; set; } = 0;
    public string Seller { get; set; }
    public string Winner { get; set; }
    public int  SoldAmount { get; set; }
    public int  CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; }  
    public DateTime UpdatedAt { get; set; }  
    public DateTime AuctionEnd { get; set; }
    public string Status { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }
    public string ImageUrl { get; set; }

}
