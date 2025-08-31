using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Admin;
using backend.Repositories.Admin;

namespace backend.Services.Admin
{
    public class AnnouncementService
    {
        private readonly AnnouncementRepository _repository;

        public AnnouncementService(AnnouncementRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<AnnouncementDto>> GetAllAnnouncementsAsync()
        {
            return _repository.GetAllAnnouncementsAsync();
        }

        public async Task<PublicAnnouncementsDto> GetPublicAnnouncementsAsync()
        {
            var allPublic = await _repository.GetPublicAnnouncementsAsync();
            return new PublicAnnouncementsDto
            {
                Urgent = allPublic.Where(a => a.Priority == "紧急"),
                Regular = allPublic.Where(a => a.Priority == "常规").Take(3)
            };
        }

        public Task<AnnouncementDto> CreateAnnouncementAsync(UpsertAnnouncementDto dto, int librarianId)
        {
            return _repository.CreateAnnouncementAsync(dto, librarianId);
        }

        public Task<AnnouncementDto> UpdateAnnouncementAsync(int id, UpsertAnnouncementDto dto)
        {
            return _repository.UpdateAnnouncementAsync(id, dto);
        }

        public Task<bool> TakedownAnnouncementAsync(int id)
        {
            return _repository.UpdateStatusAsync(id, "已撤回");
        }
    }
}