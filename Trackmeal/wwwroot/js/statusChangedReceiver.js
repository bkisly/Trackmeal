$(document).ready(function() {
    const connection = new signalR.HubConnectionBuilder().withUrl("/statusHub").build();
    connection.start();

    connection.on("ReceiveNewStatus",
        function (orderId, newStatusId, newStatusName) {
            if (parseInt($("#order-id-input").attr("value")) === orderId) {
                $("#order-status-text").text(newStatusName);
                $("#order-status-id").text(newStatusId);
            }
        }
    );
});