$(document).ready(function () {
    $('#artTable').dataTable({
        "pageLength": 20,
        "info": false,
        "autoFill": false,
        "searching": false,
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
        },
        buttons:
            [],
        dom: 'Bfrtip'
    });

    $('#diferenciasModalTable').DataTable({
        "filter": false,
        "destroy": true,
        "pageLength": 20,
        "processing": true,
        "serverSide": false,
        "info": false,
        "paging": false,
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
        },
        lengthChange: false,
        buttons:
            [
                //{
                //    extend: 'excel',
                //    className: 'btn btn-success',
                //    text: "Exportar Excel",
                //    title: 'Listado de articulos'
                //},
                //{
                //    extend: 'print',
                //    className: 'btn btn-success',
                //    text: "Imprimir",
                //    title: 'Listado de articulos'
                //},
            ]
    });
});

$("#codigoBarras").on("keypress", function (e) {
    if (e.key == "Enter") {
        buscarArtCodigoBarras();
    }
});

$("#filtro").on("keypress", function (e) {
    if (e.key == "Enter") {
        buscarArtFiltro();
    }
});

$("#cantidadRecibida").on("keypress", function (e) {
    if (e.key == "Enter") {
        guardarCantidad();
    }
});

function buscarArtCodigoBarras() {
    var codigoBarras = $("#codigoBarras").val();
    var ordenCompra = $("#ordenCompra").val();

    if (ordenCompra == '') {
        Swal.fire(
            'Alerta',
            'Debe ingresar un número de orden de compra.',
            'warning'
        )
    } else {
        var formData = new FormData();

        formData.append('CodigoBarras', codigoBarras);
        formData.append('OrdenCompra', ordenCompra);

        $.ajax({
            url: resolveUrl("~/ReciboMercaderia/BuscarArt"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.length == 0) {
                    Swal.fire({
                        timer: 3500,
                        title: 'Error',
                        text: 'El código de barras ingresado no existe',
                        icon: 'error'
                    } )
                }  else {
                    var table = $('#artModalTable').DataTable({
                        "filter": false,
                        "destroy": true,
                        "pageLength": 20,
                        "processing": true,
                        "serverSide": false,
                        "info": false,
                        "paging": false,
                        "language": {
                            "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
                        },
                        lengthChange: false,
                        columns: [
                            { data: 'idArticulo' },
                            { data: 'descripcion' },
                            { data: 'bonificado' },
                            { data: 'codigoBarras' },
                            { data: 'codigoBarrasAnt' },
                            { data: 'cantidad' }
                        ]
                    });
                    table.clear();
                    var cantidadRec = 0;
                    var cantidadMaxima = 0;
                    var tipoArt = '';
                    var color = '';

                    $.each(data, function (idx, element) {
                        var id = element.id + '_' + element.idArticulo + '_' + element.bonificado;
                        var bonificado = '';
                        if (element.bonificado == 1) {
                            bonificado = '<span><i class="fa fa-check"></i></span>'
                        } else {
                            bonificado = '<span><i class="fa fa-times"></i></span>'
                        }

                        cantidadRec = element.cantidadRecibida;
                        cantidadMaxima += element.cantidad;
                        tipoArt = element.tipo;

                        if (tipoArt == 'Serie') {
                            color = 'blue';
                        } else {
                            color = 'red';
                        }

                        table.row.add({
                            "idArticulo": element.idArticulo,
                            "descripcion": element.descripcion,
                            "bonificado": bonificado,
                            "codigoBarras": element.asT_CodigoBarras,
                            "codigoBarrasAnt": element.asT_CodigoBarrasAnt,
                            "cantidad": element.cantidad
                        }).node().id = id;
                        table.draw();

                    });

                    //$("#cantidadRecibida").val(cantidadRec);
                    $("#cantidadMaxima").val(cantidadMaxima);
                    $("#tipoArticulo").text(tipoArt);
                    $("#tipoArticulo").css({ 'color': color })
                    $('#modalArticulos').on('shown.bs.modal', function () {
                        $('#cantidadRecibida').focus()
                    });

                    $('#modalArticulos').modal('show');
                }
            }
        });
    }
}

