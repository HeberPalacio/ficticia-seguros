using FicticiaBackend.Repositories;
using System.Threading.Tasks;

namespace FicticiaBackend.Commands
{
    public class DeletePersonaCommand
    {
        private readonly IPersonaRepository _repository;

        public DeletePersonaCommand(IPersonaRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
