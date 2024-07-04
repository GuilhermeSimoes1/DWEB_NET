﻿// <auto-generated />
using System;
using DWEB_NET.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DWEB_NET.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240704115749_SegundaMigracao")]
    partial class SegundaMigracao
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DWEB_NET.Models.TblCategorias", b =>
                {
                    b.Property<int>("CategoriaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoriaID"));

                    b.Property<string>("NomeCategoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoriaID");

                    b.ToTable("Categorias");

                    b.HasData(
                        new
                        {
                            CategoriaID = 1,
                            NomeCategoria = "Saúde"
                        },
                        new
                        {
                            CategoriaID = 2,
                            NomeCategoria = "Lazer"
                        },
                        new
                        {
                            CategoriaID = 3,
                            NomeCategoria = "Casa"
                        },
                        new
                        {
                            CategoriaID = 4,
                            NomeCategoria = "Educação"
                        },
                        new
                        {
                            CategoriaID = 5,
                            NomeCategoria = "Alimentação"
                        },
                        new
                        {
                            CategoriaID = 6,
                            NomeCategoria = "Outros"
                        },
                        new
                        {
                            CategoriaID = 7,
                            NomeCategoria = "Salário"
                        },
                        new
                        {
                            CategoriaID = 8,
                            NomeCategoria = "Investimentos"
                        });
                });

            modelBuilder.Entity("DWEB_NET.Models.TblContas", b =>
                {
                    b.Property<int>("ContaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContaID"));

                    b.Property<string>("NomeConta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Saldo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserFK")
                        .HasColumnType("int");

                    b.HasKey("ContaID");

                    b.HasIndex("UserFK");

                    b.ToTable("Contas");
                });

            modelBuilder.Entity("DWEB_NET.Models.TblOrcamentos", b =>
                {
                    b.Property<int>("OrcamentoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrcamentoID"));

                    b.Property<DateTime?>("DataFinal")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInicial")
                        .HasColumnType("datetime2");

                    b.Property<string>("NomeOrcamento")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserFK")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorAtual")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ValorNecessario")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("OrcamentoID");

                    b.HasIndex("UserFK");

                    b.ToTable("Orcamentos");
                });

            modelBuilder.Entity("DWEB_NET.Models.TblTransacoes", b =>
                {
                    b.Property<int>("TransacaoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransacaoID"));

                    b.Property<int>("CategoriaFK")
                        .HasColumnType("int");

                    b.Property<int>("ContaFK")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserFK")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorTransacao")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("TransacaoID");

                    b.HasIndex("CategoriaFK");

                    b.HasIndex("ContaFK");

                    b.HasIndex("UserFK");

                    b.ToTable("Transacoes");
                });

            modelBuilder.Entity("DWEB_NET.Models.TblTransacoesCategorias", b =>
                {
                    b.Property<int>("TransacaoFK")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaFK")
                        .HasColumnType("int");

                    b.HasKey("TransacaoFK", "CategoriaFK");

                    b.HasIndex("CategoriaFK");

                    b.ToTable("TransacoesCategorias");
                });

            modelBuilder.Entity("DWEB_NET.Models.TblUtilizadores", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Passwd")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UserAutent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserID");

                    b.ToTable("Utilizadores");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DWEB_NET.Models.TblContas", b =>
                {
                    b.HasOne("DWEB_NET.Models.TblUtilizadores", "UserID")
                        .WithMany("ListaContas")
                        .HasForeignKey("UserFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserID");
                });

            modelBuilder.Entity("DWEB_NET.Models.TblOrcamentos", b =>
                {
                    b.HasOne("DWEB_NET.Models.TblUtilizadores", "UserID")
                        .WithMany("ListaOrcamentos")
                        .HasForeignKey("UserFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserID");
                });

            modelBuilder.Entity("DWEB_NET.Models.TblTransacoes", b =>
                {
                    b.HasOne("DWEB_NET.Models.TblCategorias", "CategoriaID")
                        .WithMany()
                        .HasForeignKey("CategoriaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DWEB_NET.Models.TblContas", "ContaID")
                        .WithMany("ListaTransacoes")
                        .HasForeignKey("ContaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DWEB_NET.Models.TblUtilizadores", "UserID")
                        .WithMany("ListaTransacoes")
                        .HasForeignKey("UserFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoriaID");

                    b.Navigation("ContaID");

                    b.Navigation("UserID");
                });

            modelBuilder.Entity("DWEB_NET.Models.TblTransacoesCategorias", b =>
                {
                    b.HasOne("DWEB_NET.Models.TblCategorias", "CategoriaID")
                        .WithMany("ListaTransacoesCategorias")
                        .HasForeignKey("CategoriaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DWEB_NET.Models.TblTransacoes", "TransacaoID")
                        .WithMany("ListaTransacoesCategorias")
                        .HasForeignKey("TransacaoFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoriaID");

                    b.Navigation("TransacaoID");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DWEB_NET.Models.TblCategorias", b =>
                {
                    b.Navigation("ListaTransacoesCategorias");
                });

            modelBuilder.Entity("DWEB_NET.Models.TblContas", b =>
                {
                    b.Navigation("ListaTransacoes");
                });

            modelBuilder.Entity("DWEB_NET.Models.TblTransacoes", b =>
                {
                    b.Navigation("ListaTransacoesCategorias");
                });

            modelBuilder.Entity("DWEB_NET.Models.TblUtilizadores", b =>
                {
                    b.Navigation("ListaContas");

                    b.Navigation("ListaOrcamentos");

                    b.Navigation("ListaTransacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
