/// <reference path="../System/SystemFunction.js" />


var Extension = {
    Image: ['jpg', 'jpeg', 'png', 'gif'],
    Video: ['mov', 'wmv', 'avi', 'mp4'],
    PDF: ['pdf'],
    Document: ['doc', 'docx', 'xls', 'xlsx', 'txt'],
    Word: ["doc", "docx"],
    Excel: ["xls", "xlsx"],
    Powpoint: ["pptx", "pdf", "ppt"],
    txt: ["txt"],
    //Email: ["msg"],
    Other: ['rar', 'zip'],
    GetAll: function () {
        var arrExt = [];
        for (var key in this) {
            if (key != 'GetAll') arrExt = arrExt.concat(this[key]);
        }
        return arrExt;
    }
};

//  , / .txt / . / . / . / . / . / ./ . / .mp4 และ URL Link
var IconExtension = {
    Image: "imageImage",
    Video: "videoImage",
    PDF: "pdfImage",
    Word: "wordImage",
    Excel: "excelImage",
    Powpoint: "powerpointImage",
    txt: "txtImage",
    Link: "linkImage",
    Email: "EmailImage",
    Other: "zipImage",
}
var ColorExtension = {
    Image: "",
    Video: "",
    PDF: "",
    Word: "",
    Excel: "",
    Powpoint: "",
    Link: "",
    Email: "",
    Other: "",
}
var AshxSysFunc = {
    url: "/Config/SaveToServer",
    FuncEncrypt: "encrypt",
    FuncDecrypt: "decrypt",
    FuncEncodeForJavaDecode: "encrypt_decodejava",
    FuncErrorLog: "logerror",
    FuncTempFile: "DeleteTempFile",
    UrlFileUpload: "Ashx/Fileuploader.ashx",
    sUploadFolder: "~/UploadFiles/File",
    sUploadFolderTemp: "UploadFile/Temp/",

}

function UploadSetup() {

    var Config = new Object();
    Config.sID = "";
    Config.$ful = null;
    Config.nFileSize = 10;
    Config.nAllFileSize = null;
    Config.Extensions = Extension.Image;
    Config.ModeMulti = true;
    Config.ArrFileSuccess = null;
    Config.ArrFileError = null;
    Config.PathTemp = null;
    Config.ArrFile = [];
    Config.BeforeSend = null;//เก็บฟังชั่นที่ Bind Data
    Config.OnSuccess = function () { };//เก็บฟังชั่นที่ Bind Data
    Config.dvFile = "";
    Config.PushFileAllSuccess = false;
    Config.IsHasPRMS = true;
    Config.IsView = false;
    Config.nNum = 1;
    Config.HasURL = false;
    Config.IsValidateETC = false;
    Config.sValidateETC = "";
    Config.Url = "";
    return Config;
}

