using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CashFlower.Contracts;
using NUnit.Framework;

namespace CashFlower.BankTransferStorage.File.Test
{
    [TestFixture]
    public class BankTransferFileStoreTests
    {
        [Test]
        public void CleanStore_SaveStoreCreatesFileWithoutBanktransfers()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\TargetXmlFile.xml");
            _deleteFile(filename);

            var store = new BankTransferFileStore();
            store.SaveAs(filename);
            var storedFileAsString = _readFileString(filename);
            Assert.IsTrue(storedFileAsString.Contains("xml"));
            Assert.IsTrue(storedFileAsString.Contains("ArrayOfBankTransfer"));
            Assert.IsFalse(storedFileAsString.Contains("<BankTransfer"));
        }

        [Test]
        public void GivenBankTransfersInStore_SaveStoresBankTransfersToFile()
        {
            const string testAccountNumber = "Qwert1234";
            const string testContraAccountNumber = "Qwerty234";
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\TargetXmlFile.xml");
            _clearFile(filename);

            var store = new BankTransferFileStoreWithTestExtension();
            _setTestStorage(store, testAccountNumber, testContraAccountNumber);

            store.SaveAs(filename);
            var storedFileAsString = _readFileString(filename);
            Assert.IsTrue(storedFileAsString.Contains("<BankTransfer"));
            Assert.IsTrue(storedFileAsString.Contains(testAccountNumber));
            Assert.IsTrue(storedFileAsString.Contains(testContraAccountNumber));
            Assert.IsTrue(storedFileAsString.Contains("Month"));
        }

        private static void _deleteFile(string fileName)
        {
            System.IO.File.Delete(fileName);
        }

        private static void _clearFile(string fileName)
        {
            var writer = new StreamWriter(fileName, false);
            writer.Write("");
            writer.Close();
        }

        private static string _readFileString(string fileName)
        {
            var reader = new StreamReader(fileName);
            var result = reader.ReadToEnd();
            reader.Close();
            return result;
        }

        private static void _setTestStorage(
            BankTransferFileStoreWithTestExtension store, string testAccountNumber, string testContraAccountNumber)
        {
            store.SetStorage(
                new List<BankTransfer> {
                    new BankTransfer {
                        Account = new Account {
                            AccountNumber = testAccountNumber,
                            Description = "My Account",
                            Iban = "Bla Bla Bla"
                        },
                        ContraAccount = new Account {
                            AccountNumber = testContraAccountNumber,
                            Iban = "Yada yada yada",
                            Description = "The other party's account"
                        },
                        Amount = 323,
                        InterestDate = DateTime.Today,
                        TransactionDate = DateTime.Now,
                        InitialBalance = 1000,
                        FinalBalance = 1323,
                        Extension = new BankTransferExtension() {
                            DistributionType = DistributionType.Month,
                            CalculationDate = DateTime.Today.AddDays(-7),
                            HideFromCalculations = false
                        }
                    }
                });
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void givenUnexistingFile_OpenThrowsException()
        {
            new BankTransferFileStore().OpenFrom("ThisFileShouldNotExist.xml");
        }

        [Test]
        public void GivenEmtyFile_OpenImportsEmtyList()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\EmptyXmlFile.xml");
            var store = new BankTransferFileStore();

            store.OpenFrom(filename);
            var bankTransfers = store.GetAll();

            Assert.IsEmpty(bankTransfers);
        }

        [Test]
        public void GivenFileWithOneBanktransfer_OpenImportsBanktransfer()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\XmlFileWithOneBanktransfer.xml");
            var store = new BankTransferFileStore();

            store.OpenFrom(filename);
            var bankTransfers = store.GetAll();

