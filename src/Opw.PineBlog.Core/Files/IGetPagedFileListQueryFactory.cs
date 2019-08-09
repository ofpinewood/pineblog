namespace Opw.PineBlog.Files
{
    public interface IGetPagedFileListQueryFactory
    {
        IGetPagedFileListQuery Create(int? page, int? itemsPerPage, string directoryPath, FileType? fileType);
    }
}