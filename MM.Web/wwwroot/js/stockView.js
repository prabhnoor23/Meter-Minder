var addStock = {
    calculateQty: function () {
        var fromVal = $(this).closest('tr').find('.from-input').val();
        var toVal = $(this).closest('tr').find('.to-input').val();
        if (fromVal && toVal) {
            var qtyVal = parseInt(toVal) - parseInt(fromVal) + 1;
            $(this).closest('tr').find('.qty-input').val(qtyVal);
        }
        else {
            $(this).closest('tr').find('.qty-input').val('');
        }
    },

    addRow: function () {
        var rowCount = $("#myTableBody tr").length;
        var rowCounter = rowCount + 1;
        var newRow = $('<tr>');
        var cols = '';
        cols += '<td><span class="required text-danger">*</span><input name="row_' + rowCounter + '_from" type="number" min="1" class="from-input" placeholder="From" required></td>';
        cols += '<td><span class="required text-danger">*</span><input name="row_' + rowCounter + '_to" type="number" min="1" class="to-input" placeholder="To" required></td>';
        cols += '<td><input name="row_' + rowCounter + '_qty" type="number" class="qty-input" placeholder="Quantity" readonly></td>';
        cols += '<td><button type="button" class="btn btn-danger remove-row"><i class="fas fa-minus"></i></button></td>';
        newRow.append(cols);
        newRow.attr('id', 'row_' + (rowCounter));
        $('tbody').append(newRow);

        if (rowCount > 0) {
            $("#myTableBody tr:last-child .remove-row").show();
        }
    }
    ,
    removeRow: function () {
        var rowCount = $("#myTableBody tr").length;
        if (rowCount > 1) {
            $(this).closest('tr').remove();
        }
    }
};

$(document).ready(() => {
    $(document).on('input', '.from-input, .to-input', addStock.calculateQty);
    $(document).on('click', '#addMaterialButton', addStock.addRow);
    $(document).on('click', '.remove-row', addStock.removeRow);
    $('tbody tr:first-child .remove-row').hide();
    var pageType = '';
    var currentPageUrl = window.location.href;
    if (currentPageUrl.includes('StockOutReport')) {
        pageType = 'stockOut';
    } else if (currentPageUrl.includes('StockInReport')) {
        pageType = 'stockIn';
    } else if (currentPageUrl.includes('AvailableStock')) {
        pageType = 'availableStocks';
    }
    CustomPagination(pageType);
});
function CustomPagination(pageType) {
    $('#paginate').pagination({
        items: 100,
        itemsOnPage: 10,
        cssStyle: 'light-theme'
    });
    var tableSelector = '';
    if (pageType === 'stockIn') {
        tableSelector = "#stockInReportTable";
    } else if (pageType === 'stockOut') {
        tableSelector = "#stockOutReportTable";
    } else {
        tableSelector = "#availableStockTable";
    }
    var items = $(tableSelector + " tbody tr");
    var numItems = items.length;
    var perPage = 10;

    items.slice(perPage).hide();

    $('#paginate').pagination({
        items: numItems,
        itemsOnPage: perPage,
        prevText: "&laquo;",
        nextText: "&raquo;",
        onPageClick: function (pageNumber) {
            var showFrom = perPage * (pageNumber - 1);
            var showTo = showFrom + perPage;
            items.hide().slice(showFrom, showTo).show();
        }
    });
}


$(function () {
    $("#materialGroupId").on("change", function () {
        $('#issueMaterial').hide();
        $("#Cost").val('0');
        makesAndUnits = {};

        var materialGroupId = $(this).val();
        $("#materialTypeId").empty();
        $("#materialId").empty().append($('<option>').text("--Select Material Code--").val(""));;
        $("#makeId").empty();
        $("#ratingId").empty().append($('<option>').text("--Select Rating--").val(""));
        if (materialGroupId) {
            $.ajax({
                url: "/StockView/getMaterialTypes",
                type: "GET",
                data: { materialGroupId: materialGroupId },
                success: function (result) {
                    $("#materialTypeId").append($('<option> ').text("--Select Material Type--").val(""));
                    $.each(result, function (i, response) {
                        $("#materialTypeId").append($('<option>').text(response.text).val(response.value));
                    });
                }
            });
        }
        else {
            $("#materialTypeId").append($('<option>').text("--Select Material Type--").val(""));
        }
    });
});
$(function () {
    $("#materialTypeId").on("change", function () {
        var materialTypeId = $(this).val();
        $("#makeId").empty();
        $("#ratingId").empty();
        if (materialTypeId) {
            $.ajax({
                url: "/StockView/GetRating",
                type: "GET",
                data: { materialTypeId: materialTypeId },
                success: function (result) {
                    $("#ratingId").append($('<option>').text("--Select Rating--").val(""));
                    $.each(result, function (i, response) {
                        if (response.text == null) {
                            $("#ratingId").append($('<option>').text("None").val(response.value));
                        } else {
                            $("#ratingId").append($('<option>').text(response.text).val(response.value));
                        }
                    });
                }
            });
        }
        else {
            $("#ratingId").append($('<option>').text("--Select Rating--").val(""));
        }
    });
});

