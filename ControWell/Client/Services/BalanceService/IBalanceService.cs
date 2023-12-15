using ControWell.Shared;

namespace ControWell.Client.Services.BalanceService
{
    public interface IBalanceService
    {

        List<Balance> Balances { get; set; }
        Task<Balance> GetSingleBalance(int id);

        Task CreateBalance(Balance miBalance);
        Task DeleteBalance(int id);
        Task UpdateBalance(Balance miBalance);
    }
}
