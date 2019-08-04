var fileManager = function (dataService) {
    var _openCallback;
    var _directoryPath;
    var _fileType;

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

    function pick(id) {
        var items = $('.filemanager .item-check:checked');
        if (_openCallback.name === 'insertImageCallback') {
            if (items.length === 0) {
                _openCallback(id);
            }
            else {
                for (i = 0; i < items.length; i++) {
                    _openCallback(items[i].id);
                }
            }
        }
        else {
            if (id === '') {
                if (items.length === 0) {
                    toastr.error('Please select an item');
                }
                else {
                    id = items[0].id;
                }
            }
            var url = 'assets/' + id;
            if (_openCallback.name === 'updatePostCoverCallback') {
                url = 'api/file/pick?type=postCover&asset=' + id + '&post=' + $('#Post_Id').val();
            }
            //else if (callBack.name === 'updateAppCoverCallback') {
            //    url = 'api/assets/pick?type=appCover&asset=' + id;
            //}
            //else if (callBack.name === 'updateAppLogoCallback') {
            //    url = 'api/assets/pick?type=appLogo&asset=' + id;
            //}
            //else if (callBack.name === 'updateAvatarCallback') {
            //    url = 'api/assets/pick?type=avatar&asset=' + id;
            //}
            dataService.get(url, _openCallback, fail);
        }
        close();

    }

    function uploadClick() {
        $('#files').trigger('click');
        return false;
    }

    function uploadSubmit() {
        var data = new FormData($('#frmUpload')[0]);
        dataService.upload('api/file/upload?targetPath=' + _directoryPath, data, submitCallback, fail);
    }

    function submitCallback() {
        load(1);
    }

    //function remove() {
    //    loading();
    //    var items = $('#fileManagerList input:checked');
    //    for (i = 0; i < items.length; i++) {
    //        if (i + 1 < items.length) {
    //            dataService.remove('api/assets/remove?url=' + items[i].id, emptyCallback, fail);
    //        }
    //        else {
    //            dataService.remove('api/assets/remove?url=' + items[i].id, removeCallback, fail);
    //        }
    //    }
    //}
    //function removeCallback(data) {
    //    loaded();
    //    toastr.success('Deleted');
    //    load(1);
    //}

    function load(page) {
        dataService.get('api/file?page=' + page + '&fileType=' + _fileType + '&directoryPath=' + _directoryPath, loadCallback, fail);
        return false;

        //$('#check-all').prop('checked', false);
        //var filter = $('input[name=filter]:checked').val();
        //if (!filter) {
        //    filter = 'filterAll';
        //}
        //var search = $('#asset-search').val();
        //if (search && search.length > 0) {
        //    dataService.get('api/assets?page=' + page + '&filter=' + filter + '&search=' + search, loadCallback, fail);
        //}
        //else {
        //    dataService.get('api/files?page=' + page + '&filter=' + filter, loadCallback, fail);
        //}
    }

    function loadCallback(data) {
        $('#fileManagerList').empty();
        var files = data.files;
        $.each(files, function (index) {
            var file = files[index];
            var tag = '<div class="col-md-4">' +
                '	<div class="file">' +
                '		<div class="file-image" onclick="fileManager.pick(\'' + file + '\'); return false"><img src="' + file + '" /></div>' +
                //'		<label class="custom-control custom-checkbox item-name">' +
                //'			<input type="checkbox" id="' + file + '" class="custom-control-input item-check" onchange="fileManager.check(this)">' +
                //'			<span class="custom-control-label">' + file + '</span>' +
                //'		</label>' +
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

        $('#file-pagination').append(pager);
        //showBtns();
    }

    //function loading() {
    //    $('#btnDelete').hide();
    //    $('.loading').fadeIn();
    //}
    //function loaded() {
    //    $('.loading').hide();
    //}

    //function emptyCallback(data) { }

    //function check(cbx) {
    //    if (!cbx.checked) {
    //        $('#check-all').prop('checked', false);
    //    }
    //    showBtns();
    //}

    //function showBtns() {
    //    var items = $('.bf-filemanager .item-check:checked');
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
        //remove: remove,
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
    var output = data + '](' + data + ')';

    //if (data.toLowerCase().match(/.(mp4|ogv|webm)$/i)) {
    //    var extv = 'mp4';
    //    if (data.toLowerCase().match(/.(ogv)$/i)) { extv = 'ogg'; }
    //    if (data.toLowerCase().match(/.(webm)$/i)) { extv = 'webm'; }
    //    output = '<video width="320" height="240" controls>\r\n  <source src="' + webRoot + data;
    //    output += '" type="video/' + extv + '">Your browser does not support the video tag.\r\n</video>';
    //}
    //else if (data.toLowerCase().match(/.(mp3|ogg|wav)$/i)) {
    //    var exta = 'mp3';
    //    if (data.toLowerCase().match(/.(ogg)$/i)) { exta = 'ogg'; }
    //    if (data.toLowerCase().match(/.(wav)$/i)) { exta = 'wav'; }
    //    output = '<audio controls>\r\n  <source src="' + webRoot + data;
    //    output += '" type="audio/' + exta + '">Your browser does not support the audio tag.\r\n</audio>';
    //}
    //else
    if (data.toLowerCase().match(/.(jpg|jpeg|png|gif)$/i)) {
        output = '\r\n![' + output;
    }
    else {
        output = '\r\n[' + output;
    }
    var selectedText = cm.getSelection();
    cm.replaceSelection(output);
};

var updatePostCoverCallback = function (data) {
    $('.post-cover').css('background-image', 'url(' + data.url + ')');
    $('#hdnPostImg').val(data.url);
    //toastr.success('Updated');
};