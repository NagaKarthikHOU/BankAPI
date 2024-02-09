using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using BankAPI.Repository;
using BankAPI.Interfaces;

namespace BankAPI.Endpoints
{
    public static class StaffEndpoints
    {
        public static void MapStaffEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("GetAllStaff", async (IStaffService staffService) =>
            {
                var staffs = await staffService.GetAllStaff();
                if (staffs != null)
                {
                    return Results.Ok(staffs);
                }
                else
                {
                    return Results.NotFound("No staff found");
                }
            });

            builder.MapGet("GetStaff/{staffId}", async (string staffId, IStaffService staffService) =>
            {
                var staff = await staffService.GetStaff(staffId);
                if (staff != null)
                {
                    return Results.Ok(staff);
                }
                else
                {
                    return Results.NotFound("Staff with ID " + staffId + " not found");
                }
            });

            builder.MapPost("CreateStaff", async (Staff staff, IStaffService staffService) =>
            {
                int rowsAffected = await staffService.CreateStaff(staff);
                if (rowsAffected > 0)
                {
                    return Results.Ok("Staff Created Successfully");
                }
                else
                {
                    return Results.BadRequest("Failed to create staff");
                }
            });

            builder.MapPost("CreateAccount", async (Account account, IStaffService staffService) =>
            {
                int rowsInserted = await staffService.CreateAccount(account);
                if (rowsInserted > 0)
                {
                    return Results.Ok("Account Created Successfully");
                }
                else
                {
                    return Results.BadRequest("Failed to create Account");
                }

            });

            builder.MapDelete("DeleteAccount/{accountId}", async (string accountId, IStaffService staffService) =>
            {
                int rowsAffected = await staffService.DeleteAccount(accountId);
                if (rowsAffected > 0)
                {
                    return Results.Ok("Account Deleted Successfully");
                }
                else
                {
                    return Results.BadRequest("Failed to Delete Account");
                }
            });

            builder.MapPut("ResetPassword", async (string accountId, string password, IStaffService staffService) =>
            {
                int rowsAffected = await staffService.ResetPassword(accountId, password);
                if (rowsAffected > 0)
                {
                    return Results.Ok("Password Reset Successfull");
                }
                else
                {
                    return Results.BadRequest("Password Reset Failed");
                }
            });

            builder.MapPut("AddCurrency", async (Currency currency, IStaffService staffService) =>
            {
                int rowsAffected = await staffService.AddCurrency(currency.CurrencyCode, currency.ExchangeRate);
                if (rowsAffected > 0)
                {
                    return Results.Ok("Currency Added Successfully");
                }
                else
                {
                    return Results.BadRequest("Failed to Add Currency");
                }
            });
        }
    }
}
