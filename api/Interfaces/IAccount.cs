using api.Models;

namespace api.Interfaces
{
    public interface IAccount
    {
        IEnumerable<Account> GetAccounts();
        Account? GetAccount(int accountId);
        Account? GetAccountByName(string accountName);
        Account CreateAccount(RegisterModel registerModel);
        
    }
}
