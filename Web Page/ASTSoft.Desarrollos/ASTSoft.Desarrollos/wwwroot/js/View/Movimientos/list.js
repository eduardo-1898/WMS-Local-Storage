var afiliados = (function () {

    $(document).ready(function () {
        var table = $('#tableComparativo').DataTable({
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
            "ajax": resolveUrl("~/Movimientos/List"),
            "columns": [
                {
                    "orderable": false,
                    "data": "id",
                    "width": "2%",
                    "render": function (data, type, row, meta) {
                        return getCheckbox(data, row);
                    }
                },
                {
                    "orderable": true,
                    "data": "movimiento",
                    "title": "Movimiento",
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
                    "data": "fechaEmision",
                    "title": "Fecha de Emisión",
                    "render": function (data, type, row, meta) {
                        return data.split('T')[0];
                    }
                },
                {
                    "orderable": true,
                    "data": "saldo",
                    "title": "Saldo",
                    "render": function (data, type, row, meta) {
                        return data.toLocaleString('en-US');
                    }
                },
                {
                    "orderable": true,
                    "data": "autorizacion",
                    "title": "Autorización CXC",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "conciliado",
                    "title": "Conciliado",
                    "render": function (data, type, row, meta) {
                        return getBadge(data,row);
                    }
                },
                {
                    "orderable": true,
                    "data": "facturacion",
                    "title": "Facturación",
                    "render": function (data, type, row, meta) {
                        return data.toLocaleString('en-US');
                    }    
                },
                {
                    "orderable": true,
                    "data": "renta",
                    "title": "Renta 1.76%",
                    "render": function (data, type, row, meta) {
                        return data.toLocaleString('en-US');
                    }
                },
                {
                    "orderable": true,
                    "data": "comision",
                    "title": "Comisión 2.25% - 7%",
                    "render": function (data, type, row, meta) {
                        return (row.comision + row.comisionInternacional).toLocaleString('en-US');
                    }
                },
                {
                    "orderable": true,
                    "data": "retencion",
                    "title": "Venta 1.77%",
                    "render": function (data, type, row, meta) {
                        return data.toLocaleString('en-US');
                    }
                },
                {
                    "orderable": true,
                    "data": "montoNeto",
                    "title": "Monto neto",
                    "render": function (data, type, row, meta) {
                        return data.toLocaleString('en-US');
                    }
                },
                {
                    "orderable": true,
                    "data": "autorizacion1",
                    "title": "Autorización banco",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "liquidacion",
                    "title": "Liquidación",
                    "render": function (data, type, row, meta) {
                        return data;
                    }
                },
                {
                    "orderable": true,
                    "data": "afiliado",
                    "title": "Afiliado",
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

        var getCheckbox = function (data, row) {
            if (row.conciliado === 'VERDADERO') {
                html = `<input class='form-check text-center' type='checkbox' id='${row.id + ":"
                    + row.liquidacion + ":"
                    + (row.comision + row.comisionInternacional) + ":"
                    + row.retencion + ":"
                    + row.renta + ":"
                    + row.cuenta + ":"
                    + row.idbac }' />`;
                return html;
            }
            else {
                html = '';
                return html;
            }
        }

        var getBadge = function (data, row) {
            if (row.conciliado === 'VERDADERO' && row.ban > 0) {
                html = `<span class='badge badge-success'>${row.conciliado}</span>`;
                return html;
            }
            if (row.autorizacion1 == null || row.autorizacion1 == "") {
                html = `<span class='badge badge-info'>Sin registro en bancos</span>`;
                return html;
            }
            if (row.ban == 0) {
                html = `<span class='badge badge-warning'>Sin registro en ABAN</span>`;
                return html;
            }
            else {
                html = `<span class="badge badge-danger">${row.conciliado}</span>`;
                return html;
            }

        }

        $('#select-all').on('click', function () {
            var rows = table.rows({ 'search': 'applied' }).nodes();
            $('input[type="checkbox"]', rows).prop('checked', this.checked);
        });

        $('#btnAfectar').on('click', function (e) {

            var movsSelected = 0;
            var arr = [];

            table.rows().nodes().to$().find('input[type="checkbox"]').each(function () {
                if ($.contains(document, this)) {
                    if (this.checked) {
                        movsSelected += 1;
                        arr.push(this.id);
                    }
                }
            });

            if (movsSelected == 0) {
                Swal.fire(
                    'Upss..',
                    'No se han encontrado movimientos seleccionados o disponibles para afectar',
                    'info'
                )
            }
            else {
                var formData = new FormData();
                formData.append('list', arr);
                Swal.fire({
                    title: 'Procesando',
                    html: 'Esto tomará solo un momento..',
                    timer: 4000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading()
                        $.ajax({
                            type: 'POST',
                            url: resolveUrl("~/Movimientos/ProcesarMovimientos"),
                            data: formData,
                            type: 'POST',
                            contentType: false,
                            processData: false,
                            success: function (response) {
                                Swal.fire(
                                    'Éxito',
                                    'Los movimientos seleccionados se han procesado éxitosamente',
                                    'success',
                                    'timer:5000'
                                ).then((dismiss) => {
                                    var table1 = $('#tableComparativo').DataTable();
                                    table1.search();
                                    table1.ajax.reload();
                                })

                            },
                            error: function (msg) {
                                Swal.fire(
                                    'Error',
                                    msg.responseText,
                                    'error',
                                    'timer:5000'
                                ).then((dismiss) => {
                                    var table1 = $('#tableComparativo').DataTable();
                                    table1.search();
                                    table1.ajax.reload();
                                })
                            }
                        });
                    },
                    willClose: () => {

                    }
                }).then((result) => {
                    
                })
            }

        });
    });

}());