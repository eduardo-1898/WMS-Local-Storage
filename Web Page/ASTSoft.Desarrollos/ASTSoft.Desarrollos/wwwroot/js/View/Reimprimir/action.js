$(document).ready(function () {
    $("#btnBuscar").on('click', function () {
        var articulo = $("#txtArticuloB").val();
        var serieLote = $("#txtSerieLoteB").val();

        $("#txtArticulo").val(articulo)
        $("#txtSerieLote").val(serieLote)
    })

    $("#btnImprimir").on('click', function () {

        var articulo = $("#txtArticulo").val();
        var serieLote = $("#txtSerieLote").val();
        var inicio = $("#txtRangoInicio").val();
        var final = $("#txtRangoFinal").val();

        if (final < inicio) {
            Swal.fire(
                'Uppsss..',
                'El consecutivo de fin no puede ser menor al de inicio',
                'info',
                'timer:5000'
            )
        } else {

            let timerInterval
            Swal.fire({
                title: 'Un momento..',
                html: 'Se estan procesando los datos',
                timer: 50000,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading()

                    var formData = new FormData();
                    formData.append('articulo', articulo);
                    formData.append('serieLote', serieLote);
                    formData.append('inicio', inicio);
                    formData.append('final', final);

                    $.ajax({
                        type: 'GET',
                        url: resolveUrl(`~/DataMatrix/ReimprimirDatamatrix?articulo=${articulo}&serieLote=${serieLote}&inicio=${inicio}&final=${final}`),
                        data: formData,
                        contentType: false,
                        processData: false,
                        success: function (response) {

                            swal.close();

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
                            window.open(link, '', 'height=650,width=840');

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


                },
                willClose: () => {
                }
            }).then((result) => {
            })
        }
    })

})