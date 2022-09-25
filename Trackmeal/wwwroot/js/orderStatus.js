$(document).ready(function() {
    $("#prev-state-btn").on("click",
        function () {
            $.ajax({
                url: `/api/order/prevstate/${$(this).parent().attr("data-order-id")}`,
                method: "PUT",
                success: function (data) {
                    updateStateView(data);
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