$(document).ready(function () {
    let addReady = true;
    let deleteReady = true;

    $("#products").on("click",
        ".js-add",
        function () {
            if (addReady) {
                addReady = false;
                const button = $(this);

                $.ajax({
                    url: `/api/cart/${button.attr("data-product-id")}`,
                    method: "POST",
                    success: function () {
                        updateDom(button.parent().parent());
                        addReady = true;
                    }
                });
            }
        }
    );

    $("#products").on("click",
        ".js-delete",
        function () {
            if (deleteReady) {
                deleteReady = false;
                const button = $(this);

                $.ajax({
                    url: `/api/cart/${button.attr("data-product-id")}`,
                    method: "DELETE",
                    success: function () {
                        updateDom(button.parent().parent(), true);
                        deleteReady = true;
                    }
                });
            }
        }
    );
});

function updateDom(container, isDelete = false) {
    const addButton = container.find(".js-add");
    const deleteButton = container.find(".js-delete");
    let newVal;

    container.find(".js-amount").text(function (index, prevVal) {
        const prevValInt = parseInt(prevVal);
        newVal = isDelete ? prevValInt - 1 : prevValInt + 1;
        return newVal;
    });

    addButton.attr("disabled", newVal === 100);
    deleteButton.attr("disabled", newVal === 0);
}
