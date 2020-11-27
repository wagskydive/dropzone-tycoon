using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FinanceLogic;

namespace Tests
{
    public class FinancialTests
    {
        [Test]
        public void can_create_an_account_and_verify_exists()
        {
            string accountRequestedID = "Test Account Name String";
            Bank bank = new Bank();
            string accountGivenID = FinancialDataCreator.CreateNewAccount(bank, accountRequestedID);


            Assert.NotNull(FinancialDataSupplier.AccountsIDs(bank));
            Assert.AreEqual(FinancialDataSupplier.AccountsIDs(bank)[FinancialDataSupplier.AccountsIDs(bank).Length - 1], accountGivenID);
        }


        [Test]
        public void can_create_two_accounts_with_same_name_and_make_names_unique()
        {
            string accountRequestedID = "Test Account Name String";
            Bank bank = new Bank();
            string account_one_GivenID = FinancialDataCreator.CreateNewAccount(bank, accountRequestedID);
            string account_two_GivenID = FinancialDataCreator.CreateNewAccount(bank, accountRequestedID);

            Assert.AreNotEqual(account_one_GivenID, account_two_GivenID);
        }

        [Test]
        public void can_create_two_accounts_with_100_and_transfer_50_from_a_to_b()
        {
            string accountsRequestedID = "Test Account Name String";
            Bank bank = new Bank();
            string account_a_GivenID = FinancialDataCreator.CreateNewAccount(bank, accountsRequestedID,100);
            string account_b_GivenID = FinancialDataCreator.CreateNewAccount(bank, accountsRequestedID,100);


            FinancialDataCreator.MakeTransactionFromIdString(bank, 50, account_a_GivenID, account_b_GivenID);

            Assert.AreEqual(50, FinancialDataSupplier.GetBalance(bank, account_a_GivenID));
            Assert.AreEqual(150, FinancialDataSupplier.GetBalance(bank, account_b_GivenID));
        }

    }
}
