
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiControleMedico.Repositorio
{
    public class EntidadeNegocio<TContext>
        where TContext : Entidade, new()
    {
        public IEnumerable<TContext> GetAll(IMongoCollection<TContext> collection)
        {
            return collection.Find(f => true).ToList();
        }

        public TContext GetOne(IMongoCollection<TContext> collection, TContext context)
        {
            return collection.Find(new BsonDocument("_id", context.Id)).FirstOrDefault();
        }

        public TContext GetOne(IMongoCollection<TContext> collection, string id)
        {
            return GetOne(collection, new TContext { Id = id });
        }

        public IEnumerable<TContext> GetMany(IMongoCollection<TContext> collection,
            IEnumerable<TContext> contexts)
        {
            var list = new List<TContext>();
            foreach (var context in contexts)
            {
                var doc = GetOne(collection, context);
                if (doc == null) continue;
                list.Add(doc);
            }

            return list;
        }

        public IEnumerable<TContext> GetMany(IMongoCollection<TContext> collection,
            IEnumerable<string> ids)
        {
            var list = new List<TContext>();
            foreach (var id in ids)
            {
                var doc = GetOne(collection, id);
                if (doc == null) continue;
                list.Add(doc);
            }

            return list;
        }

        public IEnumerable<TContext> SaveMany(IMongoCollection<TContext> collection, IEnumerable<TContext> collectionToInsert)
        {
            foreach (var context in collectionToInsert)
            {
                if (string.IsNullOrEmpty(context.Id))
                {
                    context.Id = ObjectId.GenerateNewId().ToString();
                    collection.InsertOne(context);
                }
                else
                {
                    collection.ReplaceOne(c => c.Id == context.Id, context);
                }
            }

            return collectionToInsert;
        }

        public void SaveOne(IMongoCollection<TContext> collection, TContext context)
        {
            if (string.IsNullOrEmpty(context.Id))
            {
                context.Id = ObjectId.GenerateNewId().ToString();
                collection.InsertOne(context);
            }
            else
            {
                collection.ReplaceOne(c => c.Id == context.Id, context);
            }
        }

        public bool RemoveOne(IMongoCollection<TContext> collection, TContext context)
        {
            if (context == null) return false;

            collection.DeleteOne(new BsonDocument("_id", context.Id));
            return true;
        }

        public bool RemoveOne(IMongoCollection<TContext> collection, string id)
        {
            return RemoveOne(collection, new TContext { Id = id });
        }

        public bool RemoveMany(IMongoCollection<TContext> collection,
            IEnumerable<TContext> contexts)
        {
            foreach (var context in contexts)
                RemoveOne(collection, context);
            return true;
        }

        public bool RemoveMany(IMongoCollection<TContext> collection,
            IEnumerable<string> ids)
        {
            foreach (var id in ids)
                RemoveOne(collection, id);
            return true;
        }
    }
}