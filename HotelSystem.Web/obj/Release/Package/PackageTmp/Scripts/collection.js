$(document).ready(function () {
    $(".collection-btn").each(function () {
        var id = $(this).data("hotel");
        var c = $(this).data("collection");
        if (c) {
            $(this).addClass("disabled");
            $(this).text("已收藏该酒店");
        }
        else {
            $(this).on("click", function () {
                var t = $(this);
                var id = $(this).data("hotel");
                $.ajax({
                    url: "/User/AddCollection",
                    type: "post",
                    dateType: "json",
                    data: {
                        id: id
                    },
                    success: function (r) {
                        console.log(r);
                        if (r == "true") {
                            t.addClass("disabled");
                            t.text("已收藏该酒店");
                            alert("收藏成功")
                            window.location = window.location;
                        } else {
                            alert(r);
                        }

                    },
                    error: function (e) {
                        console.log(e);
                    }
                });
            });
        }
    });
   
});