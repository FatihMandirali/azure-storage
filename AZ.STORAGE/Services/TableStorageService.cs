using System.Linq.Expressions;
using AZ.STORAGE.Entities;
using Azure.Data.Tables;

namespace AZ.STORAGE.Services;

public class TableStorageService<TEntity>:INoSqlStorage<TEntity> where TEntity: class, ITableEntity,new()
{
    private readonly TableClient _tableClient;

    public TableStorageService()
    {
        var tablename = typeof(TEntity).Name;
        //Local
        var connectionString1 = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
        //AZURE
        var connectionString = "DefaultEndpointsProtocol=https;AccountName=teststorageaccountfm;AccountKey=LSZswUwYWXX9fbiG4IUE+A00LQUC6oKqIazJGcy1spibhbv8tjyHDwQINmQMwRTlsfRchpQFWWCP+AStcrz2Vw==;EndpointSuffix=core.windows.net";
        
        _tableClient = new TableClient(connectionString, tablename);
        _tableClient.CreateIfNotExists();
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        var a =await _tableClient.AddEntityAsync(entity);
        return entity;
    }

    public async Task Update(TEntity entity)
    {
        await _tableClient.UpsertEntityAsync(entity);
    }

    public async Task<TEntity> Get(string rowKey, string partitionKey)
    {
        var res = await _tableClient.GetEntityAsync<TEntity>(partitionKey, rowKey);
        return res.Value;
    }

    public async Task<IQueryable<TEntity>> All()
    {
        //TODO: TO RESEARCH
        return null;
    }

    public async Task Delete(string rowKey, string partitionKey)
    {
        await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public List<TEntity> Query(Expression<Func<TEntity, bool>> expression)
    {
        var res = _tableClient.Query<TEntity>(expression);
        return res.ToList();
    }
}