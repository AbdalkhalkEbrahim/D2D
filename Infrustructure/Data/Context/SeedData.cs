using Domain.Entities.Chats;
using Domain.Entities.Chats.AiModel;
using Domain.Entities.Customers;
using Domain.Entities.Designers;
using Domain.Entities.Designs;
using Domain.Entities.Offers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AL = Domain.Entities.Offers;
namespace Infrastructure.Data.Context
{
    public static class SeedData
    {
        public static async Task SeedAsync(D2DContext context, UserManager<User> userManager)
        {
     /*       if (await context.Users.AnyAsync())
                return;*/

            //await context.Database.EnsureCreatedAsync();

            // 2. Seed 50 Customers
            for (int i = 1; i <= 50; i++)
            {
                var cust = new Customer
                {
                    UserName = $"customer_{i}",
                    Email = $"customer{i}@d2d.com",
                    EmailConfirmed = true,
                    FirstName = $"CustomerFirst{i}",
                    LastName = $"CustomerLast{i}",
                    BD = new DateTime(1995, 1, i % 28 + 1),
                    UserType = UserType.Customer,
                    AnonName = $"cust_anon_{i}",
                    Balance = 2000.0m
                };
                await userManager.CreateAsync(cust, "P@ssw0rd1!");
            }

            // 3. Seed 50 Producers
            for (int i = 1; i <= 50; i++)
            {
                var prod = new Producer
                {
                    UserName = $"producer_{i}",
                    Email = $"producer{i}@d2d.com",
                    EmailConfirmed = true,
                    FirstName = $"ProducerFirst{i}",
                    LastName = $"ProducerLast{i}",
                    BD = new DateTime(1990, 5, i % 28 + 1),
                    UserType = UserType.Producer,
                    AnonName = $"prod_anon_{i}",
                    Balance = 1500.0m,
                    Rate = 20.0d + (i % 6),
                    RateCount = 5
                };
                await userManager.CreateAsync(prod, "P@ssw0rd1!");
            }

            // 4. Seed 50 Designers
            for (int i = 1; i <= 50; i++)
            {
                var des = new Designer
                {
                    UserName = $"designer_{i}",
                    Email = $"designer{i}@d2d.com",
                    EmailConfirmed = true,
                    FirstName = $"DesignerFirst{i}",
                    LastName = $"DesignerLast{i}",
                    BD = new DateTime(1993, 8, i % 28 + 1),
                    UserType = UserType.Designer,
                    AnonName = $"des_anon_{i}",
                    Balance = 1000.0m
                };
                await userManager.CreateAsync(des, "P@ssw0rd1!");
            }

            await context.SaveChangesAsync();

            // load created users as derived types to ensure EF materializes the correct CLR type
            var dbCustomers = await context.Users.OfType<Customer>().ToListAsync();
            var dbProducers = await context.Users.OfType<Producer>().ToListAsync();

            // 5.a Seed addresses for each customer (1-3 addresses)
            for (int i = 0; i < dbCustomers.Count; i++)
            {
                var cust = dbCustomers[i];
                var addrCount = (i % 3) + 1; // 1..3 addresses
                for (int a = 0; a < addrCount; a++)
                {
                    var address = new Address
                    {
                        AppartmentNo = (a + 1).ToString(),
                        BuildingNumber = 100 + i,
                        Street = $"Street {i + 1}-{a + 1}",
                        District = $"District {(i % 5) + 1}",
                        City = $"City {(i % 10) + 1}",
                        Goverate = $"Goverate {(i % 3) + 1}",
                        Selected = a == 0,
                        CustomerID = cust.Id
                    };
                    context.Addresses.Add(address);
                }
            }
            await context.SaveChangesAsync();

            // 5. Seed 50 CustomerDesigns
            var customerDesigns = new List<CustomerDesign>();
            for (int i = 0; i < 50; i++)
            {
                var design = new CustomerDesign
                {
                    ID = Guid.NewGuid(),
                    Name = $"Design Layout Project {i + 1}",
                    Status = (i % 2 == 0) ? DesignStatus.Published : DesignStatus.Drafted,
                    CustomerId = dbCustomers[i % dbCustomers.Count].Id
                };
                design.DesignImages.Add(new DesignImage { ImageUrl = $"https://d2d-cdn.com/designs/img_{i + 1}.png" });
                context.CustomerDesigns.Add(design);
                customerDesigns.Add(design);
            }
            await context.SaveChangesAsync();

            // 6. Seed 50 CustomerPublishedOffers
            var publishedOffers = new List<CustomerPublishedOffer>();
            for (int i = 0; i < 50; i++)
            {
                var currentDesign = customerDesigns[i];
                var publishedOffer = new CustomerPublishedOffer
                {
                    CustomerID = currentDesign.CustomerId,
                    CustomerDesignID = currentDesign.ID,
                    // Populate required Offer properties
                    Name = currentDesign.Name + " - Offer",
                    Category = (i % 3 == 0) ? "Apparel" : (i % 3 == 1) ? "Accessories" : "Home",
                    Description = $"Published offer for {currentDesign.Name} - seed #{i + 1}",
                    TargetAudience = (i % 2 == 0) ? "Adults" : "Teens",
                    Gender = (i % 2 == 0),
                    Colors = new List<string> { "Red", "Blue", "White" },
                    Sizes = new List<string> { "S", "M", "L" },
                    Material = "Cotton",
                    PrintingType = "DTG",
                    SizesFile = null,
                    Duration = 7 + (i % 10),
                    Amount = 10 + (i % 5),
                    MaxPrice = 100.0m + (i * 2),
                    IsActive = true,
                    // Customer offer specific
                    CustomerOfferStatus = OfferStatus.OnHold
                };
                context.CustomerPublishedOffers.Add(publishedOffer);
                publishedOffers.Add(publishedOffer);
            }
            await context.SaveChangesAsync();

            // 6.b Ensure all customer designs are published and linked to a CustomerPublishedOffer
            for (int i = 0; i < customerDesigns.Count; i++)
            {
                var design = customerDesigns[i];

                // set status to Published if not already
                if (design.Status != DesignStatus.Published)
                {
                    design.Status = DesignStatus.Published;
                    context.CustomerDesigns.Update(design);
                }

                // ensure there's a published offer for this design
                var existing = publishedOffers.FirstOrDefault(p => p.CustomerDesignID == design.ID);
                if (existing == null)
                {
                    var newPub = new CustomerPublishedOffer
                    {
                        CustomerID = design.CustomerId,
                        CustomerDesignID = design.ID
                    };
                    context.CustomerPublishedOffers.Add(newPub);
                    publishedOffers.Add(newPub);
                }
                else
                {
                    // link design -> published offer id if navigation property used elsewhere
                    design.CustomerPublishedOfferID = existing.ID;
                }
            }

            // 9.b Add many detailed logs for every active published offer
            var activePublished = publishedOffers.Where(p => p.IsActive).ToList();
            var detailSteps = new[] { "Inquiry", "Negotiation", "Sample Request", "Payment Started", "Production", "Quality Check", "Packaging", "Shipped", "Delivered", "Feedback" };
            foreach (var pub in activePublished)
            {
                // Try to find an existing chat for the same customer; fall back to any chat ID or 0
                var chat = await context.Chats.FirstOrDefaultAsync(c => c.CustomerID == pub.CustomerID);
                var chatId = chat != null
                    ? chat.ID
                    : (await context.Chats.Select(c => c.ID).FirstOrDefaultAsync());

                // insert 15 logs to simulate detailed lifecycle
                var idx = publishedOffers.IndexOf(pub);
                for (int j = 0; j < 15; j++)
                {
                    context.ActiveOfferLogs.Add(new AL.ActiveOfferLogs
                    {
                        ChatID = chatId,
                        PublishedOfferID = pub.ID,
                        Step = detailSteps[j % detailSteps.Length],
                        Notes = $"Auto-generated detailed log #{j + 1} for published offer {pub.ID}",
                        IsPublishedOfferActive = true,
                        CreatedAt = DateTime.UtcNow.AddMinutes(-((idx >= 0 ? idx : 0) * 60) - j * 10)
                    });
                }
            }
            await context.SaveChangesAsync();
            await context.SaveChangesAsync();

            // 7. Seed 50 ProducerCustomerOffers
            var producerOffers = new List<ProducerCustomerOffer>();
            for (int i = 0; i < 50; i++)
            {
                var pubOffer = publishedOffers[i];
                var assignedProducer = dbProducers[i % dbProducers.Count];

                // create base producer offer
                var prodOffer = new ProducerCustomerOffer()
                {
                    ProducerID = assignedProducer.Id,
                    Producer = assignedProducer,
                    CustomerPublishedOfferID = pubOffer.ID,
                    CustomerPublishedOffer = pubOffer,
                    Price = 150.0m + (i * 5),
                    Duration = 5 + (i % 10),
                    Diposit = 30.0m + (i % 5),
                    OfferStatus = (i % 3 == 0) ? OfferStatus.Accepted : OfferStatus.OnHold,
                    Steps = new List<ProducerSteps>()
                };

                // add detailed steps and link them to the offer so FK is set
                prodOffer.Steps.Add(new ProducerSteps { StepName = "Material Selection & Gathering", MinDuration = 1, MaxDuration = 2, ProducerCustomerOffer = prodOffer });
                prodOffer.Steps.Add(new ProducerSteps { StepName = "Pattern Sizing & Initial Cutout", MinDuration = 2, MaxDuration = 4, ProducerCustomerOffer = prodOffer });
                prodOffer.Steps.Add(new ProducerSteps { StepName = "Finishing, Stitching & Brand Packing", MinDuration = 3, MaxDuration = 5, ProducerCustomerOffer = prodOffer });

                // optional: attach a simple timeline or notes via ActiveOfferLogs already added later
                context.ProducerCustomerOffers.Add(prodOffer);
                producerOffers.Add(prodOffer);
            }
            await context.SaveChangesAsync();

            // 8. Seed 50 Chats
            var chats = new List<Chat>();
            for (int i = 0; i < 50; i++)
            {
                var chat = new Chat
                {
                    Name = $"Order Coordination Channel {i + 1}",
                    CustomerID = dbCustomers[i % dbCustomers.Count].Id,
                    ProducerID = dbProducers[i % dbProducers.Count].Id,
                    ProducerCount = 1,
                    CustomerCount = 1
                };
                chat.Messages.Add(new Message
                {
                    Content = new List<string> { $"Hi, regarding the design specifications for offer reference #{i + 1}." },
                    Sender = MessageSender.Customer,
                    IsRead = true,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-30)
                });
                context.Chats.Add(chat);
                chats.Add(chat);
            }
            await context.SaveChangesAsync();

