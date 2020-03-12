$('.rolldown-list li').each(() => {
    let delay = ($(this).index() / 4) + 's';
    $(this).css({
        webkitAnimationDelay: delay,
        mozAnimationDelay: delay,
        animationDelay: delay
    });
});	

function giveAnswer(questionId, ansewrId) {

    let ul = document.getElementById(`question_${questionId}`);
    let lis = ul.children;

    for (let i = 0; i < lis.length; i++) {
        lis[i].style.background = "white";
    }

    let li = document.getElementById(`question_${questionId}_answer_${ansewrId}`);
    li.style.background = 'lightyellow';

    DotNet.invokeMethodAsync("OnlineExamer.Web", "SelectAnswer", questionId, ansewrId)
        .then(result => console.log(result));
}

$('document').ready(function () {
	$('input[type="text"], input[type="email"], textarea').focus(function () {
		var background = $(this).attr('id');
		$('#' + background + '-form').addClass('formgroup-active');
		$('#' + background + '-form').removeClass('formgroup-error');
	});
	$('input[type="text"], input[type="email"], textarea').blur(function () {
		var background = $(this).attr('id');
		$('#' + background + '-form').removeClass('formgroup-active');
	});

	function errorfield(field) {
		$(field).addClass('formgroup-error');
		console.log(field);
	}

	$("#waterform").submit(function () {
		var stopsubmit = false;

		if ($('#name').val() == "") {
			errorfield('#name-form');
			stopsubmit = true;
		}
		if ($('#email').val() == "") {
			errorfield('#email-form');
			stopsubmit = true;
		}
		if (stopsubmit) return false;
	});

});

$('#myModal').on('shown.bs.modal', function () {
	$('#myInput').trigger('focus')
})