$(function () {
    $("#materialTypeId").on("change", function () {
        $('#issueMaterial').hide();
        $("#Cost").val('0');

        makesAndUnits = {};

        var materialTypeId = $(this).val();
        $("#materialId").empty();
        $("#makeId").empty();
        if (materialTypeId) {
            $.ajax({
                url: "/StockView/getMaterialCodes",
                type: "GET",
                data: { materialTypeId: materialTypeId },
                success: function (result) {
                    $("#materialId").append($('<option>').text("--Select Material Code--").val(""));
                    $.each(result, function (i, response) {
                        if (response.text == null) {
                            $("#materialId").append($('<option>').text("None").val(response.value));
                        } else {
                            $("#materialId").append($('<option>').text(response.text).val(response.value));
                        }
                    });
                }
            });
        }
        else {
            $("#materialId").append($('<option>').text("--Select Material Code--").val(""));
        }
    });
});

$(function () {
    $("#SelectedSubDivId").on("change", function () {
        var selectedSubDivId = $(this)[0].selectedIndex;
        $("#Division").val("");
        $("#Circle").val("");
        $("#LocationCode").val("");
        $("#error-message").remove();

        if (selectedSubDivId) {
            $.ajax({
                url: "/IssueStock/GetCircleAndDivisionAndLocationCode",
                type: "GET",
                data: { SelectedSubDivId: selectedSubDivId },
                success: function (result) {

                    $("#Division").val(result[0]);
                    $("#DivisionId").val(result[2]);
                    $("#Circle").val(result[1]);
                    $("#CircleId").val(result[3]);
                    $("#LocationCode").val(result[4]);
                },
                error: function (xhr, status, error) {
                    $("#mainModalContent").text("An error occurred while fetching data. Please try again later.")
                    $('#stockNotAvailableModal').modal('show');
                    console.log("AJAX request failed. Status: " + status + ", Error: " + error);
                }
            });
        }
    });
});


var alertMessage = '';
function validateInputs() {
    var isValidMsg = "";
    var listOfSerialNumber = [];
    $('.to-input').each(function () {
        var $this = $(this);
        var $row = $this.closest('tr');

        var fromVal = $row.find('.from-input').val();
        var toVal = $this.val();

        for (i = parseInt(fromVal); i <= toVal; i++) {
            listOfSerialNumber.push(i);
        }

        if (fromVal && toVal && parseInt(fromVal) > parseInt(toVal)) {
            isValidMsg = "qtynegative";
            $this.addClass('is-invalid');
        }
        else {
            $this.removeClass('is-invalid');
        }

    });

    if (listOfSerialNumber.length != Array.from(new Set(listOfSerialNumber)).length) {

        isValidMsg = "duplicatesrno";
    }
    return isValidMsg;
}