function SetUploadFileAjax(Config) {
    CreateDvFile(Config);
    if (Config.ArrFile.length > 0) {

        Enumerable.From(Config.ArrFile).ToArray().forEach(function (f) {
            f.IsDelete = false;
            f.name = f.FileName;
            f.size = f.sSize;
            f.file = f.sPath + f.sSystemFileName;
            return f;
        });
    }
    Config.$ful = $("input#ful_" + Config.sID);
    ArrFile = Config.ArrFile;
    var sDivValidate = GetTextValidate(Config);
    var API_Obj = Config.$ful.fileuploader({

        limit: Config.ModeMulti ? 5 : 1, // limit of the files {Number}
        maxSize: Config.nAllFileSize == null ? null : Config.nAllFileSize, // files maximal size in Mb {Number}
        fileMaxSize: Config.nFileSize == null ? null : Config.nFileSize, // file maximal size in Mb {Number}
        extensions: (isEmpty(Config.Extensions) ? Extension.GetAll() : Config.Extensions), // allowed extensions or types {Array}
        changeInput: '<div class="fileuploader-input" id="dv_' + Config.sID + '">' +
            '<div class="fileuploader-input-caption">' +
            '<span>${captions.feedback}</span>' +
            '</div>' +
            '<div class="fileuploader-input-button">' +
            '<span>${captions.button}</span>' +
            '</div>' +
            '</div>' + sDivValidate,
        inputNameBrackets: false,
        theme: 'default',
        thumbnails: {
            box: '<div class="fileuploader-items" style="background-color: #f5f6fA;">' +
                '<ul class="fileuploader-items-list" id="ul_' + Config.sID + '"></ul>' +
                '</div>',
            boxAppendTo: null,
            item: '<li class="fileuploader-item" id="lstFile_${nID}" style="line-height: 25px;padding-left: 25px;min-height: 92px;"  >' +
                '<div class="columns">' +
                '<div class="column-thumbnail">${image}</div>' +
                '<div class="column-title">' +
                '<div title="${name}"  data-toggle="tooltip"><span>${name}</span></div>' +
                '<span>${size2}</span>' +
                '</div>' +
                '<div class="column-actions">' +
                //'<button type="button" class="btn btn-sm btn-info tooltipstered" onclick="FancyBox_ViewFile(\'${file}\')"  data-toggle="tooltip" title="View File"><i class="fa fa-eye" ></i></button>&nbsp;' +
                '<button type="button" class="btn btn-sm btn-danger tooltipstered"   data-toggle="tooltip" title="Delete File"><i class="fa fa-trash"></i></button>' +
                '</div>' +
                '</div>' +
                '<div class="progress-bar2">${progressBar}<span></span></div>' +
                '</li>',
            item2: '<li class="fileuploader-item" id="lstFile_${nID}" style="line-height: 25px;padding-left: 25px;min-height: 92px;" >' +
                '<div class="columns">' +
                '<div class="column-thumbnail" >${image}</div>' +
                '<div class="column-title" onclick="FancyBox_ViewFile(\'${url}\')" style="cursor: pointer;">' +
                '<div title="${name}"  data-toggle="tooltip"><span>${name}</span></div>' +
                '<span>${size2}</span>' +
                '</div>' +
                '<div class="column-actions">' +
                //'<button type="button" class="btn btn-sm btn-info tooltipstered" onclick="FancyBox_ViewFile(\'${file}\')"  data-toggle="tooltip" title="View File"><i class="fa fa-eye" ></i></button>&nbsp;' +
                '<button type="button" class="btn btn-sm btn-danger tooltipstered"   data-toggle="tooltip" title="Delete File"><i class="fa fa-trash"></i></button>' +
                '</div>' +
                '</div>' +
                '<div class="progress-bar2">${progressBar}<span></span></div>' +
                '</li>',
            itemPrepend: false,
            removeConfirmation: true,
            startImageRenderer: false,
            synchronImages: true,
            canvasImage: {
                width: null,
                height: null
            },
            dialogs: {
                alert: function (text) {

                    return DialogWarning(text);
                },
                confirm: function (text, callback) {
                    DialogConfirmSubmit(callback(), function () {
                        swal.close();
                        null
                    })
                }
            },
            _selectors: {
                list: '.fileuploader-items-list',
                item: '.fileuploader-item',
                start: '.fileuploader-action-start',
                retry: '.btn-warning',
                remove: '.btn-danger'
            },
            onImageLoaded: function (itemEl, listEl, parentEl, newInputEl, inputEl) {
                // your callback here
            },
        },
        upload: {
            url: Config.Url,
            data: { savetopath: AshxSysFunc.sUploadFolderTemp },
            type: 'POST',
            enctype: 'multipart/form-data',
            start: true,
            synchron: false,
            beforeSend: function (item, listEl, parentEl, newInputEl, inputEl) {
                item.upload.data.isTest = 'no';
                BlockUI();
                return true;
            },
            onSuccess: function (data, item, listEl, parentEl, newInputEl, inputEl, textStatus, jqXHR) {
                UnblockUI();
                data = data.d;

                var nID = 1;
                if (Config.ArrFile.length > 0) {
                    nID = Enumerable.From(Config.ArrFile).Max(function (m) { return m.nID }) + 1;
                }
                data.nID = nID;
            //         public DateTime dStartDate { get; set; }
            //public DateTime dEndDate { get; set; }
            //public string TotalTime { get; set; }
            //public List < ProcessLog > lstProcess { get; set; }
                Config.ArrFile.push({
                    nID: nID,
                    sPath: data.SaveToPath,
                    sSystemFileName: data.SaveToFileName,
                    sFileName: data.FileName,
                    IsDelete: false,
                    IsNew: true,
                    sSize: item.size + "",
                    //dStartDate: data.dStartDate,
                    //dEndDate: data.dEndDate,
                    //TotalTime : data.TotalTime,
                    //lstProcess : data.lstProcess
                })


                item.data = data;
                item.html.find('.column-actions').empty();
                var btn = '<button type="button" class="btn btn-sm btn-info  tooltipstered" onClick="FancyBox_ViewFile(\'' + data.url + '\')"  data-toggle="tooltip" title="View File"><i class="fa fa-eye"></i></button>&nbsp;';
                btn = '<button type="button" class="btn btn-sm btn-danger  tooltipstered" data-toggle="tooltip" title="Delete File"><i class="fa fa-trash"></i></button>';
                item.html.find('.column-actions').append(btn)
                //item.html.find('.column-title').empty();
                item.html.find('.column-title').attr("onclick", "FancyBox_ViewFile(\'" + data.url + "\')");
                item.html.find('.column-title').css("cursor", "pointer");

                //var dvTitle = '<div class="titleFile" title="' + data.FileName + '"  data-toggle="tooltip" onclick="FancyBox_ViewFile(\'' + data.url + '\')" style="cursor: pointer;"><span>' + data.FileName + '</span></div>';
                //dvTitle += '<span>' + GetSize(item.size) + '</span>';
                //item.html.find('.column-title').append(dvTitle);


                item.html.attr("id", "lstFile_" + nID);

                item.html.find("button").tooltipster({
                    animation: 'fade',
                    delay: 100,
                    theme: 'tooltipster-borderless',
                    trigger: 'hover'
                });
                item.html.find("div").tooltipster({
                    animation: 'fade',
                    delay: 100,
                    theme: 'tooltipster-borderless',
                    trigger: 'hover'
                });
                setTimeout(function () {
                    item.html.find('.progress-bar2').fadeOut(400);
                }, 400);

            },
            onError: function (item, listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus, errorThrown) {
                UnblockUI();
                var progressBar = item.html.find('.progress-bar2');
                if (progressBar.length > 0) {
                    progressBar.find('span').html(0 + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(0 + "%");
                    item.html.find('.progress-bar2').fadeOut(400);
                }

                item.upload.status != 'cancelled' && item.html.find('.fileuploader-action-retry').length == 0 ? item.html.find('.column-actions').prepend(
                    '<button class="btn btn-sm btn-warning"  data-toggle="tooltip" title="Retry" style="margin-right:6px;"><i class="fa fa-redo"></i></button>'
                ) : null;
            },
            onProgress: function (data, item, listEl, parentEl, newInputEl, inputEl) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length > 0) {
                    progressBar.show();
                    progressBar.find('span').html(data.percentage + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(data.percentage + "%");
                }
            },
            onComplete: function (listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus) {
                Config.OnSuccess();
            },
        },
        addMore: true,
        files: null,
        clipboardPaste: 2000,
        listInput: false,
        enableApi: true,
        onListInput: function (fileList, listInputEl, listEl, parentEl, newInputEl, inputEl) {
            var list = [];

            $.each(fileList, function (index, value) {
                list.push(value.name);
            });

            return list;
        },

        onRemove: function (item, listEl, parentEl, newInputEl, inputEl) {

            Enumerable.From(Config.ArrFile).Where(function (w) { return w.nID == item.nID || w.nID == item.data.nID }).ToArray().forEach(function (f) {
                f.IsDelete = true;

                return f;
            });
            return true;
        },
        captions: {
            button: function (options) { return 'Choose ' + (options.limit == 1 ? 'File' : 'Files'); },
            feedback: function (options) { return 'Choose ' + (options.limit == 1 ? 'file' : 'files') + ' to upload'; },
            feedback2: function (options) { return ' ' + (options.length > 1 ? ' files were' : ' file was') + ' chosen'; },
            drop: 'Drop the files here to Upload',
            paste: '<div class="fileuploader-pending-loader"><div class="left-half" style="animation-duration: ${ms}s"></div><div class="spinner" style="animation-duration: ${ms}s"></div><div class="right-half" style="animation-duration: ${ms}s"></div></div> Pasting a file, click here to cancel.',
            removeConfirmation: 'Do you want to delete data',
            errors: {
                filesLimit: 'Only ${limit} files are allowed to be uploaded.',
                filesType: 'Only ${extensions} files are allowed to be uploaded.',
                fileSize: '${name} is too large! Please choose a file up to ${fileMaxSize}MB.',
                filesSizeAll: 'Files that you choosed are too large! Please upload files up to ${maxSize} MB.',
                fileName: 'File with the name ${name} is already selected.',
                folderUpload: 'You are not allowed to upload folders.'
            }
        }

    });
    $("div#dvLink_" + Config.sID).hide("fast");
    if (Config.HasURL) {
        $("select#ddlTypeFile_" + Config.sID).change(function () {
            var nVal = $(this).val();
            if (nVal == 0) {
                $("div#dv_" + Config.sID).show("fast");
                $("div#dvLink_" + Config.sID).hide("fast");
                $("span#spValidateLink_" + Config.sID).hide("fast");
                $("span#spValidateFile_" + Config.sID).show("fast");
            }
            else {
                $("div#dvLink_" + Config.sID).show("fast");
                $("div#dv_" + Config.sID).hide("fast");

                $("span#spValidateLink_" + Config.sID).show("fast");
                $("span#spValidateFile_" + Config.sID).hide("fast");
            }
        });
        objValidate = {};
        objValidate[$("input#txtNameFile_" + Config.sID).attr("name")] = addValidate_notEmpty('Please specify Link name');
        objValidate[$("input#txtLinkFile_" + Config.sID).attr("name")] = addValidate_notEmpty('Please specify Link');

        BindValidate("dvLink_" + Config.sID, objValidate);
        $("button#btnAdd_" + Config.sID).click(function () {

            if (CheckValidate("dvLink_" + Config.sID) == true) {
                var nID = 1;
                if (Config.ArrFile.length > 0) {
                    nID = Enumerable.From(Config.ArrFile).Max(function (w) { return w.nID }) + 1;
                }
                Config.ArrFile.push({
                    nID: nID,
                    sFileName: $("input#txtNameFile_" + Config.sID).val(),
                    sPath: $("input#txtLinkFile_" + Config.sID).val(),
                    IsNew: true,
                    IsDelete: false,
                    name: $("input#txtNameFile_" + Config.sID).val(),
                    size: "",
                    url: $("input#txtLinkFile_" + Config.sID).val(),
                })
                var ArrFileTemp = [];
                ArrFileTemp.push({
                    nID: nID,
                    sFileName: $("input#txtNameFile_" + Config.sID).val(),
                    sPath: $("input#txtLinkFile_" + Config.sID).val(),
                    IsNew: true,
                    IsDelete: false,
                    name: $("input#txtNameFile_" + Config.sID).val(),
                    size: "",
                    url: $("input#txtLinkFile_" + Config.sID).val(),
                })
                //Config.API_Obj.reset();
                Config.API_Obj.append(ArrFileTemp);
                //$("ul#ul_" + Config.sID).append(FormatFileLiUpload("" + $("input#txtLinkFile_" + Config.sID).val() + "", "" + $("input#txtNameFile_" + Config.sID).val(), "", nID, true))
                $("input#txtNameFile_" + Config.sID).val("");
                $("input#txtLinkFile_" + Config.sID).val("")
                $("#dvLink_" + Config.sID).formValidation('updateStatus', '' + $("input#txtLinkFile_" + Config.sID).attr("name") + '', 'NOT_VALIDATED');
                $("#dvLink_" + Config.sID).formValidation('updateStatus', '' + $("input#txtNameFile_" + Config.sID).attr("name") + '', 'NOT_VALIDATED');
            }

        });
    }
    if (Config.IsView) {
        $("select#ddlTypeFile_" + Config.sID).remove();
        $(".fileuploader-input").remove();
        $(".column-actions button.btn-danger").remove();
    }
    $("ul#ul_" + Config.sID).delegate(" button[id^='btnDelFile_']", 'click', function () {
        var nID = $(this).attr('data-nID');

        var funcYes = function () {
            Enumerable.From(Config.ArrFile).Where(function (w) { return w.nID == nID }).ToArray().forEach(function (f) {
                f.IsDelete = true;
                DeleteRow(nID);
                return f;
            });

            swal.close();
        }
        DialogConfirmCancel(funcYes);
    });
    Config.API_Obj = $.fileuploader.getInstance(API_Obj);
}


