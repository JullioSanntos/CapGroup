using System.Collections.Generic;

namespace ACE.Client.Model.Helpers
{
    internal class InvestorsAccountCache : ICache<InvestorAccount>
    {
        public InvestorsAccountCache(int numberOfMocks)
        {
            Cache = LoadCache(numberOfMocks);
        }
        public List<InvestorAccount> Cache { get; set; }

        public List<InvestorAccount> LoadCache(object numberOfAccount)
        {
            var randomizer = new Randomizer();
            var accounts = new List<InvestorAccount>();
            for (long i = 0; i < (int)numberOfAccount; i++)
            {
                var acct = new InvestorAccount() { Number = long.Parse(randomizer.Next(Randomizer.NumericValues, 6)) };
                acct.CurrAcctVal = decimal.Parse(randomizer.Next(Randomizer.NumericValues, 7))/100;
                acct.ResetIsDirty();
                accounts.Add(acct);
            }
            
            return accounts;
        }
    }
}