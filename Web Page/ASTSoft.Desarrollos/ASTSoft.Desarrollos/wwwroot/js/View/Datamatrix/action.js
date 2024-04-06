$(document).ready(function () {
    $("#cbxConsecutivo").change(function () {
        var cajaInf = document.getElementById('cajas');
        if ($("#cbxConsecutivo").is(':checked')) {
            cajaInf.style.visibility = 'visible';
            cajaInf.style.display = 'inline-flex';
            document.getElementById("cantidadCajas").value = "";
            document.getElementById("ArticulosCaja").value = "";
            document.getElementById("slctRevisado").value = "";
        }
        else {
            cajaInf.style.visibility = 'hidden';
            cajaInf.style.display = 'none';
            document.getElementById("slctRevisado").value = "";
        }

    })

    $("#btnImprimir").on('click', function () {

        var lote = $("#txtLote").val();
        var fechaVencimiento = $("#txtFechaVencimiento").val();
        var anno = fechaVencimiento.substring(0, 2);
        var mes = fechaVencimiento.substring(4, 2);
        var date = new Date('20' + anno, mes, '01');
        var disponibles = parseInt($("#txtDisponibles").val());
        var aImprimir = parseInt($("#txtImprimir").val());
        var tipo = $("#txtTipo").val();

        if (lote == "" && (tipo != "Normal" && tipo != "NORMAL")) {
            Swal.fire(
                'Upps..',
                'No se ha ingresado un lote, este campo es necesario',
                'info',
                'timer:5000'
            ).then((dismiss) => {
            })
        }
        else if (fechaVencimiento == "" && (tipo != "Normal" && tipo != "NORMAL")) {
            Swal.fire(
                'Upps..',
                'No se ha ingresado una fecha de vencimiento, este campo es necesario',
                'info',
                'timer:5000'
            ).then((dismiss) => {

            })
        }
        else if (disponibles < aImprimir) {
            Swal.fire(
                'Upps..',
                'No puedes imprimir más etiquetas de las que se encuentran disponibles',
                'info',
                'timer:5000'
            ).then((dismiss) => {
            })
        }
        else if ($("#txtDisponibles").val()=="0") {
            Swal.fire(
                'Upps..',
                'Ya se han procesado todas las etiquetas de este pedido',
                'info',
                'timer:5000'
            ).then((dismiss) => {
            })
        }
        else if (monthDiff(new Date(), date) <= 4 && (tipo != "Normal" && tipo != "NORMAL")) {
            Swal.fire({
                title: 'La fecha de vencimiento se encuentra próxima a vencer, aún así deseas continuar?',
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: 'Guardar',
                denyButtonText: `Cancelar`,
            }).then((result) => {
                if (result.isConfirmed) {
                    let timerInterval
                    Swal.fire({
                        title: 'Un momento..',
                        html: 'Se estan procesando los datos',
                        timer: 5000000000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading()
                            var etiqueta = $("#cbxEtiqueta").is(':checked');
                            var consecutivo = $("#cbxConsecutivo").is(':checked');
                            var revisador = $("#slctRevisado").val();
                            var imprimir = $("#txtImprimir").val();
                            var articulo = $("#txtArticulo").val();
                            var qr = $("#txtQr").val();
                            var renglon = $("#txtRenglon").val();
                            var id = $("#txtID").val();
                            var cantidadCajas = $("#cantidadCajas").val();
                            var cantidadArt = $("#ArticulosCaja").val();
                            var modulo = $("#txtModulo").val();
                            var almacen = $("#slctAlmDestino option:selected").val();
                            var tipo = $("#txtTipo").val();

                            $.ajax({
                                url: resolveUrl(`~/DataMatrix/EnviarDatosDatamatrix?lote=${lote}
                                &fechaVencimiento=${fechaVencimiento}
                                &etiqueta=${etiqueta}
                                &consecutivo=${consecutivo}
                                &revisador=${revisador}
                                &imprimir=${imprimir}
                                &articulo=${articulo}
                                &qr=${qr}
                                &renglon=${renglon}
                                &id=${id}
                                &cantidadCajas=${cantidadCajas}
                                &cantidadArt=${cantidadArt}
                                &almacen=${almacen}
                                &modulo="${modulo}"
                                &tipo=${tipo}`),
                                type: 'GET',
                                contentType: false,
                                processData: false,
                                success: function (response) {

                                    swal.close();

                                    if (tipo == "Normal" || tipo == "NORMAL") {

                                        var table = $('#tableArticulos').DataTable();
                                        table.search();
                                        table.ajax.reload();
                                    }
                                    else {
                                        var arrrayBuffer = base64ToArrayBuffer(response.datamatrix); //data is the base64 encoded string
                                        function base64ToArrayBuffer(base64) {
                                            var binaryString = window.atob(base64);
                                            var binaryLen = binaryString.length;
                                            var bytes = new Uint8Array(binaryLen);
                                            for (var i = 0; i < binaryLen; i++) {
                                                var ascii = binaryString.charCodeAt(i);
                                                bytes[i] = ascii;
                                            }
                                            return bytes;
                                        }

                                        var blob = new Blob([arrrayBuffer], { type: "application/pdf" });
                                        var link = window.URL.createObjectURL(blob);
                                        window.open(link, '', 'height=650,width=840');


                                        var arrrayBuffer2 = base64ToArrayBuffer(response.caja); //data is the base64 encoded string
                                        function base64ToArrayBuffer(base642) {
                                            var binaryString2 = window.atob(base642);
                                            var binaryLen2 = binaryString2.length;
                                            var bytes2 = new Uint8Array(binaryLen2);
                                            for (var i = 0; i < binaryLen2; i++) {
                                                var ascii2 = binaryString2.charCodeAt(i);
                                                bytes2[i] = ascii2;
                                            }
                                            return bytes2;
                                        }

                                        var blob2 = new Blob([arrrayBuffer2], { type: "application/pdf" });
                                        var link2 = window.URL.createObjectURL(blob2);
                                        window.open(link2, '', 'height=650,width=840');

                                        var table = $('#tableArticulos').DataTable();
                                        table.search();
                                        table.ajax.reload();
                                    }
                                },
                                error: function (msg) {
                                    Swal.fire(
                                        'Upps..',
                                        msg.responseText,
                                        'error',
                                        'timer:5000'
                                    ).then((dismiss) => {
                                        var table = $('#tableArticulos').DataTable();
                                        table.search();
                                        table.ajax.reload();
                                    })
                                }
                            });
                        },
                        willClose: () => {
                        }
                    }).then((result) => {
                    })
                } else if (result.isDenied) {
                    Swal.fire('Se ha cancelado la creación de estos consecutivos', '', 'info')
                }
            })
        }
        else {
            let timerInterval
            Swal.fire({
                title: 'Un momento..',
                html: 'Se estan procesando los datos',
                timer: 50000,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading()
                    var etiqueta = $("#cbxEtiqueta").is(':checked');
                    var consecutivo = $("#cbxConsecutivo").is(':checked');
                    var revisador = $("#slctRevisado").val();
                    var imprimir = $("#txtImprimir").val();
                    var articulo = $("#txtArticulo").val();
                    var qr = $("#txtQr").val();
                    var renglon = $("#txtRenglon").val();
                    var id = $("#txtID").val();
                    var cantidadCajas = $("#cantidadCajas").val();
                    var cantidadArt = $("#ArticulosCaja").val();
                    var modulo = $("#txtModulo").val();
                    var almacen = $("#slctAlmDestino option:selected").val();
                    var tipo = $("#txtTipo").val();

                    $.ajax({
                        url: resolveUrl(`~/DataMatrix/EnviarDatosDatamatrix?lote=${lote}
                                &fechaVencimiento=${fechaVencimiento}
                                &etiqueta=${etiqueta}
                                &consecutivo=${consecutivo}
                                &revisador=${revisador}
                                &imprimir=${imprimir}
                                &articulo=${articulo}
                                &qr=${qr}
                                &renglon=${renglon}
                                &id=${id}
                                &cantidadCajas=${cantidadCajas}
                                &cantidadArt=${cantidadArt}
                                &almacen=${almacen}
                                &modulo=${modulo},
                                &tipo=${tipo}`),
                        type: 'GET',
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            swal.close();

                            if (tipo == "Normal" || tipo == "NORMAL") {

                                var table = $('#tableArticulos').DataTable();
                                table.search();
                                table.ajax.reload();
                            }
                            else {
                                var arrrayBuffer = base64ToArrayBuffer(response.datamatrix); //data is the base64 encoded string
                                function base64ToArrayBuffer(base64) {
                                    var binaryString = window.atob(base64);
                                    var binaryLen = binaryString.length;
                                    var bytes = new Uint8Array(binaryLen);
                                    for (var i = 0; i < binaryLen; i++) {
                                        var ascii = binaryString.charCodeAt(i);
                                        bytes[i] = ascii;
                                    }
                                    return bytes;
                                }

                                var blob = new Blob([arrrayBuffer], { type: "application/pdf" });
                                var link = window.URL.createObjectURL(blob);
                                window.open(link, '', 'height=650,width=840');


                                var arrrayBuffer2 = base64ToArrayBuffer(response.caja); //data is the base64 encoded string
                                function base64ToArrayBuffer(base642) {
                                    var binaryString2 = window.atob(base642);
                                    var binaryLen2 = binaryString2.length;
                                    var bytes2 = new Uint8Array(binaryLen2);
                                    for (var i = 0; i < binaryLen2; i++) {
                                        var ascii2 = binaryString2.charCodeAt(i);
                                        bytes2[i] = ascii2;
                                    }
                                    return bytes2;
                                }

                                var blob2 = new Blob([arrrayBuffer2], { type: "application/pdf" });
                                var link2 = window.URL.createObjectURL(blob2);
                                window.open(link2, '', 'height=650,width=840');

                                var table = $('#tableArticulos').DataTable();
                                table.search();
                                table.ajax.reload();

                            }

                        },
                        error: function (msg) {
                            Swal.fire(
                                'Upps..',
                                msg.responseText,
                                'error',
                                'timer:5000'
                            ).then((dismiss) => {
                                var table = $('#tableArticulos').DataTable();
                                table.search();
                                table.ajax.reload();
                            })
                        }
                    });
                },
                willClose: () => {
                }
            }).then((result) => {
            })
        }

    })



    $("#botonAlmacen").on('click', function () {

        var almacen = $("#slctAlmDestino option:selected").val();

        if (almacen == "" || almacen == undefined) {
            Swal.fire(
                'Upps..',
                'No se ha ingresado un lote, este campo es necesario',
                'info',
                'timer:5000'
            ).then((dismiss) => {
            })
        }
        else {
            let timerInterval
            Swal.fire({
                title: 'Un momento..',
                html: 'Se estan procesando los datos',
                timer: 50000,
                timerProgressBar: true,
                didOpen: () => {

                    Swal.showLoading()

                    var renglon = $("#txtRenglon").val();
                    var id = $("#txtID").val();
                    var almacen = $("#slctAlmDestino option:selected").val();

                    $.ajax({
                        url: resolveUrl(`~/DataMatrix/ModificarAlmacenCompra?
                                &id=${id}
                                &renglon=${renglon}
                                &almacen=${almacen}`),
                        type: 'POST',
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            Swal.fire(
                                'Éxito',
                                'Se ha almacenado el nuevo almacen correctamente',
                                'success',
                                'timer:5000'
                            ).then((dismiss) => {
                                var table = $('#tableArticulos').DataTable();
                                table.search();
                                table.ajax.reload();
                            })
                        },
                        error: function (msg) {
                            Swal.fire(
                                'Upps..',
                                msg.responseText,
                                'error',
                                'timer:5000'
                            ).then((dismiss) => {
                                var table = $('#tableArticulos').DataTable();
                                table.search();
                                table.ajax.reload();
                            })
                        }
                    });
                                    },
                willClose: () => {
                }
            }).then((result) => {
            })
        }

    })


    function monthDiff(d1, d2) {
        var months;
        months = (d2.getFullYear() - d1.getFullYear()) * 12;
        months -= d1.getMonth();
        months += d2.getMonth();
        return months <= 0 ? 0 : months;
    }

})


//document.addEventListener('DOMContentLoaded', function () {
//    const inputs = document.querySelectorAll("input, textarea, select");
//    for (let i = 0; i < inputs.length; i++) {
//        inputs[i].addEventListener('keydown', function (event) {
//            if (event.key === 'Enter') {
//                event.preventDefault();
//                let nextIndex = (i + 1) % inputs.length;
//                while (inputs[nextIndex].type === 'checkbox') {
//                    nextIndex = (nextIndex + 1) % inputs.length;
//                }
//                inputs[nextIndex].focus();
//            }
//        });
//    }
//});
document.addEventListener('DOMContentLoaded', function () {
    const inputs = Array.from(document.querySelectorAll('input, textarea, select')).toSorted((a, b) => a.tabIndex - b.tabIndex);

    for (let i = 0; i < inputs.length; i++) {
        inputs[i].addEventListener('keydown', function (event) {
            if (event.key === 'Enter') {
                event.preventDefault();
                inputs[(i + 1) % inputs.length].focus();
            }
        });
    }
});