function validateSerialNumbers(listOfSerialNumber) {
    var isValid = true;

    var materialGroupId = parseInt($("#materialGroupId").val());
    var materialTypeId = parseInt($("#materialTypeId").val());
    var materialId = parseInt($("#materialId").val());
    var make = $("#Make").val();

    $.ajax({

        url: "/StockView/serverSideSerialNumberValidation",
        type: "POST",
        data: {
            listOfSerialNumber: listOfSerialNumber,
            materialGroupId: materialGroupId,
            materialTypeId: materialTypeId,
            materialId: materialId,
            make: make
        },
        success: function (result) {
            var isPresent = result;
            console.log(result);
            if (isPresent) {
                isvalid = false;
            }
            else {
                isvalid = true;
            }
        },
        error: function (xhr, status, error) {
            // Handle the error
        }
    });

    return isValid;
}
$(function () {
    $("#materialId").on("change", function () {

        $("#Cost").val('');

        var materialGroupId = $("#materialGroupId").val();
        var materialTypeId = $("#materialTypeId").val();
        var materialId = $(this).val();
        $("#AvailableStock").val('');

        if (materialId) {
            $.ajax({
                url: "/IssueStock/DisplayMakeWithQuantity",
                type: "GET",
                data: { materialGroupId: materialGroupId, materialTypeId: materialTypeId, materialId: materialId },
                success: function (response) {
                    $('#issueMaterialTableBody').empty();

                    var keys = Object.keys(response);
                    if (keys.length > 0) {

                        $('#issueMaterial').show();

                        for (var i = 0; i < keys.length; i++) {
                            var key = keys[i];
                            var rowCounter = i + 1;
                            var value = response[key];
                            var rowHtml = '<tr>';
                            rowHtml += '<td><input type="text" class="Make_reqQty MakeClass" name="row_' + rowCounter + '_make" id="row_' + rowCounter + '_Make" value="' + key + '" readonly/></td>';
                            rowHtml += '<td><input type="number" class="Make_reqQty AvailableQtyClass" name="row_' + rowCounter + '_availQty" value="' + value + '" readonly/></td>';
                            rowHtml += '<td><span class="required text-danger">*</span><input type="number" min="0" class="Make_reqQty" name="row_' + rowCounter + '_reqAty" id="row_' + key + '_ReqQty" oninput="handleRequiredQuantity(event)" required /></td>';
                            rowHtml += '</tr>';
                            $('#issueMaterialTableBody').append(rowHtml);
                        }
                    }
                    else {
                        // Hide the table if the response is empty
                        $('#issueMaterial').hide();
                        $("#mainModalContent").text("Stock not available!")

                        $('#stockNotAvailableModal').modal('show');

                    }
                },
                error: function (xhr, status, error) {
                    $("#mainModalContent").text("An error occurred while fetching data. Please try again later.")
                    $('#stockNotAvailableModal').modal('show');
                    console.log("AJAX request failed. Status: " + status + ", Error: " + error);
                }
            });
        }
    });
});

$(document).ready(function () {
    showModal('', '');
    $('#stockNotAvailableModal').hide();

});
function showModal(alertMessage, status) {
    var successMessage = $("#successMessage").val();
    if (successMessage) {
        $("#staticBackdropLiveLabel").text('Success');
        $("#successMessagePlaceholder").text(successMessage);
        $('#staticBackdropLive').modal('show');
    }

    if (alertMessage) {
        $("#successMessagePlaceholder").text(alertMessage);
        $("#staticBackdropLiveLabel").text(status);
        $("#staticBackdropLive").modal("show");
    }

}
$(document).on('click', "#saveStock", function (event) {
    var $submitButton = $(this);
    var $form = $submitButton.closest('form');

    // Show the loading indicator
    $('#loadingIndicator').show();

    $form.submit();
});

$(document).on('submit', "#UserForm", function (event) {
    event.preventDefault();
    var userForm = document.getElementById('UserForm');

    $('#loadingIndicator').show();
    userForm.submit();
});

$(document).on('submit', "#IssueStockForm1", function (event) {
    event.preventDefault();
    var issueForm = document.getElementById('IssueStockForm1');

    // Show the loading indicator
    $('#loadingIndicator').show();
    issueForm.submit();
});

$('#checkBoxAll').click(function () {
    if ($(this).is(":checked")) {
        $(".eachStockRow").prop("checked", true)
    }
    else {
        $(".eachStockRow").prop("checked", false)
    }
});

$("#deleteStockBtn").on("click", function () {
    var checkedRows = $('.eachStockRow:checked');
    if (checkedRows.length === 0) {
        $("#mainModalContent").text('No record selected!');
        $("#mySmallModalLabel").text('ERROR..!');
        $("#stockNotAvailableModal").modal("show");
    } else {
        $('#confirmationModal').modal('show');
    }
});

