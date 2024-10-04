using System;

namespace AuctionService.DTOs;

public class UpdateAuctionDto
{//这个类用于更新拍卖的信息 用户不能再修改imageurl
    public string Make { get; set; }
    public string Model { get; set; }
    public int? Year { get; set; }
    public string Color { get; set; }
    public int? Mileage { get; set; }
}


//接下来我们需要我们需要mapping profiles for automapper to map us from the auction entity to the auction dto  
//把entity中的属性映射到dto中