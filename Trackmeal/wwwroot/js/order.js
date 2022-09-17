$(document).ready(function () {
    $("#products").on("click",
        ".js-add",
        function () {
            const button = $(this);
            $.ajax({
                url: `/Order/AddProduct/${button.attr("data-product-id")}`,
                method: "POST",
                success: function () {
                    updateDom(button.parent().parent());
                }
            });
        }
    );

    $("#products").on("click",
        ".js-delete",
        function () {
            const button = $(this);
            $.ajax({
                url: `/Order/RemoveProduct/${button.attr("data-product-id")}`,
                method: "DELETE",
                success: function () {
                    updateDom(button.parent().parent(), true);
                }
            });
        }
    );
});

function updateDom(container, isDelete = false) {
    const addButton = container.find(".js-add");
    const deleteButton = container.find(".js-delete");
    const amountParagraph = container.find(".js-amount");
    let newVal;

    amountParagraph.text(function (index, prevVal) {
        const prevValInt = parseInt(prevVal);
        newVal = isDelete ? prevValInt - 1 : prevValInt + 1;
        return newVal;
    });

    if (newVal === 0) {
        deleteButton.attr("disabled", true);
    } else if (newVal === 100) {
        addButton.attr("disabled", true);
    } else {
        addButton.attr("disabled", false);
        deleteButton.attr("disabled", false);
    }
}