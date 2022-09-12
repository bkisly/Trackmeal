$(document).ready(function () {
    $("#products").on("click", ".js-delete", function () {
        let deleteButton = $(this);

        if (confirm("Are you sure you want to delete this item?")) {
            $.ajax({
                url: "/Manage/Products/Delete/" + deleteButton.attr("data-product-id"),
                method: "DELETE",
                success: function () {
                    deleteButton.parents("tr").remove();
                    if ($("#products").children("tbody").children("tr").length === 0)
                        window.location.reload();
                }
            });
        }
    });
});