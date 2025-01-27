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
            result.Items.Any(x => x.Id == Guid.Parse("d8546118-2cbe-4d8d-9c74-9e3cf1a21016")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("0d4dc5f6-8ab4-49a4-ae49-a01e41da0344")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _ramaisAppService.GetAsync(Guid.Parse("d8546118-2cbe-4d8d-9c74-9e3cf1a21016"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("d8546118-2cbe-4d8d-9c74-9e3cf1a21016"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new RamalCreateDto
            {
                Numero = "3dc769e3c3",
                Departamento = "91cc82efd67e4d90b4229363014b97552bf8d42ece93492eaa46da2facd905f7e31d5c4b2ebb4d12bed5b5555d7cc4b05dad",
                Responsavel = "10c2de47a6504cc9bef1592b05385433895a7afdbc194afda21f7f42dfae45f036ac5b1ba3b34f3e9dfd6b9eb8f8b4289595",
                Email = "f324317ef53a423497bd804fb9c59490fb64e033b9e642adb34bb8b7357cc3227753e478fbf04781aa0392f86dbd9119ffe7",
                Telefone = "36fda6c6b2c74e33bb43"
            };

            // Act
            var serviceResult = await _ramaisAppService.CreateAsync(input);

            // Assert
            var result = await _ramalRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Numero.ShouldBe("3dc769e3c3");
            result.Departamento.ShouldBe("91cc82efd67e4d90b4229363014b97552bf8d42ece93492eaa46da2facd905f7e31d5c4b2ebb4d12bed5b5555d7cc4b05dad");
            result.Responsavel.ShouldBe("10c2de47a6504cc9bef1592b05385433895a7afdbc194afda21f7f42dfae45f036ac5b1ba3b34f3e9dfd6b9eb8f8b4289595");
            result.Email.ShouldBe("f324317ef53a423497bd804fb9c59490fb64e033b9e642adb34bb8b7357cc3227753e478fbf04781aa0392f86dbd9119ffe7");
            result.Telefone.ShouldBe("36fda6c6b2c74e33bb43");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new RamalUpdateDto()
            {
                Numero = "e907f43d5c",
                Departamento = "a93e655363784b95946b476b1d3849dbe3a88305d10a4bfb9ad1f702765f6ca7c7f1aa587017451fb86a6303c36bc73f3f7c",
                Responsavel = "70823e2829d64b3db99eebbb22b6595f41b998cf2ea74489a9f1c3d62b4a27a5aba386b4f4564859a0db36b5a5f51b680571",
                Email = "3d7c0811e5b34d9ab967e21ab5454d47ce7266296d3843a8af1ed97e103bda17efcd15f9f8c2461bb29e4f731cf6d287112b",
                Telefone = "fde190fe90884af78f22"
            };

            // Act
            var serviceResult = await _ramaisAppService.UpdateAsync(Guid.Parse("d8546118-2cbe-4d8d-9c74-9e3cf1a21016"), input);

            // Assert
            var result = await _ramalRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Numero.ShouldBe("e907f43d5c");
            result.Departamento.ShouldBe("a93e655363784b95946b476b1d3849dbe3a88305d10a4bfb9ad1f702765f6ca7c7f1aa587017451fb86a6303c36bc73f3f7c");
            result.Responsavel.ShouldBe("70823e2829d64b3db99eebbb22b6595f41b998cf2ea74489a9f1c3d62b4a27a5aba386b4f4564859a0db36b5a5f51b680571");
            result.Email.ShouldBe("3d7c0811e5b34d9ab967e21ab5454d47ce7266296d3843a8af1ed97e103bda17efcd15f9f8c2461bb29e4f731cf6d287112b");
            result.Telefone.ShouldBe("fde190fe90884af78f22");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _ramaisAppService.DeleteAsync(Guid.Parse("d8546118-2cbe-4d8d-9c74-9e3cf1a21016"));

            // Assert
            var result = await _ramalRepository.FindAsync(c => c.Id == Guid.Parse("d8546118-2cbe-4d8d-9c74-9e3cf1a21016"));

            result.ShouldBeNull();
        }
    }
}