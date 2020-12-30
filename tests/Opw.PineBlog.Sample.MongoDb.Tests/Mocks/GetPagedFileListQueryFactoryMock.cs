using Opw.PineBlog.Files;

namespace Opw.PineBlog.Sample.MongoDb.Mocks
{
    public class GetPagedFileListQueryFactoryMock : IGetPagedFileListQueryFactory
    {
        public IGetPagedFileListQuery Create(int? page, int? itemsPerPage, string directoryPath, FileType? fileType)
        {
            if (!page.HasValue) page = 1;
            if (!itemsPerPage.HasValue) itemsPerPage = 3;
            if (string.IsNullOrWhiteSpace(directoryPath)) directoryPath = "";
            if (!fileType.HasValue) fileType = FileType.All;

            return new GetPagedFileListQueryMock
            {
                Page = page.Value,
                ItemsPerPage = itemsPerPage.Value,
                DirectoryPath = directoryPath,
                FileType = fileType.Value
            };
        }
    }
}
