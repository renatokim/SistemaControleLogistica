using MaxiPago.DataContract.Transactional;
using MaxiPago.Gateway;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Testes
{
    [TestClass]
    public class MaxiPago
    {
        public void RealizarPagemento()
        {
            Transaction transaction = new Transaction();
            transaction.Environment = "TEST";

            String merchantId = "9999";
            String merchantKey = "m1fjw61dzvnrq4xxc91ngxwq";
            String referenceNum = "123456789"; ;
            decimal chargeTotal = 10;
            String creditCardNumber = "8888888";
            String expMonth = "12";
            String expYear = "2025";
            String cvvInd = null;
            String cvvNumber = "888";
            String authentication = null;
            String processorId = "1";
            String numberOfInstallments = null;
            String chargeInterest = "N";
            String ipAddress = null;
            String customerIdExt = null;
            String currencyCode = null;
            String fraudCheck = "Y";
            String softDescriptor = null;
            decimal? iataFee = null;

            var response = transaction.Auth(merchantId,
                merchantKey,
                referenceNum,
                chargeTotal,
                creditCardNumber,
                expMonth,
                expYear,
                cvvInd,
                cvvNumber,
                authentication,
                processorId,
                numberOfInstallments,
                chargeInterest,
                ipAddress,
                customerIdExt,
                currencyCode,
                fraudCheck,
                softDescriptor,
                iataFee);

            if (response.IsTransactionResponse)
            {
                TransactionResponse result = response as TransactionResponse;

                if (result.ResponseCode == "0")
                {
                    // Success
                }
                else
                {
                    // Decline
                }
            }
            else if (response.IsErrorResponse)
            {
                ErrorResponse result = response as ErrorResponse;
                // Fail
            }
        }
    }
}
