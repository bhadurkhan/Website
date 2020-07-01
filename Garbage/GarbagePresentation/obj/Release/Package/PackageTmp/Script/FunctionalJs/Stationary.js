
//Kendo Grid
    var KendoGrid = function () {
        var initialize = function () {
            var container = $('#KendoGrid').html('<div></div>');
            var grid = $('div', container);
            var columnOptions;
            columnOptions = [
               { title: "BookId", field: "BookId", width: "70px", hidden: true },
               { title: "Title", field: "BookTitle", width: "150px" },
               { title: "Description", field: "BookDescription", width: "200px" },
               { title: "Class Title", field: "ClassName", width: "150px" },
               { title: "Purchase Price", field: "BookPurchasePrice", width: "100px" },
               { title: "Sell Price", field: "BookSellPrice", width: "100px" },
               { title: "Quantity", field: "BookQuantity", width: "100px" },
               {
                   attributes: { "class": "center" }, headerAttributes: { "style": "text-align: center !important;" },
                   field: "ClassId", title: "Action", width: "100px", template: '<input class="btnedit" value="Edit" type="button" title="Edit" alt="Edit" onclick=edit(this) />'
               },
            ]
            var gridOptions = {
                dataSource: {
                    transport: {
                        read: {
                            url: '/Admin/SelectStationary',
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
            var grid = $('#KendoGrid>div');
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

$(document).delegate('#KendoGrid table tbody tr', 'mouseover', function () {
    $(this).css("cursor", "pointer");
});

$(document).ready(function () {
    KendoGrid.Initialize();
});

var ClassId = $("#ClassId").kendoDropDownList({
    filter: "contains",
    optionLabel: "Select Stationary Class",
    dataTextField: "ClassName",
    dataValueField: "ClassId",
    dataSource: {
        transport: {
            read: {
                url: "/Admin/SelectClassDropdown",
                dataType: "json",
                traditional: true,
                serverFiltering: false,
                complete: function () {
                }
            }
        }
    }
}).data("kendoDropDownList");


$("#showModel").click(function () {
    $('#New').show();
    $('#Save').show();
    $('#Edit').hide();
    $('#Update').hide();
    $("#BookId").val('');
    $("#BookTitle").val('');
    $("#BookDescription").val('');
    $("#BookPurchasePrice").val('');
    $("#BookSellPrice").val('');
    $("#BookQuantity").val('');
    ClassId.value(0);
});

$("#Save").click(function () {
    if(!validateForm()) {
        $("#divLoading").show();
        var dataobj = JSON.stringify({
            BookTitle: $("#BookTitle").val(),
            ClassId: $("#ClassId").val(),
            BookDescription: $("#BookDescription").val(),
            BookPurchasePrice: $("#BookPurchasePrice").val(),
            BookSellPrice: $("#BookSellPrice").val(),
            BookQuantity: $("#BookQuantity").val(),
        });
        $.ajax({
            url: "/Admin/InsertStationary",
            type: "POST",
            data: dataobj,
            dataType: "json",
            traditional: true,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                KendoGrid.Initialize();
                $('#AddModal').modal('hide');
                $("#divLoading").hide();
                toastr.success('Sationary Inserted Successfully');

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
        url: "/Admin/EditStationary/",
        type: "GET",
        data: { BookId: value },
        dataType: "json",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#BookId").val(data.BookId);
            $("#BookTitle").val(data.BookTitle);
            $("#BookDescription").val(data.BookDescription);
            $("#BookPurchasePrice").val(data.BookPurchasePrice);
            $("#BookSellPrice").val(data.BookSellPrice);
            $("#BookQuantity").val(data.BookQuantity);
            ClassId.value(data.ClassId);
            $("#divLoading").hide();
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
            BookId: $("#BookId").val(),
            BookTitle: $("#BookTitle").val(),
            ClassId: $("#ClassId").val(),
            BookDescription: $("#BookDescription").val(),
            BookPurchasePrice: $("#BookPurchasePrice").val(),
            BookSellPrice: $("#BookSellPrice").val(),
            BookQuantity: $("#BookQuantity").val(),
        });
        $.ajax({
            url: "/Admin/UpdateStationary",
            type: "POST",
            data: dataobj,
            dataType: "json",
            traditional: true,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                KendoGrid.Initialize();
                $('#AddModal').modal('hide');
                $("#divLoading").hide();
                toastr.success('Sationary Updated Successfully');

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
    $("#BookTitle").removeClass("errorClass");
    $("#ClassId1").removeClass("errorClass");
    $("#BookDescription").removeClass("errorClass");
    $("#BookPurchasePrice").removeClass("errorClass");
    $("#BookSellPrice").removeClass("errorClass");
    $("#BookQuantity").removeClass("errorClass");
    var isErrorOccurd = false;
    if ($.trim($("#ClassId").val()) == "") {
        $("#ClassId1").addClass('errorClass');
        isErrorOccurd = true;
    }

    if ($.trim($("#BookTitle").val()) == "") {
        $("#BookTitle").addClass('errorClass');
        isErrorOccurd = true;
    }
    if ($.trim($("#BookDescription").val()) == "") {
        $("#BookDescription").addClass('errorClass');
        isErrorOccurd = true;
    }
    if ($.trim($("#BookPurchasePrice").val()) == "") {
        $("#BookPurchasePrice").addClass('errorClass');
        isErrorOccurd = true;
    }
    if ($.trim($("#BookSellPrice").val()) == "") {
        $("#BookSellPrice").addClass('errorClass');
        isErrorOccurd = true;
    }
    if ($.trim($("#BookQuantity").val()) == "") {
        $("#BookQuantity").addClass('errorClass');
        isErrorOccurd = true;
    }
    return isErrorOccurd;
}