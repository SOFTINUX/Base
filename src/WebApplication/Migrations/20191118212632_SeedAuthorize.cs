using System;
using Microsoft.EntityFrameworkCore.Migrations;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses;
using SoftinuxBase.Security.RefreshClaimsParts;

namespace WebApplication.Migrations
{
    public partial class SeedAuthorize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder_)
        {
            migrationBuilder_.InsertData(
                "TimeStores",
                new string[] { nameof(TimeStore.Key), nameof(TimeStore.LastUpdatedTicks) },
                new object[] { AuthChangesConsts.FeatureCacheKey, DateTime.Now.Ticks });
        }

        protected override void Down(MigrationBuilder migrationBuilder_)
        {

        }
    }
}
