var despacho = (function () {
    $(document).ready(function () {
        var table = $('#tableDespacho').DataTable({
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
                    "data": "documento",
                    "title": "Documento",
                    "render": function (data, type, row, meta) {
                        return data;
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
                    "data": "fecha",
                    "title": "Fecha de empacado",
                    "render": function (data, type, row, meta) {
                        return data.split('T')[0] + ' ' + data.split('T')[1];
                    }
                },
                {
                    "orderable": true,
                    "data": "usuario",
                    "title": "Usuario alistador",
                    "render": function (data, type, row, meta) {
                        return data;
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

        var getSelectRutas = function (data, row) {
            html = `<select class='form-control' id='slc${row.pedido}' class='js-select2'>`;
            html += `<option value='0'>SELECCIONE...</option>`;
            $.each(row.rutas, function (key, value) {
                if (row.idRuta == value.id) {
                    html += `<option value='${value.id}' selected>${value.descripcion}</option>`;
                }
                html += `<option value='${value.id}'>${value.descripcion}</option>`;
            });
            html += '</select>';

            //$(`#slc${row.pedido}`).select2({
            //    width: '200px'
            //});

            return html;
        }

        var getSelectUsuarios = function (data, row) {
            html = `<select class='form-control' id='slc${row.usuario}' >`;
            if (row.usuario == null || row.usuario == "") {
                html += `<option value='0' selected>SELECCIONE...</option>`;
            }
            else {
                html += `<option value='0'>SELECCIONE...</option>`;
            }
            $.each(row.usuarios, function (key, value) {
                if (row.usuario == value.usuario) {
                    html += `<option value='${value.usuario}' selected>${value.usuario}</option>`;
                }
                html += `<option value='${value.usuario}'>${value.usuario}</option>`;
            });
            html += '</select>';

            return html;
        }

    });

}());