            // 9. Seed ActiveOfferLogs (many logs per offer, include inactive published offers and pending producer offers)
            for (int i = 0; i < 50; i++)
            {
                // base log for each published offer
                var baseLog = new AL.ActiveOfferLogs
                {
                    ChatID = chats[i].ID,
                    PublishedOfferID = publishedOffers[i].ID,
                    Step = "Material Selection & Gathering",
                    Notes = "Initial step created by seeder",
                    IsPublishedOfferActive = publishedOffers[i].IsActive,
                    CreatedAt = DateTime.UtcNow.AddDays(-(i % 7))
                };
                context.ActiveOfferLogs.Add(baseLog);

                // add additional progress logs for the same published offer
                var progressSteps = new[] { "Pattern Sizing", "Prototype Review", "Finishing" };
                for (int s = 0; s < progressSteps.Length; s++)
                {
                    context.ActiveOfferLogs.Add(new AL.ActiveOfferLogs
                    {
                        ChatID = chats[i].ID,
                        PublishedOfferID = publishedOffers[i].ID,
                        Step = progressSteps[s],
                        Notes = $"Auto progress step {s + 1}",
                        IsPublishedOfferActive = publishedOffers[i].IsActive,
                        CreatedAt = DateTime.UtcNow.AddDays(-(i % 7) + s)
                    });
                }

                // for some published offers, mark them inactive and add failure/archival logs
                if (i % 10 == 0)
                {
                    publishedOffers[i].IsActive = false;
                    context.CustomerPublishedOffers.Update(publishedOffers[i]);
                    context.ActiveOfferLogs.Add(new AL.ActiveOfferLogs
                    {
                        ChatID = chats[i].ID,
                        PublishedOfferID = publishedOffers[i].ID,
                        Step = "Archived",
                        Notes = "Published offer archived due to policy review",
                        IsPublishedOfferActive = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-(i % 7) - 1)
                    });
                }

