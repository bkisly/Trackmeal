$(document).ready(function() {
    const connection = new signalR.HubConnectionBuilder().withUrl("/statusHub").build();
    connection.start();

    connection.on("ReceiveNewStatus",
        function (orderId, newStatusId, newStatusName, newStatusDescription) {
            if (parseInt($("#order-id-input").attr("value")) === orderId) {
                $("#order-status-text").text(newStatusName);
                $("#order-status-description").text(newStatusDescription);
                $("#status-progressbar").attr("aria-valuenow", newStatusId);
                $("#status-progressbar").css("width", `${(newStatusId / 4) * 100}%`);
            }
        }
    );
});