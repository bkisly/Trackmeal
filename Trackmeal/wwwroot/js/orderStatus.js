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

function updateStateView(stateData) {
    $("#status-name").text(stateData.name);
    $("#prev-state-btn").attr("disabled", stateData.id <= 1);
    $("#next-state-btn").attr("disabled", stateData.id >= 4);
}