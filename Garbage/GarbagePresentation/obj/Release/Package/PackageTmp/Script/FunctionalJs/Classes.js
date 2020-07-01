

//Kendo Grid
    var KendoGrid = function () {
        var initialize = function () {
            var container = $('#ClassesKendo').html('<div></div>');
            var grid = $('div', container);
            var columnOptions;
            columnOptions = [
               { title: "Class Id", field: "ClassId", width: "70px", hidden: true },
               { title: "Class Title", field: "ClassName", width: "80%" },
               {
                   attributes: { "class": "center" }, headerAttributes: { "style": "text-align: center !important;" },
                   field: "ClassId", title: "Action", width: "20%", template: '<input class="btnedit" value="Edit" type="button" title="Edit" alt="Edit" onclick=edit(this) />'
               },
            ]
            var gridOptions = {
                dataSource: {
                    transport: {
                        read: {
                            url: '/Admin/SelectClass',
                            type: "GET",
                            dataType: "json",
                            data: {},
                            complete: function () {
                            }
                        }
                    },
                    schema: { data: "data", total: "total" },
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true
                },
                columns: columnOptions,
                height: "70vh",
                action: ["maximize"],
                resizable: true,
                scrollable: true,
                columnMenu: true,
                sortable: true,
                serverSorting: true,
                navigatable: true,
                selectable: "row",
                mobile: true,
                pageable: {
                    pageSize: 25,
                    pageSizes: [10, 25, 50, 100, 250],
                    refresh: true,
                }
            };
            grid.kendoGrid(gridOptions)
                .delegate("tbody>tr", "dblclick", function (e) {
                });
            grid.kendoGrid(gridOptions)
        };
        var refresh = function () {
            var grid = $('#ClassesKendo>div');
            if (grid.length < 1) {
                return;
            }
            grid.data("kendoGrid").dataSource.read();
        };
        return {
            Initialize: initialize,
            Refresh: refresh
        };
    }();

$(document).delegate('#ClassesKendo table tbody tr', 'mouseover', function () {
    $(this).css("cursor", "pointer");
});

$(document).ready(function () {
    KendoGrid.Initialize();
});

$("#showModel").click(function () {
    $('#New').show();
    $('#Save').show();
    $('#Edit').hide();
    $('#Update').hide();
    $("#ClassId").val('');
    $("#ClassName").val('');
});

$("#Save").click(function () {
    if(!validateForm()) {
        $("#divLoading").show();
        var dataobj = JSON.stringify({
            ClassName: $("#ClassName").val(),
        });
        $.ajax({
            url: "/Admin/InsertClass",
            type: "POST",
            data: dataobj,
            dataType: "json",
            traditional: true,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#ClassName").val('');
                KendoGrid.Initialize();
                $('#AddModal').modal('hide');
                $("#divLoading").hide();
                toastr.success('Class Inserted Successfully');

            },
            error: function () {
            }
        });
    }
    else
    {
        return false;
    }
});

function edit(e) {
    $("#divLoading").show();
    var tr = $(e).closest("tr");
    var value = $(tr).find("td:eq(0)").html();
    $.ajax({
        url: "/Admin/EditClass/",
        type: "GET",
        data: { ClassId: value },
        dataType: "json",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#divLoading").hide();
            $("#ClassId").val(data.ClassId);
            $("#ClassName").val(data.ClassName);
            $('#AddModal').modal('show');
            $('#New').hide();
            $('#Save').hide();
            $('#Edit').show();
            $('#Update').show();
        },
        error: function () {
        }
    });
}

$("#Update").click(function () {
    if (!validateForm()) {
        $("#divLoading").show();
        var dataobj = JSON.stringify({
            ClassId: $("#ClassId").val(),
            ClassName: $("#ClassName").val(),
        });
        $.ajax({
            url: "/Admin/UpdateClass",
            type: "POST",
            data: dataobj,
            dataType: "json",
            traditional: true,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                KendoGrid.Initialize();
                $('#AddModal').modal('hide');
                $("#divLoading").hide();
                toastr.success('Class Updated Successfully');

            },
            error: function () {
            }
        });
    }
    else {
        return false;
    }
});

function validateForm() {
    $("#ClassName").removeClass("errorClass");
    var isErrorOccurd = false;
    if ($.trim($("#ClassName").val()) == "") {
        $("#ClassName").addClass('errorClass');
        isErrorOccurd = true;
    }
    return isErrorOccurd;
}