var _UploadFIle = {
    OpenFile: function (sPath) {

        var ArrFileSplit = sPath.split('.');
        var FileType = ArrFileSplit[ArrFileSplit.length - 1].toLowerCase();

        var IsDocument = false;
        if (Extension.Document.indexOf(FileType) != -1) {
            IsDocument = true;
        }

        if (IsDocument) {
            $.fancybox.open({
                type: 'iframe',
                href: sPath,
                iframe: { preload: false }, // fixes issue with iframe and IE
                fitToView: false,
                minWidth: '90%',
                minheight: '90%',
                autoSize: false,
                closeClick: false,
                openEffect: 'elastic',
                closeEffect: 'elastic',
                helpers: {
                    overlay: { closeClick: false } // prevents closing when clicking OUTSIDE fancybox
                }
            });
        }
        else {
            $.fancybox.open({
                href: sPath,
                iframe: { preload: false }, // fixes issue with iframe and IE
                fitToView: false,
                maxWidth: '90%',
                maxheight: '90%',
                autoSize: false,
                closeClick: false,
                openEffect: 'elastic',
                closeEffect: 'elastic',
                helpers: {
                    overlay: { closeClick: false } // prevents closing when clicking OUTSIDE fancybox
                }
            });
        }
    },
    BindFileSingle: function ($input, _Data, DeleteFunction, IsDelete) {

        //หาคอนโทรน
        var _DIVUpload = $input.find(".UploadFile");//Div ของตัวอัพโหลด
        var _DIVDetail = $input.find(".DetailFile");//Div ของตัวแสดงผลไฟล์

        var sHTML = "";

        var DataFilter = Enumerable.From(_Data).Where("x => x.sDelete == 'N'").ToArray();

        for (var i = 0; i < DataFilter.length; i++) {

            var item = DataFilter[i];

            //ใส่ HTML FIle
            var sFilePath = item.SaveToPath + item.SaveToFileName;
            var sFileNameShort = item.FileName.length > 25 ? item.FileName.substring(0, 25) + "..." : item.FileName;
            sHTML += "<a class=\"btn btn-primary text-white\" onclick=\"_UploadFIle.OpenFile('" + sFilePath + "')\" title=\"" + item.FileName + "\"><i class=\"fa fa-search\"></i>&nbsp;&nbsp;" + sFileNameShort + "</a>";

            if (DeleteFunction != "") {
                if (IsDelete) {
                    sHTML += "<a class=\"btn btn-danger text-white\" onclick=\"" + DeleteFunction + "('" + item.ID + "')\"><i class=\"glyphicon glyphicon-trash\"></i></a>";
                }

            }


        }

        if (DataFilter.length > 0) {
            _DIVUpload.hide();
            _DIVDetail.show();
            _DIVDetail.empty();
            _DIVDetail.html(sHTML);

        }
        else {
            _DIVUpload.show();
            _DIVDetail.hide();
            _DIVDetail.empty();
        }

    },
    BindFileMulti: function ($input, _Data, DeleteFunction) {

        //หาคอนโทรน
        var _DIVUpload = $input.find(".UploadFile");//Div ของตัวอัพโหลด
        var _DIVDetail = $input.find(".DetailFile");//Div ของตัวแสดงผลไฟล์

        var sHTML = "";

        var DataFilter = Enumerable.From(_Data).Where("x => x.sDelete == 'N'").ToArray();

        for (var i = 0; i < DataFilter.length; i++) {

            var item = DataFilter[i];

            //ใส่ HTML FIle
            var sFilePath = item.SaveToPath + item.SaveToFileName;
            var sFileNameShort = item.FileName.length > 25 ? item.FileName.substring(0, 25) + "..." : item.FileName;
            sHTML += "<a class=\"btn btn-primary text-white\" onclick=\"_UploadFIle.OpenFile('" + sFilePath + "')\" title=\"" + item.FileName + "\"><i class=\"fa fa-search\"></i>&nbsp;&nbsp;" + sFileNameShort + "</a>";
            sHTML += "<a class=\"btn btn-danger text-white\" onclick=\"" + DeleteFunction + "('" + item.ID + "')\"><i class=\"glyphicon glyphicon-trash\"></i></a>";
            sHTML += "<div class=\"MarginBT15\"></div>";
        }

        if (DataFilter.length > 0) {
            //_DIVUpload.hide();
            _DIVDetail.show();
            _DIVDetail.empty();
            _DIVDetail.html(sHTML);

        }
        else {
            //_DIVUpload.show();
            _DIVDetail.hide();
            _DIVDetail.empty();
        }

    },
    DeleteFileAshx: function (Arr_Data, File_ID, funDelComplete) {

        var MSG = "";
        var item = Enumerable.From(Arr_Data).FirstOrDefault(null, function (x) { return x.ID == File_ID });

        if (item != null) {
            item.sDelete = "Y";
            if (Boolean(item.IsNewFile)) {
                $.ajax({
                    type: "POST",
                    url: AshxSysFunc.UrlFileUpload,
                    data: { funcname: "DEL", delpath: item.SaveToPath, delfilename: item.SaveToFileName },
                    success: function (response) {
                        if (Boolean(response.IsCompleted)) {

                            if (funDelComplete) {
                                funDelComplete();
                            }

                        } else {

                        }
                    },
                    complete: function (jqXHR, status) {//finaly

                    }
                });
            }
            else {
                if (funDelComplete) {
                    funDelComplete();
                }
            }

        } else {
            MSG = "Not found !";
        }

        return MSG;
    },
    RerunID: function (ArrFile) {

        //อัพเดท nID ให้เรียงลำดับใหม่ จาก 1 เพื่อใช้กรณีอัพเดท Flag เป็นลบ
        for (var i = 0; i < ArrFile.length; i++) {
            var item = ArrFile[i];
            item.ID = i + 1;
        }
    },
    BindFileSingleOnly: function ($input, _Data, DeleteFunction) {

        //หาคอนโทรน
        var _DIVUpload = $input.find(".UploadFile");//Div ของตัวอัพโหลด
        var _DIVDetail = $input.find(".DetailFile");//Div ของตัวแสดงผลไฟล์

        var sHTML = "";

        var DataFilter = Enumerable.From(_Data).Where("x => x.sDelete == 'N'").ToArray();

        for (var i = 0; i < DataFilter.length; i++) {

            var item = DataFilter[i];

            //ใส่ HTML FIle
            var sFilePath = item.SaveToPath + item.SaveToFileName;
            var sFileNameShort = item.FileName.length > 25 ? item.FileName.substring(0, 25) + "..." : item.FileName;
            sHTML += "<a class=\"btn btn-primary text-white\" onclick=\"_UploadFIle.OpenFile('" + sFilePath + "')\" title=\"" + item.FileName + "\"><i class=\"fa fa-search\"></i>&nbsp;&nbsp;" + sFileNameShort + "</a>";

            if (DeleteFunction != "") {
                sHTML += "<a class=\"btn btn-danger text-white\" onclick=\"" + DeleteFunction + "('" + item.ID + "')\"><i class=\"glyphicon glyphicon-trash\"></i></a>";
            }


        }

        _DIVUpload.hide();
        _DIVDetail.show();
        _DIVDetail.empty();
        _DIVDetail.html(sHTML);



    },
}


