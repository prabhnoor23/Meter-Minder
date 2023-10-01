var path = window.location.pathname;

// Add active class to the corresponding menu item
$('.menu-item').each(function () {
    if ($(this).attr('href') === path) {
        $(this).addClass('mm-active');
    }
});

// Add active class to the corresponding submenu item
$('.submenu-item').each(function () {
    if ($(this).attr('href') === path) {
        $(this).addClass('mm-active');
        $(this).parents('.collapse').addClass('show');
        $(this).parents('.li-item').addClass('mm-active');
    }
});

// Menu fixed
$(window).scroll(function(){
  if ($(window).scrollTop() ) {
	$('body').addClass('header-sticky');
   }
   else {
	$('body').removeClass('header-sticky');
   }
});