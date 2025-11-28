using AutoMapper;
using Kitapix.Application.Features.AuthFeatures;
using Kitapix.Application.Features.BookChapterFeatures;
using Kitapix.Application.Features.BookFeatures;
using Kitapix.Application.Features.CategoryFeatures;
using Kitapix.Application.Features.UserFeatures;
using Kitapix.Domain.Dtos;
using Kitapix.Domain.Entities;
using BookChapter = Kitapix.Domain.Entities.MongoEntities.BookChapter;

namespace Kitapix.Infrastructure.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{

			CreateMap<RegisterCommand, AppUser>().ReverseMap();

			CreateMap<CreateBookCommand, Book>().ReverseMap();
			CreateMap<UpdateBookCommand, Book>().ReverseMap();

			CreateMap<Book, BookDto>()
				.ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
				.ForMember(dest => dest.AuthorSurName, opt => opt.MapFrom(src => src.Author.SurName))
				.ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.Stats.ViewCount))
				.ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Stats.LikeCount))
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src =>
					src.BookCategories.Select(bc => bc.Category.Name).ToList()))
				.ReverseMap();

			CreateMap<Book, GetAllBookWithAllInfoQueryResponse>()
				.ForMember(dest => dest.BookDto, opt => opt.MapFrom(src => src))
				.ReverseMap();

			CreateMap<Book, GetBookByViewCountQueryResponse>()
				.ForMember(dest => dest.BookDto, opt => opt.MapFrom(src => src))
				.ReverseMap();

			CreateMap<GetBookByIdResponse, Book>().ReverseMap();
			CreateMap<GetAllBookResponse, Book>().ReverseMap();
			CreateMap<GetAllBookWithChaptersResponse, Book>().ReverseMap();
			CreateMap<GetAllBookByCreateDateQuery, Book>().ReverseMap();
			CreateMap<GetBookListByUserIdQueryResponse, Book>().ReverseMap();

			CreateMap<CreateBookChapterCommand, BookChapter>().ReverseMap();
			CreateMap<UpdateBookChapterCommand, BookChapter>().ReverseMap();
			CreateMap<GetByIdBookChapterQueryResponse, BookChapter>().ReverseMap();
			CreateMap<GetAllChaptersQueryResponse, BookChapter>().ReverseMap();

			CreateMap<CreateCategoryCommand, Category>().ReverseMap();
			CreateMap<UpdateCategoryCommand, Category>().ReverseMap();
			CreateMap<GetByIdCategoryQuery, Category>().ReverseMap();
			CreateMap<GetAllCategoryQuery, Category>().ReverseMap();

			CreateMap<CreateUserBookCategoriesCommand, List<UserBookCategory>>()
				.ConvertUsing(src => src.CategoryIds
					.Select(categoryId => new UserBookCategory
					{
						CategoryId = categoryId
					}).ToList());
		}
	}
}