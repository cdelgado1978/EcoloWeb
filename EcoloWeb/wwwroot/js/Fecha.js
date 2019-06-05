var Fechainicial;

window.onload = function () {
    var fecha = new Date(); //Fecha actual
    var mes = fecha.getMonth() + 1; //obteniendo mes
    var dia = fecha.getDate(); //obteniendo dia
    var ano = fecha.getFullYear(); //obteniendo año
    if (dia < 10)
        dia = '0' + dia; //agrega cero si el menor de 10
    if (mes < 10)
        mes = '0' + mes //agrega cero si el menor de 10
    document.getElementById('fechaActual').value = ano + "-" + mes + "-" + dia;
    Fechainicial =ano + "-" + mes + "-" + dia;
}

function Edad()
{
 
    var anoN = document.getElementById("fechaActual").value;
    var hoy = new Date();
    var cumpleanos = new Date(anoN);    
    var edad = hoy.getFullYear() - cumpleanos.getFullYear();

    var Mes = hoy.getMonth() - cumpleanos.getMonth();

    if (Mes < 0 || (Mes === 0 && hoy.getDate() < cumpleanos.getDate())) {
        edad--;
    }

    if (edad > 18 || edad === 18) {
        document.getElementById('Mother').style.visibility = "visible";
        document.getElementById('Mother1').style.visibility = "visible";

        document.getElementById('Father').style.visibility = "visible";
        document.getElementById('Father1').style.visibility = "visible";
    }
    else
    {
        document.getElementById('Mother').style.visibility = "hidden";
        document.getElementById('Mother1').style.visibility = "hidden";

        document.getElementById('Father').style.visibility = "hidden";
        document.getElementById('Father1').style.visibility = "hidden";

    }
   

  
}

