using System.Data.SqlClient;
using Dapper;
using BankAPI.Repository;
using BankAPI.Services;
using BankAPI.Interfaces;

namespace BankAPI.Endpoints
{
    public static class BankEndpoints
    {
        public static void MapBankEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("GetAllBanks", async (IBankService bankService) =>
            {
                var banks = await bankService.GetAllBanks();
                if (banks != null)
                {
                    return Results.Ok(banks);
                }
                else
                {
                    return Results.NotFound("No banks found");
                }
            });

            builder.MapGet("GetBank/{bankId}", async (string bankId, IBankService bankService) =>
            {
                var bank = await bankService.GetBank(bankId);
                if (bank != null)
                {
                    return Results.Ok(bank);
                }
                else
                {
                    return Results.NotFound("Bank with ID " + bankId + " not found");
                }
            });

            builder.MapPost("CreateBank", async (Bank bank, IBankService bankService) =>
            {
                var bankId = bank.BankName.Substring(0, 3) + DateTime.Now.ToString("ddMMyyyy");
                int rowsAffected = await bankService.CreateBank(bankId, bank.BankName);
                if (rowsAffected > 0)
                {
                    return Results.Ok("Bank Created Successfully");
                }
                else
                {
                    return Results.BadRequest("Failed to create bank");
                }
            });

        }
    }
}
