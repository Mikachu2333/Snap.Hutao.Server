﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Snap.Hutao.Server.Model.Context;

#nullable disable

namespace Snap.Hutao.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Achievement.EntityAchievement", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<uint>("Id"));

                    b.Property<long>("ArchiveId")
                        .HasColumnType("bigint");

                    b.Property<uint>("Current")
                        .HasColumnType("int unsigned");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ArchiveId");

                    b.ToTable("achievements");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Achievement.EntityAchievementArchive", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("achievement_archives");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.AllowedVersion", b =>
                {
                    b.Property<string>("Header")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Header");

                    b.ToTable("allowed_versions");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Announcement.EntityAnnouncement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Locale")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Kind")
                        .HasColumnType("int");

                    b.Property<long>("LastUpdateTime")
                        .HasColumnType("bigint");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MaxPresentVersion")
                        .HasColumnType("longtext");

                    b.Property<int>("Severity")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id", "Locale");

                    b.ToTable("announcements");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.GachaLog.EntityGachaItem", b =>
                {
                    b.Property<string>("Uid")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<int>("GachaType")
                        .HasColumnType("int");

                    b.Property<bool>("IsTrusted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("QueryType")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Time")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Uid", "Id");

                    b.HasIndex("UserId");

                    b.ToTable("gacha_items");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.GachaLog.GachaStatistics", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("PrimaryId");

                    b.ToTable("gacha_statistics");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.GachaLog.InvalidGachaUid", b =>
                {
                    b.Property<string>("Uid")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Uid");

                    b.ToTable("invalid_gacha_uids");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.LicenseApplicationRecord", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<string>("ApprovalCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ProjectUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PrimaryId");

                    b.HasIndex("UserId");

                    b.ToTable("license_application_records");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Passport.GithubIdentity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ExipresAt")
                        .HasColumnType("bigint");

                    b.Property<string>("NodeId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GithubIdentities");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Passport.HutaoUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<long>("GachaLogExpireAt")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsLicensedDeveloper")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsMaintainer")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Passport.PassportVerification", b =>
                {
                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(255)");

                    b.Property<long>("ExpireTimestamp")
                        .HasColumnType("bigint");

                    b.Property<long>("GeneratedTimestamp")
                        .HasColumnType("bigint");

                    b.Property<string>("VerifyCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("NormalizedUserName");

                    b.ToTable("passport_verifications");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.RequestStatistics", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("PrimaryId");

                    b.ToTable("request_statistics");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.Banned", b =>
                {
                    b.Property<string>("Uid")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Uid");

                    b.ToTable("banned");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityAvatar", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<int>("ActivedConstellationNumber")
                        .HasColumnType("int");

                    b.Property<int>("AvatarId")
                        .HasColumnType("int");

                    b.Property<long>("RecordId")
                        .HasColumnType("bigint");

                    b.Property<string>("ReliquarySet")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("WeaponId")
                        .HasColumnType("int");

                    b.HasKey("PrimaryId");

                    b.HasIndex("RecordId");

                    b.ToTable("avatars");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityDamageRank", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<int>("AvatarId")
                        .HasColumnType("int");

                    b.Property<long>("SpiralAbyssId")
                        .HasColumnType("bigint");

                    b.Property<string>("Uid")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("PrimaryId");

                    b.HasIndex("SpiralAbyssId")
                        .IsUnique();

                    b.HasIndex("Value");

                    b.ToTable("damage_ranks");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityFloor", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Levels")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("SpiralAbyssId")
                        .HasColumnType("bigint");

                    b.Property<int>("Star")
                        .HasColumnType("int");

                    b.HasKey("PrimaryId");

                    b.HasIndex("SpiralAbyssId");

                    b.ToTable("spiral_abysses_floors");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityRecord", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<string>("Uid")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<long>("UploadTime")
                        .HasColumnType("bigint");

                    b.Property<string>("Uploader")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.HasKey("PrimaryId");

                    b.ToTable("records");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntitySpiralAbyss", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<long>("RecordId")
                        .HasColumnType("bigint");

                    b.Property<int>("TotalBattleTimes")
                        .HasColumnType("int");

                    b.Property<int>("TotalWinTimes")
                        .HasColumnType("int");

                    b.HasKey("PrimaryId");

                    b.HasIndex("RecordId")
                        .IsUnique();

                    b.ToTable("spiral_abysses");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityTakeDamageRank", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<int>("AvatarId")
                        .HasColumnType("int");

                    b.Property<long>("SpiralAbyssId")
                        .HasColumnType("bigint");

                    b.Property<string>("Uid")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("PrimaryId");

                    b.HasIndex("SpiralAbyssId")
                        .IsUnique();

                    b.HasIndex("Value");

                    b.ToTable("take_damage_ranks");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.LegacyStatistics", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int");

                    b.HasKey("PrimaryId");

                    b.ToTable("spiral_abysses_statistics");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Telemetry.HutaoLog", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Resolved")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("PrimaryId");

                    b.ToTable("hutao_logs");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Telemetry.HutaoLogSingleItem", b =>
                {
                    b.Property<long>("PrimaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("PrimaryId"));

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<long>("LogId")
                        .HasColumnType("bigint");

                    b.Property<long>("Time")
                        .HasColumnType("bigint");

                    b.HasKey("PrimaryId");

                    b.HasIndex("LogId");

                    b.ToTable("hutao_log_items");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.Passport.HutaoUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.Passport.HutaoUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Snap.Hutao.Server.Model.Entity.Passport.HutaoUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.Passport.HutaoUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Achievement.EntityAchievement", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.Achievement.EntityAchievementArchive", "Archive")
                        .WithMany()
                        .HasForeignKey("ArchiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Archive");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Achievement.EntityAchievementArchive", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.Passport.HutaoUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.GachaLog.EntityGachaItem", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.Passport.HutaoUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.LicenseApplicationRecord", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.Passport.HutaoUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Passport.GithubIdentity", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.Passport.HutaoUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityAvatar", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityRecord", "Record")
                        .WithMany("Avatars")
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Record");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityDamageRank", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntitySpiralAbyss", "SpiralAbyss")
                        .WithOne("Damage")
                        .HasForeignKey("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityDamageRank", "SpiralAbyssId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SpiralAbyss");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityFloor", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntitySpiralAbyss", "SpiralAbyss")
                        .WithMany("Floors")
                        .HasForeignKey("SpiralAbyssId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SpiralAbyss");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntitySpiralAbyss", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityRecord", "Record")
                        .WithOne("SpiralAbyss")
                        .HasForeignKey("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntitySpiralAbyss", "RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Record");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityTakeDamageRank", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntitySpiralAbyss", "SpiralAbyss")
                        .WithOne("TakeDamage")
                        .HasForeignKey("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityTakeDamageRank", "SpiralAbyssId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SpiralAbyss");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.Telemetry.HutaoLogSingleItem", b =>
                {
                    b.HasOne("Snap.Hutao.Server.Model.Entity.Telemetry.HutaoLog", "Log")
                        .WithMany()
                        .HasForeignKey("LogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Log");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntityRecord", b =>
                {
                    b.Navigation("Avatars");

                    b.Navigation("SpiralAbyss");
                });

            modelBuilder.Entity("Snap.Hutao.Server.Model.Entity.SpiralAbyss.EntitySpiralAbyss", b =>
                {
                    b.Navigation("Damage")
                        .IsRequired();

                    b.Navigation("Floors");

                    b.Navigation("TakeDamage")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