$("#retrieveRowsBtn").on("click", function () {
    if ($('.eachStockRow').is(":checked")) {
        var selectedRows = [];

        $('.eachStockRow').each(function () {
            if ($(this).is(':checked')) {
                var row = $(this).closest('tr');
                var rowData = {
                    StockMaterialId: row.find('td:eq(11)').text(),
                    SrNoFrom: row.find('td:eq(7)').text(),
                    SrNoTo: row.find('td:eq(8)').text(),
                    Quantity: row.find('td:eq(9)').text()
                };

                selectedRows.push(rowData);
            }
        });

        $.ajax({
            url: "/DeleteStock/StockToDelete",
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(selectedRows),
            success: function (response) {
                $('.eachStockRow:checked').each(function () {
                    $(this).closest('tr').remove();
                    $("#mySmallModalLabel").text('SUCCESS!');
                    $("#mainModalContent").text("Stock deleted successfully.")
                    $('#stockNotAvailableModal').modal('show');
                });
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#mainModalContent").text("An error occurred while fetching data. Please try again later.")
                $('#stockNotAvailableModal').modal('show');
            }
        });
        $('#confirmationModal').modal('hide');
    }
    else {
        $("#mainModalContent").text('No rows selected!');
        $("#stockNotAvailableModal").modal("show");
    }
});

$(document).on('submit', '#StockForm', function (event) {

    event.preventDefault();
    var userEnteredRate = $("#Rate").val();

    var response = validateInputs();
    if (response == "qtynegative") {
        alertMessage = 'Quantity cannot be zero or negative';
        showModal(alertMessage, 'Error..!');
    } else if (response == "duplicatesrno") {
        alertMessage = 'Duplicate serial numbers entered. Please enter unique serial numbers.';
        showModal(alertMessage, 'Error..!');
    }
    else if (userEnteredRate > 1000000) {
        alertMessage = 'Rate cannot exceed Rs 10,00,000';
        showModal(alertMessage, 'Error..!');
    }
    else if (userEnteredRate < 0) {
        $('.invalidEnteredRate').text('Please enter valid rate..!')
    }
    else {
        this.submit();
    }

});

document.getElementById("exportButton").addEventListener("click", function () {
    var table = document.querySelector(".table");
    var clonedTable = table.cloneNode(true);
    var rows = clonedTable.querySelectorAll("tbody tr");
    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        row.style.display = "";
    }
    var wb = XLSX.utils.table_to_book(clonedTable, { sheet: "Sheet 1" });
    var wbout = XLSX.write(wb, { bookType: "xlsx", type: "array" });
    var blob = new Blob([wbout], { type: "application/octet-stream" });
    var downloadLink = document.createElement("a");
    var url = URL.createObjectURL(blob);
    downloadLink.href = url;
    downloadLink.download = "report.xlsx";
    downloadLink.click();
    setTimeout(function () {
        URL.revokeObjectURL(url);
    }, 0);
});

function getCorrespondingMakeValue(invoiceNumber) {
    $.ajax({

        url: "/StockView/GetCorrespondingMakeValue",
        type: "GET",
        data: { invoiceNumber: invoiceNumber },
        success: function (result) {


            if (result != "Enter Make") {
                $('#Make').val(result);
                $('#Make').prop('readonly', true);
            }

            else {
                $('#Make').val('');
                $('#Make').prop('readonly', false);
            }
        },
        error: function (xhr, status, error) {
            $("#mainModalContent").text("An error occurred while fetching data. Please try again later.")
            $('#stockNotAvailableModal').modal('show');
            console.log("AJAX request failed. Status: " + status + ", Error: " + error);
        }
    });
}
function GrnValidation(GrnNumber) {
    $.ajax({

        url: "/StockView/isGrnNumberExist",
        type: "GET",
        data: { GrnNumber: GrnNumber },
        success: function (result) {
            console.log(result);
            if (result) {
                $('#GrnNumber').text('Entered GRN already exists..!')
                $('#GRNfield').val(null);
            }
            else {
                $('#GrnNumber').text('')
            }
        },
        error: function (xhr, status, error) {
            // Handle the error
        }
    });


}
function validateDates() {
    var invoiceDate = document.getElementById("invoiceDate").value;
    var grnDate = document.getElementById("grnDate").value;

    if (invoiceDate === "" && grnDate !== "") {
        displayModal("Please enter the Invoice Date first.");
        document.getElementById("grnDate").value = "";
        return;
    }

    if (invoiceDate !== "" && grnDate !== "" && new Date(grnDate) < new Date(invoiceDate)) {
        displayModal("GRN Date must be greater than or equal to Invoice Date.");
        document.getElementById("grnDate").value = "";
    }
}

function displayModal(message) {
    var modalErrorMessage = document.getElementById("modalErrorMessage");
    modalErrorMessage.innerText = message;

    var validationModal = new bootstrap.Modal(document.getElementById("validationModal"));
    validationModal.show();
}

function ClearGrnDate() {
    document.getElementById("grnDate").value = "";
}

var makesAndUnits = {};

