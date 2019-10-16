var fileManager = function (dataService) {
    var _openCallback;
    var _directoryPath;
    var _fileType;
    var _files = [];

    function open(openCallback, directoryPath, fileType) {
        _openCallback = openCallback;
        _directoryPath = directoryPath;
        _fileType = fileType;

        $('#fileManagerModal').modal();
        load(1);
    }

    function close() {
        $('#fileManagerModal').modal('hide');
    }

    function pick(index) {
        var file = _files[index];
        var files = $('.filemanager .item-check:checked');

        if (_openCallback.name === 'insertImageCallback') {
            if (files.length === 0) {
                _openCallback(file);
            }
            else {
                for (i = 0; i < files.length; i++) {
                    _openCallback(files[i]);
                }
            }
        }
        else {
            if (!file) {
                if (files.length === 0) {
                    toastr.error('Please select an item');
                }
                else {
                    file = files[0];
                }
            }
            //var url = 'assets/' + id;
            if (_openCallback.name === 'updatePostCoverCallback'
                || _openCallback.name === 'updateBlogCoverCallback') {
                _openCallback(file);
                //url = 'admin/file/pick?type=postCover&asset=' + id + '&post=' + $('#Post_Id').val();
            }
            //else if (callBack.name === 'updateAppCoverCallback') {
            //    url = 'admin/assets/pick?type=appCover&asset=' + id;
            //}
            //else if (callBack.name === 'updateAppLogoCallback') {
            //    url = 'admin/assets/pick?type=appLogo&asset=' + id;
            //}
            //else if (callBack.name === 'updateAvatarCallback') {
            //    url = 'admin/assets/pick?type=avatar&asset=' + id;
            //}
            //dataService.get(url, _openCallback, fail);
        }

        close();
    }

    function uploadClick() {
        $('#files').trigger('click');
        return false;
    }

    function uploadSubmit() {
        var data = new FormData($('#frmUpload')[0]);
        dataService.upload('admin/file/upload?targetPath=' + _directoryPath, data, submitCallback, fail);
    }

    function submitCallback() {
        load(1);
    }

    function remove() {
        loading();
        var items = $('#fileManagerList input:checked');
        for (i = 0; i < items.length; i++) {
            if (i + 1 < items.length) {
                dataService.remove('admin/file/delete?targetPath=' + _directoryPath + '&fileName=' + items[i].id, emptyCallback, fail);
            }
            else {
                dataService.remove('admin/file/delete?targetPath=' + _directoryPath + '&fileName=' + items[i].id, removeCallback, fail);
            }
        }
    }

    function removeCallback(data) {
        loaded();
        toastr.success('Deleted');
        load(1);
    }

    function load(page) {
        $('#checkAll').prop('checked', false);

        dataService.get('admin/file?page=' + page + '&fileType=' + _fileType + '&directoryPath=' + _directoryPath, loadCallback, fail);
        return false;

        //var filter = $('input[name=filter]:checked').val();
        //if (!filter) {
        //    filter = 'filterAll';
        //}
        //var search = $('#asset-search').val();
        //if (search && search.length > 0) {
        //    dataService.get('admin/assets?page=' + page + '&filter=' + filter + '&search=' + search, loadCallback, fail);
        //}
        //else {
        //    dataService.get('admin/files?page=' + page + '&filter=' + filter, loadCallback, fail);
        //}
    }

    function loadCallback(data) {
        $('#fileManagerList').empty();
        _files = data.files;

        $.each(_files, function (index) {
            var file = _files[index];
            var tag = '<div class="col-md-4">' +
                '	<div class="file" title="' + file.fileName + '">' +
                '		<div class="file-image" onclick="fileManager.pick(' + index + '); return false"><img src="' + file.url + '" /></div>' +
                '       <div class="file-name">' + file.fileName + '</div>' +
                '		<label class="custom-control custom-checkbox file-name" title="' + file.fileName + '">' +
                '			<input type="checkbox" id="file' + index + '" class="custom-control-input file-check" onchange="fileManager.check(this)">' +
                '			<span class="custom-control-label">' + file.fileName + '</span>' +
                '		</label>' +
                '	</div>' +
                '</div>';
            $("#fileManagerList").append(tag);
        });

        loadPager(data.pager);
    }

    function loadPager(pg) {
        $('#file-pagination').empty();

        var last = pg.currentPage * pg.itemsPerPage;
        var first = pg.currentPage === 1 ? 1 : ((pg.currentPage - 1) * pg.itemsPerPage) + 1;
        if (last > pg.total) { last = pg.total; }

        var pager = "";

        if (pg.showOlder === true) {
            pager += '<button type="button" class="btn btn-link" onclick="return fileManager.load(' + pg.older + ')"><i class="fa fa-chevron-left"></i></button>';
        }
        pager += '<span class="filemanager-pagination">' + first + '-' + last + ' out of ' + pg.total + '</span>';
        if (pg.showNewer === true) {
            pager += '<button type="button" class="btn btn-link" onclick="return fileManager.load(' + pg.newer + ')"><i class="fa fa-chevron-right"></i></button>';
        }

        $('#filePagination').append(pager);
        //showBtns();
    }

    //function loading() {
    //    $('#btnDelete').hide();
    //    $('.loading').fadeIn();
    //}
    //function loaded() {
    //    $('.loading').hide();
    //}

    function emptyCallback(data) { }

    //function check(cbx) {
    //    if (!cbx.checked) {
    //        $('#checkAll').prop('checked', false);
    //    }
    //    showBtns();
    //}

    //function showBtns() {
    //    var items = $('#fileManagerList .file-check:checked');
    //    console.log('showBtns', items.length);
    //    if (items.length > 0) {
    //        $('#btnDelete').show();
    //        $('#btnSelect').show();
    //    }
    //    else {
    //        $('#btnDelete').hide();
    //        $('#btnSelect').hide();
    //    }
    //}

    return {
        open: open,
        close: close,
        load: load,
        pick: pick,
        uploadClick: uploadClick,
        uploadSubmit: uploadSubmit,
        remove: remove,
        //check: check,
        //showBtns: showBtns
    };
}(DataService);

