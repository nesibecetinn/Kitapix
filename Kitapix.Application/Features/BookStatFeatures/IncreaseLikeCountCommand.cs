using FluentValidation;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using MediatR;

namespace Kitapix.Application.Features.BookStatFeatures
{
	public class IncreaseLikeCountCommand : IRequest<IncreaseLikeCountCommandReponse>
	{
		public int BookId { get; set; }
	}
	public class IncreaseLikeCountValidator : AbstractValidator<IncreaseLikeCountCommand>
	{
		public IncreaseLikeCountValidator()
		{
			RuleFor(x => x.BookId).NotEmpty().WithMessage("Kitap Id boş olamaz");
		}
	}
	public class IncreaseLikeCountHandler : IRequestHandler<IncreaseLikeCountCommand, IncreaseLikeCountCommandReponse>
	{
		private readonly IBookStatRepository _bookStatRepository;
		private readonly IUnitOfWork _unitOfWork;
		public IncreaseLikeCountHandler(IBookStatRepository bookStatRepository, IUnitOfWork unitOfWork)
		{
			_bookStatRepository = bookStatRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<IncreaseLikeCountCommandReponse> Handle(IncreaseLikeCountCommand request, CancellationToken cancellationToken)
		{
			var existingbook =await _bookStatRepository.GetBookStatByBookIdAsync(request.BookId);
			if(existingbook == null)
			{
				throw new Exception("kitap bulunamadı");
			}

			existingbook.LikeCount = +1;
			 _bookStatRepository.Update(existingbook);
			await _unitOfWork.SaveChangesAsync();
			return new IncreaseLikeCountCommandReponse();
		}
	}

	public class IncreaseLikeCountCommandReponse
	{
		public string Message { get; set; } = "Beğenildi";
	}
}
