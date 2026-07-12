using Application.Services.zena;
using Domain.Entities.Chats.AiModel;
using Domain.Entities.Designs;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V02;
using V02.DTOs;
using V02.Services;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatModelController : ControllerBase
    {
        private readonly IModelChatService _chatService;
        private readonly IImageGenerationService _imageGenerationService;
        private readonly IPromptBuilder _promptBuilder;
        private readonly D2DContext _context;

        public ChatModelController(IModelChatService chatService, IImageGenerationService imageGenerationService, IPromptBuilder promptBuilder, D2DContext context)
        {
            _imageGenerationService = imageGenerationService;
            _context = context;
            _chatService = chatService;
            _promptBuilder = promptBuilder;
        }

        [HttpPost("Create-design")]
        public async Task<IActionResult> CreateDesignwithState([FromBody] GenerateDesignRequestdto request)
        {
            try
            {
                DesignState state;
                ModelGeneratedDesign design;
                ModelChat chat;
                GeneratedDesignResponse response = new GeneratedDesignResponse();
                bool isNewDesign = false;

                if (request.DesignId == null)
                {
                    // 1. الدورة الأولى: إنشاء تصميم جديد وحساب حالة الـ State لأول مرة عبر Claude
                    state = await _chatService.EnhancePromptAsync(request.UserMessage);

                    design = new ModelGeneratedDesign
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = request.UserId,
                        DesignState = state,
                    };
                    chat = new ModelChat
                    {
                        CustomerID = request.UserId,
                        Title = "D2D AI Designer Hub",
                        Message = new List<ModelChatMessage>
                        {
                            new ModelChatMessage
                            {
                                Sender = MessageSender.Customer,
                                Text = request.UserMessage,
                            }
                        }
                    };
                    _context.Add(chat);
                    response.ModelChat = chat;
                    _context.Add(design);
                    isNewDesign = true;
                }
                else
                {
                    // 2. دورة التعديل: جلب السياق الحالي وحقن آخر صورة تم إنتاجها كمستند مرجعي للـ Inpaint
                    var existingDesign = await _context.ModelGeneratedDesigns
                        .Include(m => m.Customer)
                        .ThenInclude(c => c.ModelChat)
                        .ThenInclude(ch => ch.Message)
                        .FirstOrDefaultAsync(d => d.Id == request.DesignId && d.CustomerId == request.UserId);

                    if (existingDesign == null)
                        return NotFound("Requested design setup context does not exist.");

                    var lastAssistantMessage = existingDesign.Customer.ModelChat.Message
                        .LastOrDefault(m => m.Sender == MessageSender.Assistant && !string.IsNullOrEmpty(m.ImgUrl));

                    var currentState = existingDesign.DesignState!;
                    if (lastAssistantMessage != null)
                    {
                        // تمرير رابط الصورة الحالية ليتم التعديل عليها بداخل الموديل
                        currentState.OriginalImageReference = lastAssistantMessage.ImgUrl;
                    }

                    // تحديث الحالة وتعيين ميزات المنطقة والـ Mask المراد تغييرها عبر Claude
                    state = await _chatService.UpdateDesignStateAsync(currentState, request.UserMessage);
                    existingDesign.DesignState = state;

                    design = existingDesign;
                    response.ModelChat = existingDesign.Customer.ModelChat;

                    response.ModelChat.Message.Add(new ModelChatMessage
                    {
                        Sender = MessageSender.Customer,
                        Text = request.UserMessage
                    });
                }

                // بناء الـ Prompt النصي المحسن من كائن الـ JSON المتكامل
                var prompt = _promptBuilder.Build(state);

                // استدعاء محرك رسم الصور المدمج (توجيه تلقائي لنوفا كانفاس أو ستابل إنبينت)
                var serviceExtended = (ImageGenerationService)_imageGenerationService;
                var images = await serviceExtended.GenerateOrEditImageAsync(prompt, state);

                response.ModelChat.Message.Add(new ModelChatMessage
                {
                    ImgUrl = images,
                    Sender = MessageSender.Assistant,
                });

                if (!isNewDesign)
                    _context.Update(response.ModelChat);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    designId = design.Id,
                    images
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal execution error occurred: {ex.Message}");
            }
        }
    }
}