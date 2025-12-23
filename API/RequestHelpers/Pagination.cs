using System;

namespace API.RequestHelpers;

public class Pagination<T>(int pageNumber, int entitiesPerPage, int totalEntities, IList<T> data)
{
    public int PageNumber { get; set; } = pageNumber;
    public int EntitiesPerPage { get; set; } = entitiesPerPage;
    public int TotalEntities { get; set; } = totalEntities;
    public IList<T> Data { get; set; } = data;
}
