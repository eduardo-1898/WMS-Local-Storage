var documentos = (function () {
    $(document).ready(function () {
        var table = $('#docuentosTable').DataTable({
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
            "ajax": resolveUrl("~/Etiquetas/List"),
            "columns": [
                {
                    "orderable": true,
                    "data": "id",
                    "title": "Documento",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "fechaCreacion",
                    "title": "Fecha de creación",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "usuarioCreador",
                    "title": "Usuario",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "cantidadBultos",
                    "title": "Cantidad de pedidos",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "id",
                    "width": "10%",
                    "render": function (data, type, row, meta) {
                        return getButtton(data, row);
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

    });

    var getButtton = function (data, row) {
        var url = resolveUrl("~/Etiquetas/ImprimirEtiquetaDoc?id=" + row.id);
        html = `<button class='btn btn-success btn-sm text-white' onclick='documentos.edit(${row.id})'><i class='fas fa-pencil-alt'></i></button>`;
        if (row.cantidadBultos > 0) {
            html += `<a target="_blank" href='${url}' class='btn btn-warning btn-sm text-white' style='margin-left:5px;' ><i class='fas fa-print'></i></a>`;
        }
        html += `<button style='margin-left:5px;' class='btn btn-danger btn-sm text-white' onclick='documentos.deleteData(${row.id}) '><i class='fas fa-trash'></i></button>`;
        return html;
    }

    var getDeleteButtton = function (data, row) {
        html = `<button class='btn btn-danger btn-sm text-white' onclick='documentos.deleteData(${row.id})'><i class='fas fa-trash'></i></button>`
        return html;
    }

    var view = function (id) {

        $.ajax({
            url: resolveUrl("~/Etiquetas/ImprimirEtiquetaDoc?id="+id),
            type: 'GET',
            contentType: false,
            processData: false,
            success: function (response) {

                var arrrayBuffer = base64ToArrayBuffer(response); //data is the base64 encoded string
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
                window.open(link);
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

    var deleteData = function(id) {
        var formData = new FormData();
        formData.append('id', id);

        $.ajax({
            url: resolveUrl("~/Etiquetas/EliminarRegistro"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (response) {
                var table = $('#docuentosTable').DataTable();
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
                    var table = $('#docuentosTable').DataTable();
                    table.search();
                    table.ajax.reload();
                })
            }
        });
    }

    var edit = function (id) {
        var formData = new FormData();
        formData.append('id',id);

        $.ajax({
            url: resolveUrl("~/Etiquetas/ObtenerDocumento"),
            data: formData,
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
        edit: edit,
        deleteData: deleteData,
        view: view
    };

}());