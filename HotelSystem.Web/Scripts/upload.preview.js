
$(function () {
    $(document.body).on("change",".upload-image", function () {
	    var target = $(this).next().find(".preview");
		target.html('');
		var reader = new FileReader();
		if (this.files) {
			$.each(this.files, function (index, value, array) {
				reader.readAsDataURL(value)
				var img = $("<img src='' style='height:100%;width:100%;' />");
				target.append(img);
				reader.onload = function (e) {
					img.attr("src", e.target.result);
				}
			});
		}
	});
})