            Assert.IsNotEmpty(bankTransfers);
            Assert.AreEqual(1, bankTransfers.Count);
            Assert.AreEqual("Qwert1234", bankTransfers.Single().Account.AccountNumber);
            Assert.AreEqual("The other party's account", bankTransfers.Single().ContraAccount.Description);
            Assert.AreEqual(new DateTime(2017, 4, 29), bankTransfers.Single().TransactionDate.Date );
            Assert.IsNull(bankTransfers.Single().Extension);
        }

        [Test]
        public void GivenEmptyStore_ExistsReturnsFalse()
        {
            var store = new BankTransferFileStore();
            Assert.IsFalse(store.Exists(new BankTransferLine()));
        }

        [Test]
        public void GivenStoreWithOneRecord_ExistReturnsFalseForRecordWithOtherAccount()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    Account = new Account {
                        AccountNumber = "4801234"
                    }
                }
            });
            Assert.IsFalse(store.Exists(new BankTransferLine {
                AccountNumber = "1231234"
            }));
        }

        [Test]
        public void GivenStoreWithOneRecord_ExistReturnsFalseForRecordWithOtherContraAccountNumber()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    ContraAccount = new Account {
                        AccountNumber = "3711234"
                    }
                }
            });
            Assert.IsFalse(store.Exists(new BankTransferLine
            {
                ContraAccountNumber = "1231234"
            }));
        }

        [Test]
        public void GivenStoreWithOneRecord_ExistReturnsFalseForRecordWithOtherContraAccountIban()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    ContraAccount = new Account {
                        Iban = "NL33ABN12345"
                    }
                }
            });
            Assert.IsFalse(store.Exists(new BankTransferLine
            {
                ContraAccountIban = "NL33ABN12344"
            }));
        }

        [Test]
        public void GivenStoreWithOneRecord_ExistReturnsFalseForRecordWithOtherContraAccountDescription()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    ContraAccount = new Account {
                        Description = "My Cool Shop 348543.5345"
                    }
                }
            });
            Assert.IsFalse(store.Exists(new BankTransferLine
            {
                ContraAccountDescription = "My Other Shop 934543.3455"
            }));
        }

        [Test]
        public void GivenStoreWithOneRecord_ExistReturnsFalseForRecordWithOtherTransactionDate()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    TransactionDate = DateTime.Today
                }
            });
            Assert.IsFalse(store.Exists(new BankTransferLine
            {
                TransactionDate = DateTime.Today.AddDays(1)
            }));
        }

        [Test]
        public void GivenStoreWithOneRecord_ExistReturnsFalseForRecordWithOtherInterestDate()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    InterestDate = DateTime.Today
                }
            });
            Assert.IsFalse(store.Exists(new BankTransferLine
            {
                InterestDate = DateTime.Today.AddDays(1)
            }));
        }


        [Test] public void GivenStoreWithOneRecord_ExistReturnsFalseForRecordWithOtherInitialBalance()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    InitialBalance = (decimal) 8536.44
                }
            });
            Assert.IsFalse(store.Exists(new BankTransferLine
            {
                InitialBalance = (decimal) 123.25
            }));
        }

        [Test]
        public void GivenStoreWithOneRecord_ExistReturnsFalseForRecordWithOtherFinalBalance()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    FinalBalance= (decimal) 8536.44
                }
            });
            Assert.IsFalse(store.Exists(new BankTransferLine
            {
                FinalBalance = (decimal)123.25
            }));
        }

        [Test]
        public void GivenStoreWithOneRecord_ExistReturnsTrueForMathingRecord()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    Account = new Account {
                        AccountNumber = "4801234"
                    },
                    ContraAccount = new Account {
                        AccountNumber = "3711234",
                        Iban = "NL22ING343123",
                        Description = "Store 182-90210"
                    },
                    Amount = (decimal) 27.75,
                    InitialBalance = (decimal) 27.75,
                    FinalBalance = (decimal) 55.50,
                    TransactionDate = DateTime.Today.AddHours(10),
                    InterestDate = DateTime.Today,
                    Extension = new BankTransferExtension {
                        DistributionType = DistributionType.Year,
                        CalculationDate = DateTime.Today.AddDays(3),
                        HideFromCalculations = true
                    }
                }
            });
            Assert.IsTrue(store.Exists(new BankTransferLine
            {
                AccountNumber = "4801234",
                ContraAccountNumber = "3711234",
                ContraAccountIban = "NL22ING343123",
                ContraAccountDescription = "Store 182-90210",
                Amount = (decimal)27.75,
                InitialBalance = (decimal)27.75,
                FinalBalance = (decimal)55.50,
                TransactionDate = DateTime.Today.AddHours(10),
                InterestDate = DateTime.Today
            }));
        }

        [Test]
        public void GivenEmptyStore_WhenStoreBankTransfer_StoreContainsExpectedBankTransfer()
        {
            var store = new BankTransferFileStore();
            store.Store(
                new BankTransferLine {
                    AccountNumber = "12345",
                    ContraAccountNumber = "67890",
                    ContraAccountDescription = "Hallo 1234",
                    ContraAccountIban = "IBAN293473",
                    Amount = 10,
                    InitialBalance = 20,
                    FinalBalance = 30,
                    TransactionDate = DateTime.Today,
                    InterestDate = DateTime.Today.AddDays(-1)
                });
            Assert.AreEqual(1, store.GetAll().Count);
            var banktransfer = store.GetAll().Single();
            Assert.AreEqual("12345", banktransfer.Account.AccountNumber);
            Assert.AreEqual("67890", banktransfer.ContraAccount.AccountNumber);
            Assert.AreEqual("Hallo 1234", banktransfer.ContraAccount.Description);
            Assert.AreEqual("IBAN293473", banktransfer.ContraAccount.Iban);
            Assert.AreEqual(10, banktransfer.Amount);
            Assert.AreEqual(20, banktransfer.InitialBalance);
            Assert.AreEqual(30, banktransfer.FinalBalance);
            Assert.AreEqual(DateTime.Today, banktransfer.TransactionDate);
            Assert.AreEqual(DateTime.Today.AddDays(-1), banktransfer.InterestDate);
        }

        [Test]
        public void GivenStoreWithOneBankTransfer_WhenStoreBankTransferWithSameAccount_StoreContainsTwoBankTransferLinkedToTheSameAccount()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    Account = new Account {
                        AccountNumber = "3485434"
                    }
                }
            });
            store.Store(new BankTransferLine {
                AccountNumber = "3485434"
            });
            Assert.AreEqual(2, store.GetAll().Count);
            var accountOfFirstBankTransaction = store.GetAll().First().Account;
            var accountOfSecondBankTransaction = store.GetAll().Last().Account;
            Assert.AreEqual(accountOfFirstBankTransaction, accountOfSecondBankTransaction);
        }

        [Test]
        public void GivenStoreWithOneBankTransfer_WhenStoreBankTransferWithSameContraAccount_StoreContainsTwoBankTransferLinkedToTheSameContraAccount()
        {
            var store = new BankTransferFileStoreWithTestExtension();
            store.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    ContraAccount = new Account {
                        Description = "Jan Linders 3486"
                    }
                }
            });
            store.Store(new BankTransferLine
            {
                ContraAccountDescription = "Jan Linders 3486"
            });

            Assert.AreEqual(2, store.GetAll().Count);
            var contraAccountOfFirstBankTransaction = store.GetAll().First().ContraAccount;
            var contraAccountOfSecondBankTransaction = store.GetAll().Last().ContraAccount;
            Assert.AreEqual(contraAccountOfFirstBankTransaction, contraAccountOfSecondBankTransaction);
        }
    }
}
