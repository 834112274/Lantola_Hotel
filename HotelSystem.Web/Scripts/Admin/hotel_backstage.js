/*
<div class=""></div>
font-size:px;color:#;line-height:px;text-align:center;
background-image:url(../img/.png);background-position:center center;background-repeat:no-repeat;background-size:100% 100%;background-color:#fff;
*/
$(document).ready(function(){
	$('.kuang').height($(window).height());
	$(window).resize(function(){
		$('.kuang').height($(window).height());
	})
	$('.input input').focus(function(){
		$(this).parent().css({'border-color':'#4ebdf0','box-shadow':'0px 0px 5px #4ebdf0','z-index':'6'});
	})
	$('.input input').blur(function(){
		$(this).parent().css({'border-color':'#c9c9c9','box-shadow':'none','z-index':'5'});
	})
	$('.input input').keyup(function(){
		if($('.username input').val()!="" && $('.password input').val()!=""){$('.inputsubmit1').hide();}else{$('.inputsubmit1').show();}
	})
})