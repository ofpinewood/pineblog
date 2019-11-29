using System.Threading.Tasks;
using Opw.PineBlog.Files;
using Opw.PineBlog.Models;
using System.Threading;
using System.Collections.Generic;

namespace Opw.PineBlog.Sample.Mocks
{

    public class GetPagedFileListQueryMock : IGetPagedFileListQuery
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public string DirectoryPath { get; set; }
        public FileType FileType { get; set; }

        public class Handler : IGetPagedFileListQueryHandler<GetPagedFileListQueryMock>
        {
            public async Task<Result<FileListModel>> Handle(GetPagedFileListQueryMock request, CancellationToken cancellationToken)
            {
                var pager = new Pager(request.Page, request.ItemsPerPage);
                var files = new List<FileModel>();
                files.Add(new FileModel { FileName = "image.jpg", MimeType = "image/jpeg", Url = "https://www.example.com/image.jpg" });

                var model = new FileListModel
                {
                    Files = files,
                    Pager = pager
                };

                return Result<FileListModel>.Success(model);
            }
        }
    }
}
