using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace SistemaRamais.Ramais
{
    public abstract class RamaisAppServiceTests<TStartupModule> : SistemaRamaisApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IRamaisAppService _ramaisAppService;
        private readonly IRepository<Ramal, Guid> _ramalRepository;

        public RamaisAppServiceTests()
        {
            _ramaisAppService = GetRequiredService<IRamaisAppService>();
            _ramalRepository = GetRequiredService<IRepository<Ramal, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _ramaisAppService.GetListAsync(new GetRamaisInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("237af739-6a9a-49cd-a2e1-fbb1e8fac944")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("05c5044b-50ba-4588-8afe-f4ea7186c04b")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _ramaisAppService.GetAsync(Guid.Parse("237af739-6a9a-49cd-a2e1-fbb1e8fac944"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("237af739-6a9a-49cd-a2e1-fbb1e8fac944"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new RamalCreateDto
            {
                Nome = "f01322f358334c2abaa898dd45933c9082e213ddde0c4d358a5ce6c6184e53c27f53248a46c84c9c8d4dc3bac31ed96df537",
                Numero = "fdd821d762ab490b81043cb4835c6ee4bdb7f598240747c7a22d442db710aee5d6d9f345307c435fa50b02dbfbe339514e3e",
                Departamento = "4cde1b23b1ca41e6b592c0a304a6989c8fd633b9300b4b4badef17169f307c7569128e31382e4243a22506b2d1de26236bb6",
                Email = "15060a5b221b425ca818e84b31d8e7411c0d8f877d8b44c39620aefb2387f33724f0b3d974294f349f083553f81865575d21"
            };

            // Act
            var serviceResult = await _ramaisAppService.CreateAsync(input);

            // Assert
            var result = await _ramalRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Nome.ShouldBe("f01322f358334c2abaa898dd45933c9082e213ddde0c4d358a5ce6c6184e53c27f53248a46c84c9c8d4dc3bac31ed96df537");
            result.Numero.ShouldBe("fdd821d762ab490b81043cb4835c6ee4bdb7f598240747c7a22d442db710aee5d6d9f345307c435fa50b02dbfbe339514e3e");
            result.Departamento.ShouldBe("4cde1b23b1ca41e6b592c0a304a6989c8fd633b9300b4b4badef17169f307c7569128e31382e4243a22506b2d1de26236bb6");
            result.Email.ShouldBe("15060a5b221b425ca818e84b31d8e7411c0d8f877d8b44c39620aefb2387f33724f0b3d974294f349f083553f81865575d21");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new RamalUpdateDto()
            {
                Nome = "bda5a68b0aef449aafd96516480330962d20f2323eca4113bed8f0136449cb882adee342b76a4c07bd299dbb093d52626889",
                Numero = "3f92526889f549c2b900305b64414dd9d0867c77c9674c578d2b6bccd755f7f3c459e7b11cc4406bb7b3d8e2daac1128d70a",
                Departamento = "a556aae43062448e8b94f64f226a1c872323c98dfd3b4f629aa2b8809b7aeccc98c4e42d1b224a0ebf651a13ce2d9663b7f7",
                Email = "5e4a8fc1f94b4feaa9c9f97f3043da5f21372d48e5f141a597948e7ccf4efa1b82f158a6586a47778726297dab63b39614ec"
            };

            // Act
            var serviceResult = await _ramaisAppService.UpdateAsync(Guid.Parse("237af739-6a9a-49cd-a2e1-fbb1e8fac944"), input);

            // Assert
            var result = await _ramalRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Nome.ShouldBe("bda5a68b0aef449aafd96516480330962d20f2323eca4113bed8f0136449cb882adee342b76a4c07bd299dbb093d52626889");
            result.Numero.ShouldBe("3f92526889f549c2b900305b64414dd9d0867c77c9674c578d2b6bccd755f7f3c459e7b11cc4406bb7b3d8e2daac1128d70a");
            result.Departamento.ShouldBe("a556aae43062448e8b94f64f226a1c872323c98dfd3b4f629aa2b8809b7aeccc98c4e42d1b224a0ebf651a13ce2d9663b7f7");
            result.Email.ShouldBe("5e4a8fc1f94b4feaa9c9f97f3043da5f21372d48e5f141a597948e7ccf4efa1b82f158a6586a47778726297dab63b39614ec");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _ramaisAppService.DeleteAsync(Guid.Parse("237af739-6a9a-49cd-a2e1-fbb1e8fac944"));

            // Assert
            var result = await _ramalRepository.FindAsync(c => c.Id == Guid.Parse("237af739-6a9a-49cd-a2e1-fbb1e8fac944"));

            result.ShouldBeNull();
        }
    }
}