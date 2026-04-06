using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Token;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using CommonTestUtilities.Token;
using FluentAssertions;
using FluentValidation.Internal;
using Wallet.Application.UseCases.Auth.RefreshToken;
using Wallet.Domain.Entities;
using Wallet.Exceptions;
using Wallet.Exceptions.ExceptionsBase;
using Wallet.Exceptions.Token;

namespace UseCases.Test.RefreshTokens
{
    public class UseRefreshTokenUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();
            var refreshToken = RefreshTokenBuilder.Build(user);

            var useCase = CreateUseCase(refreshToken);
            var request = RequestNewTokenJsonBuilder.Build();

            var response = await useCase.Execute(request);

            response.RefreshToken.Should().NotBeNull();
            response.RefreshToken.Should().NotBe(refreshToken.Value);
            response.AccessToken.Should().NotBeNull();
        }

        [Fact]
        public async Task Error_RefreshTokenNotFoundException()
        {
            (var user, var _) = UserBuilder.Build();

            var useCase = CreateUseCase();
            var request = RequestNewTokenJsonBuilder.Build();

            Func<Task> act = async () => await useCase.Execute(request);
            (await act.Should().ThrowAsync<RefreshTokenNotFoundException>())
                .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessageException.REFRESH_TOKEN_NOT_FOUND));
        }

        [Fact]
        public async Task Error_RefreshTokenExpiredException()
        {
            (var user, var _) = UserBuilder.Build();
            var refreshToken = RefreshTokenBuilder.Build(user);
            refreshToken.Created_On = DateTime.MinValue;

            var useCase = CreateUseCase(refreshToken);
            var request = RequestNewTokenJsonBuilder.Build();

            Func<Task> act = async () => await useCase.Execute(request);
            (await act.Should().ThrowAsync<RefreshTokenExpiredException>())
                .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessageException.REFRESH_TOKEN_EXPIRED));
        }


        private static UseRefreshTokenUseCase CreateUseCase(RefreshToken? refreshEntity = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
            var tokenRepository = new TokenRepositoryBuilder();

            if (refreshEntity is not null)
                tokenRepository.Get(refreshEntity);

            var refreshTokenGen = RefreshTokenGeneratorBuilder.Build();

            return new UseRefreshTokenUseCase(unitOfWork, accessTokenGenerator, tokenRepository.Build(), refreshTokenGen);
        }
    }
}
