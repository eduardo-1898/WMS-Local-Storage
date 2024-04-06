$(document).ready(function () {
    $("#btnBuscarArticulo").on('click', function () {
        if ($("#txtCodigoBarras").val() == "") {
            Swal.fire(
                'Upps..',
                'No se ha seleccionado o escaneado ningun articulo para realizar la busqueda',
                'info',
                'timer:5000'
            ).then((dismiss) => {
            })
        }
        else {

            try {
                $('#tableArticulos').DataTable().destroy().clear();
            } catch (e) {

            }

            var table = $('#tableArticulos').DataTable({
                "pageLength": 10,
                "processing": true,
                "serverSide": false,
                "paging": false,
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
                },
                lengthChange: false,
                buttons:
                    [
                        {
                            extend: 'copy',
                            className: 'btn btn-success',
                            text: "Copiar",
                            title: 'Listado de movimientos'
                        },
                        {
                            extend: 'excel',
                            className: 'btn btn-success',
                            text: "Exportar Excel",
                            title: 'Listado de movimientos'
                        },
                        {
                            extend: 'print',
                            className: 'btn btn-success',
                            text: "Imprimir",
                            title: 'Listado de movimientos'
                        },
                    ],
                "ajax": resolveUrl("~/Datamatrix/ObtenerDatosArticulo?articulo=" + $("#txtCodigoBarras").val()),
                "columns": [
                    {
                        "orderable": true,
                        "data": "orden",
                        "title": "Orden",
                        "render": function (data, type, row, meta) {
                            return data;
                        }
                    },
                    {
                        "orderable": true,
                        "data": "articulo",
                        "title": "Articulo",
                        "render": function (data, type, row, meta) {
                            return data;
                        }
                    },
                    //{
                    //    "orderable": true,
                    //    "data": "tipo",
                    //    "title": "Tipo",
                    //    "render": function (data, type, row, meta) {
                    //        return data;
                    //    }
                    //},
                    {
                        "orderable": true,
                        "data": "descripcion",
                        "title": "Descripción",
                        "render": function (data, type, row, meta) {
                            return data;
                        }
                    },
                    //{
                    //    "orderable": true,
                    //    "data": "documento",
                    //    "title": "Documento",
                    //    "render": function (data, type, row, meta) {
                    //        return data;
                    //    }
                    //},
                    {
                        "orderable": true,
                        "data": "cantidad",
                        "title": "Cantidad",
                        "render": function (data, type, row, meta) {
                            return data;
                        }
                    },
                    {
                        "orderable": true,
                        "data": "cantidadRecibida",
                        "title": "Cantidad Recibida",
                        "render": function (data, type, row, meta) {
                            return data;
                        }
                    },
                    {
                        "orderable": true,
                        "data": "fecha",
                        "title": "Fecha",
                        "render": function (data, type, row, meta) {
                            return data.split('T')[0];
                        }
                    },
                    {
                        "orderable": true,
                        "data": "id",
                        "width": "5%",
                        "render": function (data, type, row, meta) {
                            return getButtton(data, row);
                        }
                    }
                ],
                'select': {
                    'style': 'multi'
                },
                "order": [[0, 'des']],
                dom: 'Bfrtip',
            });

            table.buttons().container()
                .appendTo('#example_wrapper .col-md-6:eq(0)');

            var getButtton = function (data, row) {
                html = `<button class='btn btn-success btn-sm text-white' onclick='select(${row.ordenID},"${row.articulo}","${row.modulo}","${row.renglonID}")'><i class='fas fa-pencil-alt'></i></button>`
                return html;
            }
        }
    })

})

$("#txtCodigoBarras").on("keypress", function (e) {
    if (e.key == "Enter") {
        buscarArtConEnter()
    }
});

