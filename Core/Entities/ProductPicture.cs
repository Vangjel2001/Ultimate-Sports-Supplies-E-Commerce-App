using System;

namespace Core.Entities;

public class ProductPicture : BaseEntity
{
    public required string PictureUrl { get; set; }
}
