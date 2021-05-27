using Microsoft.AspNetCore.Http;

namespace diplom.api.Models.RequestModels
{
    public class GenreRequestModel
    {
        public int GenreId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsMovement { get; set; }
        public IFormFile Image { get; set; }
    }
}
