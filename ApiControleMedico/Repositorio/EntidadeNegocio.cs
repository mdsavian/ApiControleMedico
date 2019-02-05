using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiControleMedico.Repositorio
{
    public class EntidadeNegocio<TCollection, TContext>
        where TCollection : Entidade
        where TContext : Entidade, new()
    {
        public async Task<IEnumerable<TCollection>> GetAllAsync(IMongoCollection<TCollection> collection)
        {
            return await collection.Find(f => true).ToListAsync();
        }

        public async Task<TCollection> GetOneAsync(IMongoCollection<TCollection> collection, TContext context)
        {
            return await collection.Find(new BsonDocument("_id", context.Id)).FirstOrDefaultAsync();
        }

        public async Task<TCollection> GetOneAsync(IMongoCollection<TCollection> collection, string id)
        {
            return await GetOneAsync(collection, new TContext {Id = id});
        }

       
        public async Task<IEnumerable<TCollection>> GetManyAsync(IMongoCollection<TCollection> collection,
            IEnumerable<TContext> contexts)
        {
            var list = new List<TCollection>();
            foreach (var context in contexts)
            {
                var doc = await GetOneAsync(collection, context);
                if (doc == null) continue;
                list.Add(doc);
            }

            return list;
        }

        public async Task<IEnumerable<TCollection>> GetManyAsync(IMongoCollection<TCollection> collection,
            IEnumerable<string> ids)
        {
            var list = new List<TCollection>();
            foreach (var id in ids)
            {
                var doc = await GetOneAsync(collection, id);
                if (doc == null) continue;
                list.Add(doc);
            }

            return list;
        }

        public async Task SaveOneAsync(IMongoCollection<TCollection> collection, TCollection context)
        {
            await collection.InsertOneAsync(context);
        }

        public async Task<bool> RemoveOneAsync(IMongoCollection<TCollection> collection, TContext context)
        {
            if (context == null) return false;

            await collection.DeleteOneAsync(new BsonDocument("_id", context.Id));           
            return true;
        }

        public async Task<bool> RemoveOneAsync(IMongoCollection<TCollection> collection, string id)
        {
            return await RemoveOneAsync(collection, new TContext {Id = id});
        }

        public async Task<bool> RemoveManyAsync(IMongoCollection<TCollection> collection,
            IEnumerable<TContext> contexts)
        {
            foreach (var context in contexts)
                await RemoveOneAsync(collection, context);
            return true;
        }

        public async Task<bool> RemoveManyAsync(IMongoCollection<TCollection> collection,
            IEnumerable<string> ids)
        {
            foreach (var id in ids)
                await RemoveOneAsync(collection, id);
            return true;
        }
    }
}