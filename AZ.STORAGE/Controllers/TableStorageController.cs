using AZ.STORAGE.Entities;
using AZ.STORAGE.Services;
using Microsoft.AspNetCore.Mvc;

namespace AZ.STORAGE.Controllers;
[ApiController]
[Route("[controller]")]
public class TableStorageController:ControllerBase
{
    //Resources:
    //https://dev.to/alikolahdoozan/insert-update-delete-and-read-operations-in-azure-table-storage-by-cnet--ggd
    //https://code-maze.com/azure-table-storage-aspnetcore/
    
    private readonly INoSqlStorage<Product> _table;

    public TableStorageController(INoSqlStorage<Product> table)
    {
        _table = table;
    }

    [HttpGet("add")]
    public async Task<IEnumerable<object>> Add(string name)
    {
        var product = new Product
        {
            Name = name,
            PartitionKey = "Product",
            RowKey = Guid.NewGuid().ToString(),
        };
        await _table.Add(product);
        return Enumerable.Empty<object>();
    }
    [HttpGet("get")]
    public async Task<Product> Get(string id)
    {
        var res = await _table.Get(id,"Product");
        return res;
    }
    [HttpGet("get-query")]
    public List<Product> GetQuery(string key)
    {
        var res =  _table.Query(x=>x.PartitionKey==key);
        return res;
    }
    [HttpGet("update")]
    public async Task Update(string id,string name)
    {
        var res = await _table.Get(id,"Product");
        res.Name = name;
        await _table.Update(res);
    }
    [HttpGet("delete")]
    public async Task Delete(string id)
    {
        await _table.Delete(id,"Product");
    }
}