$(document).ready(function () {
    makesAndUnits = {};
});

function handleRequiredQuantity(event) {
    var materialGroupId = $("#materialGroupId").val();
    var materialTypeId = $("#materialTypeId").val();
    var materialId = $("#materialId").val();
    //$("#Cost").val('');

    var units = event.target.value;

    var $input = $(event.target);
    var $row = $input.closest('tr');
    var make = $row.find('.MakeClass').val();
    var availableQty = $row.find('.AvailableQtyClass').val();

    var localMakesAndUnits = Object.assign({}, makesAndUnits);

    if (parseInt(availableQty) < parseInt(units)) {

        $('#stockNotAvailableModal').modal('show');

        // $("#Cost").val('0');
        $input.val('0');
        units = 0;

        if (make in localMakesAndUnits) {

            delete localMakesAndUnits[make];
            delete makesAndUnits[make];

            updateCost(localMakesAndUnits, make, units, materialGroupId, materialTypeId, materialId);
        }
    }

    else {
        if (!units) {
            units = 0;
        }
        else if (parseInt(units) < 0) {
            // $("#Cost").val('');
            $input.val('0');
            units = 0;
        }
        if (make in localMakesAndUnits) {
            delete localMakesAndUnits[make];
            delete makesAndUnits[make];
        }
        updateCost(localMakesAndUnits, make, units, materialGroupId, materialTypeId, materialId);
    }
}

function updateCost(localMakesAndUnits, make, units, materialGroupId, materialTypeId, materialId) {

    console.log(units);
    localMakesAndUnits[make] = units;
    makesAndUnits = Object.assign({}, localMakesAndUnits);
    var noOfUnits = 0;

    for (var key in makesAndUnits) {
        if (makesAndUnits.hasOwnProperty(key)) {
            var value = parseInt(makesAndUnits[key]);
            noOfUnits += value;
        }
    }

    $.ajax({
        url: "/IssueStock/GetCost",
        type: "GET",
        data: { materialId: materialId, noOfUnits: noOfUnits },
        success: function (response) {
            var cost = parseFloat(response); // Parse the response as float
            $('#Cost').val(cost);
        }
    });
}
function displayErrorModal(message, heading) {
    var modalErrorMessage = document.getElementById("modalErrorMessage");
    var modalTitle = document.getElementById("validationModalLabel");
    modalErrorMessage.innerText = message;
    modalTitle.innerText = heading;

    var validationModal = new bootstrap.Modal(document.getElementById("validationModal"));
    validationModal.show();
}


function clearTable() {
    setTimeout(() => {
        var currentDateField = document.getElementById('currentDate');
        var currentDate = new Date().toLocaleDateString();
        currentDateField.value = currentDate;
        $('#issueMaterial').hide();
    }, 100);

}

