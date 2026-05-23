using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    AnnonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportsCounter = table.Column<int>(type: "int", nullable: false),
                    FrontImageID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackImageID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityStatus = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Designers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Designers_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationsType = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    RefrenceUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producers_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppartmentNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Goverate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Selected = table.Column<bool>(type: "bit", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Addresses_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelChats",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelChats", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ModelChats_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignerDesigns",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DesignerID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignerDesigns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DesignerDesigns_Designers_DesignerID",
                        column: x => x.DesignerID,
                        principalTable: "Designers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignVerifications",
                columns: table => new
                {
                    StepUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DesignerID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignVerifications", x => x.StepUrl);
                    table.ForeignKey(
                        name: "FK_DesignVerifications_Designers_DesignerID",
                        column: x => x.DesignerID,
                        principalTable: "Designers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProdicerLimit = table.Column<int>(type: "int", nullable: false),
                    CustomerLimit = table.Column<int>(type: "int", nullable: false),
                    ProducerCount = table.Column<int>(type: "int", nullable: false),
                    CustomerCount = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProducerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Chats_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chats_Producers_ProducerID",
                        column: x => x.ProducerID,
                        principalTable: "Producers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LicenseVerifications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseStatus = table.Column<int>(type: "int", nullable: false),
                    ProducerID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseVerifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LicenseVerifications_Producers_ProducerID",
                        column: x => x.ProducerID,
                        principalTable: "Producers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsProducer = table.Column<bool>(type: "bit", nullable: false),
                    ProducerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DesignerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reports_Designers_DesignerID",
                        column: x => x.DesignerID,
                        principalTable: "Designers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Producers_ProducerID",
                        column: x => x.ProducerID,
                        principalTable: "Producers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProducerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reviews_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_Producers_ProducerID",
                        column: x => x.ProducerID,
                        principalTable: "Producers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModelChatMessages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Limit = table.Column<int>(type: "int", nullable: false),
                    LimitCounter = table.Column<int>(type: "int", nullable: false),
                    Sender = table.Column<int>(type: "int", nullable: false),
                    ModelChatID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelChatMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ModelChatMessages_ModelChats_ModelChatID",
                        column: x => x.ModelChatID,
                        principalTable: "ModelChats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignerDesignProducer",
                columns: table => new
                {
                    FavoritedByID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FavouriteID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignerDesignProducer", x => new { x.FavoritedByID, x.FavouriteID });
                    table.ForeignKey(
                        name: "FK_DesignerDesignProducer_DesignerDesigns_FavouriteID",
                        column: x => x.FavouriteID,
                        principalTable: "DesignerDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesignerDesignProducer_Producers_FavoritedByID",
                        column: x => x.FavoritedByID,
                        principalTable: "Producers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProducerDesignerOffers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OfferStatus = table.Column<int>(type: "int", nullable: false),
                    ProducerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DesignerDesignID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerDesignerOffers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProducerDesignerOffers_DesignerDesigns_DesignerDesignID",
                        column: x => x.DesignerDesignID,
                        principalTable: "DesignerDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProducerDesignerOffers_Producers_ProducerID",
                        column: x => x.ProducerID,
                        principalTable: "Producers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sender = table.Column<int>(type: "int", nullable: false),
                    ChatID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatID",
                        column: x => x.ChatID,
                        principalTable: "Chats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActiveOfferLogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsOfferActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveOfferLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCustomOffer",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CustomerOfferStatus = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProducerDesignID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProducerCustomerOfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCustomOffer", x => x.ID);
                    table.UniqueConstraint("AK_CustomerCustomOffer_ID_IsActive", x => new { x.ID, x.IsActive });
                    table.ForeignKey(
                        name: "FK_CustomerCustomOffer_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProducerDesigns",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CustomerCustomOfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProducerID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerDesigns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProducerDesigns_CustomerCustomOffer_CustomerCustomOfferID",
                        column: x => x.CustomerCustomOfferID,
                        principalTable: "CustomerCustomOffer",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProducerDesigns_Producers_ProducerID",
                        column: x => x.ProducerID,
                        principalTable: "Producers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDesigns",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CustomerPublishedOfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDesigns", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPublishedOffers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CustomerOfferStatus = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerDesignID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPublishedOffers", x => x.ID);
                    table.UniqueConstraint("AK_CustomerPublishedOffers_ID_IsActive", x => new { x.ID, x.IsActive });
                    table.ForeignKey(
                        name: "FK_CustomerPublishedOffers_CustomerDesigns_CustomerDesignID",
                        column: x => x.CustomerDesignID,
                        principalTable: "CustomerDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerPublishedOffers_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignImages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerDesignID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProducerDesignID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DesignerDesignID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DesignImages_CustomerDesigns_CustomerDesignID",
                        column: x => x.CustomerDesignID,
                        principalTable: "CustomerDesigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesignImages_DesignerDesigns_DesignerDesignID",
                        column: x => x.DesignerDesignID,
                        principalTable: "DesignerDesigns",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DesignImages_ProducerDesigns_ProducerDesignID",
                        column: x => x.ProducerDesignID,
                        principalTable: "ProducerDesigns",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Escrows",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HeldAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleasedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EscrowStatus = table.Column<int>(type: "int", nullable: false),
                    TransactionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProducerOfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerOfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsOfferActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escrows", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Escrows_CustomerCustomOffer_CustomerOfferID_IsOfferActive",
                        columns: x => new { x.CustomerOfferID, x.IsOfferActive },
                        principalTable: "CustomerCustomOffer",
                        principalColumns: new[] { "ID", "IsActive" });
                    table.ForeignKey(
                        name: "FK_Escrows_CustomerPublishedOffers_CustomerOfferID_IsOfferActive",
                        columns: x => new { x.CustomerOfferID, x.IsOfferActive },
                        principalTable: "CustomerPublishedOffers",
                        principalColumns: new[] { "ID", "IsActive" });
                });

            migrationBuilder.CreateTable(
                name: "ProducerCustomerOffers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OfferStatus = table.Column<int>(type: "int", nullable: false),
                    ProducerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerCustomOfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerPublishedOfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerCustomerOffers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProducerCustomerOffers_CustomerCustomOffer_CustomerCustomOfferID",
                        column: x => x.CustomerCustomOfferID,
                        principalTable: "CustomerCustomOffer",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProducerCustomerOffers_CustomerPublishedOffers_CustomerPublishedOfferID",
                        column: x => x.CustomerPublishedOfferID,
                        principalTable: "CustomerPublishedOffers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProducerCustomerOffers_Producers_ProducerID",
                        column: x => x.ProducerID,
                        principalTable: "Producers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscrowID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Escrows_EscrowID",
                        column: x => x.EscrowID,
                        principalTable: "Escrows",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveOfferLogs_OfferID_IsOfferActive",
                table: "ActiveOfferLogs",
                columns: new[] { "OfferID", "IsOfferActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerID",
                table: "Addresses",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_CustomerID",
                table: "Chats",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ProducerID",
                table: "Chats",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCustomOffer_CustomerID",
                table: "CustomerCustomOffer",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCustomOffer_ID_IsActive",
                table: "CustomerCustomOffer",
                columns: new[] { "ID", "IsActive" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCustomOffer_ProducerCustomerOfferID",
                table: "CustomerCustomOffer",
                column: "ProducerCustomerOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCustomOffer_ProducerDesignID",
                table: "CustomerCustomOffer",
                column: "ProducerDesignID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDesigns_CustomerPublishedOfferID",
                table: "CustomerDesigns",
                column: "CustomerPublishedOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPublishedOffers_CustomerDesignID",
                table: "CustomerPublishedOffers",
                column: "CustomerDesignID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPublishedOffers_CustomerID",
                table: "CustomerPublishedOffers",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPublishedOffers_ID_IsActive",
                table: "CustomerPublishedOffers",
                columns: new[] { "ID", "IsActive" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_DesignerDesignProducer_FavouriteID",
                table: "DesignerDesignProducer",
                column: "FavouriteID");

            migrationBuilder.CreateIndex(
                name: "IX_DesignerDesigns_DesignerID",
                table: "DesignerDesigns",
                column: "DesignerID");

            migrationBuilder.CreateIndex(
                name: "IX_DesignImages_CustomerDesignID",
                table: "DesignImages",
                column: "CustomerDesignID");

            migrationBuilder.CreateIndex(
                name: "IX_DesignImages_DesignerDesignID",
                table: "DesignImages",
                column: "DesignerDesignID");

            migrationBuilder.CreateIndex(
                name: "IX_DesignImages_ProducerDesignID",
                table: "DesignImages",
                column: "ProducerDesignID");

            migrationBuilder.CreateIndex(
                name: "IX_DesignVerifications_DesignerID",
                table: "DesignVerifications",
                column: "DesignerID");

            migrationBuilder.CreateIndex(
                name: "IX_Escrows_CustomerOfferID_IsOfferActive",
                table: "Escrows",
                columns: new[] { "CustomerOfferID", "IsOfferActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Escrows_ProducerOfferID",
                table: "Escrows",
                column: "ProducerOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseVerifications_ProducerID",
                table: "LicenseVerifications",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatID",
                table: "Messages",
                column: "ChatID");

            migrationBuilder.CreateIndex(
                name: "IX_ModelChatMessages_ModelChatID",
                table: "ModelChatMessages",
                column: "ModelChatID");

            migrationBuilder.CreateIndex(
                name: "IX_ModelChats_CustomerID",
                table: "ModelChats",
                column: "CustomerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserID",
                table: "Notifications",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerCustomerOffers_CustomerCustomOfferID",
                table: "ProducerCustomerOffers",
                column: "CustomerCustomOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerCustomerOffers_CustomerPublishedOfferID",
                table: "ProducerCustomerOffers",
                column: "CustomerPublishedOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerCustomerOffers_ProducerID",
                table: "ProducerCustomerOffers",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerDesignerOffers_DesignerDesignID",
                table: "ProducerDesignerOffers",
                column: "DesignerDesignID");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerDesignerOffers_ProducerID",
                table: "ProducerDesignerOffers",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerDesigns_CustomerCustomOfferID",
                table: "ProducerDesigns",
                column: "CustomerCustomOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerDesigns_ProducerID",
                table: "ProducerDesigns",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_DesignerID",
                table: "Reports",
                column: "DesignerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ProducerID",
                table: "Reports",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerID",
                table: "Reviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProducerID",
                table: "Reviews",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_EscrowID",
                table: "Transactions",
                column: "EscrowID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserID",
                table: "Transactions",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffer_OfferID_IsOfferActive",
                table: "ActiveOfferLogs",
                columns: new[] { "OfferID", "IsOfferActive" },
                principalTable: "CustomerCustomOffer",
                principalColumns: new[] { "ID", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerPublishedOffers_OfferID_IsOfferActive",
                table: "ActiveOfferLogs",
                columns: new[] { "OfferID", "IsOfferActive" },
                principalTable: "CustomerPublishedOffers",
                principalColumns: new[] { "ID", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCustomOffer_ProducerCustomerOffers_ProducerCustomerOfferID",
                table: "CustomerCustomOffer",
                column: "ProducerCustomerOfferID",
                principalTable: "ProducerCustomerOffers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCustomOffer_ProducerDesigns_ProducerDesignID",
                table: "CustomerCustomOffer",
                column: "ProducerDesignID",
                principalTable: "ProducerDesigns",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDesigns_CustomerPublishedOffers_CustomerPublishedOfferID",
                table: "CustomerDesigns",
                column: "CustomerPublishedOfferID",
                principalTable: "CustomerPublishedOffers",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProducerCustomerOffers_CustomerCustomOffer_CustomerCustomOfferID",
                table: "ProducerCustomerOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProducerDesigns_CustomerCustomOffer_CustomerCustomOfferID",
                table: "ProducerDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDesigns_CustomerPublishedOffers_CustomerPublishedOfferID",
                table: "CustomerDesigns");

            migrationBuilder.DropTable(
                name: "ActiveOfferLogs");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DesignerDesignProducer");

            migrationBuilder.DropTable(
                name: "DesignImages");

            migrationBuilder.DropTable(
                name: "DesignVerifications");

            migrationBuilder.DropTable(
                name: "LicenseVerifications");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ModelChatMessages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ProducerDesignerOffers");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "ModelChats");

            migrationBuilder.DropTable(
                name: "DesignerDesigns");

            migrationBuilder.DropTable(
                name: "Escrows");

            migrationBuilder.DropTable(
                name: "Designers");

            migrationBuilder.DropTable(
                name: "CustomerCustomOffer");

            migrationBuilder.DropTable(
                name: "ProducerCustomerOffers");

            migrationBuilder.DropTable(
                name: "ProducerDesigns");

            migrationBuilder.DropTable(
                name: "Producers");

            migrationBuilder.DropTable(
                name: "CustomerPublishedOffers");

            migrationBuilder.DropTable(
                name: "CustomerDesigns");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
