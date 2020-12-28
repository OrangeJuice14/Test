$.extend({
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    getUrlVar: function (name) {
        return $.getUrlVars()[name];
    },
    getIds: function (list) {
        var ids = [];
        for (var i = 0; i < list.length; i++) {
            ids.push(list[i].Id);
        }
        return ids;
    },
    Redirect: function (url) {
        if (url) {
            var link = url.toString().replace("~", String.format("http://{0}", window.location.host));
            window.location.assign(link);
        } else
            window.location.reload();
    },    
    NewGuid: function () {        
        var guid = "";
        while (true) {
            $.ajax({
                url: '/Helper/NewGuid',
                success: function (data) {
                    guid = data;
                },
                async: false
            });
            if(guid!="")
                break;
        }        
        return guid;
    },
    post: function(params) {
        $.ajax({
            type: 'POST',
            url: params.url,
            async: false,
            traditional: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(params.data),
            success: params.success,
            error: params.error
        });
    },
    callApi: function (params) {
        $.ajax({
            type: params.type,
            url: params.url,
            async: false,
            traditional: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ko.toJSON(params.data),
            success: params.success,
            error: params.error
        });
    },
    //get: function(params) {
    //    $.ajax({
    //        type: 'GET',
    //        url: params.url,
    //        dataType: "json",
    //        async: false,
    //        success: params.success,
    //        error: params.error
    //    });
    //},
    pageFilter: function pagerFilter(data) {
        if (typeof data.length == 'number' && typeof data.splice == 'function') {    
            data = {
                total: data.length,
                rows: data
            };
        }
        var dg = $(this);
        var opts = dg.datagrid('options');
        var pager = dg.datagrid('getPager');
        pager.pagination({
            onSelectPage: function (pageNum, pageSize) {
                opts.pageNumber = pageNum;
                opts.pageSize = pageSize;
                pager.pagination('refresh', {
                    pageNumber: pageNum,
                    pageSize: pageSize
                });
                dg.datagrid('loadData', data);
            }
        });
        if (!data.originalRows) {
            data.originalRows = (data.rows);
        }
        var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
        var end = start + parseInt(opts.pageSize);
        data.rows = (data.originalRows.slice(start, end));
        return data;
    },
    showDialog: function (dataUrl, viewUrl, dialog, dataHandler, beforeHandler) {
        $.get(dataUrl, function(data) {
            dataHandler(data);
            $.get(viewUrl, function(view) {
                var node = dialog.find(".dialog-content");
                node.html(view);
                ko.applyBindings(data, node[0]);
                if(beforeHandler)
                    beforeHandler(data);
                dialog.dialog('open');
            });
        });       
    },
    showDialogView: function (viewUrl, dialog, beforeHandler) {
        $.get(viewUrl, function (view) {
            var node = dialog.find(".dialog-content");
            node.html(view);
            if(beforeHandler)
                beforeHandler(view);
            dialog.dialog('open');
        });
    },
    IdsToArray: function (objs) {
        var array = [];
        $(objs).each(function(idx, obj) {
            array.push(obj.Id);
        });
        return array;
    },
    ArrayToIds: function (array) {
        var objs = [];
        $(array).each(function(idx, item) {
            objs.push({ Id: item });
        });
        return objs;
    },
    GetNameFromList: function(id, list) {
        for (var i = 0; i < list.length; i++) {
            if (list[i].Id == id)
                return list[i].Name;
        }
    },
    IsFormAvailable: function (id) {
        var flag = false;
        $.ajax({
            url: String.format('/{0}/{1}', "Permission", "GetFunctionsByRoleId"),
            async: false,
            success: function (data) {
                $(data).each(function (idx, obj) {
                    if (obj == id)
                        flag = true;
                });
            }
        });
        return flag;
    }
});