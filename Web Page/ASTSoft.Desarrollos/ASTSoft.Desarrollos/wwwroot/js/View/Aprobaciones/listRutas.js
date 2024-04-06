var bodegas = (function () {

    $(document).ready(function () {
        var table = $('#tableAprobaciones').DataTable({
            "pageLength": 10,
            "processing": true,
            "serverSide": false,
            "paging": true,
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
                ],
            "ajax": resolveUrl("~/Alisto/List?situacion=Despacho"),
            "columns": [
                {
                    "orderable": false,
                    "data": "pedido",
                    "title": "Pedido",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "cliente",
                    "title": "Cliente",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "nombre",
                    "title": "Nombre",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "observ",
                    "title": "Observaciones",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "estado",
                    "title": "Situación",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "prod",
                    "title": "Pro",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "ruta",
                    "title": "Ruta",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "fecha",
                    "title": "Fecha de Emisión",
                    "render": function (data, type, row, meta) {
                        return data.split('T')[0];
                    }
                },
                {
                    "orderable": true,
                    "data": "usuario",
                    "title": "Usuario",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "tunidad",
                    "title": "Tunidad",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "fecha",
                    "title": "Fecha de Empacado",
                    "render": function (data, type, row, meta) {
                        return data.split('T')[0];
                    }
                },
                {
                    "orderable": true,
                    "data": "usuario",
                    "title": "Usuario alistador",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "pedido",
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

        table.buttons().container().appendTo('#example_wrapper .col-md-6:eq(0)');

    });

    var getButtton = function (data, row) {
        html = `<button class='btn btn-success btn-sm text-white' style='margin-right: 10px;' onclick='bodegas.aprobar(${row.pedido})'><i class='fas fa-check-double'></i></button>`
        html += `<button class='btn btn-warning btn-sm text-white' onclick='bodegas.cambiar(${row.pedido})'><i class='fas fa-undo-alt'></i></button>`
        return html;
    }

    var aprobar = function (pedido) {

        var formData = new FormData();
        formData.append('id', pedido);

        $.ajax({
            url: resolveUrl("~/Rutas/FinalizarDespacho"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (response) {

                Swal.fire(
                    'Éxito',
                    'Se ha modificado el registro éxitosamente',
                    'success',
                    'timer:5000'
                ).then((dismiss) => {
                    var table = $('#tableAprobaciones').DataTable();
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
                    var table = $('#tableAprobaciones').DataTable();
                    table.search();
                    table.ajax.reload();
                })
            }
        });
    }

    var cambiar = function (pedido) {

        var formData = new FormData();
        formData.append('id', pedido);

        $.ajax({
            url: resolveUrl("~/Rutas/ReDespachar"),
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (response) {
                Swal.fire(
                    'Éxito',
                    'Se ha modificado el registro éxitosamente',
                    'success',
                    'timer:5000'
                ).then((dismiss) => {
                    var table = $('#tableAprobaciones').DataTable();
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
                    var table = $('#tableAprobaciones').DataTable();
                    table.search();
                    table.ajax.reload();
                })
            }
        });
    }

    return {
        aprobar: aprobar,
        cambiar: cambiar
    };

}());