using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiControleMedico.Repositorio
{
    public interface ILogic<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetOneAsync(T context);
        Task<T> GetOneAsync(string id);
        Task<T> GetManyAsync(IEnumerable<T> contexts);
        Task<T> GetManyAsync(IEnumerable<string> ids);
        Task<T> SaveOneAsync(T Context);
        Task<IEnumerable<T>> SaveManyAsync(IEnumerable<T> contexts);
        Task<bool> RemoveOneAsync(T context);
        Task<bool> RemoveOneAsync(string id);
        Task<bool> RemoveManyAsync(IEnumerable<T> contexts);
        Task<bool> RemoveManyAsync(IEnumerable<string> ids);
    }
}