function buscarArtFiltro() {
    var filtro = $("#filtro").val();

    if (filtro == '') {
        Swal.fire(
            'Alerta',
            'Debe ingresar el código de artículo o código de barras para realizar la búsqueda.',
            'warning'
        )
    } else {
        var formData = new FormData();
        formData.append('Filtro', filtro);

        $.ajax({
            url: resolveUrl("~/ReciboMercaderia/Buscador"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.length == 0) {
                    var table = $('#artDetalleTable').DataTable({
                        "filter": false,
                        "destroy": true,
                        "pageLength": 20,
                        "processing": true,
                        "serverSide": false,
                        "info": false,
                        "paging": false,
                        "language": {
                            "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
                        },
                        lengthChange: false,
                        columns: [
                            { data: 'ordenCompra' },
                            { data: 'idArticulo' },
                            { data: 'descripcion' },
                            { data: 'cantidad' },
                            { data: 'cantidadRecibida' },
                            { data: 'diferencias' },
                            { data: 'codigoBarras' },
                            { data: 'codigoBarrasAnt' }
                        ]
                    });
                    table.clear();
                    table.draw();

                } else {
                    var table = $('#artDetalleTable').DataTable({
                        "filter": false,
                        "destroy": true,
                        "pageLength": 20,
                        "processing": true,
                        "serverSide": false,
                        "info": false,
                        "paging": false,
                        "language": {
                            "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
                        },
                        lengthChange: false,
                        columns: [
                            { data: 'ordenCompra' },
                            { data: 'idArticulo' },
                            { data: 'descripcion' },
                            { data: 'cantidad' },
                            { data: 'cantidadRecibida' },
                            { data: 'diferencias' },
                            { data: 'codigoBarras' },
                            { data: 'codigoBarrasAnt' }
                        ]
                    });
                    table.clear();

                    $.each(data, function (idx, element) {
                        var orden = 'Orden Compra ' + element.ordenCompra;
                        var id = element.id;
                        table.row.add({
                            "ordenCompra": orden,
                            "idArticulo": element.idArticulo,
                            "descripcion": element.descripcion,
                            "cantidad": element.cantidad,
                            "cantidadRecibida": element.cantidadRecibida,
                            "diferencias": element.diferencia,
                            "codigoBarras": element.asT_CodigoBarras,
                            "codigoBarrasAnt": element.asT_CodigoBarrasAnt
                        }).node().id = id;
                        table.draw();

                    });
                }
            }
        });
    }
}

