using FluentValidation;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using MediatR;

namespace Kitapix.Application.Features.BookStatFeatures
{
	public class DecreaseLikeCountCommand : IRequest<DecreaseLikeCountCommandReponse>
	{
		public int BookId { get; set; }
	}
	public class DecreaseLikeCountValidator : AbstractValidator<DecreaseLikeCountCommand>
	{
		public DecreaseLikeCountValidator()
		{
			RuleFor(x => x.BookId).NotEmpty().WithMessage("Kitap Id boş olamaz");
		}
	}
	public class DecreaseLikeCountHandler : IRequestHandler<DecreaseLikeCountCommand, DecreaseLikeCountCommandReponse>
	{
		private readonly IBookStatRepository _bookStatRepository;
		private readonly IUnitOfWork _unitOfWork;

		public DecreaseLikeCountHandler(IBookStatRepository bookStatRepository, IUnitOfWork unitOfWork)
		{
			_bookStatRepository = bookStatRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<DecreaseLikeCountCommandReponse> Handle(DecreaseLikeCountCommand request, CancellationToken cancellationToken)
		{
			var existingbook = await _bookStatRepository.GetBookStatByBookIdAsync(request.BookId);
			if (existingbook == null)
			{
				throw new Exception("kitap bulunamadı");
			}

			existingbook.LikeCount = -1;
			_bookStatRepository.Update(existingbook);
			await _unitOfWork.SaveChangesAsync();
			return new DecreaseLikeCountCommandReponse();
		}
	}

	public class DecreaseLikeCountCommandReponse
	{
		public string Message { get; set; } = "Beğeni geri alındı";
	}
	
}
