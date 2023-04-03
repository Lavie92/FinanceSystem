//$('.btnedit').click(function () {
//    var transactionId = $(this).data('transactionid');
//    editItem(transactionId);
//});
//$(document).ready(function () {
//    $('body').on('click', '.btnedit', function () {
//        var transactionId = $(this).data('transactionid');
//        var item = $(this);
//        if (confirm("Bạn có muốn Edit này?")) {
//    $.ajax({
//        url: '/Transactions/Edit/' + TransactionId,
//        type: 'POST',
//        dataType: 'json',
//        data: { id: transactionId },
//        success: function () {
//            // Xử lý dữ liệu trả về
//            alert('Có ádasd');
//        },
//        error: function () {
//            alert('Có lỗi xảy ra');
//        }
//    });
//    }
//});

//function editItem(elem) {
//    // Get the transaction ID from the data-attribute
//    var transactionId = $(elem).data("transactionid");

//    // Send an Ajax request to get the edit form
//    $.ajax({
//        url: '/Transactions/Edit/' + transactionId,
//        type: "GET",
//        data: { id: transactionId },
//        success: function (result) {
//            // Display the edit form in the modal
//            $('#editModal .modal-body').html(result);
//            $('#editModal').modal('show');
//        },
//        error: function () {
//            alert('Error occurred while loading data.');
//        }
//    });
//}

function editItem(elem) {
    // Get the transaction ID from the data-attribute
    var transactionId = $(elem).data("transactionid");
    alert('vô dây rồi nè');
    // Send an Ajax request to get the edit form
    $.ajax({
        url: '/Transactions/Edit/' + transactionId,
        type: "GET",

        success: function (result) {
            // Display the edit form in the modal
            $('#editModal .modal-body').html(result);
            $('#editModal').modal('show');
        },
        error: function () {
            alert('Error occurred while loading data.');
        }
    });
}
