using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiControleMedico.Repositorio
{
    public class EntidadeNegocio<TContext>
        where TContext : Entidade, new()
    {
        public async Task<IEnumerable<TContext>> GetAllAsync(IMongoCollection<TContext> collection)
        {
            return await collection.Find(f => true).ToListAsync();
        }

        public async Task<TContext> GetOneAsync(IMongoCollection<TContext> collection, TContext context)
        {
            return await collection.Find(new BsonDocument("_id", context.Id)).FirstOrDefaultAsync();
        }

        public async Task<TContext> GetOneAsync(IMongoCollection<TContext> collection, string id)
        {
            return await GetOneAsync(collection, new TContext {Id = id});
        }
        
        public async Task<IEnumerable<TContext>> GetManyAsync(IMongoCollection<TContext> collection,
            IEnumerable<TContext> contexts)
        {
            var list = new List<TContext>();
            foreach (var context in contexts)
            {
                var doc = await GetOneAsync(collection, context);
                if (doc == null) continue;
                list.Add(doc);
            }

            return list;
        }

        public async Task<IEnumerable<TContext>> GetManyAsync(IMongoCollection<TContext> collection,
            IEnumerable<string> ids)
        {
            var list = new List<TContext>();
            foreach (var id in ids)
            {
                var doc = await GetOneAsync(collection, id);
                if (doc == null) continue;
                list.Add(doc);
            }

            return list;
        }

        public async Task<IEnumerable<TContext>> SaveManyAsync(IMongoCollection<TContext> collection, IEnumerable<TContext> collectionToInsert)
        {
            foreach (var context in collectionToInsert)
            {
                if (string.IsNullOrEmpty(context.Id))
                {
                    context.Id = ObjectId.GenerateNewId().ToString();
                    await collection.InsertOneAsync(context);
                }
                else
                {
                    await collection.ReplaceOneAsync(c => c.Id == context.Id, context);
                }
            }

            return collectionToInsert;
        }

        public async Task SaveOneAsync(IMongoCollection<TContext> collection, TContext context)
        {
            if (string.IsNullOrEmpty(context.Id))
            {
                context.Id = ObjectId.GenerateNewId().ToString();
                await collection.InsertOneAsync(context);
            }
            else
            {
                await collection.ReplaceOneAsync(c => c.Id == context.Id, context);
            }
        }

        public async Task<bool> RemoveOneAsync(IMongoCollection<TContext> collection, TContext context)
        {
            if (context == null) return false;

            await collection.DeleteOneAsync(new BsonDocument("_id", context.Id));           
            return true;
        }

        public async Task<bool> RemoveOneAsync(IMongoCollection<TContext> collection, string id)
        {
            return await RemoveOneAsync(collection, new TContext {Id = id});
        }

        public async Task<bool> RemoveManyAsync(IMongoCollection<TContext> collection,
            IEnumerable<TContext> contexts)
        {
            foreach (var context in contexts)
                await RemoveOneAsync(collection, context);
            return true;
        }

        public async Task<bool> RemoveManyAsync(IMongoCollection<TContext> collection,
            IEnumerable<string> ids)
        {
            foreach (var id in ids)
                await RemoveOneAsync(collection, id);
            return true;
        }
    }
}