function buscarArtConEnter() {
    if ($("#txtCodigoBarras").val() == "") {
        Swal.fire(
            'Upps..',
            'No se ha seleccionado o escaneado ningun articulo para realizar la busqueda',
            'info',
            'timer:5000'
        ).then((dismiss) => {
        })
    }
    else {

        try {
            $('#tableArticulos').DataTable().destroy().clear();
        } catch (e) {

        }

        var table = $('#tableArticulos').DataTable({
            "pageLength": 10,
            "processing": true,
            "serverSide": false,
            "paging": false,
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json"
            },
            lengthChange: false,
            buttons:
                [
                    {
                        extend: 'copy',
                        className: 'btn btn-success',
                        text: "Copiar",
                        title: 'Listado de movimientos'
                    },
                    {
                        extend: 'excel',
                        className: 'btn btn-success',
                        text: "Exportar Excel",
                        title: 'Listado de movimientos'
                    },
                    {
                        extend: 'print',
                        className: 'btn btn-success',
                        text: "Imprimir",
                        title: 'Listado de movimientos'
                    },
                ],
            "ajax": resolveUrl("~/Datamatrix/ObtenerDatosArticulo?articulo=" + $("#txtCodigoBarras").val()),
            "columns": [
                {
                    "orderable": true,
                    "data": "orden",
                    "title": "Orden",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "articulo",
                    "title": "Articulo",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                //{
                //    "orderable": true,
                //    "data": "tipo",
                //    "title": "Tipo",
                //    "render": function (data, type, row, meta) {
                //        return data;
                //    }
                //},
                {
                    "orderable": true,
                    "data": "descripcion",
                    "title": "Descripción",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                //{
                //    "orderable": true,
                //    "data": "documento",
                //    "title": "Documento",
                //    "render": function (data, type, row, meta) {
                //        return data;
                //    }
                //},
                {
                    "orderable": true,
                    "data": "cantidad",
                    "title": "Cantidad",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "cantidadRecibida",
                    "title": "Cantidad Recibida",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "fecha",
                    "title": "Fecha",
                    "render": function (data, type, row, meta) {
                        return data.split('T')[0];
                    }
                },
                {
                    "orderable": true,
                    "data": "id",
                    "width": "5%",
                    "render": function (data, type, row, meta) {
                        return getButtton(data, row);
                    }
                }
            ],
            'select': {
                'style': 'multi'
            },
            "order": [[0, 'des']],
            dom: 'Bfrtip',
        });

        table.buttons().container()
            .appendTo('#example_wrapper .col-md-6:eq(0)');

        var getButtton = function (data, row) {
            html = `<button class='btn btn-success btn-sm text-white' onclick='select(${row.ordenID},"${row.articulo}","${row.modulo}","${row.renglonID}")'><i class='fas fa-pencil-alt'></i></button>`
            return html;
        }
    }
}


function select(ordenID, articulo, modulo, renglonID) {
    var formData = new FormData();
    formData.append('articulo', articulo);
    formData.append('id', ordenID);
    formData.append('modulo', modulo);
    formData.append('renglonID', renglonID);

    $("#txtImprimir").val('');
    $("#txtLote").val('');
    document.getElementById("cantidadCajas").value = "";
    document.getElementById("ArticulosCaja").value = "";
    document.getElementById("slctRevisado").value = "";
    document.getElementById("cbxConsecutivo").checked = false;
    document.getElementById("txtFechaVencimiento").value = "";
    

    $.ajax({
        url: resolveUrl("~/DataMatrix/ObtenerDatosAsociados"),
        data: formData,
        type: 'POST',
        contentType: false,
        processData: false,
        success: function (response) {

            var imprimir = document.getElementById('imprimirDatos');
            var lote = document.getElementById('LoteDatos');
            var boton = document.getElementById('boton');
            var cantidades = document.getElementById('cantidades');

            $("#txtEmbarque").val(response.ordenID);
            $("#txtArticulo").val(response.articulo);
            $("#txtDescripcion").val(response.descripcion);
            $("#txtCantidadEtiquetas").val(response.numeroEtiquetas);
            $("#txtRecibidas").val(response.cantidadRecibidas);
            $("#txtDisponibles").val(response.cantidadDisponibles);
            $("#txtRenglon").val(response.renglonID);
            $("#txtID").val(response.id);
            $("#txtTipo").val(response.tipo);
            $("#txtQr").val(response.qr);
            $("#txtModulo").val(modulo);

            if (response.tipo == "Serie") {
                imprimir.style.display = 'block';
                imprimir.style.visibility = 'visible';
                lote.style.display = 'block';
                lote.style.visibility = 'visible';
                boton.style.display = 'block';
                boton.style.visibility = 'visible';
                cantidades.style.display = 'flex';
                cantidades.style.visibility = 'visible';
            }
            if (response.tipo == "Lote") {
                imprimir.style.display = 'none';
                imprimir.style.visibility = 'hidden';
                lote.style.display = 'block';
                lote.style.visibility = 'visible';
                boton.style.display = 'block';
                boton.style.visibility = 'visible';
                cantidades.style.display = 'flex';
                cantidades.style.visibility = 'visible';
            }
            if (response.tipo == "Normal" || response.tipo == "NORMAL") {
                imprimir.style.display = 'block';
                imprimir.style.visibility = 'visible';
                lote.style.display = 'none';
                lote.style.visibility = 'hidden';
                boton.style.display = 'block';
                boton.style.visibility = 'visible';
                cantidades.style.display = 'flex';
                cantidades.style.visibility = 'visible';

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
}

