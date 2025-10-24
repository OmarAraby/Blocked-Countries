using BlockedCountries.Application.DTOs.Requests;
using BlockedCountries.Application.Helpers.GeneralResult;
using BlockedCountries.Application.Interfaces;
using BlockedCountries.Domain.Entities;
using BlockedCountries.Domain.Interfaces.Repositories;

namespace BlockedCountries.Application.Services
{
    public class TemporalBlockService : ITemporalBlockService
    {
        private readonly ITemporalBlocksRepository _tempRepo;
        private readonly IBlockedCountriesRepository _blockedCountriesRepo;

        public TemporalBlockService(ITemporalBlocksRepository tempRepo,IBlockedCountriesRepository blockedCountriesRepo)
        {
            _tempRepo = tempRepo;
            _blockedCountriesRepo = blockedCountriesRepo;
        }
        public ApiResponse BlockTemporarily(TemporalBlockRequestDto requestDto)
        {
            var code= requestDto.CountryCode.ToUpperInvariant();
            // check if it already blocked 
            if (_blockedCountriesRepo.Exists(code))
                return ApiResponse.ErrorResponse("counntry already blockedf");

            if (_tempRepo.Exists(code))
                return ApiResponse.ErrorResponse("country already temp blocked");
            //// validate time --> duration must be 1:1440
            //if (requestDto.DurationMinutes is < 1 or > 1440)
            //    return ApiResponse.ErrorResponse("duration must be betweeen 1:1440 minutes");

            var block = new TemporalBlock
            {
                CountryCode = code,
                DurationMinutes = requestDto.DurationMinutes
            };

            _tempRepo.Add(block);
            return ApiResponse.SuccessResponse($"Country {code} blocked for {requestDto.DurationMinutes} minutes.");


        }

        public bool IsBlocked(string countryCode)
        {
            //throw new NotImplementedException();
            return _tempRepo.Exists(countryCode.ToUpperInvariant());
        }

        public ApiResponse Unblock(string countryCode)
        {
            var code = countryCode.ToUpperInvariant();
            if (!_tempRepo.Exists(code))
                return ApiResponse.ErrorResponse("country is not temporaly blocked");
            _tempRepo.Remove(code);
            return ApiResponse.SuccessResponse("Temp blocked removed");
        }
    }
}
