using System.IO;
using System.Threading.Tasks;

namespace TransactionApp.Services.Abstractions
{
    public interface ITransactionsService
    {
        Task AddTransactionsAsync(Stream data);
    }
}