function DeleteRow(nID) {

    $("li#lstFile_" + nID).animate({ 'opacity': 0 }, 200, function () {
        setTimeout(function () {
            $("li#lstFile_" + nID).slideUp(200, function () {
                $("li#lstFile_" + nID).remove();
            });
        }, 100);
    });
}
function CreateDvFile(Config) {
    var sDiv = "";
    sDiv = '<div>';
    if (Config.HasURL) {
        sDiv += '<div class="col-lg-3 col-md-12 form-group row input-group" style="padding-left: 0;padding-right: 0;margin-right: 0;margin-left: 0;">';
        sDiv += '<div class="input-group-append"><span class="input-group-text"><i class="fas fa-list"></i></span></div>';
        sDiv += '<select class="form-control form-control-sm" id="ddlTypeFile_' + Config.sID + '"><option value="0">Files</option><option value="1">Link</option></select>';
        sDiv += '</div>';

    }

    sDiv += '<div class="col-lg-12 col-md-12 row" style="padding-left: 0;padding-right: 0;margin-right: 0;margin-left: 0;">';
    if (Config.HasURL) {
        sDiv += '<div id="dvLink_' + Config.sID + '" class="row col-12 form-group" style="margin-bottom: -16px;"><div class="form-group col-lg-3 col-12" style="padding-left: 0;"><input type="text" class="form-control form-control-sm" id="txtNameFile_' + Config.sID + '" name="txtNameFile_' + Config.sID + '" style="margin-right: 1rem;" placeholder="Link name (Google)"/></div>';
        sDiv += '<div  class="form-group col-lg-7  col-12"><input type="text" class="form-control form-control-sm" id="txtLinkFile_' + Config.sID + '" name="txtLinkFile_' + Config.sID + '"  style="margin-right: 1rem;;" placeholder="Link (http://www.google.com)"/></div>';
        sDiv += '<div class="form-group col-lg-2 col-12"><button type="button" class="btn btn-sm btn-success" title="Add Link" data-toggle="tooltip" id="btnAdd_' + Config.sID + '" ><i class="fa fa-plus" aria-hidden="true"></i></button></div></div>';
    }
    sDiv += '<input type="file" id="ful_' + Config.sID + '" name="ful_' + Config.sID + '"></div>';

    sDiv += '</div>';
    sDiv += '<div class="col-xs-12 col-md-12 DetailFile">';
    sDiv += '</div>';
    sDiv += '</div>';

    $("div#" + Config.sID).append(sDiv);
}
function CreateUlUploadFile(Config) {
    var ul = "";
    $("ul#ul_" + Config.sID).empty();
    Config.API_Obj.reset();
    Enumerable.From(Config.ArrFile).ToArray().forEach(function (f) {
        f.IsDelete = false;
        f.name = f.sFileName;
        f.size = f.sSize;
        f.url = f.sPath + f.sSystemFileName;
        return f;
    });
    if (Config.ArrFile.length > 0) {

        Config.API_Obj.append(Config.ArrFile);
    }

    //$.each(Config.ArrFile, function () {
    //    ul += FormatFileLiUpload("" + this.sPath + this.sSystemFileName + "", "" + this.sFileName + "", "" + this.sSize + "", this.nID, Config.IsHasPRMS);
    //})
    //$("ul#ul_" + Config.sID).append(ul);



}
function FormatFileLiUpload(sPath, sName, sSize, nID, IsHasPRMS) {
    var li = "";
    li = '<li class="fileuploader-item file-type-application file-ext-pdf upload-successful" id=lstFile_' + nID + ' style="line-height: 25px;padding-left: 25px;">';
    li += '<div class="columns"><div class="column-thumbnail">';
    li += '<div class="fileuploader-item-image fileupload-no-thumbnail">';
    li += '<div class="fileuploader-item-icon">' + GenNameTypeFile(sPath, sSize) + '</div>';

    var sTypeSize = GetSize(sSize);
    if (sSize == "") {
        sTypeSize = "";
    }
    li += '</div></div><div class="column-title"><div title="' + sName + '" data-toggle="tooltip" onclick="FancyBox_ViewFile(\'' + sPath + '\')" style="cursor: pointer;"><span>' + sName + '</span></div><span>' + sTypeSize + '.</span></div>';
    //if (IsHasPRMS) {
    //    if (sSize != "") {
    //        li += '<div class="column-actions tooltipstered"><button type="button" class="btn btn-sm btn-info tooltipstered" onclick="FancyBox_ViewFile(\'' + sPath + '\')" data-toggle="tooltip" title="View File"><i class="fa fa-eye"></i></button>&nbsp;<button type="button" class="btn btn-sm btn-danger  tooltipstered" id="btnDelFile_' + nID + '" data-nID="' + nID + '" data-toggle="tooltip" title="Delete File"><i class="fa fa-trash"></i></button></div>';
    //    }
    //    else {
    //        li += '<div class="column-actions tooltipstered"><button type="button" class="btn btn-sm btn-info tooltipstered" onclick="window.open(\'' + sPath + '\',\'_blank\')" data-toggle="tooltip" title="View File"><i class="fa fa-eye"></i></button>&nbsp;<button type="button" class="btn btn-sm btn-danger  tooltipstered" id="btnDelFile_' + nID + '" data-nID="' + nID + '"  data-toggle="tooltip" title="Delete File"><i class="fa fa-trash"></i></button></div>';
    //    }

    //}
    //else {
    //    if (sSize != "") {
    //        li += '<div class="column-actions tooltipstered"><button type="button" class="btn btn-sm btn-info tooltipstered" onclick="FancyBox_ViewFile(\'' + sPath + '\')" data-toggle="tooltip" title="View File"><i class="fa fa-eye"></i></button></div>';
    //    }
    //    else {
    //        li += '<div class="column-actions tooltipstered"><button type="button" class="btn btn-sm btn-info tooltipstered" onclick="window.open(\'' + sPath + '\',\'_blank\')" data-toggle="tooltip" title="View File"><i class="fa fa-eye"></i></button></div>';
    //    }

    //}

    li += '</div></li>';

    return li;
}
function ClearUlUpload(Config) {
    Config.API_Obj.reset();
    $("ul#ul_" + Config.sID).empty();
}
function GetSize(bytes) {
    if (bytes == 0) return '0 Byte';
    var k = 1000,
        sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
        i = Math.floor(Math.log(bytes) / Math.log(k));
    var result = "";
    if (bytes != "") {
        result = (bytes / Math.pow(k, i)).toPrecision(3) + ' ' + sizes[i]
    }

    return result;
}
function GenColorCode(sPath, sSize) {

    var ArrFileSplit = [];
    var str = "";
    if (sSize != "") {
        ArrFileSplit = sPath.split('.');
        str = ArrFileSplit[ArrFileSplit.length - 1].toLowerCase();
    }
    else {
        str = "html";
    }

    if (!str || str.length == 0)
        return false;

    for (var i = 0, hash = 0; i < str.length; hash = str.charCodeAt(i++) + ((hash << 5) - hash));
    for (var i = 0, colour = '#'; i < 3; colour += ('00' + ((hash >> i++ * 2) & 0xFF)
            .toString(16))
        .slice(-2));
    return colour;
}
function GenNameTypeFile(sPath, sSize) {
    var ArrFileSplit = [];
    var str = "";
    if (sSize != "") {
        ArrFileSplit = sPath.split('.');
        str = ArrFileSplit[ArrFileSplit.length - 1].toLowerCase();
    }
    else {
        str = "Link";
    }
    var nameIcon = "";
    if ($.inArray(str, Extension.Image) > -1) {
        nameIcon = IconExtension.Image;
    }
    else if ($.inArray(str, Extension.Video) > -1) {
        nameIcon = IconExtension.Video;
    }
    else if ($.inArray(str, Extension.PDF) > -1) {
        nameIcon = IconExtension.PDF;
    }
    else if ($.inArray(str, Extension.PDF) > -1) {
        nameIcon = IconExtension.PDF;
    }
    else if ($.inArray(str, Extension.Word) > -1) {
        nameIcon = IconExtension.Word;
    }
    else if ($.inArray(str, Extension.Excel) > -1) {
        nameIcon = IconExtension.Excel;
    }
    else if ($.inArray(str, Extension.Powpoint) > -1) {
        nameIcon = IconExtension.Powpoint;
    }
    else if ($.inArray(str, Extension.txt) > -1) {
        nameIcon = IconExtension.txt;
    }
    else if ($.inArray(str, Extension.Email) > -1) {
        nameIcon = IconExtension.Email;
    } else if ($.inArray(str, Extension.Other) > -1) {
        nameIcon = IconExtension.Other;
    }
    else {
        nameIcon = IconExtension.Link;
    }

    var sPath = "";
    if (window.location.pathname.split("/").length > 3) {
        sPath = "../";
    }
    return '<img src="' + sPath+'Images/IconSystem/' + nameIcon + '.png">';
}


