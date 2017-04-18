using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CashFlower.Contracts;
using CashFlower.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers
{
    public class BankTransferLineParser
    {
        private readonly char[] _tabDelimiter = new char[] { '\t' };
        private const string Currency = "EUR";

        public BankTransferLine Execute(string line)
        {
            var parts = line.Split(_tabDelimiter, StringSplitOptions.None);
            _validateNumberOfParts(line, parts);
            _validateCurrency(parts[1]);
            var transactionDate = _retrieveTransactionDate(parts[2]);
            var contraAccountDetails = _getContraAccountDetails(parts[7]);

            return
                new BankTransferLine {
                    AccountNumber = parts[0],
                    TransactionDate = contraAccountDetails.DateTimeStamp ?? transactionDate,
                    InitialBalance = _retrieveInitialBalance(parts[3]),
                    FinalBalance = _retrieveFinalBalance(parts[4]),
                    InterestDate = _retrieveInterestDate(parts[5]),
                    Amount = _retrieveAmount(parts[6]),
                    ContraAccountDescription = contraAccountDetails.ContraAccountName

                };
        }

        private class ContraAccountDetails
        {
            public DateTime? DateTimeStamp { get; set; }
            public string ContraAccountName { get; set; }
        }

        private ContraAccountDetails _getContraAccountDetails(string contraAccountDetails)
        {
            if (contraAccountDetails.StartsWith("BEA   NR:"))
                return _getDetailsFromPointOfSaleTerminalDetails(contraAccountDetails);
            throw new NotImplementedException();
        }

        private static ContraAccountDetails _getDetailsFromPointOfSaleTerminalDetails(string contraAccountDetails)
        {
            if (contraAccountDetails.Length<33)
                throw new CashFlowerException("CFE_ABN_008", 
                    "Fewer characters ({0}) than excpected (33) in Point of Sale Terminal Details: '{1}'"
                    , contraAccountDetails.Length, contraAccountDetails);
                
            return new ContraAccountDetails {
                DateTimeStamp = _parsePointOfSaleTerminalDetailsTimeStamp(contraAccountDetails.Substring(18, 14)),
                ContraAccountName = _stripCardnumberIfSpecified(contraAccountDetails.Substring(33))
            };
        }

        private static string _stripCardnumberIfSpecified(string contraAccountName)
        {
            return contraAccountName.IndexOf(",PAS", StringComparison.InvariantCulture) == -1 ? 
            contraAccountName : 
            contraAccountName.Substring(0, contraAccountName.IndexOf(",PAS", StringComparison.InvariantCulture));
        }

        private static DateTime _parsePointOfSaleTerminalDetailsTimeStamp(string contraAccountDetailsTimeStamp)
        {
            DateTime myDate;
            if (_tryParsePointOfSaleTerminalDetailsTimeStamp(contraAccountDetailsTimeStamp, out myDate))
                return myDate;
            throw new CashFlowerException("CFE_ABN_009", "No Valid contraAccountDetailsTimeStamp ({0}) given.", contraAccountDetailsTimeStamp);
        }

        private static bool _tryParsePointOfSaleTerminalDetailsTimeStamp(string contraAccountDetailsTimeStamp, out DateTime myDate)
        {
            return DateTime.TryParseExact(contraAccountDetailsTimeStamp, @"dd.MM.yy/HH.mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out myDate);
        }

        private decimal _retrieveAmount(string amount)
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

        private decimal _retrieveFinalBalance(string finalBalance)
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

        private decimal _retrieveInitialBalance(string initialBalance)
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

        private decimal _parseAbnAmroDecimalString(string initialBalance)
        {
            if (initialBalance.Contains(".")) throw new ArgumentException("ABN AMRO decimal numbers never contain dots.");

            if (initialBalance.IndexOf(",", StringComparison.InvariantCulture)==-1) throw new ArgumentException("ABN AMRO decimals always contain one comma.");

            return
                decimal.Parse(
                    initialBalance.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture);
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

        private static bool _tryParseAbnAmroDateString(string transActionDate, out DateTime myDate)
        {
            return DateTime.TryParseExact(transActionDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out myDate);
        }

        private DateTime _retrieveInterestDate(string interestDate)
        {
            DateTime myDate;
            if (_tryParseAbnAmroDateString(interestDate, out myDate))
                return myDate;
            throw new CashFlowerException("CFE_ABN_006", "No Valid transactiondate ({0}) given.", interestDate);
        }

        private static int _getNumberOfTabs(IEnumerable<string> parts)
        {
            return (parts.Count() - 1);
        }
    }
}
