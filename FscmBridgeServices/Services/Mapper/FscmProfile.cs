using AutoMapper;
using FscmBridgeServices.Common.DTOS;
using FscmBridgeServices.Repository.Entity;

namespace FscmBridgeServices.Services.Mapper
{
    public class FscmProfile : Profile
    {
        public FscmProfile() 
        {
            CreateMap<OrganizationBuyer, OrganizationBuyerDto>()
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.reference))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.customerType))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.fullName))
                .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.registrationNumber))
                .ForMember(dest => dest.TimeZoneId, opt => opt.MapFrom(src => src.timeZoneId))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.city))
                .ForMember(dest => dest.BranchInformation, opt => opt.MapFrom(src => src.branchInformation))
                .ForMember(dest => dest.CifNumber, opt => opt.MapFrom(src => src.cifNumber))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.postalCode))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.contactPerson))
                .ForMember(dest => dest.ContactPersonDepartment, opt => opt.MapFrom(src => src.contactPersonDepartment))
                .ForMember(dest => dest.ContactPersonPhone, opt => opt.MapFrom(src => src.contactPersonPhone))
                .ForMember(dest => dest.ContactPersonEmail, opt => opt.MapFrom(src => src.contactPersonEmail))
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.address1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.address2))
                .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.industry))
                .ForMember(dest => dest.SubIndustry, opt => opt.MapFrom(src => src.subIndustry))
                .ReverseMap();

            CreateMap<FinanceOrganizationDto, FinanceOrganization>()
                .ForMember(dest => dest.accountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.organizationUuid, opt => opt.MapFrom(src => src.OrganizationUuid))
                .ForMember(dest => dest.accountName, opt => opt.MapFrom(src => src.AccountName))
                .ForMember(dest => dest.currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.bankName, opt => opt.MapFrom(src => src.BankName))
                .ForMember(dest => dest.city, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.swiftCode, opt => opt.MapFrom(src => src.SwiftCode))
                .ReverseMap();

            CreateMap<FscmLog, FscmLogDto>()
               .ForMember(dest => dest.ApRegno, opt => opt.MapFrom(src => src.ap_regno)).ReverseMap()
               .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<FscmContract, FscmContractDto>()
               .ForMember(dest => dest.ProgramUuid, opt => opt.MapFrom(src => src.programUuid))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
               .ForMember(dest => dest.IsDeactivated, opt => opt.MapFrom(src => src.isDeactivated))
               .ForMember(dest => dest.FinancingAfterCutOffTime, opt => opt.MapFrom(src => src.financingAfterCutOffTime))
               .ForMember(dest => dest.Funder, opt => opt.MapFrom(src => src.funder))
               .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.buyer))
               .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.seller))
               .ForMember(dest => dest.OptionRates, opt => opt.MapFrom(src => src.optionRates))
               .ForMember(dest => dest.Suspensions, opt => opt.MapFrom(src => src.suspensions))
               .ForMember(dest => dest.IsAutoRetrySettlement, opt => opt.MapFrom(src => src.isAutoRetrySettlement))
               .ReverseMap();

            CreateMap<Funder, FunderDto>()
                .ForMember(dest => dest.OrganizationUuid, opt => opt.MapFrom(src => src.organizationUuid))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.organizationName))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.code))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.type))
                .ReverseMap();

            CreateMap<Buyer, BuyerDto>()
                .ForMember(dest => dest.OrganizationUuid, opt => opt.MapFrom(src => src.organizationUuid))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.organizationName))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.code))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.type))
                .ForMember(dest => dest.isPrincipal, opt => opt.MapFrom(src => src.isPrincipal))
                .ReverseMap();

            CreateMap<Seller, SellerDto>()
                .ForMember(dest => dest.OrganizationUuid, opt => opt.MapFrom(src => src.organizationUuid))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.organizationName))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.code))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.type))
                .ForMember(dest => dest.isPrincipal, opt => opt.MapFrom(src => src.isPrincipal))
                .ReverseMap();

            CreateMap<OptionRate, OptionRateDto>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.currency))
                .ForMember(dest => dest.divisor, opt => opt.MapFrom(src => src.divisor))
                .ReverseMap();

            CreateMap<Suspension, SuspensionDto>()
                .ForMember(dest => dest.IsForever, opt => opt.MapFrom(src => src.isForever))
                .ForMember(dest => dest.IsSpesificDate, opt => opt.MapFrom(src => src.isSpesificDate))
                .ForMember(dest => dest.OverdueDays, opt => opt.MapFrom(src => src.overdueDays))
                .ForMember(dest => dest.OverdueAmount, opt => opt.MapFrom(src => src.overdueAmount))
                .ForMember(dest => dest.AllowFinance, opt => opt.MapFrom(src => src.allowFinance))
                .ForMember(dest => dest.FromInvoice, opt => opt.MapFrom(src => src.fromInvoice))
                .ForMember(dest => dest.SpesificDateTime, opt => opt.MapFrom(src => src.spesificDateTime))
                .ForMember(dest => dest.EmailNotificationBefore, opt => opt.MapFrom(src => src.emailNotificationBefore))
                .ForMember(dest => dest.EmailNotificationAfter, opt => opt.MapFrom(src => src.emailNotificationAfter))
                .ForMember(dest => dest.ParticipantsOverdueAmount, opt => opt.MapFrom(src => src.participantsOverdueAmount))
                .ForMember(dest => dest.CreditRating, opt => opt.MapFrom(src => src.creditRating))
                .ForMember(dest => dest.QuoteRestrictionDays, opt => opt.MapFrom(src => src.quoteRestrictionDays))
                .ReverseMap();

        }
    }
}
