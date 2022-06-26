using MongoDB.Driver;
using System.Linq.Expressions;

namespace Tests.Common.MongoDB
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        //private const string collectionName = "items";

        private readonly IMongoCollection<T> dbCollection;

        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;
        //public MongoRepository()
        //{
        //}

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            //var MongoClient = new MongoClient("mongodb://localhost:27017");
            //var database = MongoClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<T>(collectionName);

        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        async Task IRepository<T>.CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new NotImplementedException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }

        async Task<IReadOnlyCollection<T>> IRepository<T>.GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        async Task<T> IRepository<T>.GetAsync(Guid id)
        {

            return await dbCollection.Find(filterBuilder.Eq(entity => entity.Id, id)).FirstOrDefaultAsync();
        }

        async Task IRepository<T>.RemoveAsync(Guid Id)
        {
            await dbCollection.DeleteOneAsync(filterBuilder.Eq(entity => entity.Id, Id));
        }

        async Task IRepository<T>.UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
           
            await dbCollection.ReplaceOneAsync(filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id), entity);
        }

        //public async Task<IReadOnlyCollection<T>> GetAllAsync()
        //{
        //    return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        //}
        //public async Task<T> GetAsync(Guid id)
        //{
        //    FieldDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
        //    return await dbCollection.Find(filter,entity).FirstOrDefaultAsync();
        //}
        //public async Task CreateAsync(T entity)
        //{
        //    if (entity == null)
        //    {
        //        throw new ArgumentNullException(nameof(entity));
        //    }

        //    await dbCollection.InsertOneAsync(entity);

        //}

        //public async Task UpdateAsync(T entity)
        //{
        //    if (entity == null)
        //    {
        //        throw new ArgumentNullException(nameof(entity));
        //    }
        //    FieldDefinition<T> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
        //    await dbCollection.ReplaceOneAsync(filter, entity);
        //}

        //public async Task RemoveAsync(Guid Id)
        //{
        //    await dbCollection.DeleteOneAsync(filterBuilder.Eq(entity => entity.Id, Id));
        //}
    }
}
