//import blazor from './BlazorScripts'

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

window.clientFunctions = {
    RedirectTo: (path) => {
        window.location.href = 'https://localhost:44330' + path;
    }
}