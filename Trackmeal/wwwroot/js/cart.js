$(document).ready(function () {
    $("#clear-btn").on("click",
        function () {
            if (confirm("Are you sure you want to clear the cart?")) {
                $.ajax({
                    url: "/Order/ClearCart",
                    method: "DELETE",
                    success: function () {
                        window.location.reload();
                    }
                });
            }
        }
    );
});