//$('#asset-search').keypress(function (event) {
//    var keycode = event.keyCode ? event.keyCode : event.which;
//    if (keycode === 13) {
//        fileManagerController.load(1);
//        return false;
//    }
//});

//$('.bf-posts-list .item-link-desktop').click(function () {
//    $('.bf-posts-list .item-link-desktop').removeClass('active');
//    $(this).addClass('active');
//});

//// check all
//var itemCheckfm = $('.bf-filemanager .item-check');
//var firstItemCheckfm = itemCheckfm.first();

//$(firstItemCheckfm).on('change', function () {
//    var itemCheckfm = $('.bf-filemanager .item-check');
//    $(itemCheckfm).prop('checked', this.checked);
//    fileManagerController.showBtns();
//});

//// callbacks
//var updateAvatarCallback = function (data) {
//    $('#author-avatar').val(data.url);
//    toastr.success('Updated');
//};
//var updateAppCoverCallback = function (data) {
//    $('#BlogItem_Cover').val(data.url);
//    toastr.success('Updated');
//};
//var updateAppLogoCallback = function (data) {
//    $('#BlogItem_Logo').val(data.url);
//    toastr.success('Updated');
//};

var insertImageCallback = function (data) {
    var cm = _editor.codemirror;
    var output = data.fileName + '](' + data.url + ')';

    //if (data.url.toLowerCase().match(/.(mp4|ogv|webm)$/i)) {
    //    var extv = 'mp4';
    //    if (data.url.toLowerCase().match(/.(ogv)$/i)) { extv = 'ogg'; }
    //    if (data.url.toLowerCase().match(/.(webm)$/i)) { extv = 'webm'; }
    //    output = '<video width="320" height="240" controls>\r\n  <source src="' + webRoot + data.url;
    //    output += '" type="video/' + extv + '">Your browser does not support the video tag.\r\n</video>';
    //}
    //else if (data.url.toLowerCase().match(/.(mp3|ogg|wav)$/i)) {
    //    var exta = 'mp3';
    //    if (data.url.toLowerCase().match(/.(ogg)$/i)) { exta = 'ogg'; }
    //    if (data.url.toLowerCase().match(/.(wav)$/i)) { exta = 'wav'; }
    //    output = '<audio controls>\r\n  <source src="' + webRoot + data.url;
    //    output += '" type="audio/' + exta + '">Your browser does not support the audio tag.\r\n</audio>';
    //}
    //else
    if (data.url.toLowerCase().match(/.(jpg|jpeg|png|gif)$/i)) {
        output = '\r\n![' + output;
    }
    else {
        output = '\r\n[' + output;
    }

    //var selectedText = cm.getSelection();
    cm.replaceSelection(output);
};

var updatePostCoverCallback = function (data) {
    $('.post-cover').css('background-image', 'url(' + data.url + ')');
    $('#CoverUrl').val(data.url);
    //toastr.success('Updated');
};

var updateBlogCoverCallback = function (data) {
    $('.post-cover').css('background-image', 'url(' + data.url + ')');
    $('#BlogSettings_CoverUrl').val(data.url);
    //toastr.success('Updated');
};