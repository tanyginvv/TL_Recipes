using Recipes.Application.Interfaces;

namespace Recipes.Infrastructure.ImageTools
{
    public class FormFileAdapter( IFormFile formFile ) : IFile
    {
        public string FileName => formFile.FileName;
        public long Length => formFile.Length;
        public Stream OpenReadStream() => formFile.OpenReadStream();
    }
}