function FilterRecordsWithGrnDate(reportType) {
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();

    if (fromDate === '' || toDate === '') {
        displayModal("Please select 'From' and 'To' date.", "Missing Date Range");
        $('#fromDate').val('');
        $('#toDate').val('');
    }
    else if (new Date(toDate) < new Date(fromDate)) {
        displayErrorModal("'To GRN Date' must be greater than the 'From Date'.", "Invalid Date Range");
    }

    else {
        var url = '';
        if (reportType == 'stockIn') {
            url = "/Report/FilteredStockInReport";
        } else if (reportType == 'availableStock') {
            url = "/Report/FilteredAvailableStockReport";
        }
        else {
            url = "/Report/FilteredStockOutReport";
        }
        $.ajax({
            url: url,
            type: "GET",
            data: { fromDate: fromDate, toDate: toDate },
            success: function (result) {
                var tableBody = '';
                if (reportType == 'stockIn') {
                    tableBody = $('#stockInReportTable tbody');
                } else if (reportType == 'availableStock') {
                    tableBody = $('#availableStockTable tbody');
                }
                else {
                    tableBody = $('#stockOutReportTable tbody');
                }
                if (result.length === 0) {
                    displayErrorModal("No records found for the selected dates.", "Error");
                    $('#fromDate').val('');
                    $('#toDate').val('');
                }
                else {
                    tableBody.empty();
                    result.forEach(function (stockModel) {
                        var row = '';
                        if (reportType == 'stockIn') {
                            row = '<tr>' +
                                '<td>' + stockModel.stock.id + '</td>' +
                                '<td>' + new Date(stockModel.stock.grnDate).toLocaleDateString() + '</td>' +
                                '<td>' + stockModel.stock.grnNumber + '</td>' +
                                '<td>' + new Date(stockModel.stock.invoiceDate).toLocaleDateString() + '</td>' +
                                '<td>' + stockModel.stock.invoiceNumber + '</td>' +
                                '<td>' + stockModel.stock.prefixNumber + '</td>' +
                                '<td>' + stockModel.stock.make + '</td>' +
                                '<td>' + stockModel.stock.testReportReference + '</td>' +
                                '<td>' + stockModel.materialName + '</td>' +
                                '<td>' + stockModel.materialCode + '</td>' +
                                '<td>' + stockModel.stock.rate + '</td>' +
                                '<td>' + stockModel.quantity + '</td>' +
                                '</tr>';
                        } else if (reportType == 'availableStock') {
                            row = '<tr>' +
                                '<td>' + stockModel.grnNo + '</td>' +
                                '<td>' + new Date(stockModel.grnDate).toLocaleDateString() + '</td>' +
                                '<td>' + stockModel.materialGroup + '</td>' +
                                '<td>' + stockModel.materialName + '</td>' +
                                '<td>' + stockModel.materialCode + '</td>' +
                                '<td>' + stockModel.make + '</td>' +
                                '<td>' + stockModel.srNoFrom + '</td>' +
                                '<td>' + stockModel.srNoTo + '</td>' +
                                '<td>' + stockModel.availableQuantity + '</td>' +
                                '<td>' + stockModel.rate + '</td>' +
                                '<td>' + stockModel.value + '</td>' +
                                '</tr>';
                        }
                        else {
                            row = '<tr>' +
                                '<td>' + stockModel.transactionId + '</td>' +
                                '<td>' + new Date(stockModel.currentDate).toLocaleDateString() + '</td>' +
                                '<td>' + stockModel.serialNumber + '</td>' +
                                '<td>' + stockModel.srControlNumber + '</td>' +
                                '<td>' + new Date(stockModel.srNoDate).toLocaleDateString() + '</td>' +
                                '<td>' + stockModel.subDivisionName + '</td>' +
                                '<td>' + stockModel.locationID + '</td>' +
                                '<td>' + stockModel.subDivisionName + '</td>' +
                                '<td>' + stockModel.juniorEngineerName + '</td>' +
                                '<td>' + stockModel.materialName + '</td>' +
                                '<td>' + stockModel.materialCode + '</td>' +
                                '<td>' + stockModel.quantity + '</td>' +
                                '<td>' + stockModel.rate + '</td>' +
                                '<td>' + stockModel.make + '</td>' +
                                '<td>' + stockModel.cost + '</td>';

                            if (stockModel.imageName != null) {
                                row += `<td><button class="btn btn-link download-button" onclick="downloadImage('${stockModel.imageName}')"><i class="fa fa-download "></i> Download</button></td> </tr>`;
                            }
                            else {
                                row += '<td>no image uploaded</td></tr>';
                            }
                        }
                        tableBody.append(row);
                        CustomPagination(reportType);

                    });
                }
            },
            error: function (xhr, status, error) {
                displayErrorModal("An error occurred while fetching data. Please try again later", "Error");
            }
        });
    }
}


$('#filterStockInButton').click(function () {
    FilterRecordsWithGrnDate('stockIn');
});

$('#filterStockOutButton').click(function () {
    FilterRecordsWithGrnDate('stockOut');
});

$('#filterAvailableStockButton').click(function () {
    FilterRecordsWithGrnDate('availableStock');
});

function downloadImage(filename) {
    window.location.href = "/Report/DownloadImage?filename=" + encodeURIComponent(filename);
}
function printPage() {
    var printButton = document.getElementsByClassName('btn-dark')[0];
    printButton.style.display = 'none';

    // Print the page
    window.print();

    // Show the print button after printing
    printButton.style.display = 'block';
}


function validateImageFile(input) {
    var file = input.files[0];
    var allowedExtensions = ["jpg", "jpeg", "png", "gif", "bmp"];
    var fileExtension = file.name.split('.').pop().toLowerCase();

    if (allowedExtensions.indexOf(fileExtension) === -1) {
        input.value = ""; // Clear the selected file
        document.getElementById("fileValidationMessage").textContent = "Only image files with extensions: " + allowedExtensions.join(", ") + " are allowed.";
        document.getElementById("fileValidationMessage").style.color = 'red';
    } else {
        document.getElementById("fileValidationMessage").textContent = "";
    }
}