function CreateUlFile(lstFileUL) {
    var ul = "";

    ul = '<div class="fileuploader-items"><ul class="fileuploader-items-list">'
    $.each(lstFileUL, function () {
        ul += FormatFileLiUpload("" + this.sPath + this.sSystemFileName + "", "" + this.sFileName + "", "" + this.sSize + "", this.nID, false);
    })
    ul += '</ul></div>';
    return ul;
}
function DeleteFile(nID) {
    var funcYes = function () {
        Enumerable.From(SetUp.ArrFile).Where(function (w) { return w.nID == nID }).ToArray().forEach(function (f) {
            f.IsDelete = true;
            DeleteRow(nID);
            return f;
        });

        swal.close();
    }

    DialogConfirmDel(funcYes);
}
function GetTextValidate(Config) {
    var sDiv = "";
    sDiv += '<div class="form-group row col-12">';
    sDiv += '<div>';

    if (isEmpty(Config.Validate)) {

        if (Config.Extensions != null) {
            var sFileType = "";
            var i = 1;
            $.each(Config.Extensions, function () {
                //if (Config.Extensions.length != i) {
                //    sFileType += " / "+this;
                //}
                //else {
                //    sFileType += ""this;
                //}
                sFileType += " / " + this;
                i++;
            });
            sDiv += "<span class='text-red' id='spValidateFile_" + Config.sID + "'>File size limits up to " + Config.nFileSize + " MB. Allowed file types: " + sFileType + " </span><br/><span class='text-primary' id='spValidateFileOther_" + Config.sID + "'>" + (Config.sValidateETC != "" ? Config.sValidateETC : "") + "</span>";


        }
        else {
            sDiv += "<span class='text-red' id='spValidateFile_" + Config.sID + "'>File size limits up to " + Config.nFileSize + " MB. Allowed file types: /.gif /.jpeg / .jpg / .txt / .doc / .docx / .xls / .xlsx / .pdf / .pptx / .ppt / .mp4 / .rar / .zip / .msg </span><br/><span class='text-primary' id='spValidateFileOther_" + Config.sID + "'>" + (Config.sValidateETC != "" ? Config.sValidateETC : "") + "</span>";

        }
        sDiv += "<span class='text-red' id='spValidateLink_" + Config.sID + "' style='display:none;'>Example : http://www.google.com</span>";

    }
    else {
        sDiv += "<span class='text-red'>" + Config.Validate + "</span>";
    }
    sDiv += " </div>";
    sDiv += " </div>";
    return sDiv;
}

function isEmpty(str) {
    var isNull = (str === null);
    var isEmpty = !isNull ? (str === '' || str === undefined) : true;
    return isEmpty;
}