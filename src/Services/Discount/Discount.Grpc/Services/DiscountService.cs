using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;

namespace Discount.Grpc.Services;

public class DiscountService
    (DiscountContext dbContext, ILogger <DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        //TODO: GetDiscount from Database
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName, context.CancellationToken);

        if (coupon is null)
        {
            coupon = new Coupon
            {
                ProductName = "No Discount",
                Description = "No Discount Description",
                Amount = 0
            };
        }

        logger.LogInformation("Discount is retrieved for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;

    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon cannot be null"));
        }

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount is successfully created for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    override public async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon cannot be null"));
        }

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount is successfully updated for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName, context.CancellationToken);
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found"));
        }

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount is successfully deleted for ProductName: {ProductName}", request.ProductName);
        return new DeleteDiscountResponse
        {
            Success = true
        };
    }
}