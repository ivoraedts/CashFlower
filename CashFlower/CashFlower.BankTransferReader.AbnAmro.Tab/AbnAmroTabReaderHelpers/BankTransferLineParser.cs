using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CashFlower.Contracts;
using CashFlower.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers
{
    public static class BankTransferLineParser
    {
        private static readonly char[] TabDelimiter = new char[] { '\t' };
        private const string Currency = "EUR";

        public static BankTransferLine Execute(string line)
        {
            var parts = line.Split(TabDelimiter, StringSplitOptions.None);
            _validateNumberOfParts(line, parts);
            _validateCurrency(parts[1]);
            var transactionDate = _retrieveTransactionDate(parts[2]);
            var contraAccountDetails = ContraAcountDetailsExtracter.Execute(parts[7]);

            return
                new BankTransferLine {
                    AccountNumber = parts[0],
                    TransactionDate = contraAccountDetails.DateTimeStamp ?? transactionDate,
                    InitialBalance = _retrieveInitialBalance(parts[3]),
                    FinalBalance = _retrieveFinalBalance(parts[4]),
                    InterestDate = _retrieveInterestDate(parts[5]),
                    Amount = _retrieveAmount(parts[6]),
                    ContraAccountDescription = contraAccountDetails.ContraAccountName,
                    ContraAccountIban = contraAccountDetails.ContraAccountIban
                };
        }

        private static void _validateNumberOfParts(string line, string[] parts)
        {
            if (parts.Count() != 8)
            {
                throw new CashFlowerException(
                    "CFE_ABN_001", "Wrong number ({0}) of TABs in line : {1}"
                    , _getNumberOfTabs(parts)
                    , line);
            }
        }

        private static int _getNumberOfTabs(IEnumerable<string> parts)
        {
            return (parts.Count() - 1);
        }

        private static void _validateCurrency(string currency)
        {
            if (!currency.Equals(Currency))
                throw new CashFlowerException("CFE_ABN_002", "No Valid currrency ({0}) given.", currency);
        }

        private static DateTime _retrieveTransactionDate(string transActionDate)
        {
            DateTime myDate;
            if (_tryParseAbnAmroDateString(transActionDate, out myDate))
                return myDate;
            throw new CashFlowerException("CFE_ABN_003", "No Valid transactiondate ({0}) given.", transActionDate);
        }

        private static DateTime _retrieveInterestDate(string interestDate)
        {
            DateTime myDate;
            if (_tryParseAbnAmroDateString(interestDate, out myDate))
                return myDate;
            throw new CashFlowerException("CFE_ABN_006", "No Valid transactiondate ({0}) given.", interestDate);
        }

        private static bool _tryParseAbnAmroDateString(string transActionDate, out DateTime myDate)
        {
            return DateTime.TryParseExact(transActionDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out myDate);
        }

        private static decimal _retrieveInitialBalance(string initialBalance)
        {
            try
            {
                return _parseAbnAmroDecimalString(initialBalance);
            }
            catch (Exception ex)
            {
                throw new CashFlowerException("CFE_ABN_004","Failed to parse the initial balance ({0}) " +
                                                            "because of the following exception: {1}",
                    initialBalance,
                    ex.Message);
            }
        }

        private static decimal _retrieveFinalBalance(string finalBalance)
        {
            try
            {
                return _parseAbnAmroDecimalString(finalBalance);
            }
            catch (Exception ex)
            {
                throw new CashFlowerException("CFE_ABN_005", "Failed to parse the final balance ({0}) " +
                    "because of the following exception: {1}",
                    finalBalance,
                    ex.Message);
            }
        }

        private static decimal _retrieveAmount(string amount)
        {
            try
            {
                return _parseAbnAmroDecimalString(amount);
            }
            catch (Exception ex)
            {
                throw new CashFlowerException("CFE_ABN_007", "Failed to parse the amount ({0}) " +
                                                             "because of the following exception: {1}",
                    amount,
                    ex.Message);
            }
        }

        private static decimal _parseAbnAmroDecimalString(string initialBalance)
        {
            if (initialBalance.Contains(".")) throw new ArgumentException("ABN AMRO decimal numbers never contain dots.");

            if (initialBalance.IndexOf(",", StringComparison.InvariantCulture)==-1) throw new ArgumentException("ABN AMRO decimals always contain one comma.");

            return
                decimal.Parse(
                    initialBalance.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture);
        }
    }
}