                // attach logs referencing producer offers that are still pending (not accepted)
                // find a producer offer targeting this published offer (if any)
                var relatedProducer = producerOffers.FirstOrDefault(po => po.CustomerPublishedOfferID == publishedOffers[i].ID);
                if (relatedProducer != null && relatedProducer.OfferStatus != OfferStatus.Accepted)
                {
                    // add several negotiation logs
                    context.ActiveOfferLogs.Add(new AL.ActiveOfferLogs
                    {
                        ChatID = chats[i].ID,
                        PublishedOfferID = publishedOffers[i].ID,
                        Step = "Producer Negotiation",
                        Notes = $"Producer offer {relatedProducer.ID} proposed price {relatedProducer.Price}",
                        IsPublishedOfferActive = publishedOffers[i].IsActive,
                        CreatedAt = DateTime.UtcNow.AddDays(-(i % 7) + 2)
                    });

                    context.ActiveOfferLogs.Add(new AL.ActiveOfferLogs
                    {
                        ChatID = chats[i].ID,
                        PublishedOfferID = publishedOffers[i].ID,
                        Step = "Counter Offer",
                        Notes = "Customer requested lower price",
                        IsPublishedOfferActive = publishedOffers[i].IsActive,
                        CreatedAt = DateTime.UtcNow.AddDays(-(i % 7) + 3)
                    });
                }
            }

