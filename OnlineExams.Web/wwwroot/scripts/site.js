$(document).ready(function () {

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

});

$('.rolldown-list li').each(() => {
    let delay = ($(this).index() / 4) + 's';
    $(this).css({
        webkitAnimationDelay: delay,
        mozAnimationDelay: delay,
        animationDelay: delay
    });
});

$('#btnReload').click(function () {
    $('#myList').removeClass('rolldown-list');
    setTimeout(function () {
        $('#myList').addClass('rolldown-list');
    }, 1);
});