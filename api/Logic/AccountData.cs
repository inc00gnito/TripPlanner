using api.Data;
using api.Interfaces;
using api.Models;
using AutoMapper;

namespace api.Logic
{
    public class AccountData : IAccount
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public AccountData(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public Account? GetAccount(int accountId)
        {
            var acc = _db.Accounts.FirstOrDefault(a => a.Id == accountId);
            return acc;
        }

        public Account? GetAccountByName(string username)
        {
            var acc = _db.Accounts.FirstOrDefault(a => a.Username == username);
            return acc;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _db.Accounts.ToList();
        }

        public Account CreateAccount(RegisterModel registerModel)
        {
            var account = _mapper.Map<Account>(registerModel);
            _db.Accounts.Add(account);
            _db.SaveChanges();
            return account;
        }
    }
}
