$(document).ready(function () {

    $("#btnPrint").click(function () {
        var pedido = $("#txtPedido").val();
        var cantidad = $("#txtBultos").val();

        if (pedido == "" || pedido == null) {
            Swal.fire(
                'Upps..',
                'Debes de seleccionar un pedido primero',
                'info',
                'timer:5000'
            )
        }
        else if (cantidad == 0 || cantidad == null || cantidad == undefined) {
            Swal.fire(
                'Upps..',
                'Aun no se han asignado las etiquetas correspondientes',
                'info',
                'timer:5000'
            )
        }
        else {

            var formData = new FormData();
            formData.append('pedido', pedido);
            formData.append('cantidad', cantidad);

            $.ajax({
                type: 'POST',
                url: resolveUrl("~/Etiquetas/ImprimirEtiqueta"),
                data: formData,
                type: 'POST',
                contentType: false,
                processData: false,
                success: function (response) {
                    Swal.fire(
                        'Éxito',
                        'Se ha confirmado el pedido correctamente',
                        'success',
                        'timer:5000'
                    )
                },
                error: function (msg) {
                    Swal.fire(
                        'Error',
                        msg.responseText,
                        'error',
                        'timer:5000'
                    )
                }
            });
        }
    });

    $("#btnCrear").click(function () {
        $.ajax({
            type: 'POST',
            url: resolveUrl("~/Etiquetas/CrearConsecutivo"),
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (response) {

                $("#txtConsecutivo").val('Consecutivo#: ' + response.id);
                $("#txtConsecutivo2").val(response.id);
                $('#modalDetalle').modal('show');

                try {
                    $('#tableDetalle').DataTable().clear().destroy();
                } catch (e) {

                }

                var table = $('#tableDetalle').DataTable({
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
                                title: 'Listado de movimientos en bodega'
                            },
                            {
                                extend: 'excel',
                                className: 'btn btn-success',
                                text: "Exportar Excel",
                                title: 'Listado de movimientos en bodega'
                            },
                            {
                                extend: 'print',
                                className: 'btn btn-success',
                                text: "Imprimir",
                                title: 'Listado de movimientos en bodega'
                            },
                            {
                                extend: 'print',
                                className: 'btn btn-success',
                                text: "Imprimir",
                                title: 'Listado de movimientos en bodega'
                            },
                        ],
                    "ajax": resolveUrl(`~/Etiquetas/ListPedidos?idDocumento=${response.id}`),
                    "columns": [
                        {
                            "orderable": true,
                            "data": "pedido",
                            "title": "Pedido",
                            "render": function (data, type, row, meta) {
                                return data;
                            }
                        },
                        {
                            "orderable": true,
                            "data": "fechaAsociacion",
                            "title": "Fecha de asociación",
                            "render": function (data, type, row, meta) {
                                return data;
                            }
                        },
                        {
                            "orderable": true,
                            "data": "cantidadBultos",
                            "title": "Cantidad de bultos",
                            "render": function (data, type, row, meta) {
                                return data;
                            }
                        }
                    ],
                    'select': {
                        'style': 'multi'
                    },
                    "order": [[0, 'asc']],
                    dom: 'Bfrtip',
                });

                table.buttons().container().appendTo('#example_wrapper .col-md-6:eq(0)');

                var tableDe = $('#docuentosTable').DataTable();
                tableDe.search();
                tableDe.ajax.reload();

            },
            error: function (msg) {
                Swal.fire(
                    'Error',
                    msg.responseText,
                    'error',
                    'timer:5000'
                )
            }
        });
    });

    $("#btnAsociar").click(function () {

        var formData = new FormData();
        var pedido = $("#txtPedido").val();
        var documento = $("#txtConsecutivo2").val();

        formData.append('pedido', pedido);
        formData.append('idDocumento', documento);

        $.ajax({
            type: 'POST',
            url: resolveUrl("~/Etiquetas/AgregarDocumento"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (response) {

                try {
                    $('#tableDetalle').DataTable().clear().destroy();
                } catch (e) {

                }

                var table = $('#tableDetalle').DataTable({
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
                                title: 'Listado de movimientos en bodega'
                            },
                            {
                                extend: 'excel',
                                className: 'btn btn-success',
                                text: "Exportar Excel",
                                title: 'Listado de movimientos en bodega'
                            },
                            {
                                extend: 'print',
                                className: 'btn btn-success',
                                text: "Imprimir",
                                title: 'Listado de movimientos en bodega'
                            },
                            {
                                extend: 'print',
                                className: 'btn btn-success',
                                text: "Imprimir",
                                title: 'Listado de movimientos en bodega'
                            },
                        ],
                    "ajax": resolveUrl(`~/Etiquetas/ListPedidos?idDocumento=${documento}`),
                    "columns": [
                        {
                            "orderable": true,
                            "data": "pedido",
                            "title": "Pedido",
                            "render": function (data, type, row, meta) {
                                return data;
                            }
                        },
                        {
                            "orderable": true,
                            "data": "fechaAsociacion",
                            "title": "Fecha de asociación",
                            "render": function (data, type, row, meta) {
                                return data;
                            }
                        },
                        {
                            "orderable": true,
                            "data": "cantidadBultos",
                            "title": "Cantidad de bultos",
                            "render": function (data, type, row, meta) {
                                return data;
                            }
                        }
                    ],
                    'select': {
                        'style': 'multi'
                    },
                    "order": [[0, 'asc']],
                    dom: 'Bfrtip',
                });

                table.buttons().container().appendTo('#example_wrapper .col-md-6:eq(0)');

                var tableDe = $('#docuentosTable').DataTable();
                tableDe.search();
                tableDe.ajax.reload();


            },
            error: function (msg) {
                Swal.fire(
                    'Error',
                    msg.responseText,
                    'error',
                    'timer:5000'
                )
            }
        });
    })

    $("#btnSendCodes").click(function () {

        var pedido = $("#txtPedido").val();
        var bultos = $("#txtBultos").val();
        var empacador = $("#slctUsuario option:selected").val();
        var ObservacionesExtra = $("#txtObservacionExtra").val();

        if (pedido == "" || pedido == null) {
            Swal.fire(
                'Upps..',
                'Debes de seleccionar un pedido primero',
                'info',
                'timer:5000'
            )
        }
        else {

            var formData = new FormData();
            formData.append('pedido', pedido);
            formData.append('bultos', bultos);
            formData.append('usuario', empacador);
            formData.append('ObservacionesExtra', ObservacionesExtra);

            $.ajax({
                type: 'POST',
                url: resolveUrl("~/Etiquetas/AceptarAlistado"),
                data: formData,
                type: 'POST',
                contentType: false,
                processData: false,
                success: function (response) {
                    Swal.fire(
                        'Éxito',
                        'Se ha confirmado el pedido correctamente',
                        'success',
                        'timer:5000'
                    )
                },
                error: function (msg) {
                    Swal.fire(
                        'Error',
                        msg.responseText,
                        'error',
                        'timer:5000'
                    )
                }
            });
        }
    });

    var getDeleteButtton = function (data, row) {
        html = `<button class='btn btn-danger btn-sm text-white' onclick='documentos.deleteData(${row.id})'><i class='fas fa-trash'></i></button>`
        return html;
    }

    var deleteData = function (id) {
        var formData = new FormData();
        formData.append('id', id);

        $.ajax({
            url: resolveUrl("~/Etiquetas/EliminarRegistro"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (response) {
                var table = $('#tableDetalle').DataTable();
                table.search();
                table.ajax.reload();
            },
            error: function (msg) {
                Swal.fire(
                    'Upps..',
                    msg.responseText,
                    'error',
                    'timer:5000'
                ).then((dismiss) => {
                    var table = $('#tableBodega').DataTable();
                    table.search();
                    table.ajax.reload();
                })
            }
        });
    }

    return {
        deleteData: deleteData
    };
});