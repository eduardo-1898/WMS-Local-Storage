var redespacho = (function () {

    $(document).ready(function () {
        var table = $('#redespachoTable').DataTable({
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
            "ajax": resolveUrl("~/Redespacho/List"),
            "columns": [
                {
                    "orderable": true,
                    "data": "consecutivo",
                    "title": "Consecutivo",
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
                    "data": "pedido",
                    "title": "Pedido",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "bulto",
                    "title": "Bultos",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "finalizado",
                    "title": "Estado",
                    "render": function (data, type, row, meta) {
                        if (data) {
                            return "Finalizado";
                        }
                        else {
                            return "Pendiente";
                        }

                    }
                },
                {
                    "orderable": true,
                    "data": "pedido",
                    "width": "35%",
                    "render": function (data, type, row, meta) {
                        return `<input type="text" id=txt${row.pedido} class="form-control">`
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

        table.buttons().container()
            .appendTo('#example_wrapper .col-md-6:eq(0)');

    });

    var getButtton = function (data, row) {
        html = `<button class='btn btn-success btn-sm text-white' style='margin-right: 10px;' onclick='redespacho.redeespachar(${row.pedido})'><i class='fas fa-check-double'></i></button>`
        return html;
    }

    var redeespachar = function (pedido) {

        var justificacion = $(`#txt${pedido}`).val();

        if (justificacion == "" || justificacion == null || justificacion == undefined ) {

            Swal.fire(
                'Upppsss..',
                'No se ha agregado una justificación para el redespacho',
                'info',
                'timer:5000'
            ).then((dismiss) => {
            })

        } else {

            var formData = new FormData();
            formData.append('pedido', pedido);
            formData.append('justificacion', justificacion);

            $.ajax({
                url: resolveUrl("~/Redespacho/ReDespachar"),
                data: formData,
                type: 'POST',
                contentType: false,
                processData: false,
                success: function (response) {

                    Swal.fire(
                        'Éxito',
                        'Se ha redespachado el pedido correctamente',
                        'success',
                        'timer:5000'
                    ).then((dismiss) => {
                        var table = $('#redespachoTable').DataTable();
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
    }

    return {
        redeespachar: redeespachar
    };

}());