            // 10. Seed 50 Reviews
            for (int i = 0; i < 50; i++)
            {
                var targetProducer = dbProducers[i % dbProducers.Count];
                var reviewerCustomer = dbCustomers[(i + 1) % dbCustomers.Count];

                var review = new Review
                {
                    CustomerID = reviewerCustomer.Id,
                    ProducerID = targetProducer.Id,
                    Rate = (3 + (i % 3)),
                    Content = $"Excellent dedication to the design milestones. Clean execution on step number {i + 1}.",
                    CreatedAt = DateTime.UtcNow.AddDays(-i),
                    UpdatedAt = null
                };
                context.Reviews.Add(review);
            }

            // 11. Seed 50 ModelChats & ModelChatMessages
            for (int i = 0; i < 50; i++)
            {
                var modelChat = new ModelChat
                {
                    Title = $"AI Conceptualization Session {i + 1}",
                    CustomerID = dbCustomers[i % dbCustomers.Count].Id
                };
                context.ModelChats.Add(modelChat);

                context.ModelChatMessages.Add(new ModelChatMessage
                {
                    ModelChat = modelChat,
                    Text = $"Propose an oversized alternative concept containing a 3D icon version {i + 1}.",
                    Sender = MessageSender.Customer
                });
            }

            // 12. Miscellaneous Counters & Tickets
            context.SystemCounters.Add(new SystemCounter { CustomerCounter = 50, ProducerCounter = 50, DesignerCounter = 50 });

            for (int i = 1; i <= 50; i++)
            {
                context.Tickets.Add(new Tickets
                {
                    IssueType = i % 2 == 0 ? IssueType.WalletAndPaymentIssue : IssueType.OrderIssue,
                    Description = $"Automated system monitoring tracking index token #{i}.",
                    UserId = dbCustomers[i % dbCustomers.Count].Id,
                    Status = TicketStatus.Open
                });
            }

            // --- Add extra pending published offers and matching producer offers (none accepted) ---
            var extraCount = 10;
            for (int k = 0; k < extraCount; k++)
            {
                // reuse an existing design (rotate) to create an additional published offer
                var design = customerDesigns[(k + 3) % customerDesigns.Count];
                var custId = design.CustomerId;

                var pendingPub = new CustomerPublishedOffer
                {
                    CustomerID = custId,
                    CustomerDesignID = design.ID,
                    Name = design.Name + " - Extra Pending Offer",
                    Category = "Apparel",
                    Description = "Pending published offer created by seeder",
                    TargetAudience = "Adults",
                    Gender = true,
                    Colors = new List<string> { "Black", "Gray" },
                    Sizes = new List<string> { "M", "L" },
                    Material = "Polyester",
                    PrintingType = "Screen",
                    Duration = 10,
                    Amount = 5,
                    MaxPrice = 80.0m,
                    IsActive = true,
                    CustomerOfferStatus = OfferStatus.OnHold
                };
                context.CustomerPublishedOffers.Add(pendingPub);
                publishedOffers.Add(pendingPub);

                // create a producer offer that targets this published offer but is not accepted
                var assignedProducer = dbProducers[(k + 7) % dbProducers.Count];
                var pendingProdOffer = new ProducerCustomerOffer
                {
                    ProducerID = assignedProducer.Id,
                    Producer = assignedProducer,
                    CustomerPublishedOfferID = pendingPub.ID,
                    CustomerPublishedOffer = pendingPub,
                    Price = 90.0m + k * 2,
                    Duration = 7,
                    Diposit = 10.0m,
                    OfferStatus = OfferStatus.OnHold,
                    Steps = new List<ProducerSteps>
                    {
                        new ProducerSteps { StepName = "Initial Review", MinDuration = 1, MaxDuration = 2 },
                        new ProducerSteps { StepName = "Prototype", MinDuration = 2, MaxDuration = 3 }
                    }
                };
                context.ProducerCustomerOffers.Add(pendingProdOffer);
                producerOffers.Add(pendingProdOffer);

                // add negotiation logs for the pending pair
                var chatForCustomer = await context.Chats.FirstOrDefaultAsync(c => c.CustomerID == custId);
                var chatIdForPending = chatForCustomer != null ? chatForCustomer.ID : (await context.Chats.Select(c => c.ID).FirstOrDefaultAsync());
                context.ActiveOfferLogs.Add(new AL.ActiveOfferLogs
                {
                    ChatID = chatIdForPending,
                    PublishedOfferID = pendingPub.ID,
                    Step = "Pending Negotiation",
                    Notes = $"Producer {assignedProducer.Id} proposed {pendingProdOffer.Price}; awaiting customer response",
                    IsPublishedOfferActive = true,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-k * 5)
                });
            }

            await context.SaveChangesAsync();

            await context.SaveChangesAsync();
        }
    }
}