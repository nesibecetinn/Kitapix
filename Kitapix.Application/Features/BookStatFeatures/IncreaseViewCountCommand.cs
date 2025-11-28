using FluentValidation;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using MediatR;

namespace Kitapix.Application.Features.BookStatFeatures
{
	public class IncreaseViewCountCommand : IRequest<IncreaseViewCountCommandReponse>
	{
		public int BookId { get; set; }
	}
	public class IncreaseViewCountValidator : AbstractValidator<IncreaseViewCountCommand>
	{
		public IncreaseViewCountValidator()
		{
			RuleFor(x => x.BookId).NotEmpty().WithMessage("Kitap Id boş olamaz");
		}
	}
	public class IncreaseViewCountHandler : IRequestHandler<IncreaseViewCountCommand, IncreaseViewCountCommandReponse>
	{
		private readonly IBookStatRepository _bookStatRepository;
		private readonly IUnitOfWork _unitOfWork;

		public IncreaseViewCountHandler(IBookStatRepository bookStatRepository, IUnitOfWork unitOfWork)
		{
			_bookStatRepository = bookStatRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<IncreaseViewCountCommandReponse> Handle(IncreaseViewCountCommand request, CancellationToken cancellationToken)
		{
			var existingbook = await _bookStatRepository.GetBookStatByBookIdAsync(request.BookId);
			if (existingbook == null)
			{
				throw new Exception("kitap bulunamadı");
			}

			existingbook.ViewCount = +1;
			_bookStatRepository.Update(existingbook);
			await _unitOfWork.SaveChangesAsync();
			return new IncreaseViewCountCommandReponse();
		}
	}

	public class IncreaseViewCountCommandReponse
	{
		public string Message { get; set; } = "Görüntüleme başarılı";
	}
	
}
