var classNum = 0;
var num = 0;

window.addEventListener('scroll', function () {


    let len = window.pageYOffset;

        if (len > 400) {
            document.getElementById('myBtn').style.display = 'block';
        }
        else
        if (len === 0) {
                document.getElementById('myBtn').style.display = 'none';
            }

}, false);



function AddMore() {
    let el = document.getElementById(`stats[${classNum++}]`);
    let clone = el.cloneNode(true);
    clone.id = `stats[${classNum}]`;
    el.after(clone);
}

function Remove() {
    
    if (classNum > 0) {
document.getElementById(`stats[${classNum--}]`).remove();
    }

}

function RegAlert() {
    alert("Для покупки билета зарегистрируйтесь на сайте");
}


function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    document.getElementById("#myBtn").style.display = 'none';
}
