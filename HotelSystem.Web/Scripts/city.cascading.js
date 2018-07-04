$(function () {
    $(document.body).on("change", ".province,.city", function () {
        var id = $(this).val();
        var target = $(this).data("target");
        if ($(this).hasClass("province")) {
            $(target).find(".city").html("");
            $($(target).find(".city").data("target")).find(".district").html("");
        }
        else {
            $(target).find(".district").html("");
        }
        var url = $(this).hasClass("province") ? "/Partial/City" : "/Partial/District";
        $.ajax({
            url: url,
            data: { id: id },
            type: "get",
            dataType: "text",
            success: function (data) {
                if (target) {
                    $(target).find("select").html($(data).html());
                }
            },
            error: function (e) {
                console.log(e);
            }
        });
    })
})