// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function Disponible(idInm) {
    $(function () {
        var url = 'Inmuebles/Disponible/' + idInm;
        var IDd = idInm + 'd';
        var IDc = idInm + 'c';

        $.post(url).done(function (data) {
            console.log(data);
            data == 1 ? document.getElementById(idInm).style.display = "" : document.getElementById(idInm).style.display = "none";
            data == 1 ? document.getElementById(IDd).innerText = "Disponible" : document.getElementById(IDd).innerText = "No Disponible";
            data == 1 ? document.getElementById(IDc).style.backgroundColor = "white" : document.getElementById(IDc).style.backgroundColor = "orangered";
        }).fail(manejarErrorAjax).always(function () {

        });
    });

    function manejarErrorAjax(err) {
        console.log(err);
    }
}