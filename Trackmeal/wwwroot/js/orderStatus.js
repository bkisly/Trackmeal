$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder().withUrl("/statusHub").build();
    connection.start();
    const orderId = parseInt($("#order-id-input").attr("value"));

    $("#prev-state-btn").on("click",
        function () {
            $.ajax({
                url: `/api/order/prevstate/${$(this).parent().attr("data-order-id")}`,
                method: "PUT",
                success: function (data) {
                    updateStateView(data);
                    sendStatus(connection, orderId, data);
                }
            });
        }
    );

    $("#next-state-btn").on("click",
        function () {
            $.ajax({
                url: `/api/order/nextstate/${$(this).parent().attr("data-order-id")}`,
                method: "PUT",
                success: function (data) {
                    updateStateView(data);
                    sendStatus(connection, orderId, data);
                }
            });
        }
    );
});

function updateStateView(orderStatus) {
    $("#status-name").text(orderStatus.name);
    $("#prev-state-btn").attr("disabled", orderStatus.id <= 1);
    $("#next-state-btn").attr("disabled", orderStatus.id >= 4);
}

function sendStatus(connection, orderId, orderStatus) {
    console.log(`order id: ${orderId}, status id: ${orderStatus.id}, status name: ${orderStatus.name}, status descr: ${orderStatus.description}`);
    connection.invoke("SendNewStatus", orderId, orderStatus.id, orderStatus.name, orderStatus.description).catch(function (err) {
        return console.error(err.toString());
    });
}