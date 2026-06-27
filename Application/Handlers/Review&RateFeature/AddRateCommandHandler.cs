//using Application.Commands.Review_RateFeature;
//using Application.Response;
//using Domain.DTOs.Review_RateDtos;
//using Infrastructure.Data.Context;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Handlers.Review_RateFeature
//{
//    public class AddRateCommandHandler : IRequestHandler<AddRateCommand, Result<RateResponse>>
//    {
//        private readonly D2DContext _context;

//        public AddRateCommandHandler(D2DContext context)
//        {
//            _context= context;
//        }
//        public async Task<Result<RateResponse>> Handle(AddRateCommand request, CancellationToken cancellationToken)
//        {
//            var producer =await _context.Producers.FirstOrDefaultAsync(p => p.Id == request.ProducerId);
//            if (producer == null)
//                return Result<RateResponse>.Failure(Messages.NotFound.WithTarget("User"));
//            if(producer.Rate == 0)

//            producer.Rate = request.Rate;
//        }
//    }
//}
