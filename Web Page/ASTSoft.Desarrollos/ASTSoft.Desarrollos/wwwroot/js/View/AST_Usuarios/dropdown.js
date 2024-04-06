$(document).ready(function () {
    ObtenerSucursales();
});

function ObtenerSucursales() {

    $.ajax({
        type: 'GET',
        url: resolveUrl("~/AST_Usuarios/GetSucursales"),
        success: function (data) {

            $('#ddlSucursales').html("");

            $('#ddlSucursales')
                .append(data.map(function (sucursal, i) {
                    return `
                            <option value="${sucursal.sucursal}">
                                   ${sucursal.nombre}
                              </option>`;
                }));

        },
        error: function (msg) {
            alert(msg.responseText);
        }
    });
}