function guardarCantidad() {
    var ordenCompra = $("#ordenCompra").val();
    var cantidadRecibida = $("#cantidadRecibida").val();
    var cantidadMaxima = $("#cantidadMaxima").val();
    var detalle = '';
    var error = false;

    $("#artModalTable").DataTable().rows().data().$('tr').each(function () {
        var articulo, cantidad;
        $(this).children("td").each(function (index2) {
            switch (index2) {
                case 0:
                    articulo = $(this).text();
                    break;
                case 5:
                    cantidad = $(this).text();
                    break;
            }
        });
        if (articulo != undefined) {
            if (cantidadRecibida == '') {
                error = true;
            } else if (parseInt(cantidadRecibida) < 0) {
                error = true;
            } else {
                detalle += '{"ID": ' + ordenCompra + ', "idArticulo":"' + articulo + '","Cantidad":' + cantidad + ', "CantidadTotal": ' + cantidadRecibida + '},';
            }
        }
    });

    if (error) {
        Swal.fire(
            'Alerta',
            'Debe ingresar una cantidad valida para cada línea.',
            'warning'
        )
    } else if (detalle == '') {
        Swal.fire(
            'Alerta',
            'Debe ingresar al menos un artículo.',
            'warning'
        )
    } else if (parseInt(cantidadRecibida) > parseInt(cantidadMaxima)) {
        Swal.fire({ timer: 3500 ,
            title:"Alerta",
            text:"La cantidad ingresada no puede ser mayor al número de artículos de la orden de compra.",
            icon:"warning"

        })
    } else {
        detalle = detalle.substring(0, detalle.length - 1);
        var values = "[" + detalle + "]";

        var formData = new FormData();
        formData.append('Detalle', values);

        $.ajax({
            url: resolveUrl("~/ReciboMercaderia/GuardarCantidad"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (data) {
                if (data == true) {
                    Swal.fire(
                        'Éxito',
                        'Se ha guardado exitosamente.',
                        'success'
                    ).then(
                        function (isConfirm) {
                            if (isConfirm) {
                                window.location.reload();
                            }
                        }
                    );
                    setTimeout(function () {
                        window.location.reload();
                        
                    }, 3000);
                }
                else {
                    Swal.fire(
                        'Error',
                        'No se pudo guardar, ocurrió un error inesperado, intente nuevamente.',
                        'error'
                    )
                }
            }
        });
    }
}

function finalizar() {
    var ordenCompra = $("#ordenCompra").val();

    if (ordenCompra == '') {
        Swal.fire(
            'Alerta',
            'Debe ingresar un número de orden de compra.',
            'warning'
        )
    } else {
        var formData = new FormData();
        formData.append('OrdenCompra', ordenCompra);
        formData.append('Tipo', 1);

        $.ajax({
            url: resolveUrl("~/ReciboMercaderia/FinalizarOC"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (data) {
                if (data == 1) {
                    Swal.fire(
                        'Éxito',
                        'Se ha finalizado exitosamente.',
                        'success'
                    ).then(
                        function (isConfirm) {
                            if (isConfirm) {
                                window.location.reload();
                            }
                        }
                    );
                }
                else if (data == 0) {
                    Swal.fire(
                        'Error',
                        'No se pudo finalizar, ocurrió un error inesperado, intente nuevamente.',
                        'error'
                    )
                } else {
                    $('#msjConfirmacion').modal('show');
                }
            }
        });
    }
}


function confirmar() {
    var ordenCompra = $("#ordenCompra").val();

    if (ordenCompra == '') {
        Swal.fire(
            'Alerta',
            'Debe ingresar un número de orden de compra.',
            'warning'
        )
    } else {
        var formData = new FormData();
        formData.append('OrdenCompra', ordenCompra);
        formData.append('Tipo', 2);

        $.ajax({
            url: resolveUrl("~/ReciboMercaderia/FinalizarOC"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (data) {
                if (data == 1) {
                    Swal.fire(
                        'Éxito',
                        'Se ha finalizado exitosamente.',
                        'success'
                    ).then(
                        function (isConfirm) {
                            if (isConfirm) {
                                window.location.reload();
                            }
                        }
                    );
                }
                else if (data == 0) {
                    Swal.fire(
                        'Error',
                        'No se pudo finalizar, ocurrió un error inesperado, intente nuevamente.',
                        'error'
                    )
                } 
            }
        });
    }
}

function mostrarDiferencias() {
    var ordenCompra = $("#ordenCompra").val();

    if (ordenCompra == '') {
        Swal.fire(
            'Error',
            'Debe buscar la orden de compra.',
            'error'
        )
    } else {
        var formData = new FormData();
        formData.append('OrdenCompra', ordenCompra);

        $.ajax({
            url: resolveUrl("~/ReciboMercaderia/GuardarOC"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (data) {

                if (data.length == 0) {
                    Swal.fire(
                        'Error',
                        'Aún no se han registrado cantidades para esta orden de compra.',
                        'error'
                    )
                } else {
                    var table = $('#diferenciasModalTable').DataTable({
                        "filter": false,
                        "destroy": true,
                        "pageLength": 20,
                        "processing": true,
                        "serverSide": false,
                        "info": false,
                        "paging": false,
                        "language": {
                            "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
                        },
                        lengthChange: false,
                        buttons:
                            [
                                //{
                                //    extend: 'excel',
                                //    className: 'btn btn-success',
                                //    text: "Exportar Excel",
                                //    title: 'Listado de articulos'
                                //}
                            ],
                        dom: 'Bfrtip',
                        columns: [
                            { data: 'idArticulo' },
                            { data: 'descripcion' },
                            { data: 'codigoBarras' },
                            { data: 'codigoBarrasAnt' },
                            { data: 'cantidad' }, 
                            { data: 'cantidadRecibida' },
                            { data: 'diferencia' }
                        ]
                    });
                    table.clear();

                    $.each(data, function (idx, element) {
                        var id = element.id + '_' + element.idArticulo;
                        console.log(element)
                        table.row.add({
                            "idArticulo": element.idArticulo,
                            "descripcion": element.descripcion,
                            "codigoBarras": element.asT_CodigoBarras,
                            "codigoBarrasAnt": element.asT_CodigoBarrasAnt,
                            "cantidad": element.cantidad,
                            "cantidadRecibida": element.cantidadRecibida,
                            "diferencia": element.diferencia
                        }).node().id = id;
                        table.draw();

                    });
                    $('#numOrdenCompra').text(ordenCompra);
                    $('#modalDiferencias').modal('show');
                }
            }
        });

    }

}

function popUpEnviar() {
    $('#modalEnviarCorreo').modal('show');
}

function mostrarBuscador() {
    $('#modalBuscador').modal('show');
}

function descargar() {
    var ordenCompra = $("#ordenCompra").val();
    window.location.href = resolveUrl("~/ReciboMercaderia/Diferencias?OrdenCompra=" + ordenCompra);
}

function enviarCorreo() {
    var ordenCompra = $("#ordenCompra").val();
    var email = $("#correo").val();

    var formData = new FormData();
    formData.append('OrdenCompra', ordenCompra);
    formData.append('email', email);

    $.ajax({
        url: resolveUrl("~/ReciboMercaderia/EnviarCorreo"),
        data: formData,
        type: 'POST',
        contentType: false,
        processData: false,
        success: function (data) {
            $('#modalEnviarCorreo').modal('hide');
            if (data == false) {
                Swal.fire(
                    'Error',
                    'Ha ocurrido un error inesperado.',
                    'error'
                )
            } else {
                Swal.fire(
                    'Éxito',
                    'Se ha enviado el correo correctamente.',
                    'success'
                )
            }
        }
    });
}