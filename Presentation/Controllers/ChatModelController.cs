using Domain.Entities.Chats.AiModel;
using Domain.Entities.Designs;
using Hangfire.MemoryStorage.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using V02.DTOs;
using V02.Services;
using V02;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Domain.Enums.Types;
using Application.Services.zena;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatModelController : ControllerBase
    {
        private readonly IModelChatService _chatService;
        private readonly IImageGenerationService _imageGenerationService;
        //private readonly IItiImageService _itiImageService;
        private readonly IPromptBuilder _promptBuilder;
        private readonly D2DContext _context;

        public ChatModelController(IModelChatService chatService, IImageGenerationService imageGenerationService, /*IItiImageService itiImageService*/ IPromptBuilder promptBuilder, D2DContext context)
        {
            //_itiImageService = itiImageService;
            _imageGenerationService = imageGenerationService;
            _context = context;
            _chatService = chatService;
            _promptBuilder = promptBuilder;
        }


        // Enhance user prompt with fashion vesion
        

        // Generate Design with Flux API with state management
        [HttpPost("Create-design")]
        public async Task<IActionResult> CreateDesignwithState([FromBody] GenerateDesignRequestdto request)
        {
            try
            {
                DesignState state;
                ModelGeneratedDesign design;
                ModelChat chat;
                GeneratedDesignResponse response=new GeneratedDesignResponse();
                bool flag = false;
               
                if (request.DesignId == null)//check if the designId is null, if yes then create a new design, else update the existing design
                {
                    // First time creation
                    state = await _chatService.EnhancePromptAsync(request.UserMessage);

                    design = new ModelGeneratedDesign
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = request.UserId,//56f7be28-39ca-4c6a-990c-9afabdd6352d
                        DesignState = state,  // EF will serialize this to JSON automatically
                    };
                     chat = new ModelChat
                    {
                        CustomerID = request.UserId,
                        Title = "Chat Model",
                        Message = new List<ModelChatMessage>
                        {
                            new ModelChatMessage
                            {
                                Sender= MessageSender.Customer,
                                Text=request.UserMessage,
                            }
                        }
                    };
                    _context.Add(chat);
                    response.ModelChat = chat;
                    _context.Add(design);
                    flag = true;
                }
                else
                {
                    response = await _context.ModelGeneratedDesigns.Select(m => new GeneratedDesignResponse
                    {
                        GeneratedDesignId=m.Id,CustomerId=m.CustomerId,ModelChat=m.Customer.ModelChat,DesignState=m.DesignState,
                    }).FirstOrDefaultAsync(d =>
                    d.GeneratedDesignId == request.DesignId && d.CustomerId == request.UserId);

                    if (response == null)
                        return NotFound();

                    state = await _chatService.UpdateDesignStateAsync(response.DesignState!, request.UserMessage);
                    design = new ModelGeneratedDesign
                    {
                        CustomerId = request.UserId,
                        DesignState = state,
                        Id = response.GeneratedDesignId,
                        
                    };
                }

               
               

                var prompt = _promptBuilder.Build(state);//convert the design state to prompt for image generation

                var images = await _imageGenerationService.GenerateImageAsync(prompt);//generate the image from the prompt
                response.ModelChat.Message.Add(new ModelChatMessage
                {
                    ImgUrl=images,
                    Sender= MessageSender.Assistant,
                    
                });
                if(!flag)
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
                return StatusCode(500, $"Internal error has occurred: {ex.Message}");
            }
        }

        // Generate Design with ITI API with State Management
        //[HttpPost("ITI-generate-image")]
        //public async Task<IActionResult> ITIFashionDesign2([FromBody] GenerateDesignRequestdto request)
        //{
        //    try
        //    {
        //        DesignState state;
        //        Design design = null;

        //        if (request.DesignId == null)//check if the designId is null, if yes then create a new design, else update the existing design
        //        {
        //            // First time creation
        //            state = await _chatService.EnhancePromptAsync(request.UserMessage);//return the design state as json object

        //            ///create a new design and save it to the database **TODO**
        //            //design = new Design
        //            //{
        //            //    StateJson = JsonSerializer.Serialize(state)
        //            //};

        //            //_context.Designs.Add(design);
        //        }
        //        else
        //        {
        //            //// Update existing design  **TODO**
        //            design = await _context.CustomerDesigns
        //                .FirstOrDefaultAsync(d => d.ID == request.DesignId && d.CustomerId == request.UserId);  //get the design from the database by designId and userId

        //            if (design == null)
        //                return NotFound();

        //            var currentState = design.DesignState!;

        //            state = await _chatService.UpdateDesignStateAsync(currentState, request.UserMessage);//update the design state with the new user message

        //            design.DesignState = state;
        //        }

        //        await _context.SaveChangesAsync();

        //        var prompt = _promptBuilder.Build(state);//convert the design state to prompt for image generation

        //        var images = await _itiImageService.GenerateImageAsync(prompt); //generate the image from the prompt

        //        return Ok(new
        //        {
        //            designId = design.Id,
        //            images
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal error has occurred: {ex.Message}");
        //    }
        //}
    }
}
