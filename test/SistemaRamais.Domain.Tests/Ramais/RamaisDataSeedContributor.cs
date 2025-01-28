using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using SistemaRamais.Ramais;

namespace SistemaRamais.Ramais
{
    public class RamaisDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IRamalRepository _ramalRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public RamaisDataSeedContributor(IRamalRepository ramalRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _ramalRepository = ramalRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _ramalRepository.InsertAsync(new Ramal
            (
                id: Guid.Parse("d8546118-2cbe-4d8d-9c74-9e3cf1a21016"),
                nome: "612ca6b1055b46b59fae0d2d4c5e25f9b6f4492637114e2abff596ad39767406f83091352ebe42fc948e73f9a7ed65c01ced",
                numero: "d82d48dc00",
                departamento: "a0bd8edac735478a96ca8dfe74c66fd69b1fd7b438c24d2fb4289742136574ea6f20fec7abf64722ac64480669c6e571cf6a",
                email: "d4162008bb3f4a6c9a05e53974b0a9ab0295b97e52a84f7da7e006c2705809e04e4c6ae1b4894433af37b32f34d854c721a5",
                normalizedName: "",
                normalizedEmail: ""
            ));

            await _ramalRepository.InsertAsync(new Ramal
            (
                id: Guid.Parse("0d4dc5f6-8ab4-49a4-ae49-a01e41da0344"),
                nome: "625a95085182421abb1ab76c5ac389a5448d40c299e64817997b035d66ff6779a0c254947c8c4ab694cafbccc3cd5a4d72d0",
                numero: "8e50cb29b4",
                departamento: "733977b1336442139c35e030a1f3dd8739c24e3790ec4b25855e5c779c20b6bbdc61742c16bc480d98dd59aa7398ef455c4e",
                email: "5175ff43613f44128e10b07a898e65c30f22eeb23c19471c9e9a6553a85bbeefbb6998b88d2342bf8f54c12eb6684bd43695",
                normalizedName: "",
                normalizedEmail: ""
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}