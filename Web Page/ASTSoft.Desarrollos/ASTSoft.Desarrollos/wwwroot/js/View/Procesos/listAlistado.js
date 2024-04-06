var afiliados = (function () {

    $(document).ready(function () {
        var table = $('#tableAlistado').DataTable({
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
            "ajax": resolveUrl("~/Alisto/List?situacion=Alistando"),
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
                    "data": "fecha",
                    "title": "Inicio de alisto",
                    "render": function (data, type, row, meta) {
                        return data.split('T')[0] + ' ' + data.split('T')[1];
                    }
                },
                {
                    "orderable": true,
                    "data": "almacen",
                    "title": "Bodegas",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "usuarioIntelisis",
                    "title": "Usuario",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "hora",
                    "title": "Hora",
                    "render": function (data, type, row, meta) {
                        return data.split('T')[1].split('.')[0];
                    }
                },

                {
                    "orderable": true,
                    "data": "usuario",
                    "title": "Alistador",
                    "render": function (data, type, row, meta) {
                        return getSelectUsuarios(data, row);
                    }
                },
                {
                    "orderable": true,
                    "data": "usuarioBOD1",
                    "title": "Alistador bodega 112",
                    "render": function (data, type, row, meta) {
                        return getSelectUsuariosBod(data, row);
                    }
                },
                {
                    "orderable": true,
                    "data": "pedido",
                    "width": "3%",
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

    var getSelectUsuarios = function (data, row) {
        html = `<select class='form-control' id='slcUsuario${row.pedido}' >`;
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
            else {
                html += `<option value='${value.usuario}'>${value.usuario}</option>`;
            }
        });
        html += '</select>';

        return html;
    }

    var getSelectUsuariosBod = function (data, row) {
        html = `<select class='form-control' id='slcUsuarioBod${row.pedido}' >`;
        if (row.usuarioBOD1 == null || row.usuarioBOD1 == "") {
            html += `<option value='0' selected>SELECCIONE...</option>`;
        }
        else {
            html += `<option value='0'>SELECCIONE...</option>`;
        }
        $.each(row.usuarios, function (key, value) {
            if (row.usuarioBOD1 == value.usuario) {
                html += `<option value='${value.usuario}' selected>${value.usuario}</option>`;
            }
            else {
                html += `<option value='${value.usuario}'>${value.usuario}</option>`;
            }
        });
        html += '</select>';

        return html;
    }

    var getButtton = function (data, row) {
        html = `<button class='btn btn-success btn-sm text-white' onclick='bodegas.edit(${row.pedido},${row.prioridad})'><i class='fas fa-pencil-alt'></i></button>`
        html += `<button class='btn btn-warning btn-sm text-white' style='margin-left:5px;' onclick='bodegas.view(${row.pedido})'><i class='fas fa-eye'></i></button>`
        return html;
    }
    var edit = function (pedido, prioridad) {
        var formData = new FormData();
        var ruta = $("#slcRuta" + pedido + " :selected").val();
        var usuario = $("#slcUsuario" + pedido + " :selected").val();
        var usuariobod = $("#slcUsuarioBod" + pedido + " :selected").val();

        formData.append('pedido', pedido);
        formData.append('ruta', ruta);
        formData.append('user', usuario);
        formData.append('userbod', usuariobod);
        formData.append('prioridad', prioridad);


        $.ajax({
            url: resolveUrl("~/Alisto/updateData"),
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
                    var table = $('#tableBodega').DataTable();
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
                    var table = $('#tableBodega').DataTable();
                    table.search();
                    table.ajax.reload();
                })
            }
        });
    }

    return {
        edit: edit
    };

}());