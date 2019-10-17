
namespace Opw.PineBlog.Files
{
    public interface IDeleteFileCommandFactory
    {
        IDeleteFileCommand Create(string fileName, string targetPath);
    }
}