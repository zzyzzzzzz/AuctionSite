using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs;

public class CreateAuctionDtos
{
    [Required]  //这个是C#中的特性，用于表示该属性是必须的
    public string Make { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public string Color { get; set; }
    [Required]
    public int Mileage { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    [Required]
    //除了已经在Item中定义的属性之外，用户还必须要给出拍卖的开始时间
    //用户还必须要给出拍卖的结束时间 以及最低价格
    public int ReservePrice { get; set; } 
    [Required]
    public DateTime AuctionEnd { get; set; }
    
}
