using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using SistemaRamais.Ramais;
using SistemaRamais.EntityFrameworkCore;
using Xunit;

namespace SistemaRamais.EntityFrameworkCore.Domains.Ramais
{
    public class RamalRepositoryTests : SistemaRamaisEntityFrameworkCoreTestBase
    {
        private readonly IRamalRepository _ramalRepository;

        public RamalRepositoryTests()
        {
            _ramalRepository = GetRequiredService<IRamalRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _ramalRepository.GetListAsync(
                    nome: "43fbfda633584965857c4cb4bdc3150960a9bfd3392e4b0ab7f74f24820ad89cefc08808c5cf46ec8f8b0612456ec55c31ff",
                    numero: "5e92254716be46d0b35713274c9a1690de92ee71e33648dd82edc5756f0755ce7011f2e2f1024af08118bce167635f34c737",
                    departamento: "a615848c3265408883c90e12df67d6b7a0075858b0fe497a8129a56936fdc0066b3e3c8290ef45edb511a16dfad2fe74e3e3",
                    email: "5b124c1408514ba7b227d6ef2ae9068f87be26ad17b64ee1a2f92c26e0fc21cdd0d6f6c654994e4983ecacd7e6c5fc4860fe"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("237af739-6a9a-49cd-a2e1-fbb1e8fac944"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _ramalRepository.GetCountAsync(
                    nome: "b82df994c80b4dbaa112d9605a33b45b8f89d1cb403d4f9d883a76509b96986cbdd129695dd5445ead61ce3e21eb13000b59",
                    numero: "7a9ddfbfd1c24f719d2a3c805df28960b546d8c729324d2982421e133e5dbc7237ca90e521ae43ff855917e659f77cffc38c",
                    departamento: "15a44cdc0d754e9399cf6f2feed2d617689d2bfd056e457890f406f9b5a229d693e5b8e0456b4f06ac14f255ee7d01051f7a",
                    email: "68543da90a4c430c8abd513be0f005406a559a68368b4cd99d189d337ca4044c85373217d8614f92932c22ec91455cd3a25b"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}