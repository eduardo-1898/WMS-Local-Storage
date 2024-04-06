var usuariospermisosmenuList = (function () {

    $(document).ready(function () {
        var table = $('#usuariospermisosTable').DataTable({
            "pageLength": 7,
            "processing": true,
            "serverSide": false,
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
                        title: 'Listado de permisos'
                    },
                    {
                        extend: 'excel',
                        className: 'btn btn-success',
                        text: "Exportar Excel",
                        title: 'Listado de permisos'
                    },
                    {
                        extend: 'print',
                        className: 'btn btn-success',
                        text: "Imprimir",
                        title: 'Listado de permisos'
                    },
                ],
            //buttons: ['copy', 'excel', 'pdf', 'colvis'],
            "ajax": resolveUrl("~/UsuariosPermisos/list"),
            "columns": [
                {
                    "orderable": true,
                    "data": "idUsuario",
                    "title": "usuario",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "descripcion",
                    "title": "permiso",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": false,
                    "data": "id",
                    "width": "5%",
                    "render": function (data, type, row, meta) {
                        return getButtons(data, row);
                    }
                }


            ],
            "order": [[0, 'des']],
            dom: 'Bfrtip',
        });

        table.buttons().container()
            .appendTo('#example_wrapper .col-md-6:eq(0)');
    });

    var getButtons = function (data, row) {
        var html = "<div style='display: flex; padding: 0px; margin: 0px'>";
        html += "<a title='Editar' style='margin-right: 5px;' class='btn btn-primary btn-xs' href='" + resolveUrl("~/UsuariosPermisos/Edit?id=" + row.id) + "'><i class='fas fa-edit'></i></a>";
        html += "<a title='Ver detalles' style='margin-right: 5px;' class='btn btn-warning btn-xs' href='" + resolveUrl("~/UsuariosPermisos/Details?id=" + row.id) + "'><i class='fas fa-clipboard-list'></i></a>";
        //  html += "<a title='Deshabilitar' class='btn btn-danger btn-xs' style='color: #FFF;' onclick='usuariosList.deleteRow(" + '"' + data + '"' + ")'><i class='fas fa-times-circle'></i></a>";
        html += "</div>";
        return html;
    }

    var deleteRow = function (id) {
        bootbox.confirm("¿Está seguro que desea realizar esta acción?", function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: resolveUrl("~/UsuariosPermisos/Delete"),
                    data: { id: id },
                    datatype: 'json',
                    success: function (response) {
                        if (response == "success") {

                            alert("Realizado exitosamente.");
                            //window.location.reload();
                            //$("#myModal").modal('hide');
                        }
                        var table = $('#usuariospermisosTable').DataTable();
                        table.search();
                        table.ajax.reload();
                    },
                    error: function (msg) {
                        alert(msg.responseText);
                    }
                });
            }
        });
    }

    return {
        deleteRow: deleteRow
    };

}());