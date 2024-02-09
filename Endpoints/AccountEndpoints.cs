using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using Dapper;
using BankAPI.Repository;
using BankAPI.Services;
using BankAPI.Interfaces;

namespace BankAPI.Endpoints
{
    public static class AccountEndpoints
    {
        public static void MapAccountEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("GetAllAccounts", async (IAccountService accountService) =>
            {
                var accounts = await accountService.GetAllAccounts();
                if (accounts != null)
                {
                    return Results.Ok(accounts);
                }
                else
                {
                    return Results.NotFound("No accounts found");
                }
            });

            builder.MapGet("GetAccount/{accountId}", async (string accountId, IAccountService accountService) =>
            {
                var account = await accountService.GetAccount(accountId);
                if (account != null)
                {
                    return Results.Ok(account);
                }
                else
                {
                    return Results.NotFound("Account with ID " + accountId + " not found");
                }
            });

           
            builder.MapPut("DepositeAmount", async (string accountId, decimal amount,string currencyCode, IAccountService accountService) =>
            {
                int rowsAffected = await accountService.DepositeAmount(accountId, amount,currencyCode);
                if (rowsAffected > 0)
                {
                    return Results.Ok(amount + currencyCode+ " Amount Deposited Successfully");
                }
                else
                {
                    return Results.BadRequest(amount + currencyCode + " Amount Deposite Failed");
                }
            });

            builder.MapPut("WithdrawAmount", async (string accountId, decimal amount, IAccountService accountService) =>
            {
                int rowsAffected = await accountService.WithdrawAmount(accountId, amount);
                if (rowsAffected > 0)
                {
                    return Results.Ok(amount + "Amount Withdrawn Successfully");
                }
                else
                {
                    return Results.BadRequest(amount + "Amount withdraw Failed");
                }
            });

            builder.MapPut("TransferFunds", async (string sourceAccountId, string destinationAccountId, string serviceType, decimal amount,IAccountService accountService) =>
            {
                await accountService.TransferMoney(sourceAccountId, destinationAccountId, serviceType, amount);   
            });

            builder.MapGet("TransactionHistory", async (string accountId, DateTime startDate,IAccountService accountService) =>
            {
                Account account = await accountService.GetAccount(accountId);
                if (account != null)
                {
                    return Results.Ok(await accountService.GetTransactionHistory(accountId, startDate));
                }
                else
                {
                    return Results.NotFound("No Transaction Found");
                }
            });

        }
        
    }
}
